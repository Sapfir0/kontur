using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;


namespace ImageParser
{
    public class ImageParser : IImageParser
    {
        public class BMP : AbstractImage  {  // вроде я не изпользую ооп преимущства
            // смещение, длина
            (int, int) format = (0, 2);
            (int, int) width = (18, 4);
            (int, int) height = (22, 4);
        }
        
        public class PNG : AbstractImage  {
            // смещение, длина
            (int, int) format = (0, 7);
            (int, int) width = (17, 21);
            (int, int) height = (21, 25);
        }
        
        public class GIF : AbstractImage  {
            // смещение, длина
            (int, int) format = (0, 6);
            (int, int) width;
            (int, int) height;
        }

        public abstract class AbstractImage {
            // смещение, длина
            public (int, int) format = (0, 0);
            public (int, int) width = (0, 0);
            public (int, int) height = (0, 0);
            public long Size = 0;
        } 
        
        private AbstractImage detectImageType(Stream stream) {
            List<string> header = new List<string>();
            int headerIsEnd = 8; // самый длинный хедер
            for (int i = 0; i < headerIsEnd; i++){
                string hex = $"{stream.ReadByte():X2}";
                header.Add(hex);
            }
            var result = string.Join("", header.ToArray());
            const string pngMarker = "89504E470D0A1A0A";
            const string gifMarker = "474946";
            const string bmpMarker = "424d0a";
            if (result.Contains(pngMarker))  {
                PNG png;
                return png;
            }
            else if (result.Contains(gifMarker))  {
                PNG png;
                return png;
            }
            else if(result.Contains(bmpMarker))  {
                return "Bmp";
            }
            else  {
                return "Undefined format";
            }
        }

        private long getImageSize(Stream stream)  {
            return stream.Length;
        }

        private (int, int) getImageWidthHeight(Stream stream, AbstractImage imageFormat) {
            List<string> hexWidth = new List<string>();
            List<string> hexHeight = new List<string>();
            for (int i = imageFormat.width[0]; i < imageFormat.width[1]; i++) {
                string hex = $"{stream.ReadByte():X2}";
                hexWidth.Add(hex);
            }

            for (int i = imageFormat.height[0]; i < imageFormat.height[1]; i++) {
                string hex = $"{stream.ReadByte():X2}";
                hexHeight.Add(hex);
            }
            
            string hexWidthString = string.Join("", hexWidth.ToArray());
            string hexHeightString = string.Join("", hexHeight.ToArray());
            int w = Convert.ToInt32(hexWidthString, 16);
            int h = Convert.ToInt32(hexHeightString, 16);
            return (w, h);
        }

        private class ImageInfo {
            public int Height;
            public int Width;
            public string Format;
            public long Size;
        }
        
        public string GetImageInfo(Stream stream) {
            ImageInfo imgInfo = new ImageInfo();
            
            (int width, int height) = getImageWidthHeight(stream);

            imgInfo.Format = detectImageType(stream);
            imgInfo.Size = getImageSize(stream);
            imgInfo.Width = width;
            imgInfo.Height = height;
            string json = JsonConvert.SerializeObject(imgInfo);
            return json;
        }
    }
}