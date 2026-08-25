using thrucommunity.Models;

namespace thrucommunity.Services
{
    public static class RatingService
    {

        public class RatingTestResult
        {
            public TouhouGame Game { get; set; }
            public Difficulty Difficulty { get; set; }

            public string Conditions { get; set; } = "";

            public int DeathCount { get; set; }

            public int Points { get; set; }
        }


        // Базовые очки за игру
        private static readonly Dictionary<TouhouGame, double> GameBasePoints = new()
        {
            { TouhouGame.HRtP, 70 },
            { TouhouGame.SoEW, 30 },
            { TouhouGame.PoDD, 100 },
            { TouhouGame.LLS, 30 },
            { TouhouGame.MS, 50 },
            { TouhouGame.EoSD, 50 },
            { TouhouGame.PCB, 30 },
            { TouhouGame.IN, 30 },
            { TouhouGame.PoFV, 5 },
            { TouhouGame.MoF, 30 },
            { TouhouGame.SA, 70 },
            { TouhouGame.UFO, 100 },
            { TouhouGame.GFW, 70 },
            { TouhouGame.TD, 50 },
            { TouhouGame.DDC, 70 },
            { TouhouGame.LoLK, 70 },
            { TouhouGame.HSiFS, 70 },
            { TouhouGame.WBaWC, 30 },
            { TouhouGame.UM, 50 },
            { TouhouGame.UDoALG, 1 },
            { TouhouGame.FW, 70 }
        };

        // Коэффициент сложности
        private static readonly Dictionary<Difficulty, double> DifficultyMultiplier = new()
        {
            { Difficulty.Easy, 0.10 },
            { Difficulty.Normal, 0.15 },
            { Difficulty.Hard, 0.30 },
            { Difficulty.Lunatic, 3.60 },
            { Difficulty.Extra, 0.45 },
            { Difficulty.Phantasm, 0.45 }
        };


        // Бонус за No Bomb для каждой игры
        private static readonly Dictionary<TouhouGame, double> NoBombBonus = new()
        {
            { TouhouGame.UFO, 12.80 },
            { TouhouGame.PoDD, 12.75 },
            { TouhouGame.FW, 13.00 },
            { TouhouGame.HRtP, 12.85 },
            { TouhouGame.LoLK, 12.70 },
            { TouhouGame.UM, 11.56 },
            { TouhouGame.EoSD, 12.80 },
            { TouhouGame.DDC, 11.90 },
            { TouhouGame.HSiFS, 11.80 },
            { TouhouGame.SA, 11.70 },
            { TouhouGame.IN, 12.40 },
            { TouhouGame.TD, 11.80 },
            { TouhouGame.MoF, 12.30 },
            { TouhouGame.GFW, 11.00 },
            { TouhouGame.SoEW, 12.00 },
            { TouhouGame.MS, 11.35 },
            { TouhouGame.LLS, 11.80 },
            { TouhouGame.PCB, 11.60 },
            { TouhouGame.WBaWC, 11.40 },
            { TouhouGame.PoFV, 11.30 },
            { TouhouGame.UDoALG, 11.20 }
        };

        // Бонус за третье условие для каждой игры
        private static readonly Dictionary<TouhouGame, double> ThirdConditionBonus = new()
        {
            { TouhouGame.PCB, 35 },
            { TouhouGame.IN, 35 },
            { TouhouGame.UFO, 40 },
            { TouhouGame.TD, 40 },
            { TouhouGame.HSiFS, 40 },
            { TouhouGame.WBaWC, 40 },
            { TouhouGame.UM, 300 },
            { TouhouGame.FW, 55 }
        };

        // Коэффициент за количество смертей
        private static readonly Dictionary<int, double> DeathMultiplier = new()
        {
            { 0, 12.3 },
            { 1, 1.90 },
            { 2, 1.80 },
            { 3, 1.70 },
            { 4, 1.40 },
            { 5, 1.30 },
            { 6, 1.10 },
            { 7, 1.10 },
            { 8, 1.05 },
            { 9, 1.05 },

         };

        public static int CalculateSurvivalPoints(ReplayModel replay)
        {
            // Пока неизвестная игра — 0 очков
            if (!GameBasePoints.TryGetValue(replay.Game, out var basePoints))
                return 0;

            // Коэффициент сложности
            if (!DifficultyMultiplier.TryGetValue(
                    replay.Difficulty,
                    out var difficultyMultiplier))
            {
                difficultyMultiplier = 1.0;
            }

            double points = basePoints * difficultyMultiplier;

            // No Bomb
            if (replay.NoBomb)
            {
                if (NoBombBonus.TryGetValue(replay.Game, out var bonus))
                    points *= bonus;
            }

            // Третье условие
            if (replay.NoThirdCondition)
            {
                if (ThirdConditionBonus.TryGetValue(
                        replay.Game,
                        out var bonus))
                {
                    points += bonus;
                }
            }

            int deaths = 0;

            // Смерти


            if (replay.DeathCount != null)
            {
                deaths = replay.DeathCount ?? 0;
            }
            else
            {
                deaths = 9;
            }

            if (DeathMultiplier.TryGetValue(
                      deaths,
                      out var deathMultiplier))
            {
                points *= deathMultiplier;
            }
            else
            {
                // Для большого количества смертей
                points *= Math.Max(
                    0.1,
                    1.0 - deaths * 0.1
                );
            }

            return (int)Math.Round(points);


        }

        public static List<RatingTestResult> GenerateRatingTest()
        {
            var result = new List<RatingTestResult>();

            foreach (var game in Enum.GetValues<TouhouGame>())
            {
                foreach (var difficulty in Enum.GetValues<Difficulty>())
                {
                    // 1СС

                    for (int deaths = 0; deaths <= 9; deaths++)
                    {
                        result.Add(CreateTestResult(
                            game,
                            difficulty,
                            deaths,
                            noBomb: false,
                            noThirdCondition: false,
                            "1cc"
                        ));
                    }

                    // No Bomb

                    for (int deaths = 0; deaths <= 9; deaths++)
                    {
                        result.Add(CreateTestResult(
                            game,
                            difficulty,
                            deaths,
                            noBomb: true,
                            noThirdCondition: false,
                            "No Bomb"
                        ));
                    }

                    // No Bomb + третье условие

                    if (ThirdConditionBonus.ContainsKey(game))
                    {
                        for (int deaths = 0; deaths <= 9; deaths++)
                        {
                            result.Add(CreateTestResult(
                                game,
                                difficulty,
                                deaths,
                                noBomb: true,
                                noThirdCondition: true,
                                "No Bomb + Third Condition"
                            ));
                        }
                    }
                }
            }

            return result;
        }

        private static RatingTestResult CreateTestResult(TouhouGame game, Difficulty difficulty, int deaths, bool noBomb, bool noThirdCondition, string conditions)
        {
            var replay = new ReplayModel
            {
                Game = game,
                Difficulty = difficulty,
                DeathCount = deaths,
                NoBomb = noBomb,
                NoThirdCondition = noThirdCondition
            };

            return new RatingTestResult
            {
                Game = game,
                Difficulty = difficulty,
                Conditions = conditions,
                DeathCount = deaths,
                Points = CalculateSurvivalPoints(replay)
            };
        }
    }
}