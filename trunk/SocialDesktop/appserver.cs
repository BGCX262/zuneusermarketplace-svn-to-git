using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace IDWIM
{
    public delegate void moduleExecuteInvoker(Stream datastream, string moduleName);
    class webclient
    {
        public static event moduleExecuteInvoker onModuleInvoke;
        string Path;
        Stream clientStream;
        void thetar()
        {
            try
            {
                TextReader myreader = new StreamReader(clientStream);
                string text = myreader.ReadLine();
                TextWriter mywriter = new StreamWriter(clientStream);
                while (true)
                {
                    if (text.Contains("POST /"))
                    {
                        text = text.Substring("POST".Length + 1);
                        text = text.Substring(0, text.LastIndexOf(" "));
                        string dtext = "";
                        ((NetworkStream)clientStream).ReadTimeout = 500;
                        while (true)
                        {
                            try
                            {
                                dtext += (char)myreader.Read();
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                        clientStream.Close();
                    }
                    if (text.Contains("GET /"))
                    {
                        text = text.Substring("GET".Length + 1);
                        text = text.Substring(0, text.LastIndexOf(" "));
                        if (text.Contains("/getDynamicResource.xsf"))
                        {
                            Uri myeye = new Uri("http://internal-transport"+text);
                            string datastring = myeye.Query;
                            onModuleInvoke.Invoke(clientStream, datastring);
                                break;
                        }
                        if (text == "/" || text == "/accept.xsf")
                        {
                            if (text == "/")
                            {
                                mywriter.WriteLine("HTTP/1.0 200 OK");
                                mywriter.WriteLine("Content-Type:text/html");
                                mywriter.WriteLine();

                                mywriter.Flush();
                                Stream startFile = File.Open(Path + "\\index.htm", FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                                StreamReader htmlreader = new StreamReader(startFile);
                                mywriter.Write(htmlreader.ReadToEnd());
                                htmlreader.Close();
                                mywriter.Flush();

                                clientStream.Close();
                            }
                            if (text == "/accept.xsf")
                            {
                                mywriter.WriteLine("HTTP/1.0 200 OK");
                                mywriter.WriteLine("Content-Type:text/html");
                                mywriter.WriteLine();

                                mywriter.Flush();
                                Stream startFile = File.Open(Path + "\\redirect.htm", FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                                StreamReader htmlreader = new StreamReader(startFile);
                                mywriter.Write(htmlreader.ReadToEnd());
                                htmlreader.Close();
                                mywriter.Flush();
                                clientStream.Close();
                                SocialVars.Running = false;


                            }
                        }
                        else
                        {
                            Stream filestream = File.Open(Path + "\\" + Uri.UnescapeDataString(text.Substring(1)), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            mywriter.WriteLine("HTTP/1.0 200 OK");
                            if (text.Contains(".htm"))
                            {
                                mywriter.WriteLine("Content-Length: " + (filestream.Length).ToString());
                                mywriter.WriteLine("Content-Type: text/html");
                            }
                            else
                            {
                                mywriter.WriteLine("Content-Length: " + (filestream.Length).ToString());
                                mywriter.WriteLine("Content-Type: application/octet-stream");
                            }
                            mywriter.WriteLine();
                            mywriter.Flush();

                            while (filestream.Position < filestream.Length)
                            {
                                byte[] buffer;
                                if (filestream.Length - filestream.Position > 5000)
                                {
                                    buffer = new byte[5000];
                                    filestream.Read(buffer, 0, buffer.Length);
                                    clientStream.Write(buffer, 0, buffer.Length);
                                    clientStream.Flush();
                                }
                                else
                                {
                                    buffer = new byte[filestream.Length - filestream.Position];
                                    filestream.Read(buffer, 0, buffer.Length);
                                    clientStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            clientStream.Flush();

                            clientStream.Close();
                        }
                        break;
                    }
                    break;
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                try
                {
                    clientStream.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("WARNING: Unable to terminate connection.");
                }
            }
        }
        public webclient(Stream netstream, string path)
        {
            Path = path;
            clientStream = netstream;
            System.Threading.Thread mythread = new System.Threading.Thread(new System.Threading.ThreadStart(thetar));
            mythread.Start();
        }
    }
    public sealed class SocialVars
    {
        static bool isRunning = true;
        
        public static bool Running
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
               
            }
        }
    }
    class appserver
    {
        public appserver(string path)
        {
            TcpListener mylist = new TcpListener(IPAddress.Loopback, 3299);
            mylist.Start();

            while (SocialVars.Running)
            {
                if (mylist.Pending())
                {
                    Stream clientStream = mylist.AcceptTcpClient().GetStream();
                    webclient myclient = new webclient(clientStream, path);
                }
                System.Threading.Thread.Sleep(20);
            }
            mylist.Stop();
        }
    }
}
