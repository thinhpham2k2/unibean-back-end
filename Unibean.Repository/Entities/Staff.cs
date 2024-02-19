using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_staff")]
public class Staff
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("station_id", TypeName = "char(26)")]
    public string StationId { get; set; }

    public Station Station { get; set; }

    [Column("account_id", TypeName = "char(26)")]
    public string AccountId { get; set; }

    public Account Account { get; set; }

    [MaxLength(255)]
    [Column("full_name")]
    public string FullName { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
