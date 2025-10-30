using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SchoolV01.Shared.Constants.Role
{
    public static class RoleConstants
    {
        public const string AdministratorRole = "Administrator";
        public const string BasicRole = "Basic";


        public static List<string> RoleList
        {
            get
            {
                return typeof(RoleConstants).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(f => (string)f.GetValue(null))
                .ToList();
            }
        }
    }
}