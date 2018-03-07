using System;
using System.Windows;
using Grid.AccessLogs;
using Microsoft.Win32;

namespace AccessLogImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                fileName.Text = dlg.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var lines = System.IO.File.ReadAllLines(fileName.Text);
            for (var i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split(',');
                var date = DateTime.ParseExact(columns[0], "dd/MM/yyyy", null);
                var employeeCode = columns[1];
                var inTime = TimeSpan.Parse(columns[2]);
                var outTime = TimeSpan.Parse(columns[3]);

                var newAccessLog = new AccessLog();
            }
        }
    }
}
