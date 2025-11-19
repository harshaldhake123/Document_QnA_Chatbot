namespace DocQnA.Application.Utils
{
    public static class Chunker
    {
        public static List<string> ChunkText(string text, int maxCharsPerChunk, int chunkCharOverlap)
        {
            var chunks = new List<string>();

            int i = 0;
            while (i < text.Length)
            {
                int length = Math.Min(maxCharsPerChunk, text.Length - i);
                chunks.Add(text.Substring(i, length));
                i += (maxCharsPerChunk - chunkCharOverlap);
            }

            return chunks;
        }
    }
}