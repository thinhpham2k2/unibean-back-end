using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_campus")]
public class Campus
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("university_id", TypeName = "char(26)")]
    public string UniversityId { get; set; }

    public University University { get; set; }

    [Column("area_id", TypeName = "char(26)")]
    public string AreaId { get; set; }

    public Area Area { get; set; }

    [MaxLength(255)]
    [Column("campus_name")]
    public string CampusName { get; set; }

    [Column("opening_hours")]
    public TimeOnly OpeningHours { get; set; }

    [Column("closing_hours")]
    public TimeOnly ClosingHours { get; set; }

    [Column("Address", TypeName = "text")]
    public string address { get; set; }

    [Column("phone", TypeName = "char(20)")]
    public string Phone { get; set; }

    [EmailAddress]
    [MaxLength(320)]
    [Column("email")]
    public string Email { get; set; }

    [Column("link", TypeName = "text")]
    public string Link { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

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
}
