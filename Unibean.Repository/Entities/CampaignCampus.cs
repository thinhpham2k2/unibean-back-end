using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_campaign_campus")]
public class CampaignCampus
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("campaign_id", TypeName = "char(26)")]
    public string CampaignId { get; set; }

    public Campaign Campaign { get; set; }

    [Column("campus_id", TypeName = "char(26)")]
    public string CampusId { get; set; }

    public Campus Campus { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
