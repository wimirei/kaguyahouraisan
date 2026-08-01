using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thrucommunity.Data;
using thrucommunity.Models;

namespace thrucommunity.Controllers
{
    [Route("")]
    public class ChallengesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChallengesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LNB Страница
        [HttpGet("LNB")]
        public async Task<IActionResult> LNB()
        {
            var replays = await _context.Replays
                .Where(r =>
                    r.SubmissionStatus == SubmissionStatuses.Approved &&
                    r.Difficulty == Difficulty.Lunatic &&
                    r.NoBomb &&
                    (
                        // Обычный LNB
                        (
                            r.Game != TouhouGame.PoFV &&
                            !r.NoMiss
                        )

                        ||

                        // PoFV
                        (
                            r.Game == TouhouGame.PoFV &&
                            (
                                !r.DeathCount.HasValue ||
                                r.DeathCount.Value > 1
                            ) &&
                            r.ShotType != "Medicine" &&
                            r.ShotType != "Aya"
                        )
                    ))
                .ToListAsync();

            var model = BuildGameTable(replays, false);

            return View(model);
        }

        [HttpGet("ExNN")]
        public async Task<IActionResult> ExNN()
        {
            var replays = await _context.Replays
                .Where(r =>
                r.SubmissionStatus == SubmissionStatuses.Approved &&
                r.NoBomb &&
                r.NoMiss &&
                (
                    r.Difficulty == Difficulty.Extra ||
                    (
                        r.Game == TouhouGame.PCB &&
                        r.Difficulty == Difficulty.Phantasm
                    )
                ))
                .ToListAsync();

            var model = BuildGameTable(replays, true);

            return View(model);
        }

        // LNN Страница
        [HttpGet("LNN")]
        public async Task<IActionResult> LNN()
        {
            var replays = await _context.Replays
                .Where(r =>
                    r.SubmissionStatus == SubmissionStatuses.Approved &&
                    r.Difficulty == Difficulty.Lunatic &&
                    r.NoBomb && r.ShotType != "Medicine" &&
                                r.ShotType != "Aya" &&
                    (
                        r.NoMiss ||

                        (
                            r.Game == TouhouGame.PoFV &&
                            r.DeathCount == 1
                        )
                    ))
                .ToListAsync();

            var model = BuildGameTable(replays, false);

            return View(model);
        }

