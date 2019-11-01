using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace GitTask
{
    public class Git    {
        List<List<(int, int)>> gc = new List<List<(int, int)>>(); 
        // [[{ номер коммита : содержимое файла }, { номер коммита : содержимое файла }  ]    // индекс во внешнем листе - номер файла
        // [{ номер коммита : содержимое файла }, { номер коммита : содержимое файла } ]]     // индекс во внутренних, количество изменения файла
        List<int> virtualFiles = new List<int>();

        public Git(int filesCount) {// индекс листа - название файла
            for (int filename = 0; filename < filesCount; filename++) {
                List<(int, int)> temp = new List<(int, int)>();
                temp.Add((-1, 0));
                gc.Append(temp);  // 0 по-умолчанию в файле, надеюсь, сойдет  // -1 коммит будет инитом
            }
        }
        public void Update(int fileNumber, int value) {
            virtualFiles[fileNumber] = value;
        }
        public int Commit() {
            for(int i = 0; i< gc.Count; i++) {
                if (virtualFiles[i] == gc[i].Last()) {  // если текущее содержимое списка не равно значению в полсл коммите
                    Dictionary<int, int> temp = new Dictionary<int, int>();
                    temp.Add(, 0);
                }
                int filename = i;
                int textContent = virtualFilesContent[i];
                filesDescription.Add(filename, textContent);
            }
            
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