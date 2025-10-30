using System.Collections.Generic;
using System.IO;

namespace SchoolV01.Shared.Constants
{
    public static class Constants
    {
        public static int MaxFileSizeInByte = 1024 * 1024 * 10;

        public static string StudentProfile = "media/Student.png";
        public static string EmployeeProfile = "media/Student.png";
        public static string UploadFolderName = "Files/UploadFiles";

        public static string NOImagePath = Path.Combine(UploadFolderName, Enums.FileLocation.SharedFiles.ToString(), "noimage.png");
        public static string ReadyToUploadPath = Path.Combine(UploadFolderName, Enums.FileLocation.SharedFiles.ToString(), "rtu.jpg");

      //  public static string TinyMceApiKey = "ny9nwih80blxwqrhsvo6ygd2dwnkkob5xf6jivyfohxbnple";//"byezhy145g3h9nmr481i445hjv3yu3jhl1fxfune52rmdkps";

        public static Dictionary<string, object> editorConf = new Dictionary<string, object>{
           {"toolbar", "undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | styles |  alignleft aligncenter alignright alignjustify | ltr rtl | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak  | charmap emoticons | fullscreen  preview save print  | a11ycheck ltr rtl | insertfile image media pageembed template link anchor codesample | link image | showcomments addcomment | help"},
            { "height",  300},
            {"plugins", "advlist, autolink, link, image, lists, charmap, preview, anchor, pagebreak, searchreplace, wordcount, visualblocks, code, fullscreen, insertdatetime, media, table, emoticons, template, help"},
            { "menubar", "favs file edit view insert format tools table help"},
            { "menu", "{favs: { title: 'My Favorites', items: 'code visualaid | searchreplace | emoticons' } }"} };


        public static string AdminstrativeEmployee = "NoAcademic";


    }
    public class TextEditorConfig
    {
        public Dictionary<string, object> tinymc { get; set; }

        public TextEditorConfig(string _selector)
        {
            tinymc = new Dictionary<string, object>{
            {"toolbar", "undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | styles |  alignleft aligncenter alignright alignjustify | ltr rtl | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak  | charmap emoticons | fullscreen  preview save print  | a11ycheck ltr rtl | insertfile image media pageembed template link anchor codesample | link image | showcomments addcomment | help"},
            { "selector", _selector},
            { "height",  300},
            {"plugins", "advlist, autolink, link, image, lists, charmap, preview, anchor, pagebreak, searchreplace, wordcount, visualblocks, code, fullscreen, insertdatetime, media, table, emoticons, template, help"},
            { "menubar", "favs file edit view insert format tools table help"},
            { "menu", "{favs: { title: 'My Favorites', items: 'code visualaid | searchreplace | emoticons' } }"}
        };
        }

    }
}
