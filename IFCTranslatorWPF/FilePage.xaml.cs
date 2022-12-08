using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using IFCTranslator;
using Microsoft.Win32;

namespace IFCTranslatorWPF;

public partial class FilePage : Page
{
    public FilePage()
    {
        InitializeComponent();
        _translator = Utility.TranslatorAuto(Definitions.PlatformApis.Youdao);
    }

    private string _filePath = string.Empty;
    private ITranslator? _translator;

    private void Translate_OnClick(object sender, RoutedEventArgs e)
    {
        if (_filePath == string.Empty)
        {
            MessageBox.Show("Please select a file first.");
            return;
        }
    }

    private Stream? PickDocx_OnClick(object sender, EventArgs e)
    {
        try
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "All files (*.*)|*.*|Word files (*.docx)|*.docx",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fi = new FileInfo(openFileDialog.FileName);
                // Check if the user has selected a text file
                var extension = fi.Extension.ToLower();
                var fileSize = fi.Length / 1024; //File size in kilobytes
                if (extension != ".docx")
                {
                    throw new Exception("You need to select an office docx file");
                }

                // Get the path of specified file
                _filePath = openFileDialog.FileName;

                // Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                return fileStream;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return Stream.Null;
        }

        return Stream.Null;
    }

    private void Upload_OnClick(object sender, RoutedEventArgs e)
    {
        PickDocx_OnClick(sender, e);
    }

    private void Download_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}