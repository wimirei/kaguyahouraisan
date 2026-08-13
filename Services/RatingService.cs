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
            { TouhouGame.PoFV, 30 },
            { TouhouGame.MoF, 30 },
            { TouhouGame.SA, 70 },
            { TouhouGame.UFO, 100 },
            { TouhouGame.GFW, 70 },
            { TouhouGame.TD, 50 },
            { TouhouGame.DDC, 70 },
            { TouhouGame.LoLK, 70 },
            { TouhouGame.HSiFS, 70 },
            { TouhouGame.WBaWC, 50 },
            { TouhouGame.UM, 50 },
            { TouhouGame.UDoALG, 30 },
            { TouhouGame.FW, 70 }
        };

        // Коэффициент сложности
        private static readonly Dictionary<Difficulty, double> DifficultyMultiplier = new()
        {
            { Difficulty.Easy, 0.1 },
            { Difficulty.Normal, 0.25 },
            { Difficulty.Hard, 0.4 },
            { Difficulty.Lunatic, 1.1 },
            { Difficulty.Extra, 0.85 },
            { Difficulty.Phantasm, 0.85 }
        };
      

        // Бонус за No Bomb для каждой игры
        private static readonly Dictionary<TouhouGame, double> NoBombBonus = new()
        {
            { TouhouGame.HRtP, 1.8 },
            { TouhouGame.SoEW, 1.4 },
            { TouhouGame.PoDD, 1.8 },
            { TouhouGame.LLS, 1.2 },
            { TouhouGame.MS, 1.4 },
            { TouhouGame.EoSD, 1.7 },
            { TouhouGame.PCB, 1.2 },
            { TouhouGame.IN, 1.4 },
            { TouhouGame.PoFV, 1.2 },
            { TouhouGame.MoF, 1.6 },
            { TouhouGame.SA, 1.6 },
            { TouhouGame.UFO, 1.8 },
            { TouhouGame.GFW, 1.6 },
            { TouhouGame.TD, 1.4 },
            { TouhouGame.DDC, 1.6 },
            { TouhouGame.LoLK, 1.8 },
            { TouhouGame.HSiFS, 1.2 },
            { TouhouGame.WBaWC, 1.2 },
            { TouhouGame.UM, 1.2 },
            { TouhouGame.UDoALG, 1.2 },
            { TouhouGame.FW, 1.6 }
        };

        // Бонус за третье условие для каждой игры
        private static readonly Dictionary<TouhouGame, double> ThirdConditionBonus = new()
        {
            { TouhouGame.EoSD, 30 },
            { TouhouGame.PCB, 35 },
            { TouhouGame.IN, 35 },
            { TouhouGame.MoF, 40 },
            { TouhouGame.SA, 40 },
            { TouhouGame.UFO, 40 },
            { TouhouGame.GFW, 45 },
            { TouhouGame.HSiFS, 45 },
            { TouhouGame.WBaWC, 50 },
            { TouhouGame.UM, 50 },
            { TouhouGame.FW, 55 }
        };

        // Коэффициент за количество смертей
        private static readonly Dictionary<int, double> DeathMultiplier = new()
        {
            { 0, 2.00 },
            { 1, 1.50 },
            { 2, 1.20 },
            { 3, 1.0 },
            { 4, 0.80 },
            { 5, 0.60 },
            { 6, 0.40 },
            { 7, 0.20 },
            { 8, 0.10 },
            { 9, 0.05 },

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

            //points += (deaths * 0.8);

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
                            "Обычный"
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