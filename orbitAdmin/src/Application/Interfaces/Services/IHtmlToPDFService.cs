using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SchoolV01.Application.Interfaces.Services
{
    public interface IHtmlToPDFService
    {
        Task ConvertDoc(string FileName, string HtmlContent);

    }
}