// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DenaAPI.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Phone { get; set; }

    [Required]
    [StringLength(255)]
    public string Password { get; set; }

    [Required]
    [StringLength(255)]
    public string PasswordSalt { get; set; }

    [Required]
    [StringLength(255)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(255)]
    public string LastName { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime TS { get; set; }

    public bool Active { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Factor> Factors { get; } = new List<Factor>();

    [InverseProperty("User")]
    public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

    [InverseProperty("User")]
    public virtual ICollection<Sms> Sms { get; } = new List<Sms>();
}