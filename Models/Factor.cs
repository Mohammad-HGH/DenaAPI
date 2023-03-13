﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DenaAPI.Models;

[Table("Factor")]
public partial class Factor
{
    [Key]
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? AttId { get; set; }

    public int? Number { get; set; }

    public int? Collect { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreateDate { get; set; }

    public int? PostId { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Factors")]
    public virtual PostDetail Post { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Factors")]
    public virtual Product Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Factors")]
    public virtual User User { get; set; }
}