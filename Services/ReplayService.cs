using thrucommunity.Models;

namespace thrucommunity.Services
{
    public static class ReplayService
    {
        public static string BuildTypeOfSurvival(ReplayModel replay)
        {
            //if (replay.Category != RunCategory.Survival)
            // return "Scoring";

            string prefix = replay.Difficulty switch
            {
                Difficulty.Easy => "E",
                Difficulty.Normal => "N",
                Difficulty.Hard => "H",
                Difficulty.Lunatic => "L",
                Difficulty.Extra => "Ex",
                Difficulty.Phantasm => "Ph",
                _ => ""
            };

            string result = "1CC";

            if (replay.NoBomb)
            {
                if (replay.NoMiss)
                {
                    if (replay.NoThirdCondition)
                    {
                        result = replay.No4thCondition
                            ? "NNNN"
                            : "NNN";
                    }
                    else
                    {
                        result = "NN";
                    }
                }
                else
                {
                    result = "NB";

                    if (replay.NoThirdCondition && replay.No4thCondition)
                    {
                        result += "NN";
                    }
                    else
                    {
                        if (replay.NoThirdCondition)
                            result += ThirdConditionName(replay.Game);

                        if (replay.No4thCondition)
                            result += FourthConditionName(replay.Game);
                    }

                }
            }
            else if (replay.NoMiss)
            {
                result = "NM";
            }

            if (result.StartsWith("NB") && replay.DeathCount.HasValue)
            {
                result += $"({replay.DeathCount}M)";
            }

            return prefix + result;
        }

        //Приоритет показа лучших результатов в профиле
        public static int GetResultPriority(ReplayModel replay)
        {
            string type = BuildTypeOfSurvival(replay);

            if (type.StartsWith("Ex"))
                type = type[2..];
            else if (type.StartsWith("Ph"))
                type = type[2..];
            else
                type = type[1..];

            int bracket = type.IndexOf('(');
            if (bracket >= 0)
                type = type[..bracket];

            if (type == "NNNN") return 8;
            if (type == "NNN") return 7;
            if (type == "NN") return 6;

            // Любой NB с NT,NV и т.д.
            if (type.StartsWith("NB") && type != "NB")
                return 5;

            if (type == "NB") return 4;
            if (type == "NM") return 3;
            if (type == "1CC") return 2;

            return 0;
        }

        public static bool SupportsThirdCondition(TouhouGame game)
        {
            return game == TouhouGame.PCB ||
                   game == TouhouGame.IN ||
                   game == TouhouGame.UFO ||
                   game == TouhouGame.TD ||
                   game == TouhouGame.HSiFS ||
                   game == TouhouGame.WBaWC ||
                   game == TouhouGame.UM ||
                   game == TouhouGame.FW;
        }

        public static string ThirdConditionName(this TouhouGame game)
        {
            return game switch
            {
                TouhouGame.PCB => "NBB",
                TouhouGame.IN => "FS",
                TouhouGame.UFO => "NV",
                TouhouGame.TD => "NT",
                TouhouGame.HSiFS => "NR",
                TouhouGame.WBaWC => "NRB",
                TouhouGame.UM => "NC",
                TouhouGame.FW => "NHB",
                _ => ""
            };
        }

        public static string ThirdConditionRUName(this TouhouGame game)
        {
            return game switch
            {
                TouhouGame.PCB => "Без разрушения барьера",
                TouhouGame.IN => "Все спелл карты",
                TouhouGame.UFO => "Без призыва UFO",
                TouhouGame.TD => "Без трансов",
                TouhouGame.HSiFS => "Без релизов",
                TouhouGame.WBaWC => "Без режима берсерка",
                TouhouGame.UM => "Без карт",
                TouhouGame.FW => "Без разрушения гипер барьера",
                _ => ""
            };
        }

        public static bool SupportsFourthCondition(this TouhouGame game)
        {
            return game == TouhouGame.WBaWC;
        }

        public static string FourthConditionName(this TouhouGame game)
        {
            return game switch
            {
                TouhouGame.WBaWC => "NBR",
                _ => ""
            };
        }

        public static string BadgeDiffClass(this Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => "badge-easy",
                Difficulty.Normal => "badge-normal",
                Difficulty.Hard => "badge-hard",
                Difficulty.Lunatic => "badge-lunatic",
                Difficulty.Extra => "badge-extra",
                Difficulty.Phantasm => "badge-phantasm",
                _ => ""
            };
        }

        public static string BadgeResultClass(ReplayModel? replay)
        {
            if (replay == null) return "badge-empty";

            return GetResultPriority(replay) switch
            {
                8 => "badge-NNNN",
                7 => "badge-NNN",
                6 => "badge-NN",
                5 => "badge-NBplus",
                4 => "badge-NB",
                3 => "badge-NN",
                2 => "badge-1CC",

                _ => "badge-empty"
            };
        }

