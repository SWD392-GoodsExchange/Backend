﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ExchangeGood.Data.Models;

public partial class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public string FeId { get; set; }

    public string Token { get; set; }

    public DateTime ExpiryDate { get; set; }

    public virtual Member Fe { get; set; }
}