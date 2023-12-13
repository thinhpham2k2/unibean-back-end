using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_student_challenge")]
public class StudentChallenge
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("challenge_id", TypeName = "char(26)")]
    public string ChallengeId { get; set; }

    public Challenge Challenge { get; set; }

    [Column("student_id", TypeName = "char(26)")]
    public string StudentId { get; set; }

    public Student Student { get; set; }

    [Column("amount", TypeName = "decimal(38,2)")]
    public decimal? Amount { get; set; }

    [Column("current", TypeName = "decimal(38,2)")]
    public decimal? Current { get; set; }

    [Column("condition", TypeName = "decimal(38,2)")]
    public decimal? Condition { get; set; }

    [Column("is_completed", TypeName = "bit(1)")]
    public bool? IsCompleted { get; set; }

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

    public virtual ICollection<ChallengeTransaction> ChallengeTransactions { get; set; }
}
