using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using thrucommunity.Models;

namespace thrucommunity.Models
{
    public class PlayerProfileViewModel
    {
        public PlayerModel? Player { get; set; }

        public List<ReplayModel> RecentReplays { get; set; } = new();

        public List<ReplayModel> AllReplays { get; set; } = new();

        public Dictionary<TouhouGame, Dictionary<Difficulty, BestResultsViewModel>> BestResults { get; set; } = new();
    }
}
