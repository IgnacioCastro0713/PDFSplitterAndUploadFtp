using System;
using System.Configuration;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace PDFSplitter.Src
{
    public class CustomPdfSplitter : PdfSplitter
    {
        private readonly string _localPath;
        private int _pageNumber = 1;

        private static readonly int MaxFilesByDirectory = int.Parse(ConfigurationManager.AppSettings["MaxFilesByDirectory"]);

        public CustomPdfSplitter(PdfDocument pdfDocument, string localPath) : base(pdfDocument) => _localPath = localPath;

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            try
            {
                var basePath = $"{_localPath}/temp/";
                var page = _pageNumber++;
                var folder = page % MaxFilesByDirectory == 0
                    ? page / MaxFilesByDirectory
                    : page / MaxFilesByDirectory + 1;

                if (!Directory.Exists($"{basePath}/part{folder}"))
                    Directory.CreateDirectory($"{basePath}/part{folder}");

                return new PdfWriter($"{basePath}/part{folder}/page_{page}.pdf");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}