using DeepL;
using DeepL.Model;

namespace IFCTranslator
{
    public class Glossary : DeepLTranslatorApiShared
    {
        async Task<int> GetGlossaryNum()
        {
            if (translator != null)
            {
                var glossaries = await translator.ListGlossariesAsync();
                Console.WriteLine($"{glossaries.Length} glossaries found.");
                return glossaries.Length;
            }

            Console.WriteLine("Translator not initialized.");
            return 0;
        }

        public static void CreateGlossary(string name, string srcLang, string dstLang, GlossaryEntries entries)
        {
            translator?.CreateGlossaryAsync(name, srcLang, dstLang, entries);
        }
    }

    public class GlossaryTest : DeepLTranslatorApiShared
    {
        public static async Task TestCreateGlossary(string srcLang, string dstLang)
        {
            if (translator != null)
            {
                var glossaryCleanup = new GlossaryCleanupUtility(translator, "TestGlossaryCreate");
                var glossaryName = glossaryCleanup.GlossaryName;
                try
                {
                    var entries = new GlossaryEntries(new[] { ("北京大学", "Peking University") });
                    var glossary = glossaryCleanup.Capture(
                        await translator.CreateGlossaryAsync(
                            glossaryName,
                            srcLang,
                            dstLang,
                            entries));
                    var getResult = await translator.GetGlossaryAsync(glossary.GlossaryId);
                    Console.WriteLine(getResult.Name);
                }
                finally
                {
                    await glossaryCleanup.Cleanup();
                }
            }
        }


        // Utility class for labelling test glossaries and deleting them at test completion
        private sealed class GlossaryCleanupUtility
        {
            private readonly Translator _translator;
            public readonly string GlossaryName;
            private string? _glossaryId;

            public GlossaryCleanupUtility(Translator translator, string testName)
            {
                _translator = translator;
                GlossaryName = testName;
            }

            public GlossaryInfo Capture(GlossaryInfo glossary)
            {
                _glossaryId = glossary.GlossaryId;
                return glossary;
            }

            public async Task Cleanup()
            {
                try
                {
                    if (_glossaryId != null)
                    {
                        await _translator.DeleteGlossaryAsync(_glossaryId);
                    }
                }
                catch
                {
                    // All exceptions ignored
                }
            }
        }
    }
}