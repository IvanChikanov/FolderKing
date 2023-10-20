using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Http;

namespace FolderKing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void PRGRS(int count);
        private delegate void Butts();
        Keeper keeper;
        List<Button> buttons = new List<Button>();
        List<Button> starts = new List<Button>();
        public static ProgressBar pb = new ProgressBar();
        public List<string> list;
        TestList upList;
        //DirectoryInfo roots = new DirectoryInfo(Directory.GetCurrentDirectory());
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                keeper = new Keeper();
                pb.Minimum = 0;
                pb.Width = this.Width;
                pb.Height = Height;
                progss.Children.Add(pb);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Path_Buttons(object sender, RoutedEventArgs e) 
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.Description = "Выбирай папку";
                dialog.ShowDialog();
                if (dialog.SelectedPath != "")
                {
                    switch ((sender as Button).Name)
                    {
                        case "tsokIn":
                            keeper.Insider(1, 0, dialog.SelectedPath);
                            tsokIn.Background = (Brush)this.TryFindResource("full");
                            break;
                        case "tsokOut":
                            keeper.Insider(1, 1, dialog.SelectedPath);
                            tsokOut.Background = (Brush)this.TryFindResource("full");
                            break;
                        case "testIn":
                            keeper.Insider(0, 0, dialog.SelectedPath);
                            testIn.Background = (Brush)this.TryFindResource("full");
                            break;
                        case "testOut":
                            keeper.Insider(0, 1, dialog.SelectedPath);
                            testOut.Background = (Brush)this.TryFindResource("full");
                            break;
                    }
                    if (keeper.ready)
                    {
                        Writer(keeper);
                    }
                }
            }
            catch (Exception ex) 
            {
                StreamWriter sw = new StreamWriter("log.txt");
                sw.WriteLine(ex.ToString());
            }

        }
        private void Writer(Keeper keeper) 
        {
            using (FileStream fs = new FileStream("paths.json", FileMode.Create)) 
            {
                JsonSerializer.SerializeAsync<Keeper>(fs, keeper);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            buttons.Add(testIn);
            buttons.Add(testOut);
            buttons.Add(tsokOut);
            buttons.Add(tsokIn);
            if (new FileInfo($"paths.json").Exists)
            {
                using (FileStream fs = new FileStream("paths.json", FileMode.Open))
                {
                    keeper = JsonSerializer.Deserialize<Keeper>(fs);
                    foreach (var item in buttons)
                    {
                        item.Background = tsokIn.Background = (Brush)this.TryFindResource("full");
                    }
                }
            }
            starts.Add(ts);
            starts.Add(te);
        }
        private void Start_But(object sender, RoutedEventArgs e)
        {
            foreach (var b in starts)
            {
                b.IsEnabled = false;
            }
            switch ((sender as Button).Name) 
            {
                case "te":
                    pb.Value = 0;
                    Task.Run(() =>
                    {
                        Testing testing = new Testing(keeper.Test[0], keeper.Test[1]);
                        pb.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (PRGRS)delegate (int c) { pb.Maximum = c; }, list.Count);
                        foreach (string dir in list)
                        {
                            try
                            {
                                testing.DoWork(dir);
                                pb.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (PRGRS)delegate (int c) { pb.Value++; }, list.Count);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                MessageBox.Show(ex.StackTrace);

                            }
                        }
                        this.Dispatcher.BeginInvoke((Butts)delegate () { Enableng(); MessageBox.Show("Готово"); });
                    });
                    break;
                case "ts":
                    pb.Value = 0;
                    Task.Run(() =>
                    {
                        NewTsok newTsok = new NewTsok(keeper.Tsok[0], keeper.Tsok[1]);
                        List<DirectoryInfo> dirs = new List<DirectoryInfo>();
                        dirs.AddRange(newTsok.dirs);
                        pb.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (PRGRS)delegate (int c) { pb.Maximum = c; }, dirs.Count);
                        foreach (DirectoryInfo dir in dirs) 
                        {
                            try
                            {
                                newTsok.DoWork(dir);
                                pb.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (PRGRS)delegate (int c) { pb.Value++; }, dirs.Count);
                                HttpClient http = new HttpClient();
                                http.BaseAddress = new Uri("https://ivanchikanov.ru/tsok.php?tsok=" + dir.Name);
                                http.GetAsync(http.BaseAddress);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        this.Dispatcher.BeginInvoke((Butts)delegate () { Enableng(); MessageBox.Show("Готово"); });
                    });
                    break;

            }
        }
        private void Enableng() 
        {
            foreach (var b in starts) 
            {
                b.IsEnabled = true;
            }
            upList = null;
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            if (upList == null)
            {
                upList = new TestList();
                upList.updateText = Updtr;
                upList.Owner = this;
            }
            upList.Show();
        }
        private void Updtr(List<string> sss) 
        {
            list = sss;
            if (list.Count > 0) 
            {
                listBut.Background = (Brush)this.TryFindResource("full");
            }
        }
    }
}
