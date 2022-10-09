using DeepL;

namespace IFCTranslator
{
    public class TranslatorApiShared
    {
        public static Translator? translator;
    }

    public static class TranslatorApi
    {
        public static async Task Translate(Translator translator, string sc, string dst, List<Textline> lines)
        {
            int i = 0;
            string line;
            
            foreach (var item in lines)
            {
                if (item.Text != String.Empty)
                {
                    line = item.Text;
                    if (line.Contains("{\\an8}"))
                        line = line.Replace("{\\an8}", "");
                    item.TranslatedText = (await translator.TranslateTextAsync(line, sc, dst)).Text;
                    i += item.Text.Length;
                }
            }

            Console.WriteLine("Translated " + i + " characters");
        }

        public static async void GetUsage(string key)
        {
            var translator = new Translator(key);

            var usage = await translator.GetUsageAsync();
            if (usage.AnyLimitReached)
            {
                Console.WriteLine("Translation limit exceeded.");
            }
            else if (usage.Character != null)
            {
                Console.WriteLine($"Character usage: {usage.Character}");
            }
            else
            {
                Console.WriteLine($"{usage}");
            }
        }
    }
}