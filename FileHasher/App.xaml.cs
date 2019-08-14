using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

namespace FileHasher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CheckCommandLineArgs(object sender, StartupEventArgs e)
        {
            if(e.Args.Length == 0)
            {
                CheckToggleRegistration();
                Shutdown(0);
            }
            else if(e.Args.Length != 1)
            {
                this.Shutdown(1);
            }
            else
            {
                StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
                ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
        }

        private const string ShellExtensionRegistryKeyPath = @"Software\Classes\*\shell\FileHasher";

        private void CheckToggleRegistration()
        {
            bool alreadyRegistered;
            using (var regKey = Registry.CurrentUser.OpenSubKey(ShellExtensionRegistryKeyPath))
            {
                alreadyRegistered = regKey != null;
            }

            if (alreadyRegistered)
            {
                var response = MessageBox.Show("Do you want to un-register the FileHasher as a Shell Extension?",
                    "Unregister FileHasher?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);

                if (response == MessageBoxResult.Yes)
                {
                    Registry.CurrentUser.DeleteSubKeyTree(ShellExtensionRegistryKeyPath);
                }
            }
            else
            {
                var response = MessageBox.Show("Do you want to register the FileHasher as a Shell Extension?",
                    "Register FileHasher?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);

                if(response == MessageBoxResult.Yes)
                {
                    using (var subkey = Registry.CurrentUser.CreateSubKey(ShellExtensionRegistryKeyPath))
                    {
                        subkey.SetValue("", "Calculate Hashes");

                        using(var commandKey = subkey.CreateSubKey("command"))
                        {
                            var location = Assembly.GetEntryAssembly().Location;
                            var command = $"\"{location}\" \"%1\"";
                            commandKey.SetValue("", command);
                        }
                    }
                }
            }
        }
    }
}
