﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ExchangeGood.Data.Models
{
    public partial class Product
    {
        public Product()
        {
            Bookmarks = new HashSet<Bookmark>();
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
            OrderDetails = new HashSet<OrderDetail>();
            Reports = new HashSet<Report>();
        }

        public int ProductId { get; set; }
        public string FeId { get; set; }
        public int CateId { get; set; }
        public string UsageInformation { get; set; }
        public string Origin { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }

        public virtual Category Cate { get; set; }
        public virtual Member Fe { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}