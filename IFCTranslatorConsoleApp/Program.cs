#define TEST_PLATFORM_YOUDAO
using DeepL;
using IFCTranslator;
using static IFCTranslator.Definitions;

namespace IFCTranslatorConsoleApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            #region DeepL Test

#if (TEST_PLATFORM_DEEPL)
            List<Textline> Testlines = new List<Textline>();
            Testlines.Add(new Textline { Text = "北京大学" });
            DeepLTranslation deepLTranslation = new DeepLTranslation(Definitions.Apikey);
            await deepLTranslation.Translate(Testlines);
            Console.WriteLine(Testlines[0].Text + ":" + Testlines[0].TranslatedText);

            await GlossaryTest.TestCreateGlossary(LanguageCode.Chinese, LanguageCode.EnglishAmerican);
            Thread.Sleep(1000); // One minute before timeout and exit
            Console.WriteLine("Time out, disposing");
#endif

            #endregion

            #region Youdao Test

#if (TEST_PLATFORM_YOUDAO)
            //FanyiV3DemoInternalTest.Test();
            var trans = Utility.TranslatorAuto(Definitions.PlatformApis.Youdao);
            if (trans != null)
            {
                trans.OnTranslateEvent += delegate(object sender, TranslateEventArgs e) { Console.WriteLine(e.Message); };
                var ans = await trans!.TranslateAsync("测试", Definitions.YoudaoLanguageCode.fr, Definitions.YoudaoLanguageCode.zh_CHS);
                Console.WriteLine(ans);
            }

#endif

            #endregion
        }
    }
}