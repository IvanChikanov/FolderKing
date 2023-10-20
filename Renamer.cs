using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FolderKing
{
    internal class Renamer
    {
        public string Rename(string name)
        {
            string result = string.Empty;
            List<Regex> regex = new List<Regex> {
                new Regex(@"[0-9]{2}.[0-9]{2}.[0-9]{5}.m\d.\dm.[0-9]{2}.[0-9]{2}"),
                new Regex(@"[0-9]{2}.[0-9]{1}.[0-9]{2}.[0-9]{5}.m\d.\dm.[0-9]{2}.[0-9]{2}")};
            foreach (var regx in regex)
            {
                MatchCollection matches = regx.Matches(name);
                if (matches.Count > 0)
                {
                    result = matches[0].Value;
                    break;
                }
            }
            return result;
        }
        public string CheckRecursion(string fullpath, int count)
        {
            string end = null;
            FileInfo newd = new FileInfo(fullpath);
            string ext = newd.Extension;
            string[] peaceOfName = newd.Name.Split(new string[] { "_V" }, StringSplitOptions.None);
            FileInfo nnnn = null;
            if (peaceOfName.Length == 1)
            {
                nnnn = new FileInfo(newd.FullName.Replace(ext, $"_V{count}{ext}"));
            }
            else
            {
                nnnn = new FileInfo(newd.FullName.Replace(newd.Name, $"{peaceOfName[0]}_V{count}{ext}"));
            }
            Console.WriteLine(nnnn.Name);
            if (nnnn.Exists)
            {
                count += 1;
                end = CheckRecursion(nnnn.FullName, count);
            }
            else
            {
                end = nnnn.FullName;
            }
            return end;
        }
    }
}