        public static string DifficultyButtonClass(this Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => "btn-diff-easy",
                Difficulty.Normal => "btn-diff-normal",
                Difficulty.Hard => "btn-diff-hard",
                Difficulty.Lunatic => "btn-diff-lunatic",
                Difficulty.Extra => "btn-diff-extra",
                Difficulty.Phantasm => "btn-diff-phantasm",
                _ => ""
            };
        }

        public static string BadgeCategoryClass(this RunCategory category)
        {
            return category switch
            {
                RunCategory.Survival => "badge-survival",
                RunCategory.Scoring => "badge-scoring",
                _ => ""
            };
        }

        public static string BadgeGameClass(this TouhouGame game)
        {
            return game switch
            {
                TouhouGame.HRtP => "badge-HRtP",
                TouhouGame.SoEW => "badge-SoEW",
                TouhouGame.PoDD => "badge-PoDD",
                TouhouGame.LLS => "badge-LLS",
                TouhouGame.MS => "badge-MS",
                TouhouGame.EoSD => "badge-EoSD",
                TouhouGame.PCB => "badge-PCB",
                TouhouGame.IN => "badge-IN",
                TouhouGame.PoFV => "badge-PoFV",
                TouhouGame.MoF => "badge-MoF",
                TouhouGame.SA => "badge-SA",
                TouhouGame.UFO => "badge-UFO",
                TouhouGame.GFW => "badge-GFW",
                TouhouGame.TD => "badge-TD",
                TouhouGame.DDC => "badge-DDC",
                TouhouGame.LoLK => "badge-LoLK",
                TouhouGame.HSiFS => "badge-HSiFS",
                TouhouGame.WBaWC => "badge-WBaWC",
                TouhouGame.UM => "badge-UM",
                TouhouGame.UDoALG => "badge-UDoALG",
                TouhouGame.FW => "badge-FW",
                _ => ""
            };
        }

        public static string GameTextClass(this TouhouGame game)
        {
            return game switch
            {
                TouhouGame.HRtP => "東方靈異伝 - The Highly Responsive to Prayers",
                TouhouGame.SoEW => "東方封魔録 - Story of Eastern Wonderland",
                TouhouGame.PoDD => "東方夢時空 - The Phantasmagoria of Dim. Dream",
                TouhouGame.LLS => "東方幻想郷 - Lotus Land Story",
                TouhouGame.MS => "東方怪綺談 - Mystic Square",
                TouhouGame.EoSD => "東方紅魔郷 - Embodiment of Scarlet Devil",
                TouhouGame.PCB => "東方妖々夢 - Perfect Cherry Blossom",
                TouhouGame.IN => "東方永夜抄 - Imperishable Night",
                TouhouGame.PoFV => "東方花映塚 - Phantasmagoria of Flower View",
                TouhouGame.MoF => "東方風神録 - Mountain of Faith",
                TouhouGame.SA => "東方地霊殿 - Subterranean Animism",
                TouhouGame.UFO => "東方星蓮船 - Undefined Fantastic Object",
                TouhouGame.GFW => "東方三月精 - Great Fairy Wars",
                TouhouGame.TD => "東方神霊廟 - Ten Desires",
                TouhouGame.DDC => "東方輝針城 - Double Dealing Character",
                TouhouGame.LoLK => "東方紺珠伝 - Legacy of Lunatic Kingdom",
                TouhouGame.HSiFS => "東方天空璋 - Hidden Star in Four Seasons",
                TouhouGame.WBaWC => "東方鬼形獣 - Wily Beast and Weakest Creature",
                TouhouGame.UM => "東方虹龍洞 - Unconnected Marketeers",
                TouhouGame.UDoALG => "東方獣王園 - Unfinished Dream of All Living Ghost",
                TouhouGame.FW => "東方錦上京 - Fossilized Wonders",
                _ => ""
            };

        }

        public static string GameIconTextClass(this TouhouGame game)
        {
            return game switch
            {
                TouhouGame.HRtP => "Touhou 1",
                TouhouGame.SoEW => "Touhou 2",
                TouhouGame.PoDD => "Touhou 3",
                TouhouGame.LLS => "Touhou 4",
                TouhouGame.MS => "Touhou 5",
                TouhouGame.EoSD => "Touhou 6",
                TouhouGame.PCB => "Touhou 7",
                TouhouGame.IN => "Touhou 8",
                TouhouGame.PoFV => "Touhou 9",
                TouhouGame.MoF => "Touhou 10",
                TouhouGame.SA => "Touhou 11",
                TouhouGame.UFO => "Touhou 12",
                TouhouGame.GFW => "Touhou 12.8",
                TouhouGame.TD => "Touhou 13",
                TouhouGame.DDC => "Touhou 14",
                TouhouGame.LoLK => "Touhou 15",
                TouhouGame.HSiFS => "Touhou 16",
                TouhouGame.WBaWC => "Touhou 17",
                TouhouGame.UM => "Touhou 18",
                TouhouGame.UDoALG => "Touhou 19",
                TouhouGame.FW => "Touhou 20",
                _ => ""
            };

        }
    }
}