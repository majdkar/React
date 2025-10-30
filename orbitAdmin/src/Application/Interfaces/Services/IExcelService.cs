using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Services
{
    public interface IExcelService
    {
        Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Sheet1");
        byte[] GenerateFileFromXslt(string xmlData, string xsltPath);
    }
}