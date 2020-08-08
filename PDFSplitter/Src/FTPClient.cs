using System;
using System.IO;
using System.Net;

namespace PDFSplitter.Src
{
    public class FtpClient
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _address;
        private FtpWebRequest _ftpRequest;
        private FtpWebResponse _ftpResponse;
        private Stream _ftpStream;
        private const int BufferSize = 2048;

        public FtpClient(string username, string password, string address)
        {
            _username = username;
            _password = password;
            _address = address;
        }

        private void Upload(string remoteFile, string localFile)
        {
            try
            {
                _ftpRequest = (FtpWebRequest) WebRequest.Create($"{_address}/{remoteFile}");
                _ftpRequest.Credentials = new NetworkCredential(_username, _password);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                _ftpStream = _ftpRequest.GetRequestStream();
                var localFileStream = new FileStream(localFile, FileMode.Open);
                var byteBuffer = new byte[BufferSize];
                var bytesSent = localFileStream.Read(byteBuffer, 0, BufferSize);
                try
                {
                    while (bytesSent != 0)
                    {
                        _ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, BufferSize);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                localFileStream.Close();
                _ftpStream.Close();
                _ftpRequest = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void CreateDirectory(string newDirectory)
        {
            try
            {
                _ftpRequest = (FtpWebRequest) WebRequest.Create($"{_address}/{newDirectory}");
                _ftpRequest.Credentials = new NetworkCredential(_username, _password);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                _ftpResponse = (FtpWebResponse) _ftpRequest.GetResponse();
                
                var ftpStream = _ftpResponse.GetResponseStream();
                ftpStream?.Close();

                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (WebException  ex)
            {
                var response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return;
                }
                response.Close();
            }
        }

        public void UploadFiles(string dirPath, string uploadPath)
        {
            var files = Directory.GetFiles(dirPath, "*.pdf");
            var subDirs = Directory.GetDirectories(dirPath);

            foreach (var file in files)
            {
                Upload($"{uploadPath}/{Path.GetFileName(file)}", file);
            }

            foreach (var subDir in subDirs)
            {
                
                CreateDirectory($"{uploadPath}/{Path.GetFileName(subDir)}");   
                UploadFiles(subDir, $"{uploadPath}/{Path.GetFileName(subDir)}");
            }
        }
    }
}