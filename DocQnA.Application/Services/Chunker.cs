namespace DocQnA.Application.Services
{

    public static class Chunker
    {
        public static List<string> ChunkText(string text, int maxChars = 800)
        {
            var chunks = new List<string>();
            for (int i = 0; i < text.Length; i += maxChars)
            {
                var length = Math.Min(maxChars, text.Length - i);
                chunks.Add(text.Substring(i, length));
            }
            return chunks;
        }
    }
}