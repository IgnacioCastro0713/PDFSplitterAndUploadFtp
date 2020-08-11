using System.Configuration;
using System.IO;
using iText.Kernel.Pdf;

namespace PDFSplitter.Src
{
    public static class Splitter
    {
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];
        private static readonly string FtpAddress = ConfigurationManager.AppSettings["FtpAddress"];
        private static readonly int MaxPageCount = int.Parse(ConfigurationManager.AppSettings["MaxPageCount"]);

        public static void SplitPdfFiles(string path)
        {
            var localPath = Path.GetDirectoryName(path);

            var pdfDocument = new PdfDocument(new PdfReader(new FileStream(path, FileMode.Open)));
            var documents = new CustomPdfSplitter(pdfDocument, localPath).SplitByPageCount(MaxPageCount);
            foreach (var document in documents) document.Close();
            pdfDocument.Close();

            
            var ftpClient = new FtpClient(UserName, Password, FtpAddress);
            ftpClient.DeleteFtpFiles(FtpAddress);
            ftpClient.DeleteDirectories();
            ftpClient.UploadFilesToFtp(localPath + "/temp");
            
            Directory.Delete(localPath + "/temp", true);
        }
    }
}