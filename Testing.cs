using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderKing
{
    internal class Testing
    {
        private string inPath;
        private string outPath;
        public DirectoryInfo[] dirs;
        DirectoryInfo directory;
        public Testing(string inp, string outp) 
        { 
            inPath = inp;
            outPath = outp;
            directory = new DirectoryInfo(inPath);
            dirs = directory.GetDirectories();
        }
        public void DoWork(string dir) 
        {
            string[] dirName = dir.Split('-');
            foreach (DirectoryInfo year in directory.GetDirectories())
            {
                DirectoryInfo[] work = year.GetDirectories($"{dirName[0]}_*");
                string yearName = year.Name;
                if (work.Length > 0)
                {
                    try
                    {
                        Renamer renamer = new Renamer();
                        DirectoryInfo predmet = new DirectoryInfo(work[0].FullName + $"/!!_READY_FOR_UPLOAD");
                        DirectoryInfo[] directories = predmet.GetDirectories($"{dir}*");
                        if (directories.Length > 0)
                        {
                            DirectoryInfo[] eoms = directories[0].GetDirectories("Правка_*");
                            if (eoms.Length > 0)
                            {
                                List<FileInfo> docx = new List<FileInfo>();
                                FileInfo[] files = eoms[0].GetFiles("*.doc*");
                                foreach (FileInfo file in files)
                                {
                                        docx.Add(file);          
                                }
                                DirectoryInfo testing = new DirectoryInfo(outPath + "/" + yearName);
                                DirectoryInfo[] pre = testing.GetDirectories($"{dirName[0]}*");
                                if (pre.Length > 0)
                                {
                                    DirectoryInfo tsokInTest = new DirectoryInfo($"{pre[0].FullName}/{dir}");
                                    if (!tsokInTest.Exists)
                                    {
                                        tsokInTest.Create();
                                    }
                                    else
                                    {
                                        FileInfo[] fls = tsokInTest.GetFiles();
                                        foreach (FileInfo f in fls)
                                        {
                                            f.Delete();
                                        }
                                    }
                                    foreach (var s in docx)
                                    {
                                        s.CopyTo(tsokInTest.FullName + "/" + s.Name, true);
                                    }
                                    foreach (FileInfo fl in tsokInTest.GetFiles("*.doc*")) 
                                    {
                                        string newname = renamer.Rename(fl.Name);
                                        if (newname.Length > 0)
                                        {
                                            if (newname.Contains("_"))
                                            {
                                                newname.Replace("_", "-");
                                            }
                                            string ext = fl.Extension;
                                            try
                                            {
                                                fl.MoveTo($"{fl.FullName.Replace(fl.Name, newname)}{ext}");
                                            }
                                            catch
                                            {
                                                string fullpath = renamer.CheckRecursion($"{fl.FullName.Replace(fl.Name, newname)}{ext}", 2);
                                                fl.MoveTo(fullpath);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.StackTrace);
                    }
                }
                else
                {
                    continue;
                }
            }

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
    }
}
