using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thrucommunity.Data;
using thrucommunity.Models;
using thrucommunity.Services;

namespace thrucommunity.Controllers
{
    [Route("Replays")]
    public class ReplayController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ReplayParserService _parserService;
        private readonly ILogger<ReplayController> _logger;

        public ReplayController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            ReplayParserService parserService,
            ILogger<ReplayController> logger)
        {
            _context = context;
            _environment = environment;
            _parserService = parserService;
            _logger = logger;
        }

        //Загрузка игр. сложностей. шоттипов и т.д.
        private void LoadGameData()
        {
            ViewBag.Games = GameData.ShotTypes.Keys
                .Select(g => g.ToString())
                .ToList();

            ViewBag.ShotTypes = GameData.ShotTypes.ToDictionary(
                x => x.Key.ToString(),
                x => x.Value);

            ViewBag.Difficulties = GameData.Difficulties.ToDictionary(
                x => x.Key.ToString(),
                x => x.Value.Select(d => d.ToString()).ToList());

            ViewBag.Finals = GameData.INFinals.ToDictionary(
                x => x.Key.ToString(),
                x => x.Value);

            ViewBag.ThirdConditionGames = Enum.GetValues<TouhouGame>()
                .Where(ReplayService.SupportsThirdCondition)
                .Select(g => g.ToString())
                .ToList();

            ViewBag.ThirdConditionNames = Enum.GetValues<TouhouGame>()
                .ToDictionary(
                g => g.ToString(),
                g => g.ThirdConditionRUName());

            ViewBag.FourthConditionGames = Enum.GetValues<TouhouGame>()
                .Where(g => g.SupportsFourthCondition())
                .Select(g => g.ToString())
                .ToList();
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 20;

            var query = _context.Replays
                .Where(r => r.Proven)
                .OrderByDescending(r => r.SubmittedAtUtc);

            int totalCount = await query.CountAsync();

            var model = new ReplayListViewModel
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),

                Replays = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpGet("Replay/{id:int}")]
        public async Task<IActionResult> Replay(int id)
        {
            var replay = await _context.Replays
                .FirstOrDefaultAsync(r => r.Id == id);

            if (replay == null)
                return NotFound();

            return View(replay);
        }


        [HttpGet("Replay/Upload")]
        public IActionResult Create()
        {
            LoadGameData();
            return View();
        }

        [HttpPost("Replay/Parse")]
        public async Task<IActionResult> ParseReplay(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Файл не выбран."
                });
            }

            const long MaxReplaySize = 200 * 1024;

            if (file.Length > MaxReplaySize)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Размер файла реплея не должен превышать 200 КБ."
                });
            }

            string extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (extension != ".rpy")
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Можно загружать только файлы реплеев (.rpy)."
                });
            }

            try
            {
                using var stream = file.OpenReadStream();

                var parseResult = await _parserService.ParseReplayAsync(
                    stream,
                    file.FileName);

                if (parseResult == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Не удалось распарсить реплей."
                    });
                }

                return Json(new
                {
                    success = true,

                    game = parseResult.Game,
                    shot = parseResult.Shot,
                    difficulty = parseResult.Difficulty,

                    score = parseResult.Score,
                    route = parseResult.Route,

                    name = parseResult.Name,
                    timestamp = parseResult.Timestamp,

                    slowdown = parseResult.Slowdown,
                    replayType = parseResult.ReplayType
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при предварительном парсинге реплея");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Произошла ошибка при обработке реплея."
                });
            }
        }

        [HttpPost("Replay/Upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReplayModel model)
        {

            if (!ModelState.IsValid)
            {
                LoadGameData();
                return View(model);
            }


            if (model.ReplayFile == null &&
                string.IsNullOrWhiteSpace(model.ReplayLink))
            {
                ModelState.AddModelError("", "Необходимо загрузить реплей или указать ссылку на видео.");

                LoadGameData();
                return View(model);
            }

            if (model.ReplayFile != null)
            {
                {
                    const long MaxReplaySize = 200 * 1024; // 200 КБ

                    if (model.ReplayFile.Length > MaxReplaySize)
                    {
                        ModelState.AddModelError(
                            nameof(model.ReplayFile),
                            "Размер файла реплея не должен превышать 200 КБ.");

                        LoadGameData();
                        return View(model);
                    }

                    string extension = Path.GetExtension(model.ReplayFile.FileName)
                        .ToLowerInvariant();

                    if (extension != ".rpy")
                    {
                        ModelState.AddModelError(
                            nameof(model.ReplayFile),
                            "Можно загружать только файлы реплеев (.rpy).");

                        LoadGameData();
                        return View(model);
                    }

                    //Конвертазия реплеев в пользовательский формат
                    // thXX_
                    string originalName = Path.GetFileNameWithoutExtension(model.ReplayFile.FileName);

                    int underscore = originalName.IndexOf('_');

                    string prefix = underscore >= 0
                        ? originalName[..underscore]
                        : originalName;

                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var random = new Random();

                    string folder = Path.Combine(
                        _environment.WebRootPath,
                        "Replays",
                        model.Nickname,
                        model.Game.ToString());

                    Directory.CreateDirectory(folder);

                    string filePath;

                    // udXXXX.rpy
                    do
                    {
                        string randomPart = new string(
                            Enumerable.Range(0, 4)
                                .Select(_ => chars[random.Next(chars.Length)])
                                .ToArray());

                        model.ReplayFileName = $"{prefix}_ud{randomPart}.rpy";

                        filePath = Path.Combine(folder, model.ReplayFileName);

                    } while (System.IO.File.Exists(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ReplayFile.CopyToAsync(stream);
                    }

                    model.ReplayFilePath = Path.Combine(
                        "Replays",
                        model.Nickname,
                        model.Game.ToString(),
                        model.ReplayFileName);
                }

            }
            else

            if (model.NoMiss)
            {
                model.DeathCount = 0;
            }

            if (model.Game != TouhouGame.IN) { model.INFinal = null; }

            if (model.Difficulty == Difficulty.Extra) { model.INFinal = null; }

            if(model.ReplayFile != null)
            {
                model.ReplayDate = DateTime.SpecifyKind(model.ReplayDate.Value, DateTimeKind.Utc);
            }            

            model.Proven = false;

            model.SubmissionStatus =
                SubmissionStatuses.Pending;

            model.SubmittedAtUtc =
                DateTime.UtcNow;

            model.TypeOfSurvival = ReplayService.BuildTypeOfSurvival(model);

            _context.Replays.Add(model);

            await _context.SaveChangesAsync();

            await RecalculatePlayerStats(model.Nickname);

            TempData["SuccessMessage"] = "Реплей успешно отправлен и ожидает проверки модератором.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("DownloadReplay/{id:int}")]
        public async Task<IActionResult> DownloadReplay(int id)
        {
            var replay = await _context.Replays.FindAsync(id);

            if (replay == null)
                return NotFound();

            var fullPath = Path.Combine(_environment.WebRootPath, replay.ReplayFilePath);

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

        //Пересчет статы
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
            //int survivalPoints = 0;

            var survivalReplays = all.Where(r =>
                r.Category == RunCategory.Survival &&
                r.Difficulty == Difficulty.Lunatic);

            int survivalPoints = 0;

            foreach (var replay in survivalReplays)
            {
                survivalPoints += RatingService.CalculateSurvivalPoints(replay);
                if (string.IsNullOrWhiteSpace(replay.TypeOfSurvival))
                    continue;

                // Убираем префикс сложнолсть
                string type = GetSurvivalSuffix(replay.TypeOfSurvival);

                switch (type)
                {
                    case "1CC":
                        l1cc++;
                        break;

                    case "NM":
                        lnm++;
                        break;

                    case "NB":
                    case var _ when type.StartsWith("NB("):
                        lnb++;
                        break;

                    case "NN":
                        lnn++;
                        break;

                    default:

                        //LNB+
                        if (type.StartsWith("NB"))
                        {
                            lnbNx++;
                        }
                        //LNN+
                        else if (type.StartsWith("NN"))
                        {
                            lnnn++;
                        }

                        break;
                }
            }

            int exnn = 0;

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
                    //survivalPoints += 2;
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