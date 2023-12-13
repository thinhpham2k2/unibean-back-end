using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_invitation")]
public class Invitation
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [ForeignKey("Inviter")]
    [Column("inviter_id", TypeName = "char(26)")]
    public string InviterId { get; set; }

    [InverseProperty("Inviters")]
    public Student Inviter { get; set; }

    [Column("invitee_id", TypeName = "char(26)")]
    public string InviteeId { get; set; }

    [InverseProperty("Invitees")]
    public Student Invitee { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
