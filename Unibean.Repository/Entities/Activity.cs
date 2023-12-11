﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_activity")]
public class Activity
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("store_id", TypeName = "char(26)")]
    public string StoreId { get; set; }

    public Store Store { get; set; }

    [Column("student_id", TypeName = "char(26)")]
    public string StudentId { get; set; }

    public Student Student { get; set; }

    [Column("type_id", TypeName = "char(26)")]
    public string TypeId { get; set; }

    public Type Type { get; set; }

    [Column("voucher_item_id", TypeName = "char(26)")]
    public string VoucherItemId { get; set; }

    public VoucherItem VoucherItem { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<ActivityTransaction> ActivityTransactions { get; set; }
}