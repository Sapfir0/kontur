using System;
using System.Collections.Generic;
using System.IO;

namespace ImageParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var parser = new ImageParser();
            string imageInfoJson;
            List<string> files = new List<string> {"image.png", "BMP.bmp"};

            foreach (var filename in files) {
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))  {
                    imageInfoJson = parser.GetImageInfo(file);
                }
                Console.WriteLine(imageInfoJson);
            }

        }
    }
}