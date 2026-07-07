using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG2.Services
{
    public class RagService
    {
        private readonly PdfService _pdfService;
        private readonly EmbeddingService _embeddingService;
        private readonly VectorStoreService _vectorStoreService;
        private readonly IChatCompletionService _chatService;

        public RagService(
            PdfService pdfService,
            EmbeddingService embeddingService,
            VectorStoreService vectorStoreService,
            IChatCompletionService chatService)
        {
            _pdfService = pdfService;
            _embeddingService = embeddingService;
            _vectorStoreService = vectorStoreService;
            _chatService = chatService;
        }

        public async Task LoadPdfAsync(string pdfPath)
        {
            Console.WriteLine("Reading PDF...");
            var chunks = _pdfService.ExtractAndChunk(pdfPath);
            Console.WriteLine($"Found {chunks.Count} chunks");

            await _vectorStoreService.InitializeAsync();

            Console.WriteLine("Converting chunks to embeddings...");
            for (int i = 0; i < chunks.Count; i++)
            {
                var embedding = await _embeddingService.GetEmbeddingAsync(chunks[i]);
                await _vectorStoreService.AddAsync(chunks[i], embedding, (uint)i);
                Console.WriteLine($"   chunk {i + 1}/{chunks.Count} ✅");
            }

            Console.WriteLine("PDF Uploaded Successfully!\n");
        }

        public async Task<string> AskAsync(string question)
        {
            // Convert question to embedding
            var questionEmbedding = await _embeddingService.GetEmbeddingAsync(question);

            // Find the closest 3 chunks
            var relevantChunks = await _vectorStoreService.SearchAsync(questionEmbedding, topK: 3);

            // Combine chunks into one context
            var context = string.Join("\n---\n", relevantChunks);

            // Send question + context to ChatGPT
            var prompt = $"""
            You are an assistant that answers questions based only on the following information:

            {context}

            Question: {question}

            If the information is not found in the text, say "This information is not available in the file."
            """;

            var chatHistory = new ChatHistory();
            chatHistory.AddUserMessage(prompt);

            var response = await _chatService.GetChatMessageContentAsync(chatHistory);
            return response.Content ?? "No response available.";
        }
    }
}
