//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WholesaleBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbService : DbContext
    {
        public DbService()
            : base("name=DbService")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<buyer> buyers { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<manager> managers { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<unit> units { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<sales_invoice> sales_invoice { get; set; }
    }
}
