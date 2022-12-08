using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using DeepL;
using IFCTranslator;
using static IFCTranslator.Definitions;

namespace IFCTranslatorWPF
{
    public partial class EditorPage : Page
    {
        private object _sourceTextSyncRoot = new();

        public List<FieldInfo> LanguageCodeList = typeof(YoudaoLanguageCode).GetFields(BindingFlags.Public | BindingFlags.Static |
                                                                                       BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();

        public EditorPage()
        {
            InitializeComponent();
            DataContext = new EditorPageViewModel();
        }

        private void Translate_OnClick(object sender, RoutedEventArgs e)
        {
            lock (_sourceTextSyncRoot)
            {
                if (SourceText.Text != String.Empty)
                {
                    var translator = Utility.TranslatorAuto(PlatformApis.Youdao);

                    var flags = BindingFlags.Static | BindingFlags.Public;
                    var fields = typeof(YoudaoLanguageCode).GetFields(flags); // that will return all fields of any type
                    // TODO
                }
            }
        }
    }

    public class EditorPageViewModel : ViewModelBase
    {
        public EditorPageViewModel()
        {
            SrcLangList = Utility.GetEnumDescList(typeof(Definitions.YoudaoLanguages));
            DstLangList = Utility.GetEnumDescList(typeof(Definitions.YoudaoLanguages));
            PlatformList = Utility.GetEnumDescList(typeof(Definitions.PlatformApis));
        }

        public List<string> SrcLangList { get; }

        public int SrcLangIndex { get; set; }
        public int DstLangIndex { get; set; }
        public List<string> DstLangList { get; }

        public List<string> PlatformList { get; }
    }
}