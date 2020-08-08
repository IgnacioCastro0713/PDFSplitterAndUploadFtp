using System;
using System.IO;
using iText.Kernel.Pdf;
using PDFSplitter.Src;
using System.Configuration;

namespace PDFSplitter
{
    class Program
    {
        private static readonly string OriginPath = ConfigurationManager.AppSettings["OriginPath"];
        private static readonly string LocalOutput = ConfigurationManager.AppSettings["LocalOutput"];
        private static readonly int MaxPageCount = int.Parse(ConfigurationManager.AppSettings["MaxPageCount"]);

        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];
        private static readonly string FtpAddress = ConfigurationManager.AppSettings["FtpAddress"];

        static void Main(string[] args)
        {
            var pdfDocument = new PdfDocument(new PdfReader(new FileStream(OriginPath, FileMode.Open)));
            var documents = new CustomPdfSplitter(pdfDocument, LocalOutput).SplitByPageCount(MaxPageCount);
            var ftpClient = new FtpClient(UserName, Password, FtpAddress);

            foreach (var document in documents)
            {
                document.Close();
            }

            pdfDocument.Close();
            documents.Clear();

            ftpClient.UploadFiles(LocalOutput, string.Empty);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished!");
        }
    }
}