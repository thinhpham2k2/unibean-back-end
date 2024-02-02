using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

public enum CampaignState
{
    Pending = 1, Active = 2, Inactive = 3, Expired = 4, Closed = 5
}

public class CampaignActivity
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("campaign_id", TypeName = "char(26)")]
    public string CampaignId { get; set; }

    public Campaign Campaign { get; set; }

    [Column("state", 
        TypeName = "enum('Pending', 'Active', 'Inactive', 'Expired', 'Closed')")]
    public CampaignState State { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
