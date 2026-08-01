namespace thrucommunity.Models
{
    public class GameTableViewModel
    {
        public TouhouGame Game { get; set; }


        public List<ShotTypeTableRow> Records { get; set; } = new();

        public List<ShotTypeTableRow> PhantasmRecords { get; set; } = new();
    }
}
