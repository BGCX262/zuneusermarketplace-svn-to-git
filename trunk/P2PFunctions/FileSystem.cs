using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
namespace IDWIM
{
    public abstract class IDWFile
    {
        protected string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// Represents a stream that is readable, but not writable
        /// </summary>
        public Stream inputStream
        {
            get
            {
                return gridCrypto.Decrypt(WebPages.openWebpage(fileval), dbus.authHandle);

            }
        }
        /// <summary>
        /// Represents a stream that is writable, but not readable
        /// </summary>
        public Stream outputStream
        {
            get
            {
                //Open a cryptostream
                return gridCrypto.Encrypt(WebPages.openPageforWriting(fileval), 0, dbus.authHandle);

            }
        }
        protected string fileval;
    }
    class fileCreator : IDWFile
    {
        public fileCreator(string fileid, string filename)
        {
            name = filename;
            fileval = fileid;

        }
    }
    public abstract class gridCrypto
    {
        /// <summary>
        /// Decrypts the input stream to the output stream
        /// </summary>
        /// <param name="source">I</param>
        /// <param name="dest">O</param>
        /// <param name="cryptInfo">U</param>
        public static Stream Decrypt(Stream source, params keyPair[] cryptInfo)
        {
            Stream prevStream = source;
            foreach (keyPair et in cryptInfo)
            {
                Rijndael mydale = Rijndael.Create();
                mydale.BlockSize = 256;
                mydale.KeySize = 256;
                mydale.IV = et.IV;
                mydale.Key = et.key;
                CryptoStream mystream = new CryptoStream(prevStream, mydale.CreateDecryptor(), CryptoStreamMode.Read);
                prevStream = mystream;
            }
            return prevStream;
        }
        /// <summary>
        /// Encrypts the input stream and securely deletes the input file with the specified number of passes. The source stream MUST have length
        /// </summary>
        /// <param name="source">The source stream (to be deleted)</param>
        /// <param name="dest">The destination stream</param>
        /// <param name="delcount">The number of passes to erase the file</param>
        /// <param name="cryptInfo">Crypto stuff</param>
        public static Stream Encrypt(Stream source, int delcount, params keyPair[] cryptInfo)
        {

            Stream prevStream = source;
            foreach (keyPair et in cryptInfo)
            {
                Rijndael mydale = Rijndael.Create();
                mydale.BlockSize = 256;
                mydale.KeySize = 256;
                mydale.IV = et.IV;
                mydale.Key = et.key;

                CryptoStream mystream = new CryptoStream(prevStream, mydale.CreateEncryptor(), CryptoStreamMode.Write);
                prevStream = mystream;
            }
            return prevStream;
            //int cpos = 0;
            //while (cpos < delcount)
            //{
            //    source.Position = 0;
            //    while (source.Position < source.Length)
            //    {
            //        if (source.Length - source.Position > 512)
            //        {
            //            Random mrand = new Random();

            //            byte[] thearray = new byte[512];
            //            mrand.NextBytes(thearray);
            //            source.Write(thearray, 0, thearray.Length);
            //        }
            //        else
            //        {
            //            Random mrand = new Random();

            //            byte[] thearray = new byte[source.Length-source.Position];
            //            mrand.NextBytes(thearray);
            //            source.Write(thearray, 0, thearray.Length);
            //            source.Flush();
            //        }
            //    }
            //    cpos += 1;
            //}
        }
    }
    public class keyPair
    {
        public byte[] IV;
        public byte[] key;
        public keyPair(byte[] InitializationVector, byte[] Key)
        {
            IV = InitializationVector;
            key = Key;
        }
    }
    public abstract class FileSystem
    {
        public static IDWFile GetFileByName(string filename)
        {
            foreach (IDWFile et in FileSystem.files)
            {
                if (et.Name == filename)
                {
                    return et;

                }
            }
            return null;
        }
        /// <summary>
        /// Restores the drive to its previous state
        /// </summary>
        public static void RestoreToLastVersion()
        {
            WebPages.Revert();
        }
        public static IDWFile createFile(string filename)
        {
            string findex = (fsContents.Count + 1).ToString();
            fsContents.Add(filename, findex);
            return new fileCreator(findex, filename);

        }
        public static ReadOnlyCollection<IDWFile> files
        {

            get
            {
                List<IDWFile> thefiles = new List<IDWFile>();
                foreach (KeyValuePair<string, string> et in fsContents)
                {

                    IDWFile myfile = new fileCreator(et.Value, et.Key);
                    thefiles.Add(myfile);

                }
                return thefiles.AsReadOnly();
            }
        }
        /// <summary>
        /// The NAME of each file mapped to the INDEX VALUE of each file
        /// </summary>
        static Dictionary<string, string> fsContents = new Dictionary<string, string>();
        static Stream indexReader;
        static Stream indexWriter;
        static Stream fileIndex;
        /// <summary>
        /// Mounts the root filesystem using the default options
        /// </summary>
        public static void MountRoot()
        {
            fileIndex = WebPages.openPageforWriting("0");
            //Index starts at offset value of 2097152
            indexWriter = gridCrypto.Encrypt(fileIndex, 0, dbus.authHandle);
            indexReader = gridCrypto.Decrypt(fileIndex, dbus.authHandle);
            if (fileIndex.Length > 2097152)
            {


                fileIndex.Position = 2097152;
                //Read the filesystem contents
                BinaryReader myreader = new BinaryReader(indexReader);
                long count = myreader.ReadInt64();
                long cpos = 0;
                while (cpos < count)
                {
                    //Read in each value of the file
                    string name = myreader.ReadString();
                    string offset = myreader.ReadString();
                    if (offset == "-1")
                    {
                        //File has been deleted. Ignore that for now

                    }
                    else
                    {
                        fsContents.Add(name, offset);
                    }
                    cpos += 1;
                }
            }
            else
            {
                fileIndex.SetLength(2097152);
                fileIndex.Position = 2097152;
                //Write out the number of files
                BinaryWriter mywriter = new BinaryWriter(indexWriter);
                long fcount = 0;
                mywriter.Write(fcount);
                byte[] randdata = new byte[512];
                Random mrand = new Random();
                mrand.NextBytes(randdata);
                mywriter.Write(randdata);
                mywriter.Flush();
            }

        }

        public static void Commit()
        {

            //Unmount the root filesystem
            fileIndex = WebPages.openPageforWriting("0");
            //Index starts at offset value of 2097152
            indexWriter = gridCrypto.Encrypt(fileIndex, 0, dbus.authHandle);
            indexReader = gridCrypto.Decrypt(fileIndex, dbus.authHandle);
            fileIndex.Position = 2097152;
            //Write out the number of files
            BinaryWriter mywriter = new BinaryWriter(indexWriter);
            long fcount = fsContents.Count;

            mywriter.Write(fcount);
            foreach (KeyValuePair<string, string> pairs in fsContents)
            {
                mywriter.Write(pairs.Key);
                mywriter.Write(pairs.Value);
            }
            byte[] randdata = new byte[512];
            Random mrand = new Random();
            mrand.NextBytes(randdata);
            mywriter.Write(randdata);
            mywriter.Flush();
            WebPages.Commit();
        }
    }
    public class dbus
    {
        public static keyPair[] authHandle;
        public static int maxHeight;
        public static int maxWidth;
    }
}
