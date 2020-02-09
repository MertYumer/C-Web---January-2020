namespace MUSACA.Data
{
    using Microsoft.EntityFrameworkCore;
    using MUSACA.Models;

    public class MusacaDbContext : DbContext
    {
        public MusacaDbContext() : base()
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(order => order.Products)
                .WithOne(orderProduct => orderProduct.Order)
                .HasForeignKey(orderProduct => orderProduct.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(orderProduct => new { orderProduct.OrderId, orderProduct.ProductId });


            base.OnModelCreating(modelBuilder);
        }
    }
}
