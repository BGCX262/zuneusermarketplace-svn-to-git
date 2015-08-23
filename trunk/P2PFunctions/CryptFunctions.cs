using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using IDWIM;
using System.IO;

namespace P2PFunctions
{
    public abstract class cryptFS
    {
        /// <summary>
        /// Derives a checksum from a string of text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="val"></param>
        /// <param name="startval"></param>
        /// <returns></returns>
        public static int deriveChecksum(string text, int val, int startval)
        {
            byte[] data = Encoding.Unicode.GetBytes(text);
            int cval = 5;
            foreach (byte e in data)
            {
                cval += e * startval + val;
            }
            return cval;
        }
        /// <summary>
        /// Derives a checksum from an array of bytes
        /// </summary>
        /// <param name="data">The bytes to generate the checksum for</param>
        /// <param name="val"></param>
        /// <param name="startval"></param>
        /// <returns></returns>
        public static int deriveChecksum(byte[] data, int val, int startval)
        {

            int cval = startval + 5;
            foreach (byte e in data)
            {
                cval += e * startval + val;
            }
            return cval;
        }
        /// <summary>
        /// Logs into the system
        /// </summary>
        /// <param name="pswd">The password to use for login</param>
        public static void doLogin(string pswd)
        {
            if (!System.IO.Directory.Exists(WebPages.websitePath))
            {
                System.IO.Directory.CreateDirectory(WebPages.websitePath);
                System.IO.Directory.CreateDirectory(WebPages.tempDir);

            }

            
            WebPages.Initialize();
            
            if (WebPages.Pages.Count == 0)
            {
             
                    VirtualPasswordDeriveBytes mybytes = new VirtualPasswordDeriveBytes(Encoding.Unicode.GetBytes(pswd), Encoding.ASCII.GetBytes((pswd.Length + deriveChecksum(pswd, 39, 87) + deriveChecksum(pswd + pswd.Length.ToString(), pswd.Length, 97 - pswd.Length)).ToString()));
                  
                byte[] key = mybytes.GetBytes(32);
            
                    mybytes = new VirtualPasswordDeriveBytes(Encoding.Unicode.GetBytes(pswd.Length.ToString() + deriveChecksum(Encoding.UTF8.GetByteCount(pswd).ToString(), 32, 48).ToString()), Encoding.ASCII.GetBytes((pswd.Length + deriveChecksum(pswd, 12, 63) + deriveChecksum(pswd + (8 * 9 + 7) + pswd.Length.ToString(), pswd.Length, 12 - pswd.Length * 13)).ToString()));
                    byte[] iv = mybytes.GetBytes(32);

                    dbus.authHandle = new keyPair[] { new keyPair(iv, key) };
                    Stream file = WebPages.openPageforWriting("0");
                    Stream cryptData = gridCrypto.Encrypt(file, 0, dbus.authHandle);
                    BinaryWriter mywriter = new BinaryWriter(cryptData);
                    mywriter.Write("ehlo");
                    byte[] myarray = new byte[256];
                    Random mrand = new Random();
                    mrand.NextBytes(myarray);
                    mywriter.Write(myarray);
                    mywriter.Flush();
                    FileSystem.MountRoot();
                
            }
            else
            {
                
                    VirtualPasswordDeriveBytes mybytes = new VirtualPasswordDeriveBytes(Encoding.Unicode.GetBytes(pswd), Encoding.ASCII.GetBytes((pswd.Length + deriveChecksum(pswd, 39, 87) + deriveChecksum(pswd + pswd.Length.ToString(), pswd.Length, 97 - pswd.Length)).ToString()));
                    byte[] key = mybytes.GetBytes(32);
                    mybytes = new VirtualPasswordDeriveBytes(Encoding.Unicode.GetBytes(pswd.Length.ToString() + deriveChecksum(Encoding.UTF8.GetByteCount(pswd).ToString(), 32, 48).ToString()), Encoding.ASCII.GetBytes((pswd.Length + deriveChecksum(pswd, 12, 63) + deriveChecksum(pswd + (8 * 9 + 7) + pswd.Length.ToString(), pswd.Length, 12 - pswd.Length * 13)).ToString()));
                    byte[] iv = mybytes.GetBytes(32);

                    dbus.authHandle = new keyPair[] { new keyPair(iv, key) };
                
                try
                {
                    Stream file = WebPages.openPageforWriting("0");
                    Stream cryptData = gridCrypto.Decrypt(file, dbus.authHandle);
                    BinaryReader myreader = new BinaryReader(cryptData);
                    if (myreader.ReadString() == "ehlo")
                    {

                        FileSystem.MountRoot();
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("Access denied.");
                    }
                }
                catch (Exception)
                {
                    throw new UnauthorizedAccessException("Access denied");
                }
            }
        }
    }
}
