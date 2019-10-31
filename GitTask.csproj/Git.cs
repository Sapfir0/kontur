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
        private List<Dictionary<int, int>> gitTree = new List<Dictionary<int, int>>();  // {номерФайла: содеримоеФайла}}
        private List<int> virtualFilesContent = new List<int>() ; // название файла==index : содержимое в текущий момент
        
        public Git(int filesCount) {
            for (int filename = 0; filename < filesCount; filename++) {
                virtualFilesContent.Add(0);  // 0 по-умолчанию в файле, надеюсь, сойдет
            }
        }
        public void Update(int fileNumber, int value) {
            virtualFilesContent[fileNumber] = value;
        }
        public int Commit() {
            Dictionary<int, int> filesDescription = new Dictionary<int, int>(); // название файла : содержимое файла
            // пробагемся по текущим файликам и по файликам с прошлого коммита(берем -1 элементу гиттрии)
            // если файл отличается, то запосминаем его состояние
            for(int i = 0; i< virtualFilesContent.Count; i++) {
                int filename = i;
                int textContent = virtualFilesContent[i];
                filesDescription.Add(filename, textContent);
            }
            gitTree.Add(filesDescription);
            
            return gitTree.Count - 1;
        }

        public int Checkout(int commitNumber, int fileNumber) {
            int fileContent = -1;
            try {
                Dictionary<int, int>
                    ourCommit = gitTree.ElementAt(commitNumber); // название файла : содержимое файла
                fileContent = ourCommit[fileNumber];

            }
            catch (ArgumentOutOfRangeException) {
                throw new ArgumentException();
            }
            return fileContent;  // надеюсь что не вернется -1

        }
    }
}