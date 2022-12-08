using IFCTranslator;

namespace TranslateFile
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var trans = new YoudaoZhiyun();
            await trans.Download("12419442DF684A7EA38C1B09588D6249","down.pdf");
        }
    }
}