using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SchoolV01.Shared.Constants.Permission
{
    public static class Permissions
    {
      
      
        public static class Pages
        {
            public const string View = "Permissions.Pages.View";
            public const string Create = "Permissions.Pages.Create";
            public const string Edit = "Permissions.Pages.Edit";
            public const string Delete = "Permissions.Pages.Delete";
        }

       
              
        public static class Cities
        {
            public const string View = "Permissions.Cities.View";
            public const string Create = "Permissions.Cities.Create";
            public const string Edit = "Permissions.Cities.Edit";
            public const string Delete = "Permissions.Cities.Delete";
        }
             
         
 
                  public static class Suggestions
        {
            public const string View = "Permissions.Suggestions.View";
            public const string Create = "Permissions.Suggestions.Create";
            public const string Edit = "Permissions.Suggestions.Edit";
            public const string Delete = "Permissions.Suggestions.Delete";
        }
                            
   
                              
        public static class Countries
        {
            public const string View = "Permissions.Countries.View";
            public const string Create = "Permissions.Countries.Create";
            public const string Edit = "Permissions.Countries.Edit";
            public const string Delete = "Permissions.Countries.Delete";
        }





        public static class Passports
        {
            public const string View = "Permissions.Passports.View";
            public const string Create = "Permissions.Passports.Create";
            public const string Edit = "Permissions.Passports.Edit";
            public const string Delete = "Permissions.Passports.Delete";
        }
        public static class Owners
        {
            public const string View = "Permissions.Owners.View";
            public const string Create = "Permissions.Owners.Create";
            public const string Edit = "Permissions.Owners.Edit";
            public const string Delete = "Permissions.Owners.Delete";
        }
     


        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
        }

        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
        }

        public static class DocumentExtendedAttributes
        {
            public const string View = "Permissions.DocumentExtendedAttributes.View";
            public const string Create = "Permissions.DocumentExtendedAttributes.Create";
            public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
            public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
        }

        public static class Communication
        {
            public const string Chat = "Permissions.Communication.Chat";
            public const string Notification = "Permissions.Communication.Notification";
        }

        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";

            //TODO - add permissions
        }

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        public static class Schema
        {
            public const string View = "Permissions.Schema.View";
        }
        public static class WebSiteManagement
        {
            public const string View = "Permissions.WebSiteManagement.View";
            public const string Create = "Permissions.WebSiteManagement.Create";
            public const string Edit = "Permissions.WebSiteManagement.Edit";
            public const string Delete = "Permissions.WebSiteManagement.Delete";
        }



        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }
        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }



        public static class SuccessRules
        {
            public const string View = "Permissions.SuccessRules.View";
            public const string Create = "Permissions.SuccessRules.Create";
            public const string Delete = "Permissions.SuccessRules.Delete";
        }
    }
}