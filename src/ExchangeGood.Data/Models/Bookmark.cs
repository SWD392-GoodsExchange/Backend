﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ExchangeGood.Data.Models
{
    public partial class Bookmark
    {
        public int ProductId { get; set; }
        public string FeId { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Member Fe { get; set; }
        public virtual Product Product { get; set; }
    }
}