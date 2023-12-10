﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_payment_transaction")]
public class PaymentTransaction
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("wallet_id", TypeName = "char(26)")]
    public string WalletId { get; set; }

    public Wallet Wallet { get; set; }

    [Column("payment_id", TypeName = "char(26)")]
    public string PaymentId { get; set; }

    public Payment Payment { get; set; }

    [Column("amount", TypeName = "decimal(38,2)")]
    public decimal? Amount { get; set; }

    [Column("rate", TypeName = "decimal(38,2)")]
    public decimal? Rate { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? States { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
