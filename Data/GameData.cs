using thrucommunity.Models;

namespace thrucommunity.Data
{
    public static class GameData
    {

        public static readonly Dictionary<TouhouGame, List<string>> ShotTypes =
            new()
            {
                [TouhouGame.HRtP] = new()
                {
                    "Jigoku",
                    "Makai"
                },

                [TouhouGame.SoEW] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Reimu C"
                },

                [TouhouGame.PoDD] = new()
                {
                    "Reimu",
                    "Mima",
                    "Marisa",
                    "Ellen",
                    "Kotohime",
                    "Kana",
                    "Rikako",
                    "Chiyuri",
                    "Yumemi"
                },

                [TouhouGame.LLS] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Marisa A",
                    "Marisa B"
                },

                [TouhouGame.MS] = new()
                {
                    "Reimu",
                    "Marisa",
                    "Mima",
                    "Yuka"
                },

                [TouhouGame.EoSD] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Marisa A",
                    "Marisa B"
                },

                [TouhouGame.PCB] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Marisa A",
                    "Marisa B",
                    "Sakuya A",
                    "Sakuya B"
                },

                [TouhouGame.IN] = new()
                {
                    "Border Team",
                    "Magic Team",
                    "Scarlet Team",
                    "Ghost Team",
                    "Reimu",
                    "Yukari",
                    "Marisa",
                    "Alice",
                    "Sakuya",
                    "Remilia",
                    "Youmu",
                    "Yuyuko",
                },

                [TouhouGame.PoFV] = new()
                {
                    "Reimu",
                    "Marisa",
                    "Sakuya",
                    "Youmu",
                    "Reisen",
                    "Cirno",
                    "Lyrica",
                    "Mystia",
                    "Tewi",
                    "Aya",
                    "Medicine",
                    "Yuka",
                    "Komachi",
                    "Eiki"
                },

                [TouhouGame.MoF] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Reimu C",
                    "Marisa A",
                    "Marisa B",
                    "Marisa C"
                },

                [TouhouGame.SA] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Reimu C",
                    "Marisa A",
                    "Marisa B",
                    "Marisa C"
                },

                [TouhouGame.UFO] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Marisa A",
                    "Marisa B",
                    "Sanae A",
                    "Sanae B"
                },

                [TouhouGame.GFW] = new()
                {
                    "A1",
                    "A2",
                    "B1",
                    "B2",
                    "C1",
                    "C2"
                },

                [TouhouGame.TD] = new()
                {
                    "Reimu",
                    "Marisa",
                    "Youmu",
                    "Sanae"
                },

                [TouhouGame.DDC] = new()
                {
                    "Reimu A",
                    "Reimu B",
                    "Marisa A",
                    "Marisa B",
                    "Sakuya A",
                    "Sakuya B"
                },

                [TouhouGame.LoLK] = new()
                {
                    "Reimu",
                    "Marisa",
                    "Reisen",
                    "Sanae"
                },

                [TouhouGame.HSiFS] = new()
                {
                    "ReimuSpring",
                    "ReimuSummer",
                    "ReimuWinter",
                    "ReimuAutumn",

                    "MarisaSpring",
                    "MarisaSummer",
                    "MarisaWinter",
                    "MarisaAutumn",

                    "CirnoSpring",
                    "CirnoSummer",
                    "CirnoWinter",
                    "CirnoAutumn",

                    "AyaSpring",
                    "AyaSummer",
                    "AyaWinter",
                    "AyaAutumn"
                },

                [TouhouGame.WBaWC] = new()
                {
                    "ReimuWolf",
                    "ReimuOtter",
                    "ReimuEagle",

                    "MarisaWolf",
                    "MarisaOtter",
                    "MarisaEagle",

                    "YoumuWolf",
                    "YoumuOtter",
                    "YoumuEagle"
                },

                [TouhouGame.UM] = new()
                {
                    "Reimu",
                    "Marisa",
                    "Sakuya",
                    "Sanae"
                },

                [TouhouGame.UDoALG] = new()
                {
                    "Reimu",
                    "Marisa",
                    "Sanae",
                    "Ran",
                    "Auun",
                    "Nazrin",
                    "Seiran",
                    "Rin",
                    "Tsukasa",
                    "Mamizou",
                    "Yachie",
                    "Saki",
                    "Yuuma",
                    "Suika",
                    "Son Biten",
                    "Enoko",
                    "Chiyari",
                    "Hisami",
                    "Zanmu"
                },

                [TouhouGame.FW] = new()
                {
                    "ReimuR1",
                    "ReimuR2",
                    "ReimuB1",
                    "ReimuB2",
                    "ReimuY1",
                    "ReimuY2",
                    "ReimuG1",
                    "ReimuG2",

                    "MarisaR1",
                    "MarisaR2",
                    "MarisaB1",
                    "MarisaB2",
                    "MarisaY1",
                    "MarisaY2",
                    "MarisaG1",
                    "MarisaG2"
                }
            };
        public static readonly Dictionary<TouhouGame, List<Difficulty>> Difficulties =
            new()
            {
                {
                    TouhouGame.HRtP,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic
                    }
                },

                {
                    TouhouGame.PoDD,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic
                    }
                },

                {
                    TouhouGame.UDoALG,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic
                    }
                },

                {
                    TouhouGame.PCB,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra,
                        Difficulty.Phantasm
                    }
                },

                {
                    TouhouGame.SoEW,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.LLS,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.MS,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.EoSD,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.IN,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.PoFV,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.MoF,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.SA,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.UFO,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.GFW,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.TD,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.DDC,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.LoLK,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.HSiFS,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.WBaWC,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.UM,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                },

                {
                    TouhouGame.FW,
                    new()
                    {
                        Difficulty.Easy,
                        Difficulty.Normal,
                        Difficulty.Hard,
                        Difficulty.Lunatic,
                        Difficulty.Extra
                    }
                }
            };

        public static readonly Dictionary<TouhouGame, List<string>> INFinals = new()
        {
            {
                TouhouGame.IN,
                new()
                {
                    "Final A",
                    "Final B"
                }
            }
        };

        public static readonly Dictionary<TouhouGame, List<(string Character, List<string> Shots)>> ShotGroups =
            new()
            {
                [TouhouGame.HSiFS] = new()
                {
                    ("Reimu", new()
                    {
                        "ReimuSpring",
                        "ReimuSummer",
                        "ReimuAutumn",
                        "ReimuWinter"
                    }),
                    ("Marisa", new()
                    {
                        "MarisaSpring",
                        "MarisaSummer",
                        "MarisaAutumn",
                        "MarisaWinter"
                    }),
                    ("Cirno", new()
                    {
                        "CirnoSpring",
                        "CirnoSummer",
                        "CirnoAutumn",
                        "CirnoWinter"
                    }),
                    ("Aya", new()
                    {
                        "AyaSpring",
                        "AyaSummer",
                        "AyaAutumn",
                        "AyaWinter"
                    })

                }

            };

    }

}

