using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

[Table("tbl_station")]
public class Station
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [MaxLength(255)]
    [Column("station_name")]
    public string StationName { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

    [Column("file_name", TypeName = "text")]
    public string FileName { get; set; }

    [Column("opening_hours")]
    public TimeOnly? OpeningHours { get; set; }

    [Column("closing_hours")]
    public TimeOnly? ClosingHours { get; set; }

    [Column("phone", TypeName = "char(20)")]
    public string Phone { get; set; }

    [EmailAddress]
    [MaxLength(320)]
    [Column("email")]
    public string Email { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
