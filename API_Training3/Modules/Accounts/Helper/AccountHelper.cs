using Aspose.Imaging.FileFormats.Dicom;
using Aspose.Imaging.ImageOptions;
using Microsoft.AspNetCore.Http;
using RasterEdge.Imaging.DICOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Helper
{
    public class AccountHelper
    {
        private static string savePath = @"C:\Users\Tan\Desktop\Dicom Data\ConvertToPNG\";
        public void ConvertDicomToPNG(List<IFormFile> files)
        {
            string fileName;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyToAsync(ms);
                        DCMDocument doc = new DCMDocument(ms);
                        fileName = Path.ChangeExtension(file.FileName, ".png");
                        doc.ConvertToImages(RasterEdge.Imaging.Basic.ImageType.PNG, savePath, fileName);
                        /*using (DicomImage image = new DicomImage(ms))
                        {
                            // Set the active page to be converted to JPEG
                            image.ActivePage = (DicomPage)image.Pages[0];
                            // Save as PNG
                            fileName = Path.ChangeExtension(file.FileName, ".png");
                            image.Save(savePath + fileName, new PngOptions());
                        }*/
                    }
                }
            }
        }
    }
}
