﻿namespace Unibean.Service.Models.Categories;

public class CategoryModel
{
    public string Id { get; set; }
    public string CategoryName { get; set; }
    public string Image { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