        private List<GameTableViewModel> BuildGameTable(List<ReplayModel> replays, bool isExtraTable)
        {
            var result = new List<GameTableViewModel>();

            foreach (TouhouGame game in Enum.GetValues(typeof(TouhouGame)))
            {
                var gameReplays = replays
                    .Where(r => r.Game == game)
                    .ToList();

                var gameTable = new GameTableViewModel
                {
                    Game = game
                };

                var shotTypes = GameData.ShotTypes
                    .GetValueOrDefault(game, new List<string>());

                // HSiFS Extra
                if (isExtraTable && game == TouhouGame.HSiFS)
                {
                    shotTypes = new List<string>
                    {
                        "Reimu",
                        "Marisa",
                        "Aya",
                        "Cirno"
                    };
                }

                // LNB,LNN,ExNN

                foreach (var shot in shotTypes)
                {

                    // IN с финалами
                    if (game == TouhouGame.IN &&
                        gameReplays.Any(r => r.Difficulty == Difficulty.Lunatic))
                    {
                        foreach (var final in GameData.INFinals[game])
                        {
                            var players = gameReplays
                                .Where(r =>
                                    r.ShotType == shot &&
                                    r.Difficulty == Difficulty.Lunatic &&
                                    r.INFinal == final)
                                .GroupBy(r => r.Nickname)
                                .Select(g =>
                                {
                                    var best = g
                                        .OrderBy(r => r.DeathCount)
                                        .First();

                                    return new PlayerMiniViewModel
                                    {
                                        Nickname = best.Nickname,
                                        ReplayId = best.Id,
                                        DeathCount = best.DeathCount,
                                        NoThirdCondition = best.NoThirdCondition
                                    };
                                })
                                .ToList();

                            gameTable.Records.Add(new ShotTypeTableRow
                            {
                                ShotType = shot,
                                INFinal = final,
                                PlayersCount = players.Count,
                                Players = players
                            });
                        }
                    }

                    else
                    {
                        var players = gameReplays
                            .Where(r => r.ShotType == shot &&
                                        r.Difficulty != Difficulty.Phantasm)
                            .GroupBy(r => r.Nickname)
                            .Select(g =>
                            {
                                var best = g
                                    .OrderBy(r => r.DeathCount)
                                    .First();

                                return new PlayerMiniViewModel
                                {
                                    Nickname = best.Nickname,
                                    ReplayId = best.Id,
                                    DeathCount = best.DeathCount,
                                    NoThirdCondition = best.NoThirdCondition
                                };
                            })
                            .ToList();

                        gameTable.Records.Add(new ShotTypeTableRow
                        {
                            ShotType = shot,
                            PlayersCount = players.Count,
                            Players = players
                        });
                    }
                }

                //PCB Phantasm

                if (isExtraTable && game == TouhouGame.PCB)
                {

                    var shotTypesPhantasm = GameData.ShotTypes
                        .GetValueOrDefault(game, new List<string>());


                    foreach (var shot in shotTypesPhantasm)
                    {

                        var players = gameReplays
                            .Where(r =>
                                r.ShotType == shot &&
                                r.Difficulty == Difficulty.Phantasm)
                            .GroupBy(r => r.Nickname)
                            .Select(g =>
                            {
                                var best = g
                                    .OrderBy(r => r.DeathCount)
                                    .First();


                                return new PlayerMiniViewModel
                                {
                                    Nickname = best.Nickname,
                                    ReplayId = best.Id,
                                    DeathCount = best.DeathCount,
                                    NoThirdCondition = best.NoThirdCondition
                                };

                            })
                            .ToList();


                        gameTable.PhantasmRecords.Add(new ShotTypeTableRow
                        {
                            ShotType = shot,
                            PlayersCount = players.Count,
                            Players = players
                        });

                    }

                }

                result.Add(gameTable);

            }

            return result;

        }

        [HttpGet("Scoring")]
        public async Task<IActionResult> Scoring()
        {
            var model = new List<ScoringGameViewModel>();

            foreach (var game in GameData.ShotTypes.Keys)
            {
                var gameVm = new ScoringGameViewModel
                {
                    Game = game
                };

                foreach (var difficulty in GameData.Difficulties[game])
                {
                    var difficultyVm = new ScoringDifficultyViewModel
                    {
                        Difficulty = difficulty
                    };

                    var shotTypes = GameData.ShotTypes[game];

                    // HSiFS Extra 
                    if (game == TouhouGame.HSiFS &&
                        difficulty == Difficulty.Extra)
                    {
                        shotTypes = new List<string>
                        {
                            "Reimu",
                            "Marisa",
                            "Aya",
                            "Cirno"
                        };
                    }

                    foreach (var shotType in shotTypes)
                    {
                        var shotVm = new ScoringShotTypeViewModel
                        {
                            ShotType = shotType
                        };

                        var replays = await _context.Replays
                            .Where(r =>
                                r.Proven &&
                                r.Category == RunCategory.Scoring &&
                                r.Game == game &&
                                r.Difficulty == difficulty &&
                                r.ShotType == shotType)
                            .OrderByDescending(r => r.Score)
                            .ToListAsync();

                        foreach (var replay in replays)
                        {
                            shotVm.Replays.Add(new ScoringReplayViewModel
                            {
                                ReplayId = replay.Id,
                                Nickname = replay.Nickname,
                                Score = replay.Score ?? 0,
                                ReplayDate = replay.ReplayDate ?? replay.SubmittedAtUtc
                            });
                        }

                        difficultyVm.ShotTypes.Add(shotVm);
                    }

                    gameVm.Difficulties.Add(difficultyVm);
                }

                model.Add(gameVm);
            }

            return View(model);
        }
    }
}
