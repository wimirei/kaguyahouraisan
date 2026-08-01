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


            // Сурв
            var bestSurvivalResults =
                new Dictionary<TouhouGame, Dictionary<Difficulty, BestResultsViewModel>>();


            // Скоринг
            var bestScoringResults =
                new Dictionary<TouhouGame, Dictionary<Difficulty, BestResultsViewModel>>();



            foreach (TouhouGame game in Enum.GetValues(typeof(TouhouGame)))
            {

                bestSurvivalResults[game] =
                    new Dictionary<Difficulty, BestResultsViewModel>();

                bestScoringResults[game] =
                    new Dictionary<Difficulty, BestResultsViewModel>();



                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {

                    // Лучшие сурв результаты

                    var bestSurvivalReplay = replays
                        .Where(r =>
                            r.Game == game &&
                            r.Difficulty == difficulty &&
                            r.Category == RunCategory.Survival)
                        .OrderByDescending(r => ReplayService.GetResultPriority(r))
                        .ThenBy(r => r.DeathCount ?? int.MaxValue)
                        .FirstOrDefault();

                    bestSurvivalResults[game][difficulty] =
                        new BestResultsViewModel
                        {
                            Replay = bestSurvivalReplay
                        };

                    // Лучшие скоринг результаты

                    var bestScoringReplay = replays
                        .Where(r =>
                            r.Game == game &&
                            r.Difficulty == difficulty &&
                            r.Category == RunCategory.Scoring)
                        .OrderByDescending(r => r.Score)
                        .FirstOrDefault();



                    bestScoringResults[game][difficulty] =
                        new BestResultsViewModel
                        {
                            Replay = bestScoringReplay
                        };

                }

            }

            var survivalTables =
                BuildPlayerTables(replays, RunCategory.Survival);


            var scoringTables =
                BuildPlayerTables(replays, RunCategory.Scoring);



            var model = new PlayerProfileViewModel
            {
                Player = player,

                RecentReplays = replays
                .Take(5)
                .ToList(),

                AllReplays = replays,


                BestSurvivalResults = bestSurvivalResults,
                BestScoringResults = bestScoringResults,


                SurvivalTables = survivalTables,
                ScoringTables = scoringTables
            };


            return View(model);
        }



        // Таблица результатов по играм, сложностям, шоттипам


        private Dictionary<TouhouGame, Dictionary<Difficulty, Dictionary<string, BestResultsViewModel>>>
            BuildPlayerTables(List<ReplayModel> replays, RunCategory category)
        {

            var result =
                new Dictionary<TouhouGame,
                Dictionary<Difficulty,
                Dictionary<string, BestResultsViewModel>>>();


            foreach (TouhouGame game in Enum.GetValues(typeof(TouhouGame)))
            {

                result[game] =
                    new Dictionary<Difficulty, Dictionary<string, BestResultsViewModel>>();


                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {

                    result[game][difficulty] =
                        new Dictionary<string, BestResultsViewModel>();


                    var shotTypes = GameData.ShotTypes.TryGetValue(game, out var shots)
                        ? shots.ToList()
                        : new List<string>();

                    if (game == TouhouGame.HSiFS)
                    {
                        shotTypes.AddRange(new[]
                        {
                            "Reimu",
                            "Marisa",
                            "Cirno",
                            "Aya"
                        });
                    }

                    foreach (var shot in shotTypes)
                    {

                        ReplayModel? replay;


                        if (category == RunCategory.Survival)
                        {

                            replay = replays
                                .Where(r =>
                                    r.Game == game &&
                                    r.Difficulty == difficulty &&
                                    r.Category == category &&
                                    r.ShotType == shot)
                                .OrderByDescending(r => ReplayService.GetResultPriority(r))
                                .ThenBy(r => r.DeathCount ?? int.MaxValue)
                                .FirstOrDefault();

                        }
                        else
                        {

                            replay = replays
                                .Where(r =>
                                    r.Game == game &&
                                    r.Difficulty == difficulty &&
                                    r.Category == category &&
                                    r.ShotType == shot)
                                .OrderByDescending(r => r.Score)
                                .FirstOrDefault();

                        }



                        result[game][difficulty][shot] =
                            new BestResultsViewModel
                            {
                                Replay = replay
                            };

                    }

                }

            }


            return result;
        }

    }
}