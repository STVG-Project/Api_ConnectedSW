
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Model
{
    public class SqlDbContex : DbContext
    {

        public DbSet<SqlKhachhang>? KhachHangs { get; set; }
        public DbSet<SqlTuyenDuong>? Tuyenduongs { get; set; }


        public static string configSql = "Host=115.78.230.192:59060;Database=db_water_watch;Username=postgres;Password=stvg";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(configSql);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TuyenDuong>().ToTable("tb_khachhang");
            //base.OnModelCreating(modelBuilder);
           modelBuilder.Entity<SqlTuyenDuong>().HasMany<SqlKhachhang>(s => s.KhachHangs).WithOne(s => s.TuyenDuong);
        }
    }
}
