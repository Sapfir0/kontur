using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;


namespace ImageParser
{
    public class ImageParser : IImageParser
    {
        private string detectImageType(Stream stream) {
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
                return "Png";
            }
            else if (result.Contains(gifMarker))  {
                return "Gif";
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

        private List<int> getImageWidthHeight(Stream stream) {
            List<string> hexTest = new List<string>();
            const string IHDRmarker = "49484452";
            bool IHDRstarted = false;
            int count = 0;
            List<string> sizeParams = new List<string>(8);
            for (int i = 8; i < 24; i++){
                string hex = $"{stream.ReadByte():X2}";
                hexTest.Append(hex);
                string hexString = "kkk";//string.Join("", hexTest.ToArray());
                
                if (hexString.Contains(IHDRmarker))  {
                    IHDRstarted = true;
                }
                if (IHDRstarted && count < 8)  {
                    sizeParams.Append(hex);
                    count += 1;
                }
            }
            List<int> size = new List<int>(2);
            size[0] = 0;
            size[1] = 1;
            return size;
        }

        private class ImageInfo {
            //public int Height;
            //public int Width;
            public string Format;
            public long Size;
        }
        
        public string GetImageInfo(Stream stream) {
            Console.WriteLine("Я тут");
            ImageInfo imgInfo = new ImageInfo();
            imgInfo.Format = detectImageType(stream);
            imgInfo.Size = getImageSize(stream);
            string json = JsonConvert.SerializeObject(imgInfo);
            return json;
        }
    }
}