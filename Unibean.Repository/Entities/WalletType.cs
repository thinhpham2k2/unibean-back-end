using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_wallet_type")]
public class WalletType
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [MaxLength(255)]
    [Column("type_name")]
    public string TypeName { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

    [Column("file_name", TypeName = "text")]
    public string FileName { get; set; }

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

    public virtual ICollection<Wallet> Wallets { get; set; }
}
