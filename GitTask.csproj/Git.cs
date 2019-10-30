using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace GitTask
{
    public class Git
    {
        private Dictionary<int, Dictionary<int, string>> gitTree = new Dictionary<int, Dictionary<int, string>>(); // {номерКоммита: {номерФайла: содеримоеФайла}}
        public Git(int filesCount) {
            for (int i = 0; i < filesCount; i++) {
                string filepath = "./" + i + ".txt";
                FileStream fs = File.Create(filepath);
                fs.Close();
            }
        }
        public void Update(int fileNumber, int value) {
            string filepath = "./" + fileNumber + ".txt";
            using (FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Write)) {
                Byte[] info = new UTF8Encoding(true).GetBytes(value.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
        public int Commit() {
            FileInfo[] listdir = new DirectoryInfo(@"./").GetFiles();
            Dictionary<int, string> filesDescription = new Dictionary<int, string>(listdir.Length); // название файла : содержимое файла
            
            foreach (var file in listdir) {
                if (new FileInfo(file.Name).Length == 0)
                {
                    Console.WriteLine("Пытаемся считать пустой файл");
                    return -1;
                }
                string textContent = File.ReadLines(file.Name).First();    //если предположить по примеру что заполнятеся только верхняя строка, то берем просто первый элемент 

                string withoutExtension = file.Name.Split('.')[0];
                Console.WriteLine(file.Name, withoutExtension);
                Console.WriteLine(withoutExtension);
                filesDescription.Add(Convert.ToInt32(withoutExtension), textContent);
            }

            gitTree.Add(gitTree.Count + 1, filesDescription);

            return gitTree.Count + 1;
        }

        public int Checkout(int commitNumber, int fileNumber) {
            Console.WriteLine(gitTree.Values.Count);
            Dictionary<int, string> ourCommit = gitTree.Values.ElementAt(commitNumber); // название файла : содержимое файла
            int fileContent = ourCommit.Keys.ElementAt(fileNumber);
            return fileContent;

        }
    }
}