using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using DeepL;


namespace IFCTranslator
{
    public class CoreTranslation : TranslatorApiShared
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TranslationId { get; set; }

        private string SrcLanguage { get; set; }
        private string DstLanguage { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }

        public List<Textline>? Lines { get; set; }

        public static void InitTranslate(string key)
        {
            TranslatorApiShared.translator = new Translator(key);
        }

        public async Task Translate(List<Textline> lines)
        {
            if (translator != null) await TranslatorApi.Translate(translator, SrcLanguage , DstLanguage, lines);
        }

        public CoreTranslation(string apikey)
        {
            translator = new Translator(apikey);
            SrcLanguage = LanguageCode.Chinese;
            DstLanguage = LanguageCode.EnglishAmerican;
        }

        public CoreTranslation(string apikey, string srcLang, string dstLang)
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