using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderKing
{
    internal class NewTsok
    {
        private string inPath;
        private string outPath;
        DirectoryInfo directory;
        public DirectoryInfo[] dirs;
        string log;
        public NewTsok(string inp, string outp)
        {
            inPath = inp;
            outPath = outp;
            directory = new DirectoryInfo(inPath);
            dirs = directory.GetDirectories();
        }
        public void DoWork(DirectoryInfo dir)
        {
            //MainWindow.pb.Maximum = dirs.Length;
            //
                FileInfo[] files;
                DirectoryInfo inside = new DirectoryInfo($"{dir.FullName}/Правка_{dir.Name}");
                if (!inside.Exists)
                {
                    files = dir.GetFiles();
                }
                else 
                { 
                    files = inside.GetFiles();
                }
                string[] number = dir.Name.Split('-');
                DirectoryInfo work = new DirectoryInfo(outPath);
                DirectoryInfo[] works = work.GetDirectories($"{number[0]}_*");
            if (works.Length > 0 && works.Length < 2)
            {
                DirectoryInfo find = new DirectoryInfo($"{works[0].FullName}/!!_READY_FOR_UPLOAD/{dir.Name}");
                if (find.Exists)
                {
                    DirectoryInfo pravka = new DirectoryInfo($"{find.FullName}/Правка_{dir.Name}");
                    if (!pravka.Exists)
                    {
                        pravka.Create();
                    }
                    foreach (FileInfo file in files)
                    {
                        file.CopyTo($"{pravka.FullName}/{file.Name}", true);
                    }
                }
                else
                {
                    find.Create();
                    find.CreateSubdirectory($"Правка_{dir.Name}");
                    foreach (FileInfo file in files)
                    {
                        file.CopyTo($"{find.FullName}/Правка_{dir.Name}/{file.Name}", true);
                    }
                }
                DelRec(dir);
                log += $"{dir.Name.Trim()}\n";
            }
            Writer();
        }
        private void DelRec(DirectoryInfo dir) 
        {
            try
            {
                dir.Delete(true);
            }
            catch
            {
                MessageBox.Show($"Не удалось удалить {dir.Name}, нажмите ок чтобы повторить");
                DelRec(dir);
            }
        }
        private void Writer() 
        {
            using (StreamWriter sw = new StreamWriter($"{inPath}/log.txt")) 
            {
                sw.Write(log);
            }
        }
    }
}
