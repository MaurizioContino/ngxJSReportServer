using Microsoft.EntityFrameworkCore;
using ngxJSReportServer.Model;

namespace ngxJSReportServer.DataAccess
{
    // application DBContext
    public class AppDBContext : DbContext
    {
        public DbSet<TableModel> TableModels { get; set; }
        public DbSet<FieldModel> Fields { get; set; }
        public DbSet<JoinModel> Joins { get; set; }
        public DbSet<QueryModel> Queries { get; set; }
        public DbSet<SQLAuthModel> Connections { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.EnableDetailedErrors(true);
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<TableModel>()
                .HasMany(x => x.Fields)
                .WithOne(l => l.TableModel);

            model.Entity<JoinModel>()
                .HasOne(f1 => f1.f1)
                .WithMany(j => j.JoinsF1);
            
            model.Entity<JoinModel>()
                .HasOne(f2 => f2.f2)
                .WithMany(j => j.JoinsF2);

            model.Entity<QueryModel>()
                .HasMany(f => f.Fields)
                .WithMany(q => q.Queries);

            model.Entity<QueryModel>()
                .HasMany(f => f.SelectedFields)
                .WithMany(q => q.QuerySelection);

            model.Entity<QueryModel>()
                .HasMany(f => f.Joins)
                .WithMany(q => q.Queries);

        }



    }
}
