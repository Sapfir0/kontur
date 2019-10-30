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
        private class BMP : AbstractImage  {  // вроде я не изпользую ооп преимущства
            // смещение, длина
            (int, int) type = (0, 2);
            (int, int) width = (18, 4);
            (int, int) height = (22, 4);
        }
        
        private class PNG : AbstractImage  {
            // смещение, длина
            (int, int) type = (0, 7);
            (int, int) width = (17, 21);
            (int, int) height = (21, 25);
        }
        
        private class GIF : AbstractImage  {
            // смещение, длина
            (int, int) type = (0, 6);
            (int, int) width = (17, 21);
            (int, int) height = (21, 25);
        }

        private abstract class AbstractImage {
            // смещение, длина
            (int, int) type = (0, 0);
            (int, int)  width = (0, 0);
            (int, int)  height = (0, 0);
        } 
        
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

        private (int, int) getImageWidthHeight(Stream stream, AbstractImage type) {
            List<string> hexTest = new List<string>();
            const string IHDRmarker = "49484452";
            bool IHDRstarted = false;
            int count = 0;  // счетчик для правильного разделения длины и высоты
            List<string> hexHeight = new List<string>();
            List<string> hexWidth = new List<string>();

            for (int i = 8; i < 24; i++){  // нужно по-умному использовать seek
                string hex = $"{stream.ReadByte():X2}";
                hexTest.Add(hex);
                string hexString = string.Join("", hexTest.ToArray());
                
                if (hexString.Contains(IHDRmarker) && !IHDRstarted)  {
                    IHDRstarted = true;
                    continue;
                }
                if (IHDRstarted)  {
                    if (count < 4) {
                        hexWidth.Add(hex);
                    }
                    else if (count < 8) {
                        hexHeight.Add(hex);
                    }
                    count += 1;
                }
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