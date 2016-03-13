using System;
using Analyzer.ViewModels;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Analyzer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            // Inject View Model
            MainWindowViewModel mwvm = new MainWindowViewModel();
            window.DataContext = mwvm;
            window.WindowState = WindowState.Maximized;
            window.Show();

        }
    }
}
