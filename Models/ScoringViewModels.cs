namespace thrucommunity.Models
{
    public class ScoringGameViewModel
    {
        public TouhouGame Game { get; set; }

        public List<ScoringDifficultyViewModel> Difficulties { get; set; }
            = new();
    }

    public class ScoringDifficultyViewModel
    {
        public Difficulty Difficulty { get; set; }

        public List<ScoringShotTypeViewModel> ShotTypes { get; set; }
            = new();
    }

    public class ScoringShotTypeViewModel
    {
        public string ShotType { get; set; } = "";

        public List<ScoringReplayViewModel> Replays { get; set; }
            = new();
    }

    public class ScoringReplayViewModel
    {
        public int ReplayId { get; set; }

        public string Nickname { get; set; } = "";

        public long Score { get; set; }

        public DateTime ReplayDate { get; set; }

        public bool Proven { get; set; }
    }
}
