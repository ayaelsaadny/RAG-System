using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using RAG2.Services;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var apiKey = config["OpenAI:ApiKey"]!;
var embeddingModel = config["OpenAI:EmbeddingModel"]!;
var chatModel = config["OpenAI:ChatModel"]!;
var chunkSize = int.Parse(config["Chunking:ChunkSize"]!);
var overlapSize = int.Parse(config["Chunking:OverlapSize"]!);

// Build Semantic Kernel with OpenAI
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(chatModel, apiKey)
    .AddOpenAITextEmbeddingGeneration(embeddingModel, apiKey)
    .Build();

// Initialize services
var pdfService = new PdfService(chunkSize, overlapSize);
var embeddingService = new EmbeddingService(kernel.GetRequiredService<Microsoft.SemanticKernel.Embeddings.ITextEmbeddingGenerationService>());
var vectorStoreService = new VectorStoreService();
await vectorStoreService.InitializeAsync();
var chatService = kernel.GetRequiredService<Microsoft.SemanticKernel.ChatCompletion.IChatCompletionService>();
var ragService = new RagService(pdfService, embeddingService, vectorStoreService, chatService);

// Ask user for PDF path
Console.WriteLine("=== RAG System - Chat with your PDF ===\n");
Console.Write("Enter the full path of your PDF file: ");
var pdfPath = Console.ReadLine()!;

if (!File.Exists(pdfPath))
{
    Console.WriteLine("File not found!");
    return;
}

// Load and process the PDF
await ragService.LoadPdfAsync(pdfPath);

// Start chat loop
Console.WriteLine("You can now ask questions about your PDF. Type 'exit' to quit.\n");

while (true)
{
    Console.Write("Your question: ");
    var question = Console.ReadLine()!;

    if (question.ToLower() == "exit")
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    if (string.IsNullOrWhiteSpace(question))
        continue;

    Console.WriteLine("Thinking...");
    var answer = await ragService.AskAsync(question);
    Console.WriteLine($"\nAnswer: {answer}\n");
    Console.WriteLine(new string('-', 50));
}
