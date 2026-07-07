using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace RAG2.Services
{
        public class PdfService
        {
            private readonly int _chunkSize;
            private readonly int _overlapSize;

            public PdfService(int chunkSize, int overlapSize)
            {
                _chunkSize = chunkSize;
                _overlapSize = overlapSize;
            }

            // بتاخد مسار الـ PDF وترجع قائمة من الـ chunks
            public List<string> ExtractAndChunk(string pdfPath)
            {
                // خطوة 1: اقرأ كل النص من الـ PDF
                var fullText = ExtractText(pdfPath);

                // خطوة 2: قطع النص لـ chunks
                var chunks = ChunkText(fullText);

                return chunks;
            }

            private string ExtractText(string pdfPath)
            {
                var text = new System.Text.StringBuilder();

                using var pdf = PdfDocument.Open(pdfPath);
                foreach (var page in pdf.GetPages())
                {
                    text.Append(page.Text);
                    text.Append(" ");
                }

                return text.ToString();
            }

            private List<string> ChunkText(string text)
            {
                var chunks = new List<string>();
                int start = 0;

                while (start < text.Length)
                {
                    // خد chunk بحجم _chunkSize
                    int end = Math.Min(start + _chunkSize, text.Length);
                    string chunk = text.Substring(start, end - start);
                    chunks.Add(chunk);

                    // امشي للأمام بس خلي overlap
                    start += _chunkSize - _overlapSize;
                }

                return chunks;
            }
        }
}
