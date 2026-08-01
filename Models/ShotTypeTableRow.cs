namespace thrucommunity.Models
{
    public class ShotTypeTableRow
    {
        public string ShotType { get; set; } = "";

        public string? INFinal { get; set; }
        public int PlayersCount { get; set; }
        public List<PlayerMiniViewModel> Players { get; set; } = new();
    }
}
