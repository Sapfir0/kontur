using System;
using System.Collections.Generic;
using System.IO;

namespace ImageParser
{
    public class ImageParser : IImageParser
    {
        public string GetImageInfo(Stream stream) {
            Console.WriteLine("Я тут");
            long imageSize = stream.Length;
            List<String> header = new List<String>();

            for (int i = 0; i < 8; i++){
                String hex = string.Format("{0:X2}", stream.ReadByte());
                header.Add(hex);
            }
            var result = String.Join("", header.ToArray());
            if (result.Contains("89504E470D0A1A0A"))  {
                Console.Write("Png");
            }
            else if (result.Contains("474946"))  {
                Console.Write("GIF");
            }
            else if(result.Contains("424d0a"))    {
                Console.Write("BMP");
            }
            
            
            //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            //image.Height.ToString(); 
            //image.Width.ToString();

            return "s";
        }
    }
}