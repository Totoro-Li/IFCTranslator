using System;
using System.Runtime.InteropServices;

namespace IFCTranslatorWPF;

internal static class ConsoleHelper
{
    [DllImport(@"kernel32.dll", SetLastError = true)]
    static extern bool AllocConsole();

    [DllImport(@"kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport(@"user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SwHide = 0;
    const int SwShow = 5;

    public static void InitConsoleSession()
    {
        AllocConsole();
        HideConsoleWindow();
    }
    public static void ShowConsoleWindow()
    {
        var handle = GetConsoleWindow();

        if (handle == IntPtr.Zero)
        {
            AllocConsole();
        }
        else
        {
            ShowWindow(handle, SwShow);
        }
    }

    public static void HideConsoleWindow()
    {
        var handle = GetConsoleWindow();
        ShowWindow(handle, SwHide);
    }
}