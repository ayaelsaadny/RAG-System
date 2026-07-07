using Qdrant.Client;
using Qdrant.Client.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG2.Services
{
    public class VectorStoreService
    {
        private readonly QdrantClient _client;
        private const string CollectionName = "documents";
        private const int VectorSize = 1536; // text-embedding-3-small size

        public VectorStoreService()
        {
            _client = new QdrantClient("localhost", 6334);
        }

        public async Task InitializeAsync()
        {
            // Check if collection already exists
            var collections = await _client.ListCollectionsAsync();
            bool exists = collections.Any(c => c == CollectionName);

            if (!exists)
            {
                await _client.CreateCollectionAsync(CollectionName,
                    new VectorParams
                    {
                        Size = VectorSize,
                        Distance = Distance.Cosine
                    });

                Console.WriteLine("Collection created in Qdrant ✅");
            }
            else
            {
                Console.WriteLine("Collection already exists in Qdrant ✅");
            }
        }

        public async Task AddAsync(string text, float[] embedding, uint id)
        {
            var points = new List<PointStruct>
        {
            new PointStruct
            {
                Id = id,
                Vectors = embedding,
                Payload = { ["text"] = text }
            }
        };

            await _client.UpsertAsync(CollectionName, points);
        }

        public async Task<List<string>> SearchAsync(float[] queryEmbedding, int topK = 3)
        {
            var results = await _client.SearchAsync(
                CollectionName,
                queryEmbedding,
                limit: (ulong)topK
            );

            return results
                .Select(r => r.Payload["text"].StringValue)
                .ToList();
        }
    }
}
