namespace thrucommunity.Models
{
    public class PlayerProfileViewModel
    {
        public PlayerModel? Player { get; set; }

        public List<ReplayModel> RecentReplays { get; set; } = new();

        public List<ReplayModel> AllReplays { get; set; } = new();

        public Dictionary<TouhouGame, Dictionary<Difficulty, BestResultsViewModel>> BestSurvivalResults { get; set; } = new();

        public Dictionary<TouhouGame, Dictionary<Difficulty, BestResultsViewModel>> BestScoringResults { get; set; } = new();

        public Dictionary<TouhouGame, Dictionary<Difficulty, Dictionary<string, BestResultsViewModel>>> SurvivalTables { get; set; } = new();

        public Dictionary<TouhouGame, Dictionary<Difficulty, Dictionary<string, BestResultsViewModel>>> ScoringTables { get; set; } = new();

        public bool ShowUnproven { get; set; }
    }
}
