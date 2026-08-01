using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using thrucommunity.Data;
using thrucommunity.Models;
using thrucommunity.Services;

namespace thrucommunity.Controllers
{
    [Route("AdminMorkovka/[action]")]
    public class AdminMorkovkaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly PlayerService _playerService;

        public AdminMorkovkaController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment, PlayerService playerService)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
            _playerService = playerService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string password)
        {
            string? passwordHash =
                _configuration["AdminPasswordHash"];


            if (string.IsNullOrEmpty(passwordHash))
            {
                return Problem(
                    "Не удалось найти хэш пароля!");
            }

            var hasher = new PasswordHasher<string>();

            var result =
                hasher.VerifyHashedPassword(
                    null,
                    passwordHash,
                    password);


            if (result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Role,"Admin")};

                var identity =
                    new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                var principal =
                    new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("AdminCookie", principal);

                return RedirectToAction(nameof(Pending));
            }

            ViewBag.Error = "Неправильный пароль!";

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AdminMorkovka/Replays")]
        public async Task<IActionResult> Replays(string? search, int page = 1)
        {
            int pageSize = 20;

            var query = _context.Replays
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.Nickname.Contains(search));
            }

            query = query.OrderByDescending(r => r.SubmittedAtUtc);

            var totalCount = await query.CountAsync();

            var replays = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new ReplayAdminViewModel
            {
                Replays = replays,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                SearchNickname = search
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AdminMorkovka/Replays/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, string? search, int page = 1)
        {
            var replay = await _context.Replays.FindAsync(id);

            if (!string.IsNullOrWhiteSpace(replay.ReplayFilePath))
            {
                try
                {
                    if (System.IO.File.Exists(replay.ReplayFilePath))
                    {
                        System.IO.File.Delete(replay.ReplayFilePath);
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Ошибка при удалении файла!");
                }

            }

            _context.Replays.Remove(replay);
            await _context.SaveChangesAsync();

            return RedirectToAction("Replays", new { search, page });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AdminMorkovka/Replays/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var replay = await _context.Replays.FindAsync(id);

            if (replay == null)
                return NotFound();

            return View(replay);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AdminMorkovka/Replays/Edit/{id:int}")]
        public async Task<IActionResult> Edit(ReplayModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var replay = await _context.Replays.FindAsync(model.Id);

            if (replay == null)
                return NotFound();

            replay.Nickname = model.Nickname;
            replay.Comment = model.Comment;
            replay.Game = model.Game;
            replay.ShotType = model.ShotType;
            replay.Category = model.Category;
            replay.Difficulty = model.Difficulty;

            replay.Score = model.Score;
            replay.DeathCount = model.DeathCount;

            replay.NoMiss = model.NoMiss;
            replay.NoBomb = model.NoBomb;
            replay.NoThirdCondition = model.NoThirdCondition;

            replay.ReplayLink = model.ReplayLink;

            replay.TypeOfSurvival = ReplayService.BuildTypeOfSurvival(replay);

            if (model.ReplayDate != default)
            {
                replay.ReplayDate = DateTime.SpecifyKind(model.ReplayDate.Value, DateTimeKind.Utc);
            }

            if (replay.Proven)
            {
                await RecalculatePlayerStats(replay.Nickname);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Replays");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Pending()
        {
            var replays = await _context.Replays
                .Where(x =>
                    x.SubmissionStatus ==
                    SubmissionStatuses.Pending)
                .ToListAsync();

            return View(replays);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Moderate(int id)
        {
            var replay = await _context.Replays.FindAsync(id);

            if (replay == null)
                return NotFound();

            return View(replay);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Moderate(ReplayModel model, string action)
        {
            var replay = await _context.Replays
                .FindAsync(model.Id);

            if (replay == null)
                return NotFound();

            replay.Nickname = model.Nickname;
            replay.Comment = model.Comment;

            replay.Game = model.Game;
            replay.ShotType = model.ShotType;

            replay.Score = model.Score;
            if (model.NoMiss)
            {
                replay.DeathCount = 0;
            }
            else
            {
                replay.DeathCount = model.DeathCount;
            }

            replay.Category = model.Category;
            replay.Difficulty = model.Difficulty;

            replay.NoMiss = model.NoMiss;
            replay.NoBomb = model.NoBomb;
            replay.NoThirdCondition = model.NoThirdCondition;

            replay.TypeOfSurvival = ReplayService.BuildTypeOfSurvival(replay);

            replay.ReplayLink = model.ReplayLink;
            replay.ReplayDate = DateTime.SpecifyKind(model.ReplayDate.Value, DateTimeKind.Utc);

            bool wasProven = replay.Proven;

            switch (action)
            {
                case "approve_proven":

                    replay.Proven = true;

                    replay.SubmissionStatus =
                        SubmissionStatuses.Approved;

                    if (!wasProven)
                    {
                        await _playerService.UpdatePlayerStatistics(replay);
                    }

                    break;

                case "approve_unproven":

                    replay.Proven = false;

                    replay.SubmissionStatus =
                        SubmissionStatuses.Approved;

                    break;

                case "reject":

                    replay.Proven = false;

                    replay.SubmissionStatus =
                        SubmissionStatuses.Rejected;


                    if (!string.IsNullOrWhiteSpace(replay.ReplayFilePath))
                        try
                        {
                            if (System.IO.File.Exists(replay.ReplayFilePath))
                            {
                                System.IO.File.Delete(replay.ReplayFilePath);
                            }
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Ошибка при удалении файла!");
                        }

                    break;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Pending));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReplay(int id)
        {
            var replay = await _context.Replays.FindAsync(id);

            if (replay == null)
                return NotFound();

            string fullPath = Path.Combine(
                _environment.WebRootPath,
                replay.ReplayFilePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var bytes =
                await System.IO.File.ReadAllBytesAsync(
                    fullPath);

            return File(
                bytes,
                "application/octet-stream",
                replay.ReplayFileName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecalculateAllPlayers()
        {
            var nicknames = await _context.Players
                .Select(p => p.Nickname)
                .ToListAsync();

            foreach (var nickname in nicknames)
            {
                await RecalculatePlayerStats(nickname);
            }

            TempData["SuccessMessage"] = "Статистика всех игроков успешно пересчитана.";

            //Console.WriteLine("Сработало!");

            return RedirectToAction("Index", "AdminMorkovka");
        }

        private async Task RecalculatePlayerStats(string nickname)
        {
            var all = await _context.Replays
                .Where(r => r.Proven && r.Nickname == nickname)
                .ToListAsync();

            //Скоринг

            var scoringGroups = all
                .Where(r => r.Category == RunCategory.Scoring)
                .GroupBy(r => new
                {
                    r.Game,
                    r.Difficulty,
                    r.ShotType
                });

            int first = 0;
            int second = 0;
            int third = 0;

            foreach (var group in scoringGroups)
            {
                var ordered = group
                    .OrderByDescending(r => r.Score)
                    .ToList();

                if (ordered.Count > 0) first++;
                if (ordered.Count > 1) second++;
                if (ordered.Count > 2) third++;
            }

            //Сурв(только лунатик(пока что))

            int l1cc = 0;
            int lnm = 0;
            int lnb = 0;
            int lnn = 0;
            int lnnn = 0;
            int lnbNx = 0;
            int survivalPoints = 0;

            var survivalReplays = all.Where(r =>
                r.Category == RunCategory.Survival &&
                r.Difficulty == Difficulty.Lunatic);

            foreach (var replay in survivalReplays)
            {
                if (string.IsNullOrWhiteSpace(replay.TypeOfSurvival))
                    continue;

                // Убираем префикс сложнолсть
                string type = GetSurvivalSuffix(replay.TypeOfSurvival);

                //Префиксы челленджей
                switch (type)
                {
                    case "1CC":
                        l1cc++;
                        survivalPoints += 1;
                        break;

                    case "NM":
                        lnm++;
                        break;

                    case "NB":
                    case var _ when type.StartsWith("NB("):
                        lnb++;
                        survivalPoints += 5;
                        break;

                    case "NN":
                        lnn++;
                        survivalPoints += 50;
                        break;

                    default:

                        //LNB+
                        if (type.StartsWith("NB"))
                        {
                            lnbNx++;
                            survivalPoints += 5;
                        }
                        //LNN+
                        else if (type.StartsWith("NN"))
                        {
                            lnnn++;
                            survivalPoints += 50;
                        }

                        break;
                }
            }

            int exnn = 0;

            //Extra
            var extraReplays = all.Where(r =>
                r.Category == RunCategory.Survival &&
                r.Difficulty == Difficulty.Extra || r.Difficulty == Difficulty.Phantasm);

            foreach (var replay in extraReplays)
            {
                if (string.IsNullOrWhiteSpace(replay.TypeOfSurvival))
                    continue;

                string type = GetSurvivalSuffix(replay.TypeOfSurvival);

                if (type.StartsWith("NN"))
                {
                    exnn++;
                    survivalPoints += 2;
                }
            }

            //Сохранение

            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Nickname == nickname);

            if (player == null)
                return;

            player.FirstPlaceCount = first;
            player.SecondPlaceCount = second;
            player.ThirdPlaceCount = third;

            player.L1CCcount = l1cc;
            player.LNMcount = lnm;
            player.LNBcount = lnb;
            player.LNNcount = lnn;
            player.LNNNcount = lnnn;
            player.LNBNxcount = lnbNx;
            player.ExNNcount = exnn;

            player.survivalpoints = survivalPoints;

            await _context.SaveChangesAsync();

        }

        [Authorize(Roles = "Admin")]
        private static string GetSurvivalSuffix(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return "";

            //Extra
            if (type.StartsWith("Ex"))
                return type.Substring(2);


            //Phantasm
            if (type.StartsWith("Ph"))
                return type.Substring(2);

            //Easy, Mormal, Hard, Lunatic
            return type.Substring(1);
        }
    }
}
