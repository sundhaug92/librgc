﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using fakeUser;

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

        private void keyboardHandler(WebSocket ws)
        {
            byte[] buffer = new byte[4096 * 4096];
            var task = ws.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None).ContinueWith((ReceiveResult) =>
            {
                string s = System.Text.Encoding.ASCII.GetString(buffer);
                s = s.TrimEnd('\0', ' ');
                foreach (string keyboardCmd in s.Split(' ', '\0'))
                {
                    string cmd = keyboardCmd.ToLower().Trim();
                    if (keyboardCmd.StartsWith("up/") && keyboardCmd.Length > "up/".Length)
                    {
                        Keyboard.KeyUp(int.Parse(keyboardCmd.Substring("up/".Length), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                    if (keyboardCmd.StartsWith("down/") && keyboardCmd.Length > "down/".Length)
                    {
                        Keyboard.KeyDown(int.Parse(keyboardCmd.Substring("down/".Length), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                    if (keyboardCmd.StartsWith("tap/") && keyboardCmd.Length > "tap/".Length)
                    {
                        Keyboard.KeyTap(int.Parse(keyboardCmd.Substring("tap/".Length), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                }
                keyboardHandler(ws);
            });
        }

        private void mouseHandler(WebSocket ws)
        {
            byte[] buffer = new byte[4096 * 4096];
            var task = ws.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None).ContinueWith((ReceiveResult) =>
            {
                string s = System.Text.Encoding.ASCII.GetString(buffer);
                s = s.TrimEnd('\0', ' ');
                foreach (string mouseCmd in s.Split(' ', '\0'))
                {
                    string cmd = mouseCmd.ToLower().Trim();
                    if (mouseCmd == "reset/")
                    {
                        Mouse.SetCursorPos(500, 500);
                    }
                    if (mouseCmd.StartsWith("click/"))
                    {
                        switch (int.Parse(mouseCmd.Substring("click/".Length)))
                        {
                            case 0:
                                Mouse.LeftClick();
                                break;

                            case 1:
                                Mouse.RightClick();
                                break;

                            case 2:
                                Mouse.MiddleClick();
                                break;
                        }
                    }
                    if (mouseCmd.StartsWith("position/"))
                    {
                        String[] stringArray = mouseCmd.Substring("position/".Length).Split('/');
                        Mouse.SetCursorPos(int.Parse(stringArray[0]), int.Parse(stringArray[1]));
                    }
                    if (mouseCmd.StartsWith("up/"))
                    {
                        switch (int.Parse(mouseCmd.Substring("up/".Length)))
                        {
                            case 0:
                                Mouse.LeftUp();
                                break;

                            case 1:
                                Mouse.RightUp();
                                break;

                            case 2:
                                Mouse.MiddleUp();
                                break;
                        }
                    }
                    if (mouseCmd.StartsWith("down/"))
                    {
                        switch (int.Parse(mouseCmd.Substring("down/".Length)))
                        {
                            case 0:
                                Mouse.LeftDown();
                                break;

                            case 1:
                                Mouse.RightDown();
                                break;

                            case 2:
                                Mouse.MiddleDown();
                                break;
                        }
                    }
                }
                mouseHandler(ws);
            });
        }

        public async Task Start(bool useEncryption = false)
        {
            HttpListener listener = new HttpListener();
            if (useEncryption) listener.Prefixes.Add("https://+:23091/");
            else listener.Prefixes.Add("http://+:23091/");
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
                    if (useEncryption) Console.WriteLine("  netsh http add urlacl url={0} user={1}\\{2} listen=yes", "https://+:23091/", userdomain, username);
                    else Console.WriteLine("  netsh http add urlacl url={0} user={1}\\{2} listen=yes", "http://+:23091/", userdomain, username);
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
                while (true)
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    if (request.Cookies["auth"] != null)
                    {
                        if (request.IsWebSocketRequest)
                        {
                            context.AcceptWebSocketAsync(null).ContinueWith((AcceptWebSocketAsyncResult) =>
                            {
                                var wsContext = AcceptWebSocketAsyncResult.Result;
                                if (request.RawUrl.ToLower() == "/mouse")
                                {
                                    mouseHandler(wsContext.WebSocket);
                                }
                                if (request.RawUrl.ToLower() == "/keyboard")
                                {
                                    keyboardHandler(wsContext.WebSocket);
                                }
                            });
                        }
                        else
                        {
                            response.ProtocolVersion = new Version("1.0"); //Disable fancy http 1.1 features

                            string respPath = "www" + request.Url.AbsolutePath.ToLower();

                            //while ((respPath.EndsWith("/")) && (respPath!="www/")) respPath = respPath.Substring(0,respPath.LastIndexOf("/"));
                            try
                            {
                                if (respPath.EndsWith("/")) respPath += "index.html";
                                byte[] fileBuffer = System.IO.File.ReadAllBytes(respPath.Replace('/', '\\'));

                                // Headers
                                try
                                {
                                    response.AddHeader("Content-Type", MimeTypes[respPath.Substring(respPath.LastIndexOf('.') + 1)]);
                                }
                                catch (IndexOutOfRangeException)
                                {
                                }
                                response.AddHeader("Cache-Control", "max-age=1, must-revalidate");
                                response.ContentLength64 = fileBuffer.LongLength;

                                //Send file
                                response.OutputStream.Write(fileBuffer, 0, fileBuffer.Length);
                            }
                            catch (FileNotFoundException)
                            {
                                listenForNextRequest.Set();
                                response.StatusCode = 404;
                                response.StatusDescription = "File not found";
                                response.OutputStream.Close();
                                response.Close();
                            }
                            catch (DirectoryNotFoundException)
                            {
                                listenForNextRequest.Set();
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
                        listenForNextRequest.WaitOne(10);
                    }
                    else if (request.Url.AbsolutePath != "/login.cgi")
                    {
                        response.Redirect("/login.cgi?path=" + request.Url.AbsolutePath);
                        response.Close();
                    }
                    else
                    {
                        response.Cookies.Add(new Cookie("auth", "ghjk"));
                        if (!request.Url.Query.Contains("path="))
                            response.Redirect(request.UrlReferrer.AbsolutePath);
                        else
                        {
                            response.Redirect(request.Url.Query.Substring("path=".Length + 1));
                        }
                        response.Close();
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}