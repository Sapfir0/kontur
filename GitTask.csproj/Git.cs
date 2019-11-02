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
        SortedSet<int> changedFilesAfterLastCommit = new SortedSet<int>();

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
            changedFilesAfterLastCommit.Add(fileNumber);
        }
        public int Commit() {
            foreach (var i in changedFilesAfterLastCommit) {
                if (virtualFiles[i] != gc[i].Last().Item2) {  // если текущее содержимое списка не равно значению в полсл коммите
                    gc[i].Add( (commitCount, virtualFiles[i]) );
                }
            }
            changedFilesAfterLastCommit.Clear();

            commitCount += 1;
            return commitCount - 1;
        }

        public int Checkout(int commitNumber, int fileNumber) {
            int fileContent = 0;

            if (commitNumber >= commitCount) {
                throw new ArgumentException();
            }
            
            int commit = gc[fileNumber].BinarySearch((commitNumber, -1),
                Comparer<(int, int)>.Create(
                    ((int, int) x, (int, int) y) => x.Item1.CompareTo(y.Item1)
                    )
                );
            
            if (commit < 0) {
                commit = ~commit - 1;
                if (commit < 0) {
                    return 0;
                }

            } 
            
            fileContent = gc[fileNumber][commit].Item2;    
            

        return fileContent;  // надеюсь что не вернется -1

        }
    }
}