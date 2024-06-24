﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ExchangeGood.Data.Models;

public partial class Member
{
    public string FeId { get; set; }

    public int RoleId { get; set; }

    public string UserName { get; set; }

    public byte[] PasswordSalt { get; set; }

    public byte[] PasswordHash { get; set; }

    public string Address { get; set; }

    public string Gender { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime UpdatedTime { get; set; }

    public string Status { get; set; }

    public DateTime? Dob { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Notification> NotificationRecipients { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationSenders { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    
    public virtual Role Role { get; set; }
}