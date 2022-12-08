using System.ComponentModel.DataAnnotations;

namespace IFCTranslator
{
    public class Textline
    {
        [Key] public int? Id { get; set; }

        public int? LineNumber { get; set; }

        public string? TimeStamp { get; set; }
        public string? Text { get; set; }

        public string? TranslatedText { get; set; }

        public int? TranslationId { get; set; }
    }
}