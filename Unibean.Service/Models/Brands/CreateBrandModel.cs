using Microsoft.AspNetCore.Http;

namespace Unibean.Service.Models.Brands;

public class CreateBrandModel
{
    public string BrandName { get; set; }
    public string Acronym { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public IFormFile Logo { get; set; }
    public IFormFile CoverPhoto { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Link { get; set; }
    public TimeOnly? OpeningHours { get; set; }
    public TimeOnly? ClosingHours { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
}
