using System;
using System.IO;
using PDFSplitter.Src;
using System.Configuration;

namespace PDFSplitter
{
    class Program
    {
        private static readonly string OriginPath = ConfigurationManager.AppSettings["OriginPath"];
        private static readonly string LocalOutput = $"{ConfigurationManager.AppSettings["LocalOutput"]}/output";

        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];
        private static readonly string FtpAddress = ConfigurationManager.AppSettings["FtpAddress"];

        static void Main(string[] args)
        {
            var ftpClient = new FtpClient(UserName, Password, FtpAddress);
            var filesPath = Directory.GetFiles(OriginPath, "*.pdf");

            Splitter.SplitPdfFiles(filesPath);
            ftpClient.UploadFiles(LocalOutput);
            Directory.Delete(LocalOutput, true);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished!");
        }
    }
}