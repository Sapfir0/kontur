using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace GitTask
{
    class Program
    {
        static void Main(string[] args) {
            string cn = "[\"Git\",\"Update\",\"Commit\",\"Update\",\"Checkout\"]";
            string ca = "[[3],[0,5],[],[0,6],[2,0]]";
            var commandNames = Regex.Matches(cn, @"\w+").Cast<Match>().Select(x => x.Value).ToArray();
            var commandArgs = Regex.Matches(ca, @"\[([\d,]*)\]").Cast<Match>()
                .Select(
                    x => x.Groups[1].Value
                        .Split(',')
                        .Where(y => !string.IsNullOrEmpty(y))
                        .Select(int.Parse)
                        .ToArray()
                ).ToArray();


            var resultArray = new int?[commandArgs.Length];

            Git git = null;

            for (int i = 0; i < commandNames.Length; i++)
            {
                switch (commandNames[i].ToLower())
                {
                    case "git":
                        git = new Git(commandArgs[i][0]);
                        resultArray[i] = null;
                        break;
                    case "commit":
                        resultArray[i] = git.Commit();
                        break;
                    case "update":
                        git.Update(commandArgs[i][0], commandArgs[i][1]);
                        resultArray[i] = null;
                        break;
                    case "checkout":
                        resultArray[i] = git.Checkout(commandArgs[i][0], commandArgs[i][1]);
                        break;
                    default:
                        throw new ArgumentException("lol");
                }
            }

            Console.WriteLine(
                $"[{string.Join<string>(",", resultArray.Select(x => x.HasValue ? x.Value.ToString() : "null"))}]");
        }
    }
}