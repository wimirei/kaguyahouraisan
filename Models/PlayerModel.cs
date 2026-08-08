using System.ComponentModel.DataAnnotations;

namespace thrucommunity.Models
{
    public class PlayerModel
    {
        [Key]
        public int Id { get; set; }

        public string Nickname { get; set; } = "";

        public int L1CCcount { get; set; }

        public int LNMcount { get; set; }

        public int LNBcount { get; set; }

        public int LNNcount { get; set; }

        public int LNNNcount { get; set; }

        public int LNBNxcount { get; set; }

        public int ExNNcount { get; set; }

        public int ThirdPlaceCount { get; set; }

        public int SecondPlaceCount { get; set; }

        public int FirstPlaceCount { get; set; }

        public int WRcount { get; set; }

        public int survivalpoints { get; set; }

        public int scoringpoints { get; set; } = 0;

    }
}
