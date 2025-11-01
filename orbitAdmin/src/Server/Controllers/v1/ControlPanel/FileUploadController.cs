using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Shared;
using SchoolV01.Shared.Constants;

namespace SchoolV01.Api.Controllers
{
    public class FileUploadController : ApiControllerBase
    {
        private readonly IWebHostEnvironment env;

        public FileUploadController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost("{fileLocation:int}/{uploadType:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload( IFormFile file, int fileLocation, int uploadType)
        {
            try
            {
                var requestUploadType = (Enums.UploadFileTypeEnum)uploadType;
                if (!CheckFileExtension(file, requestUploadType))
                {
                    return BadRequest($"The File does not have an extension or it is not image. " +
                        $"The Expected extension is .jpg/.png/.bmp");
                }
                if (requestUploadType != Enums.UploadFileTypeEnum.Video)
                {
                    if (!CheckFileSize(file))
                    {
                        return BadRequest($"The size of file is more than 10 mb, " +
                            $"please make sure that the file size must be less than 10 mb");
                    }

                }
                else
                {
                    if (!CheckVideoFileSize(file))
                    {
                        return BadRequest($"The size of file is more than 10 mb, " +
                            $"please make sure that the file size must be less than 500 mb");
                    }

                }


                // Read the folder where the file is to be saved
                var folder = Path.Combine(Constants.UploadFolderName, ((Enums.FileLocation)fileLocation).ToString());
                var fileNameExtension = file.FileName.Split(".").LastOrDefault();
                var trustedFileNameForFileStorage = Path.GetRandomFileName() + "." + fileNameExtension;
                if (file.Length > 0)
                {
                    // Read the Uploaded File Name
                    //var postedFileName = ContentDispositionHeaderValue
                    //  .Parse(file.ContentDisposition)
                    //    .FileName.Trim('"');

                    // set the file path as FolderName/FileName
                    var finalPath = Path.Combine(folder, trustedFileNameForFileStorage);
                    using (var fs = new FileStream(finalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }
                    return Ok(trustedFileNameForFileStorage);
                }
                else
                {
                    return BadRequest("The File is not received.");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500,
                  $"Some Error Occcured while uploading File {ex.Message}");
            }
        }

        /// The file extension must be jpg/bmp/png for images and txt/docx/pdf for files

        private bool CheckFileExtension(IFormFile file, Enums.UploadFileTypeEnum type)
        {
            var fileNameExtension = file.FileName.Split(".").LastOrDefault();

            string[] extensions = new string[] { };
            if (type == Enums.UploadFileTypeEnum.Image)
                extensions = new string[] { "jpg", "bmp", "png", "jpeg", "ico" };
            if (type == Enums.UploadFileTypeEnum.Chart)
                extensions = new string[] { "jpg", "bmp", "png", "jpeg" };
            if (type == Enums.UploadFileTypeEnum.File)
                extensions = new string[] { "txt", "docx", "pdf", "xlsx" };
            if (type == Enums.UploadFileTypeEnum.Brochure)
                extensions = new string[] { "txt", "docx", "pdf", "xlsx" };
            if (type == Enums.UploadFileTypeEnum.Video)
                extensions = new string[] { "avi", "mp4", "mpeg" };

            if (string.IsNullOrEmpty(fileNameExtension) ||
                !extensions.Contains(fileNameExtension.ToLower()))
            {
                return false;
            }

            return true;
        }

        /// Check the file size, it must be less than 10 mb

        private bool CheckFileSize(IFormFile file)
        {
            if (file.Length > 1e+7)
            {
                return false;
            }
            return true;
        }
        private bool CheckVideoFileSize(IFormFile file)
        {
            if (file.Length > 5e+7)
            {
                return false;
            }
            return true;
        }
    }
}