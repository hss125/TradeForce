﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradeForce.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class tradeforceEntities : DbContext
    {
        public tradeforceEntities()
            : base("name=tradeforceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<classify> classify { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<productproperty> productproperty { get; set; }
        public virtual DbSet<question> question { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<country> country { get; set; }
        public virtual DbSet<news> news { get; set; }
    }
}
