using SchoolV01.Application.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using WkHtmlToPdfDotNet;
using System.Runtime.Loader;
using System.Reflection;

namespace SchoolV01.Infrastructure.Services
{
    public class HtmlToPDFService : IHtmlToPDFService
    {
        BasicConverter pdfconverter;
        public HtmlToPDFService()
        {
            //CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary("./runtimes/win-x64/native/wkhtmltox.dll");

            pdfconverter = new SynchronizedConverter(new PdfTools());

        }



        public async Task ConvertDoc(string FileName, string HtmlContent)
        {

            try
            {
                HtmlToPdfDocument doc = new HtmlToPdfDocument()
                {

                    GlobalSettings = {
                     ColorMode = ColorMode.Color,
                     Orientation = Orientation.Portrait,
                PaperSize = new PechkinPaperSize("1245px", "1755px" ),
                        Margins=new MarginSettings() { Top = 0, Left = 0, Right = 0, Bottom=0},
        Out = FileName,
        DPI = 150,
    },
                    Objects = {
        new ObjectSettings() {
            PagesCount = true,
            HtmlContent = HtmlContent,
            WebSettings = {DefaultEncoding = "UTF-8", LoadImages = true },
            //HeaderSettings = { FontSize = 8, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },



            //FooterSettings = { FontSize = 8, Right = "Page [page] of [toPage]", Line = false, Spacing = 2.812 }
        }
    }
                };



                pdfconverter.Convert(doc);
                await Task.Delay(3000);

            }
            catch (Exception e)
            {

            }






        }


    }


    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}