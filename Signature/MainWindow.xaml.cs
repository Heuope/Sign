using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Signature
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _loadFilePath;
        private string _saveFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // Load
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                _loadFilePath = openFileDialog.FileName;
            else
                _loadFilePath = "";
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) // Save
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                _saveFilePath = saveFileDialog.FileName;
            else
                _saveFilePath = "";
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e) // Check Sign
        {
            ulong _p = ulong.Parse(p.Text);
            ulong _q = ulong.Parse(q.Text);
            ulong __e = ulong.Parse(_e.Text);

            if (Sign.CheckSign(_loadFilePath, __e, _p * _q))
                MessageBox.Show("Sing is correct.");
            else
                MessageBox.Show($"Sign is not correct. Was expected {Sign.RealSingValue} insted {Sign.SignValue}.");
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) // Subscribe
        {
            ulong _p = ulong.Parse(p.Text);
            ulong _q = ulong.Parse(q.Text);
            ulong __e = ulong.Parse(_e.Text);
            ulong _d = ulong.Parse(d.Text);

            if (Sign.Subscribe(_loadFilePath, _saveFilePath, _p, _q, _d, __e))
                MessageBox.Show($"Success. Sign is : {Sign.SignValue}.");
            else
                MessageBox.Show("Some data are incorrect.");
        }
    }
}
