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
            public override string format => "Bmp";
            public override (int, int) formatBytes => (0, 2);
            public override (int, int) width => (18, 4);
            public override (int, int) height => (22, 4);
        }
        
        public class PNG : AbstractImage  {
            // смещение, длина
            public override string format => "Png";
            public override (int, int) formatBytes => (0, 7);
            public override (int, int) width => (16, 4);
            public override (int, int) height => (20, 4);
        }
        
        public class GIF : AbstractImage  {
            // смещение, длина
            public override string format => "Gif";

            public override (int, int) formatBytes => (0, 6);
            
        }

        public class AbstractImage {
            // смещение, длина
            public virtual string format => "Undefined";
            public virtual (int, int) formatBytes => (0, 0); 
            public virtual (int, int) width => (0, 0);
            public virtual (int, int) height => (0, 0);
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
                PNG png = new PNG();
                return png;
            }
            else if (result.Contains(gifMarker))  {
                GIF gif = new GIF();
                return gif;
            }
            else if(result.Contains(bmpMarker))  {
                BMP bmp = new BMP();
                return bmp;
            }
            else {
                throw new Exception("Undefined image format");
            }
        }

        private long getImageSize(Stream stream)  {
            return stream.Length;
        }

        private List<string> readBytes(Stream stream, int byteStart, int byteLength) {
            stream.Position = byteStart;
            List<string> hex = new List<string>();
            for (int i = byteStart; i < byteStart + byteLength; i++) {
                hex.Add($"{stream.ReadByte():X2}");
            }

            return hex;
        }

        private (int, int) getImageWidthHeight(Stream stream, AbstractImage imageFormat) {
            List<string> hexWidth = new List<string>();
            List<string> hexHeight = new List<string>();

            hexWidth = readBytes(stream, imageFormat.width.Item1, imageFormat.width.Item2);
            hexHeight = readBytes(stream, imageFormat.height.Item1, imageFormat.height.Item2);

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

            var imageFormat = detectImageType(stream);
            (int width, int height) = getImageWidthHeight(stream, imageFormat);

            imgInfo.Format = imageFormat.format;
            imgInfo.Size = getImageSize(stream);
            imgInfo.Width = width;
            imgInfo.Height = height;
            string json = JsonConvert.SerializeObject(imgInfo);
            return json;
        }
    }
}