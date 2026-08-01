using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thrucommunity.Data;
using thrucommunity.Models;
using thrucommunity.Services;

namespace thrucommunity.Controllers
{
    public class PlayersController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PlayersController(ApplicationDbContext _context, IWebHostEnvironment environment)
        {
            this._context = _context;
            _environment = environment;
        }

        [HttpGet("Leaderboard")]
        public async Task<IActionResult> Players()
        {
            var players = await _context.Players
                .OrderByDescending(p => p.survivalpoints)
                .ToListAsync();

            return View(players);   
        }

        [HttpGet("{nickname}")]
        public async Task<IActionResult> Profile(string nickname)
        {
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Nickname == nickname);

            if (player == null)
                return NotFound();

            var replays = await _context.Replays
                .Where(r => r.Proven &&
                            r.Nickname == nickname)
                .OrderByDescending(r => r.ReplayDate)
                .ToListAsync();

            //Лучшие результаты

            var bestResults = new Dictionary<TouhouGame, Dictionary<Difficulty, BestResultsViewModel>>();

            foreach (TouhouGame game in Enum.GetValues(typeof(TouhouGame)))
            {
                bestResults[game] = new Dictionary<Difficulty, BestResultsViewModel>();

                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    var bestReplay = replays
                        .Where(r => r.Game == game &&
                                    r.Difficulty == difficulty &&
                                    r.Category == RunCategory.Survival)
                        .OrderByDescending(r => ReplayService.GetResultPriority(r))
                        .ThenBy(r => r.DeathCount ?? int.MaxValue)
                        .FirstOrDefault();

                    bestResults[game][difficulty] = new BestResultsViewModel
                    {
                        Replay = bestReplay
                    };
                }
            }   

            var model = new PlayerProfileViewModel
            {
                Player = player,
                RecentReplays = replays.Take(5).ToList(),
                AllReplays = replays,
                BestResults = bestResults
            };

            return View(model);
        }

    }
}
