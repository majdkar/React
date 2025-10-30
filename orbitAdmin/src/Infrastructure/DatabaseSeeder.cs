
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Infrastructure.Contexts;
using SchoolV01.Shared.Constants.Permission;
using SchoolV01.Shared.Constants.Role;
using SchoolV01.Shared.Constants.User;
using SchoolV01.Infrastructure.Helpers;
using System.IO;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Collections.Generic;
using System.Text.Json;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SchoolV01.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly IStringLocalizer<DatabaseSeeder> _localizer;
        private readonly BlazorHeroContext _db;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly RoleManager<BlazorHeroRole> _roleManager;
        private readonly string JsonFileName = Path.Combine("file", "Countries.json");
        private readonly IUnitOfWork<int> _unitOfWork;

        public DatabaseSeeder(
            UserManager<BlazorHeroUser> userManager,
            IUnitOfWork<int> unitOfWork,
            RoleManager<BlazorHeroRole> roleManager,
            BlazorHeroContext db,
            ILogger<DatabaseSeeder> logger,
            IStringLocalizer<DatabaseSeeder> localizer)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            _localizer = localizer;
        }

        public void Initialize()
        {
            AddRoles();

            AddAdministrator();
            AddBasicUser();
            AddData();

            _db.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new BlazorHeroRole(RoleConstants.AdministratorRole, _localizer["Administrator role with full permissions"]);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                    _logger.LogInformation(_localizer["Seeded Administrator Role."]);
                }
                //Check if User Exists
                var superUser = new BlazorHeroUser
                {
                    FirstName = "admin",
                    LastName = "Master",
                    Email = UserConstants.DefaultEmail,
                    UserName = "admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default SuperAdmin User."]);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
                foreach (var permission in Permissions.GetRegisteredPermissions())
                {
                    await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new BlazorHeroRole(RoleConstants.BasicRole, _localizer["Basic role with default permissions"]);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    _logger.LogInformation(_localizer["Seeded Basic Role."]);
                }
                //Check if User Exists
                var basicUser = new BlazorHeroUser
                {
                    FirstName = "basic",
                    LastName = "user",
                    Email = UserConstants.BasicUserEmail,
                    UserName = "basic",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstants.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                    _logger.LogInformation(_localizer["Seeded User with Basic Role."]);
                }
            }).GetAwaiter().GetResult();
        }


        private void AddData()
        {
            Task.Run(async () =>
            {
                var countryInDb = await _unitOfWork.Repository<Country>().Entities.AnyAsync();
                if (!countryInDb)
                {
                    using var jsonFileReader = File.OpenText(JsonFileName);
                    JsonSerializerOptions options = new()
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var countries = JsonSerializer.Deserialize<List<Country>>(jsonFileReader.ReadToEnd(),
                        options);

                    await _unitOfWork.Repository<Country>().AddRangeAsync(countries);
                }
            }).GetAwaiter().GetResult();


        }


        private void AddRoles()
        {
            Task.Run(async () =>
            {
                var adminRoleInDb = await _roleManager.Roles.ToListAsync();
                //if (!adminRoleInDb.Any())
                {
                    foreach (var item in RoleConstants.RoleList)
                    {
                        if (!adminRoleInDb.Any(role => role.Name == item))
                        {
                            var role = new BlazorHeroRole(item, item);
                            await _roleManager.CreateAsync(role);
                        }
                    }
                }

            }).GetAwaiter().GetResult();
        }
    }
}