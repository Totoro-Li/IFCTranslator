using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using DeepL;
using static IFCTranslator.FileIO;


namespace IFCTranslator
{
    public class DeepLTranslation : DeepLTranslatorApiShared
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TranslationId { get; set; }

        private string SrcLanguage { get; set; }
        private string DstLanguage { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }

        public List<Textline>? Lines { get; set; }

        public static void InitTranslate()
        {
            var iniFile = new IniFile("config.ini");
            DeepLTranslatorApiShared.translator = new Translator(iniFile.IniReadValue("DeepL", "AppSecret"));
        }

        public async Task Translate(List<Textline> lines)
        {
            if (translator != null) await DeepLTranslatorApi.Translate(translator, SrcLanguage, DstLanguage, lines);
        }

        public DeepLTranslation(string apikey)
        {
            translator = new Translator(apikey);
            SrcLanguage = LanguageCode.Chinese;
            DstLanguage = LanguageCode.EnglishAmerican;
        }

        public DeepLTranslation(string apikey, string srcLang, string dstLang)
        {
            translator = new Translator(apikey);
            SrcLanguage = srcLang;
            DstLanguage = dstLang;
        }

        public void SetLang(string srcLang, string dstLang)
        {
            SrcLanguage = srcLang;
            DstLanguage = dstLang;
        }
    }
}