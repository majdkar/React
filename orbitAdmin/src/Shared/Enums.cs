using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared
{
    public static class Enums
    {
        public enum FileLocation
        {
            BlocksFiles = 1,
            MenusFiles = 2,
            PagesFiles = 3,
            EventsFiles = 4,
            ArticlesFiles = 5,
            SharedFiles = 6,
            ProjectsFiles = 7,
            TasksFiles = 8,
            ProductsFiles = 9,
        }

        public static Dictionary<string, string> UploadFileTypeValues = new Dictionary<string, string>
        {
            {"Image", "Image (jpg, bmp, png,jpeg)" },
            {"File", "File (txt, docx, pdf,xlsx)" },
            {"Brochure", "File (txt, docx, pdf,xlsx)" },
            {"Video", "Video (mp4, avi, mpeg)" },
            {"Chart", "Image (jpg, bmp, png,jpeg)" },

        };

        public enum UploadFileTypeEnum
        {
            Image = 1,
            File = 2,
            Video = 3,
            Chart = 4,
            Brochure = 5,
            
        }
    }

    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return value.ToString(); // Return enum name if no description is found
        }
    }
}
