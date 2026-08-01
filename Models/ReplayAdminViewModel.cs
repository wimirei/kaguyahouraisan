namespace thrucommunity.Models
{
    public class ReplayAdminViewModel
    {
        public List<ReplayModel> Replays { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string? SearchNickname { get; set; }
    }
}
