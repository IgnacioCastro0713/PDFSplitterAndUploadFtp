using System;
using System.Configuration;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace PDFSplitter.Src
{
    internal class CustomPdfSplitter : PdfSplitter
    {
        private readonly string _destination;
        private int _pageNumber = 1;
        private static readonly int MaxFilesByDirectory = int.Parse(ConfigurationManager.AppSettings["MaxFilesByDirectory"]);

        public CustomPdfSplitter(PdfDocument pdfDocument, string destination) : base(pdfDocument) => _destination = destination;

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            try
            {
                var page = _pageNumber++;
                var folder = page % MaxFilesByDirectory == 0 ? page / MaxFilesByDirectory : page / MaxFilesByDirectory + 1;

                if (!Directory.Exists($"{_destination}/output/part{folder}"))
                {
                    Directory.CreateDirectory($"{_destination}/output/part{folder}");
                }

                return new PdfWriter($"{_destination}/output/part{folder}/page_{page}.pdf");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}