using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thrucommunity.Models
{
    public class ReplayModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Это обязательно поле")]
        public string Nickname { get; set; } = "";

        public string? Comment { get; set; } = "";

        [Required(ErrorMessage = "Это обязательно поле")]
        public TouhouGame Game { get; set; }

        [Required(ErrorMessage = "Это обязательно поле")]
        public string ShotType { get; set; } = "";

        [Required]
        public RunCategory Category { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        public long? Score { get; set; }

        public int? DeathCount { get; set; }

        public bool NoMiss { get; set; }

        public bool NoBomb { get; set; }

        public bool NoThirdCondition { get; set; }

        public bool No4thCondition { get; set; }

        public string? INFinal { get; set; } = "";

        public string ReplayFileName { get; set; } = "";

        public string ReplayFilePath { get; set; } = "";

        [NotMapped]
        public IFormFile? ReplayFile { get; set; } = null!;

        public string? ReplayLink { get; set; } = "";

        public DateTime? ReplayDate { get; set; }

        public SubmissionStatuses SubmissionStatus { get; set; }

        public bool Proven { get; set; }

        public DateTime SubmittedAtUtc { get; set; }

        public string TypeOfSurvival { get; set; } = "";
    }

    public enum RunCategory
    {
        Survival,
        Scoring
    }

    public enum TouhouGame
    {
        HRtP,
        SoEW,
        PoDD,
        LLS,
        MS,
        EoSD,
        PCB,
        IN,
        PoFV,
        MoF,
        SA,
        UFO,
        GFW,
        TD,
        DDC,
        LoLK,
        HSiFS,
        WBaWC,
        UM,
        UDoALG,
        FW
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Lunatic,
        Extra,
        Phantasm
    }

    public enum SubmissionStatuses
    {
        Pending,
        Approved,
        Rejected
    }

}
