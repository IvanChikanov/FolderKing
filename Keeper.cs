using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderKing
{
    internal class Keeper
    {
        public string[] Test { get; set; }
        public string[] Tsok { get; set; }
        public bool ready;
        public Keeper() 
        {
           Test = new string[2];
           Tsok = new string[2];
           ready = false;
        }
        public void Insider(int arr, int index, string path) 
        {
            switch (arr) 
            { 
                case 0:
                    Test[index] = path;
                    break;
                case 1:
                    Tsok[index] = path;
                    break;
            }
            ready = Cheker();
        }
        private bool Cheker() 
        {
            bool check = false;
            if (Test[0] != "" && Test[1] != "" && Tsok[0] != "" && Tsok[1] != "") 
            { 
                check = true;
            }
            return check;
        }
    }
}
