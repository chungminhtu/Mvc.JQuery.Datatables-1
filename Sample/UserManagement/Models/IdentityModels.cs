﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Framework.OptionsModel;

namespace UserManagement.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public override bool TwoFactorEnabled { get; set; } = false;
        public override bool EmailConfirmed { get; set; } = true;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public override DateTimeOffset? LockoutEnd { get; set; }
        [Required]
        [Display(Description = "Email adress is used for login")]
        public override string Email { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public bool AllMigrationsApplied()
        {
            return !((IAccessor<IMigrator>)Database.AsRelational()).Service.GetUnappliedMigrations().Any();
        }
    }
    public enum Roles
    {
        Admin,
        Patient,
        Practitioner,
        Secretary,
    }
}