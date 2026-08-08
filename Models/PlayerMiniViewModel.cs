namespace thrucommunity.Models
{
    public class PlayerMiniViewModel
    {
        public string Nickname { get; set; } = "";
        public int ReplayId { get; set; }
        public int? DeathCount { get; set; }
        public bool NoThirdCondition { get; set; }

        public bool Proven { get; set; }
    }
}

