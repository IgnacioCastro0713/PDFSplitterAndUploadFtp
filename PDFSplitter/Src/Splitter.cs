using System.Collections.Generic;
using System.Configuration;
using System.IO;
using iText.Kernel.Pdf;

namespace PDFSplitter.Src
{
    public static class Splitter
    {
        private static readonly int MaxPageCount = int.Parse(ConfigurationManager.AppSettings["MaxPageCount"]);

        public static void SplitPdfFiles(IEnumerable<string> files)
        {
            foreach (var file in files) // only one
            {
                var pdfDocument = new PdfDocument(new PdfReader(new FileStream(file, FileMode.Open)));
                var documents = new CustomPdfSplitter(pdfDocument).SplitByPageCount(MaxPageCount);
                
                foreach (var document in documents) document.Close();

                pdfDocument.Close();
            }
        }
    }
}