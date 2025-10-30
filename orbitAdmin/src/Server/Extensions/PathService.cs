using SchoolV01.Application.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SchoolV01.Application.GeneralInterfaces;
using Microsoft.AspNetCore.Hosting;

namespace SchoolV01.Server.Extensions
{
    public class PathService : IPathService
    {
        private readonly IWebHostEnvironment _env;

        public PathService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetTemplatePath(string templateName)
        {
            return Path.Combine(_env.ContentRootPath, "Templates", templateName);
        }
    }

}