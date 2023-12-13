﻿namespace Unibean.Service.Models.Admins;

public class AdminModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Avatar { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
