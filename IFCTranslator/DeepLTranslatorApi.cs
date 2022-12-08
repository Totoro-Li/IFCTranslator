using DeepL;

namespace IFCTranslator
{
    public class DeepLTranslatorApiShared
    {
        public static Translator? translator;
    }

    public static class DeepLTranslatorApi
    {
        public static async Task Translate(Translator translator, string sc, string dst, List<Textline> lines)
        {
            int i = 0;
            string? line;

            foreach (var item in lines)
            {
                line = item.Text;
                if (!string.IsNullOrEmpty(line)) //Lines in class Textline is defined with nullable text
                {
                    if (line.Contains("{\\an8}"))
                        line = line.Replace("{\\an8}", "");
                    item.TranslatedText = (await translator.TranslateTextAsync(line, sc, dst)).Text;
                    i += line.Length;
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