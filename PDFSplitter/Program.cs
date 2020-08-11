using System;
using System.IO;
using PDFSplitter.Src;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace PDFSplitter
{
    class Program
    {
        private static readonly string OriginPath = ConfigurationManager.AppSettings["OriginPath"];

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Byte[]")]
        static void Main(string[] args)
        {
            try
            {
                Splitter.SplitPdfFiles(OriginPath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Finished!");   
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}