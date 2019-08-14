using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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

namespace FileHasher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filepath;
        private bool _closed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var args = Environment.GetCommandLineArgs();

            if(args.Length != 2) { this.Close(); }

            _filepath = args[1];

            if (File.Exists(_filepath) && CanReadFile())
            {
                Title = "Hashes for " + _filepath;
                CalculateHashes();
            }
            else
            {
                Close();
            }
        }

        private bool CanReadFile()
        {
            var buffer = new byte[4096];
            using(var stream = File.OpenRead(_filepath))
            {
                try
                {
                    stream.Read(buffer, 0, buffer.Length);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void CalculateHashes()
        {
            CalcMd5();
            CalcSha1();
            CalcSha256();
        }

        private Task CalcHash(Func<HashAlgorithm> hashFactory, Action<string> displayResult)
        {
            void displayResultIfWindowNotClosed(string s) => Dispatcher.Invoke(() => { if (!_closed) { displayResult(s); } });

            return Task.Run(
            () =>
            {
                displayResultIfWindowNotClosed("Calculating");
                using(var hash = hashFactory())
                using (var stream = File.OpenRead(_filepath))
                {
                    var hashValue = hash.ComputeHash(stream);
                    var hashString = BitConverter.ToString(hashValue).Replace("-", String.Empty);
                    displayResultIfWindowNotClosed(hashString);
                }
            } );
        }

        private void CalcMd5()
        {
            CalcHash(MD5.Create, (s) => this.md5.Content = s);
        }

        private void CalcSha1()
        {
            CalcHash(SHA1.Create, (s) => this.sha1.Content = s);
        }

        private void CalcSha256()
        {
            CalcHash(SHA256.Create, (s) => this.sha256.Content = s);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _closed = true;
        }
    }
}
