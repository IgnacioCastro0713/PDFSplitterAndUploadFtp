using System;
using System.IO;
using iText.Kernel.Pdf;
using PDFSplitter.Src;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace PDFSplitter
{
    class Program
    {
        private static readonly string OriginPath = ConfigurationManager.AppSettings["OriginPath"];
        private static readonly string LocalOutput = $"{ConfigurationManager.AppSettings["LocalOutput"]}/output";
        private static readonly int MaxPageCount = int.Parse(ConfigurationManager.AppSettings["MaxPageCount"]);

        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];
        private static readonly string FtpAddress = ConfigurationManager.AppSettings["FtpAddress"];

        static void Main(string[] args)
        {
            var ftpClient = new FtpClient(UserName, Password, FtpAddress);
            var filesPath = Directory.GetFiles(OriginPath, "*.pdf", SearchOption.AllDirectories);
            
            foreach (var file in filesPath)
            {
                var pdfDocument = new PdfDocument(new PdfReader(new FileStream(file, FileMode.Open)));
                var documents = new CustomPdfSplitter(pdfDocument, LocalOutput, Path.GetFileName(file)).SplitByPageCount(MaxPageCount);
                
                foreach (var document in documents) document.Close();
                
                pdfDocument.Close();
                documents.Clear();
            }
            
            ftpClient.UploadFiles(LocalOutput);
            //Directory.Delete(LocalOutput);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished!");
        }
    }
}