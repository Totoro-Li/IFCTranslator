using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace IFCTranslatorWPF
{
    public partial class ConfigPage : Page
    {
        private ConfigPageViewModel _configPageViewModel = new();

        public ConfigPage()
        {
            InitializeComponent();
            this.DataContext = _configPageViewModel;
        }

        private void Go_BtnClick(object sender, RoutedEventArgs e)
        {
            IFCNavigationService.SnackbarMessage("Empty credential field!");
        }
    }

    public class ConfigPageViewModel : ViewModelBase
    {
        public string Authkey { get; set; } = String.Empty;
    }
}