using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Ghostscript.NET.Rasterizer;
using SRCUBagTracking.Repository.DAL;
using System.Drawing.Imaging;

namespace SRCUBagTracking.Areas.Admin.Models
{
    public interface ISample
    {
        void Start(string filePath, string thumbName);
    }
    public class RasterizerCropSample : ISample
    {

        private SRCUDbContext srcuDb = new SRCUDbContext();

        public void Start(string filePath, string thumbName)
        {
            int desired_x_dpi = 10;
            int desired_y_dpi = 10;

            string inputPdfPath = filePath;
            string outputPath = @"H:\Project\Thumbnails\";
            using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.CustomSwitches.Add("-dUseCropBox");
                rasterizer.CustomSwitches.Add("-c");
                rasterizer.CustomSwitches.Add("[/CropBox [24 72 559 794] /PAGES pdfmark");
                rasterizer.CustomSwitches.Add("-f");

                rasterizer.Open(inputPdfPath);

                var pageNumber = 1;

                string pageFilePath = Path.Combine(outputPath, thumbName + ".png");

                Image img = rasterizer.GetPage(desired_x_dpi, desired_y_dpi, pageNumber);
                MemoryStream ms = new MemoryStream();

                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                //to save in folder
                img.Save(pageFilePath, ImageFormat.Png);

                
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        }
    }