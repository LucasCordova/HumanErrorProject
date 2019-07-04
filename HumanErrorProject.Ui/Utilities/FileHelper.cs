using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HumanErrorProject.Ui.Utilities
{
    public class FileHelper
    {
        public static byte[] ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState)
        {
            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"The file ({fileName}) is empty.");
            }
            else
            {
                try
                {
                    byte[] file;
                    using (var streamReader = new StreamReader(formFile.OpenReadStream()))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            streamReader.BaseStream.CopyTo(memoryStream);
                            file = memoryStream.ToArray();
                        }
                    }

                    if (file.Length > 0)
                    {
                        return file;
                    }

                    modelState.AddModelError(formFile.Name,
                        $"The file ({fileName}) is empty.");
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name,
                        $"The file ({fileName}) upload failed. " +
                        $"Please contact the Help Desk for support. Error: {ex.Message}");
                    // Log the exception
                }
            }
            return null;
        }
    }
}
