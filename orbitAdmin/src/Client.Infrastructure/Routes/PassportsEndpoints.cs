using System;
namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class PassportsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/passports/export";

        public static string GetAll = "api/v1/passports";
        public static string Delete = "api/v1/passports";
        public static string Save = "api/v1/passports";
        public static string GetCount = "api/v1/passports/count";
    }
}