using System;
using System.Linq;

namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class CitiesEndpoints
    {
        public const string Endpoints = "api/v1/Cities";

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"{Endpoints}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Length != null)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Endpoints}/export?searchString={searchString}";
        }

        public static readonly string GetAll = $"{Endpoints}/GetAll";
    }
}