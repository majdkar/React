using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Contracts;

using SchoolV01.Domain.Entities.ExtendedAttributes;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Domain.Entities.Misc;

using SchoolV01.Domain.Models.Chat;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SchoolV01.Shared.Constants.Permission.Permissions;

namespace SchoolV01.Infrastructure.Contexts
{
    public class BlazorHeroContext(DbContextOptions<BlazorHeroContext> options, ICurrentUserService currentUserService) : AuditableContext(options)
    {
        private readonly DbContextOptions<BlazorHeroContext> _options = options;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public DbSet<ChatHistory<BlazorHeroUser>> ChatHistories { get; set; }
       
       
       



       


        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }



        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockCategory> BlockCategory { get; set; }
        public DbSet<BlockPhoto> BlockPhotos { get; set; }
        public DbSet<BlockAttachement> BlockAttachments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategory { get; set; }
        public DbSet<EventPhoto> EventPhotos { get; set; }
        public DbSet<EventAttachement> EventAttachments { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PagePhoto> PagePhotos { get; set; }
        public DbSet<PageAttachement> PageAttachments { get; set; }

     



        // General Settings
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {


            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTimeGlobally.Now;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTimeGlobally.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Deleted;
                        entry.Entity.LastModifiedOn = DateTimeGlobally.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.Deleted = true;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var mutableEntityType in builder.Model.GetEntityTypes().Where(t => t.ClrType.BaseType.Name.Contains("AuditableEntity")))
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod([mutableEntityType.ClrType, mutableEntityType.GetProperty("Id").ClrType]);
                method.Invoke(this, [builder]);
            }


            builder.Entity<BlazorHeroUser>().HasQueryFilter(e => !e.Deleted);
            builder.Entity<BlazorHeroRole>().HasQueryFilter(e => !e.Deleted);
            builder.Entity<BlazorHeroRoleClaim>().HasQueryFilter(e => !e.Deleted);

            base.OnModelCreating(builder);

            builder.Entity<ChatHistory<BlazorHeroUser>>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<BlazorHeroUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<BlazorHeroRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");

                entity.HasMany(d => d.RoleClaims)
                .WithOne(d => d.Role)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");

            });

            builder.Entity<BlazorHeroRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });

    






        }

    }

}