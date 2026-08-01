namespace thrucommunity.Models
{
    public class ReplayListViewModel
    {
        public List<ReplayModel> Replays { get; set; } = new();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }
}
