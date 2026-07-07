using Microsoft.SemanticKernel.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG2.Services
{
    public class EmbeddingService
    {
        [Obsolete]
        private readonly ITextEmbeddingGenerationService _embeddingService;

        [Obsolete]
        public EmbeddingService(ITextEmbeddingGenerationService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        // بتاخد chunk وترجع vector (قائمة من الأرقام)
        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var result = await _embeddingService.GenerateEmbeddingAsync(text);
            return result.ToArray();
        }

        // بتاخد كل الـ chunks وترجع embeddings لكلهم
        public async Task<List<float[]>> GetEmbeddingsAsync(List<string> chunks)
        {
            var embeddings = new List<float[]>();

            foreach (var chunk in chunks)
            {
                var embedding = await GetEmbeddingAsync(chunk);
                embeddings.Add(embedding);
            }

            return embeddings;
        }
    }
}
