using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageParser
{
    public class ImageParser : IImageParser
    {
        public string GetImageInfo(Stream stream) {
            Console.WriteLine("Я тут");
            long imageSize = stream.Length;
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
                Console.Write("Png");
            }
            else if (result.Contains(gifMarker))  {
                Console.Write("GIF");
            }
            else if(result.Contains(bmpMarker)) {
                Console.Write("BMP");
            }
            
            List<string> hexTest = new List<string>();
            string IHDRmarker = "49484452";
            bool IHDRstarted = false;
            int count = 0;
            List<string> sizeParams = new List<string>(8);
            for (int i = 8; i < 24; i++){
                string hex = $"{stream.ReadByte():X2}";
                hexTest.Append(hex);
                string hexString = string.Join("", hexTest.ToArray());
                Console.Write(hexString);
                if (hexString.Contains(IHDRmarker))  {
                    IHDRstarted = true;
                }
                if (IHDRstarted && count < 8)  {
                    sizeParams.Append(hex);
                    count += 1;
                }
            }

            foreach (var VARIABLE in sizeParams)
            {
                Console.WriteLine(VARIABLE);                
            }
            
            //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            //image.Height.ToString(); 
            //image.Width.ToString();

            return "s";
        }
    }
}