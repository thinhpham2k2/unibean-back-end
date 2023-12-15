namespace Unibean.Service.Models.Levels;

public class LevelModel
{
    public string Id { get; set; }
    public string LevelName { get; set; }
    public decimal? Condition { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
