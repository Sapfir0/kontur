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
            Console.Write("конструктор");
            for (int i = 0; i < filesCount; i++) {
                string filepath = "./" + i;
                FileStream fs = File.Create(filepath);
            }
        }
        public void Update(int fileNumber, int value) {
            string filepath = "./" + fileNumber;
            using (FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Write)) {
                Byte[] info = new UTF8Encoding(true).GetBytes(value.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
        public int Commit() {
            FileInfo[] listdir = new DirectoryInfo(@"./").GetFiles();
            Dictionary<int, string> filesDescription = new Dictionary<int, string>(listdir.Length); // название файла : содержимое файла
            
            foreach (var file in listdir)  {
                Console.Write(file.Name);
                string[] readText = File.ReadAllLines(file.Name); //если предположить по примеру что заполнятеся только верхняя строка, то берем просто первый элемент 
                filesDescription.Add(Convert.ToInt32(file.Name), readText[0]);
            }

            gitTree.Add(gitTree.Count + 1, filesDescription);

            return gitTree.Count + 1;
        }

        public int Checkout(int commitNumber, int fileNumber) {
            Dictionary<int, string> ourCommit = gitTree.Keys.ElementAt(commitNumber); // название файла : содержимое файла
            return ourCommit[fileNumber];

        }
    }
}