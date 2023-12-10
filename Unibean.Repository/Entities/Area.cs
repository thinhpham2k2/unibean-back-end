﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

[Table("tbl_area")]
public class Area
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("district_id", TypeName = "char(26)")]
    public string DistrictId { get; set; }

    public District District { get; set; }

    [MaxLength(255)]
    [Column("area_name")]
    public string AreaName { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

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

    public virtual ICollection<Campus> Campuses { get; set; }

    //public virtual ICollection<Campus> Campuses { get; set; }
}
