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
        private Dictionary<int, Dictionary<int, int>> gitTree = new Dictionary<int, Dictionary<int, int>>(); // {номерКоммита: {номерФайла: содеримоеФайла}}
        private Dictionary<int, int> virtualFiles = new Dictionary<int, int>(); // название файла : содержимое в текущий момент
        
        public Git(int filesCount) {
            for (int filename = 0; filename < filesCount; filename++) {
                virtualFiles.Add(filename, 0);  // 0 по-умолчанию, надеюсь, сойдет
            }
        }
        public void Update(int fileNumber, int value) {
            virtualFiles[fileNumber] = value;
        }
        public int Commit() {
            Dictionary<int, int> filesDescription = new Dictionary<int, int>(); // название файла : содержимое файла

            foreach (KeyValuePair<int, int> file in virtualFiles) {
                int filename = file.Key;
                int textContent = file.Value;
                filesDescription.Add(filename, textContent);
            }

            gitTree.Add(gitTree.Count , filesDescription);

            return gitTree.Count - 1;
        }

        public int Checkout(int commitNumber, int fileNumber) {
            int fileContent = -1;
            try {
                Dictionary<int, int>
                    ourCommit = gitTree.Values.ElementAt(commitNumber); // название файла : содержимое файла
                fileContent = ourCommit[fileNumber];

            }
            catch (ArgumentOutOfRangeException) {
                throw new ArgumentException();
            }
            return fileContent;  // надеюсь что не вернется -1

        }
    }
}