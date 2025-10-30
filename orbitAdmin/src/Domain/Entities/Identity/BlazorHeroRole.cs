using System;
using System.Collections.Generic;
using SchoolV01.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace SchoolV01.Domain.Entities.Identity
{
    public class BlazorHeroRole : IdentityRole, IAuditableEntity<string>
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public ICollection<BlazorHeroRoleClaim> RoleClaims { get; set; }
        public bool Deleted { get; set; } = false;

        public BlazorHeroRole() : base()
        {
            RoleClaims = new HashSet<BlazorHeroRoleClaim>();
        }

        public BlazorHeroRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<BlazorHeroRoleClaim>();
            Description = roleDescription;
        }
    }
}