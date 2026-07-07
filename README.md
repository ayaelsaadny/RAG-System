# 📄 PDF Question Answering using RAG

## Overview

This project is a Retrieval-Augmented Generation (RAG) application built with .NET that enables users to upload PDF documents and ask questions about their content. The application extracts text from the PDF, converts it into embeddings, stores them in a Qdrant vector database, retrieves the most relevant chunks, and generates accurate answers using OpenAI.

## AI Technologies

- Retrieval-Augmented Generation (RAG)
- OpenAI
- Qdrant Vector Database
- Embeddings

## Features

- Upload PDF documents
- Extract text from PDFs
- Split documents into text chunks
- Generate embeddings for each chunk
- Store embeddings in Qdrant
- Perform semantic similarity search
- Retrieve the most relevant context
- Generate context-aware answers using OpenAI

## Project Structure

```
.
├── Program.cs
├── Services
│   ├── PdfService.cs
│   ├── EmbeddingService.cs
│   ├── VectorStoreService.cs
│   └── RagService.cs
├── RAG2.csproj
└── RAG2.sln
```

## Technologies Used

- .NET 8
- C#
- OpenAI API
- Qdrant
- Microsoft Semantic Kernel
- Azure OpenAI SDK
- PDFPig

## RAG Workflow

1. Upload a PDF document.
2. Extract text from the PDF.
3. Split the text into smaller chunks.
4. Generate embeddings for each chunk.
5. Store embeddings in Qdrant.
6. Convert the user's question into an embedding.
7. Search Qdrant for the most relevant chunks.
8. Send the retrieved context and question to OpenAI.
9. Return an accurate, context-aware answer.

## Getting Started

1. Clone the repository.

```bash
git clone https://github.com/ayaelsaadny/RAG-System.git
```

2. Create an `appsettings.json` file and add your configuration:

```json
{
  "OpenAI": {
    "ApiKey": "YOUR_OPENAI_API_KEY"
  },
  "Qdrant": {
    "Host": "localhost",
    "Port": 6334
  }
}
```

3. Run a local Qdrant instance.

4. Restore dependencies and run the project.

```bash
dotnet restore
dotnet run
```

## Future Improvements

- Support multiple PDF documents
- Conversation memory
- Source citations for answers
- Web API integration
- Frontend interface
- Hybrid search (Vector + Keyword)
