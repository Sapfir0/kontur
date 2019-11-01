using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace GitTask
{
    public class Git    {
        List<List<(int, int)>> gc = new List<List<(int, int)>>(); 
        // [[( номер коммита , содержимое файла ), ( номер коммита , содержимое файла )  ]    // индекс во внешнем листе - номер файла
        // [( номер коммита , содержимое файла ), ( номер коммита , содержимое файла ) ]]     // индекс во внутренних, количество изменения файла
        List<int> virtualFiles = new List<int>();
        private int commitCount = 0; // ахах

        public Git(int filesCount) {// индекс листа - название файла
            for (int filename = 0; filename < filesCount; filename++) {
                List<(int, int)> temp = new List<(int, int)>();
                temp.Add((-1, 0));
                gc.Add(temp);  // 0 по-умолчанию в файле, надеюсь, сойдет  // -1 коммит будет инитом
                virtualFiles.Add(0);
            }
        }
        public void Update(int fileNumber, int value) {
            virtualFiles[fileNumber] = value;
        }
        public int Commit() {
            for(int i = 0; i< gc.Count; i++) {
                if (virtualFiles[i] != gc[i].Last().Item2) {  // если текущее содержимое списка не равно значению в полсл коммите
                    gc[i].Add( (commitCount, virtualFiles[i]) );
                }
            }

            commitCount += 1;
            return commitCount - 1;
        }

        public int Checkout(int commitNumber, int fileNumber) {
            int fileContent = -1;
            try {
                for (int i = 0; i < gc[fileNumber].Count; i++) {
                    
                    if (gc[fileNumber][i].Item1 == commitNumber) {
                        return gc[fileNumber][i].Item2;
                    }

                }

            }
            catch (ArgumentOutOfRangeException) {
                throw new ArgumentException();
            }

            if (fileContent == -1) {
                throw new ArgumentException();
            }
            return fileContent;  // надеюсь что не вернется -1

        }
    }
}