namespace Unibean.Service.Models.Images;

public class ImageModel
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string Url { get; set; }
    public bool? IsCover { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
