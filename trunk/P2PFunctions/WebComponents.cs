using System;
using System.Collections.Generic;
using System.Linq;
using Ionic.Zip;
using System.IO;
/// <summary>
/// Manages web pages and components
/// </summary>
public abstract class WebPages
{
    public static List<string> webpagesMemory = new List<string>();
    public static List<string> rawFiles
    {
        get
        {
            List<string> theval = new List<string>();
            foreach (ZipEntry et in basefile.Entries)
            {
                if (!et.IsDirectory)
                {
                    theval.Add(et.FileName);
                }
            }
            return theval;
        }
    }
    public static List<string> Pages
    {
        get
        {
            List<string> thelist = new List<string>();
            foreach (ZipEntry et in basefile.Entries)
            {

                if (!et.IsDirectory)
                {

                    if (getDirName(et.FileName) == "Webpages")
                    {

                        thelist.Add(getSafeFileName(et.FileName));
                    }
                }
            }
            return thelist;
        }
        set
        {
            List<string> thelist = new List<string>();

            foreach (ZipEntry et in basefile.Entries)
            {

                if (!et.IsDirectory)
                {

                    if (getDirName(et.FileName) == "Webpages")
                    {

                        thelist.Add(getSafeFileName(et.FileName));
                    }
                }
            }
            foreach (string e in value)
            {

                if (!thelist.Contains(getSafeFileName(e)))
                {

                    basefile.AddEntry("Webpages/" + e, new byte[0]);

                }
            }
        }
    }
    static Dictionary<string, Stream> filesToBeWritten = new Dictionary<string, Stream>();
    static string getSafeFileName(string filename)
    {
        return filename.Substring(filename.LastIndexOf("/") + 1);
    }
    static Dictionary<string, Stream> internalStreams = new Dictionary<string, Stream>();
    public static Stream openWebpage(string webpageName)
    {
        internalStreams = new Dictionary<string, Stream>();
        try
        {
            if (filesToBeWritten.ContainsKey("Webpages/" + webpageName))
            {
                filesToBeWritten["Webpages/" + webpageName].Position = 0;
                return filesToBeWritten["Webpages/" + webpageName];
            }
            if (internalStreams.ContainsKey("Webpages/" + webpageName))
            {
                return internalStreams["Webpages/" + webpageName];
            }
            else
            {
                Stream thestream = basefile["Webpages/" + webpageName].OpenReader();
                internalStreams.Add("Webpages/" + webpageName, thestream);
                return thestream;
            }
        }
        catch (Exception)
        {
            Stream thestream = File.Open(tempDir + "\\" + (filesToBeWritten.Count + 1).ToString(), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

            filesToBeWritten.Add("Webpages/" + webpageName, thestream);
            return thestream;
        }
    }
    static string getDirName(string FileName)
    {
        return FileName.Substring(0, FileName.LastIndexOf("/"));
    }
    public static string websitePath ="\\flash2\\cryptoDB";
    static ZipFile basefile;
    public static void DeletePage(string pagename)
    {
        basefile.RemoveEntry("Webpages/" + pagename);
        if (filesToBeWritten.ContainsKey("Webpages/" + pagename))
        {
            filesToBeWritten.Remove("Webpages/" + pagename);
        }
    }
    public static Stream openPageforWriting(string pagename)
    {

        if (filesToBeWritten.ContainsKey("Webpages/" + pagename))
        {
            filesToBeWritten["Webpages/" + pagename].Position = 0;
            return filesToBeWritten["Webpages/" + pagename];
        }
        else
        {
            Stream sourceStream = openWebpage(pagename);
            if (!filesToBeWritten.ContainsKey("Webpages/" + pagename))
            {
                filesToBeWritten.Add("Webpages/" + pagename, File.Open(tempDir + "\\" + (filesToBeWritten.Count + 1).ToString(), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite));
            }
            Stream thestream = filesToBeWritten["Webpages/" + pagename];

            while (sourceStream.Position < sourceStream.Length)
            {
                if (sourceStream.Length - sourceStream.Position > 1024)
                {
                    byte[] buffer = new byte[1024];
                    sourceStream.Read(buffer, 0, buffer.Length);
                    thestream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    byte[] buffer = new byte[sourceStream.Length - sourceStream.Position];
                    sourceStream.Read(buffer, 0, buffer.Length);
                    thestream.Write(buffer, 0, buffer.Length);
                }

            }
            thestream.Position = 0;

            return thestream;
        }
    }
    public static void Commit()
    {
        foreach (KeyValuePair<string, Stream> e in filesToBeWritten)
        {
            e.Value.Position = 0;
            basefile.UpdateEntry(e.Key, e.Value);


        }
       
        basefile.Save();
       
      
        File.Delete(websitePath + "\\Resources");
        File.Move(tempDir + "\\tempfile", websitePath + "\\Resources");
        foreach (KeyValuePair<string, Stream> e in filesToBeWritten)
        {
            e.Value.Close();
        }
        foreach (string et in Directory.GetFiles(tempDir))
        {
            File.Delete(et);
        }
        internalStreams = new Dictionary<string, Stream>();
        filesToBeWritten = new Dictionary<string, Stream>();
        basefile.Dispose();
        Initialize();
    }
    public static void Revert()
    {
        foreach (KeyValuePair<string, Stream> e in filesToBeWritten)
        {
            e.Value.Close();
        }
        foreach (string et in Directory.GetFiles(tempDir))
        {
            File.Delete(et);
        }
        filesToBeWritten = new Dictionary<string, Stream>();
        basefile.Dispose();

        Initialize();
    }

    public static string tempDir = "/flash2/cryptTemp";
    public static void Initialize()
    {

        if (!File.Exists(websitePath + "\\Resources"))
        {
            
            ZipFile myfile = new ZipFile();
            
            //Create base directory structure for Zip file
            myfile.AddDirectoryByName("Webpages");
            
            myfile.Save(websitePath+"\\Resources");
            myfile.Dispose();
        
        }
        
        basefile = ZipFile.Read(websitePath+"\\Resources");
       
    }
  
}
