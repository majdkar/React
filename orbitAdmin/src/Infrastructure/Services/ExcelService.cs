using SchoolV01.Application.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.IO;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.Globalization;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Shared.Constants.Role;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Infrastructure.Contexts;

namespace SchoolV01.Infrastructure.Services
{
    public class ExcelService(IStringLocalizer<ExcelService> localizer, BlazorHeroContext context) : IExcelService
    {
        private readonly IStringLocalizer<ExcelService> _localizer = localizer;
        private readonly BlazorHeroContext _context = context;

        public async Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "BlazorHero";
            p.Workbook.Worksheets.Add(_localizer["Audit Trails"]);
            var ws = p.Workbook.Worksheets[0];
            ws.View.RightToLeft = CultureInfo.CurrentCulture.Name.Contains("ar");
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            var dataList = data.ToList();
            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            var byteArray = await p.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }

        public async Task<string> ExportMarksTemplateAsync<TData>(
            IEnumerable<TData> data,
            Dictionary<string, Func<TData, object>> mappers,
            string sheetName = "Sheet1",
            string headerInfo = "",
            List<string> columnGroups = null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            var ws = p.Workbook.Worksheets.Add(sheetName);
            //ws.View.RightToLeft = true;

            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            // First Header Row (Merged)
            ws.Cells[1, 1].Value = headerInfo;
            ws.Cells[1, 1, 1, mappers.Count / 2].Merge = true;

            ws.Cells[1, (mappers.Count / 2) + 1].Value = headerInfo;
            ws.Cells[1, (mappers.Count / 2) + 1, 1, mappers.Count].Merge = true;

            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, (mappers.Count / 2) + 1].Style.Font.Bold = true;
            // Second Header Row (Group Categories if provided)

            if (columnGroups != null)
            {
                int groupColIndex = 3;
                foreach (var group in columnGroups)
                {
                    ws.Cells[2, groupColIndex].Value = group;
                    ws.Cells[2, groupColIndex, 2, groupColIndex + (mappers.Count - 2) / 2 - 1].Merge = true;
                    ws.Cells[2, groupColIndex].Style.Font.Bold = true;
                    ws.Cells[2, groupColIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[2, groupColIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[2, groupColIndex].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    groupColIndex += (mappers.Count - 2) / 2;
                }
            }

            // Third Header Row (Column Names)
            var colIndex = 1;
            foreach (var header in mappers.Keys)
            {
                ws.Cells[3, colIndex].Value = header;
                ws.Cells[3, colIndex].Style.Font.Bold = true;
                ws.Cells[3, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                if (colIndex != 2)
                {
                    ws.Cells[3, colIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[3, colIndex].Style.TextRotation = 90;
                }

                colIndex++;
            }

            // Populate Data
            var rowIndex = 4;
            foreach (var item in data)
            {
                colIndex = 1;
                foreach (var header in mappers.Keys)
                {
                    ws.Cells[rowIndex, colIndex].Value = mappers[header](item);
                    colIndex++;
                }
                rowIndex++;
            }

            int rowIndexToFit = 3; // Change this to the desired row number
            // Let Excel auto-adjust the height for the row
            ws.Row(rowIndexToFit).CustomHeight = false;

            // AutoFit Columns and Finalize
            ws.Cells.AutoFitColumns();

            var byteArray = await p.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }



        public byte[] GenerateFileFromXslt(string xmlData, string xsltPath)
        {
            // Load the XSLT file
            var xslt = new XslCompiledTransform();
            xslt.Load(xsltPath);

            // Prepare an XML reader for the input XML data
            using var xmlReader = XmlReader.Create(new StringReader(xmlData));

            // Transform the XML data using the XSLT and write to a memory stream
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

            xslt.Transform(xmlReader, null, streamWriter);
            streamWriter.Flush();

            return memoryStream.ToArray();
        }

    }
}