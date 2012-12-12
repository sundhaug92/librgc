using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace librgc
{
    public class WebServer
    {
        private System.Threading.AutoResetEvent listenForNextRequest = new System.Threading.AutoResetEvent(true);

        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>()
        {
            {"html","text/html"},
            {"js","text/javascript"},
            {"ico","image/vnd.microsoft.icon"}
        };

        private static readonly List<string> hostnames = new List<string>()
        {
            {"localhost"}
        };

        private HttpListener listener;

        public void Start()
        {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://*:23091/");
        try
        {
            listener.Start();
        }
        catch (HttpListenerException hle)
        {
            var username = Environment.GetEnvironmentVariable("USERNAME");
            var userdomain = Environment.GetEnvironmentVariable("USERDOMAIN");
            if (hle.ErrorCode == 5)
            {
                Console.WriteLine("You need to run the following command:");
                Console.WriteLine("  netsh http add urlacl url={0} user={1}\\{2} listen=yes",
                    "http://*:23091/", userdomain, username);
                return;
            }
            else
            {
                throw;
            }
        }
        Console.WriteLine("Listening...");
            try
            {
                listener.Start();
                while (true)
                {
                    listener.BeginGetContext((result) =>
                    {
                        HttpListener contextListener = (HttpListener)result.AsyncState;
                        HttpListenerContext context = listener.EndGetContext(result);
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;
                        if (request.IsWebSocketRequest)
                        {
                        }
                        else{
                        response.ProtocolVersion = new Version("1.0"); //Disable fancy http 1.1 features

                        string respPath = "www" + request.Url.AbsolutePath.ToLower();

                        //while ((respPath.EndsWith("/")) && (respPath!="www/")) respPath = respPath.Substring(0,respPath.LastIndexOf("/"));
                        try
                        {
                            if (respPath.EndsWith("/")) respPath += "index.html";
                            byte[] fileBuffer = System.IO.File.ReadAllBytes(respPath.Replace('/', '\\'));

                            // MimeTypes
                            try
                            {
                                response.AddHeader("Content-Type", MimeTypes[respPath.Substring(respPath.LastIndexOf('.') + 1)]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                            }
                            response.ContentLength64 = fileBuffer.LongLength;
                            response.OutputStream.Write(fileBuffer, 0, fileBuffer.Length);
                        }
                        catch (FileNotFoundException)
                        {
                            response.StatusCode = 404;
                            response.StatusDescription = "File not found";
                            response.OutputStream.Close();
                            response.Close();
                        }
                        catch (DirectoryNotFoundException)
                        {
                            response.StatusCode = 404;
                            response.StatusDescription = "Folder not found";
                            response.OutputStream.Close();
                            response.Close();
                        }
                        finally
                        {
                            listenForNextRequest.Set();
                            response.Headers[HttpResponseHeader.Connection] = "Close";
                            response.Close();
                        }
                        }
                    }, null);
                    listenForNextRequest.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}