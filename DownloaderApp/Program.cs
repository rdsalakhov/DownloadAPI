using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace DownloaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting download!");
            string source = "https://localhost:5001/Download/file";
            string destination = @"/Users/romansalakhov/Downloads/grunge.mp4";
            Stopwatch t = new Stopwatch();
            t.Start();
            DownloadFile(source, destination);
            Console.WriteLine(t.Elapsed);
            
        }
        
        public static void DownloadFile(string sourceURL, string destinationPath)
        {
            long fileSize = 0;
            int bufferSize = 1024;
            bufferSize *= 1000;
            long existLen = 0;
            FileInfo destinationFileInfo = null;
            
            FileStream saveFileStream;
            
            // вычисляем длину если файл уже есть
            if (File.Exists(destinationPath))
            {
                destinationFileInfo = new FileInfo(destinationPath);
                existLen = destinationFileInfo.Length;
            }

            if (existLen > 0)
                saveFileStream = new FileStream(destinationPath,
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.ReadWrite);
            else
                saveFileStream = new FileStream(destinationPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.ReadWrite);
            
            HttpWebRequest httpReq;
            HttpWebResponse httpRes;
            httpReq = (HttpWebRequest)HttpWebRequest.Create(sourceURL);
            httpReq.AddRange((int) existLen);
            Stream resStream;
            try
            {
                httpRes = (HttpWebResponse) httpReq.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                saveFileStream.Close();
                return;
            }
            resStream = httpRes.GetResponseStream();
            fileSize = existLen + httpRes.ContentLength;
            
            int byteSize;
            byte[] downBuffer = new byte[bufferSize];
            double progress = existLen;
            while ((byteSize = resStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
            {
                progress += byteSize;
                Console.WriteLine($"{Math.Round(progress / fileSize * 100, 2)} %");
                
                saveFileStream.Write(downBuffer, 0, byteSize);
            }
            Console.WriteLine($"File size {fileSize}");
            saveFileStream.Close();
        }
    }
    
}