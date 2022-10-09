using IFCTranslator;

namespace IFCTranslatorConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Textline> Testlines = new List<Textline>();
            Testlines.Add(new Textline { Text = "北京大学" });
            CoreTranslation coreTranslation = new CoreTranslation(Definitions.Apikey);
            await coreTranslation.Translate(Testlines);
            Console.WriteLine(Testlines[0].Text + ":" + Testlines[0].TranslatedText);
            Thread.Sleep(1000); // One minute before timeout and exit
            Console.WriteLine("Time out, disposing");
        }
    }
}