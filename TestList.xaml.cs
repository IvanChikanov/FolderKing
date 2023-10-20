using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FolderKing
{
    /// <summary>
    /// Логика взаимодействия для TestList.xaml
    /// </summary>
    public partial class TestList : Window
    {
        public delegate void UpdateText(List<string> tsoks);
        public UpdateText updateText;
        List<string> tsoks;
        //List<string> list = new List<string>();
        public TestList()
        {
            InitializeComponent();
            tsoks = new List<string>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] s = fie.Text.Split('\n');
                for (int i = 0; i < s.Length; i++) 
                {
                    if (s[i].Length > 0) 
                    { 
                        tsoks.Add(s[i].Replace("\r", ""));
                    }
                }
                updateText(tsoks);
                this.Hide();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
