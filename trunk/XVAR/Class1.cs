using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Security;

namespace XVAR
{
    public enum priveledgeLevel
    {
        /// <summary>
        /// Indicates that the program has unrestricted access to system resources; including system drivers and libraries; as well as the ability to import libraries
        /// </summary>
    Trusted,
        /// <summary>
        /// Indicates that the system has restricted access to system resources, and may only access the protected (abstract) filesystem.
        /// </summary>
        Intermediate,
        /// <summary>
        /// Indicates that the program can only call into designated libraries as specified in params assembly. It cannot import non-system libraries
        /// </summary>
        VirtualOnly
    }
    public abstract class FileIO
    {
        /// <summary>
        /// Opens a file for reading, and throws an exception if the file does not exist
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <returns></returns>
        public abstract Stream openRead(string filename);
        /// <summary>
        /// Opens a file for writing
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public abstract Stream openWrite(string filename);

    }
    public class VirtualMachineState
    {
        public delegate bool VMRuntimeExceptionError(Exception error, string instruction);
        priveledgeLevel priveledges;
        /// <summary>
        /// Creates a new virtual machine instance with the specified trust and access to default libraries plus any specified external libraries
        /// </summary>
        /// <param name="trust">How much the application should be trusted</param>
        /// <param name="assemblies">Any assemblies to load</param>
        /// <param name="asmGuid">A unique identifier for the Virtualization Host - This will be used to check binary compatibility across different Virtual Machine Instances. Any files ran on this VM MUST match the GUID of the current Virtual Machine Instance! To prevent binary spoofing, run-time security checks are enabled by default</param>
        Guid instanceIdentifier;
        public VirtualMachineState(priveledgeLevel trust, Assembly[] assemblies, Guid asmGuid)
        {
            instanceIdentifier = asmGuid;
            priveledges = trust;
            Assemblies = assemblies;
            foreach (Assembly et in assemblies)
            {
                foreach (Type ett in et.GetTypes())
                {
                    
                    types.Add(ett.FullName,ett);
                }
            }
        }

        Assembly[] Assemblies;
        Dictionary<string, Type> types = new Dictionary<string, Type>();
        string[] intellisplit(string bind)
        {
            bool firstpart = true;
            List<string> parts = new List<string>();
            bool isenclosed = false;
            int partindex = 0;
            int charindex = 0;
            foreach (char e in bind)
            {
                
                if (e == "\""[0])
                {
                    if (isenclosed)
                    {
                        isenclosed = false;
                    }
                    else
                    {
                        isenclosed = true;
                    }
                }
                if (e == " "[0] & !isenclosed)
                {
                    if (!firstpart)
                    {
                        parts.Add(bind.Substring(partindex +1, charindex - partindex-1));
                    
                    }
                    else
                    {
                        firstpart = false;
                        parts.Add(bind.Substring(partindex, charindex - partindex-1));
                    }
                        partindex = charindex;
                }
                
                    charindex+=1;
            }
            parts.Add(bind.Substring(partindex+1));
            return parts.ToArray();
        }
        public object execXVARStream(Stream input)
        {
            Dictionary<int, object> memoryObjects = new Dictionary<int, object>();
            BinaryReader mreader = new BinaryReader(input);
            List<MethodInfo> nativeTypeIdentifiers = new List<MethodInfo>();
            foreach (Type e in types.Values)
            {
                foreach (MethodInfo et in e.GetMethods())
                {
                    nativeTypeIdentifiers.Add(et);
                }
            }
            while (true)
            {
                byte opcode = 0;
                try
                {
                     opcode = mreader.ReadByte();
                }
                catch (Exception)
                {
                    return false;
                }
                if (opcode == 0)
                {
                    //Whether or not variable is stored
                    //32-bit integer representing variable
                    //32-bit integer representing function call
                    //32-bit integer representing number of parameters
                    //A list of parameters in the following format:
                    //BOOL: TRUE = string, FALSE = IntPtr
                    //Prefixed STRING, or IntPtr
                //Exec native stuff
                    //If it MUST be stored to a variable
                    int varReference = -1;
                    if (mreader.ReadBoolean())
                    {
                        varReference = mreader.ReadInt32();
                    }
                    //Read the method info from the stream
                    MethodInfo functionCall = nativeTypeIdentifiers[mreader.ReadInt32()];
                    //Now call it with variables encoded in the format listed above
                    int parms = mreader.ReadInt32();
                    List<object> parameters = new List<object>();
                    for (int i = 0; i < parms; i++)
                    {
                        object value;
                        if (mreader.ReadBoolean())
                        {
                            value = mreader.ReadString();
                        }
                        else
                        {
                            
                            value = memoryObjects[mreader.ReadInt32()];
                           
                        }
                        parameters.Add(value);
                    }
                    object[] args = parameters.ToArray();
                    parameters.Clear();
                    parameters = null;
                    //Now, finally call the function, and optionally store its result to a variable
                    if (varReference != -1)
                    {
                        if (!memoryObjects.Keys.Contains(varReference))
                        {
                            //Initialize the variable if it doesn't already exist
                            memoryObjects.Add(varReference, null);
                        }
                        memoryObjects[varReference] = functionCall.Invoke(null, args);
                    }
                    else
                    {
                        functionCall.Invoke(null, args);
                    }
                }
                //Now; LET'S DO IT ALL OVER AGAIN!
            }
        }
        string stringParser(string input)
        {
            return input.Replace("\\n","\n");
        }
        Dictionary<string,object> objects = new Dictionary<string,object>();
        public void compileXVARScript(StreamReader script, Stream output)
        {
            //A list containing variable identifiers
            List<string> functionIdentifiers = new List<string>();
            List<MethodInfo> nativeTypeIdentifiers = new List<MethodInfo>();
            foreach (Type e in types.Values)
            {
                foreach (MethodInfo et in e.GetMethods())
                {
                    nativeTypeIdentifiers.Add(et);
                }
            }
            BinaryWriter mwriter = new BinaryWriter(output);
            while (!script.EndOfStream)
            {
                string bind = script.ReadLine();
                string[] parts = intellisplit(bind);
                List<string> stringparts = new List<string>(parts);
                string instruction = bind.Substring(0, bind.IndexOf(" "));
                int i = 0;
           
                //TODO: Functions
                //To begin a function:
                //begin functionname
                //To end a function
                //end functionname
                
                if (instruction == "callnative")
                {
                    //0 indicates native instruction
                    mwriter.Write((byte)0);
                        //0 part is instruction type
                        //1 part is object type reference
                        //2 part is command
                        //3 part is parameters, seperated by commas (todo)
                        //4 part (optional) is optional variable output
                        Type objectRef = types[parts[1]];
                        MethodInfo command = objectRef.GetMethod(parts[2]);
                        object[] arg = null;

                        if (parts[3].Contains("\""))
                        {
                            arg = new object[] { stringParser(parts[3].Replace("\"", "")) };
                        }
                        else
                        {
                            if (parts[3] == "null")
                            {
                                arg = new object[0];
                            }
                            else
                            {
                                string[] vparams = parts[3].Split(","[0]);
                                List<object> vargs = new List<object>();
                                foreach (string et in vparams)
                                {
                                    vargs.Add(functionIdentifiers.IndexOf(et));
                                }
                                arg = vargs.ToArray();
                            }
                        }

                        if (parts.Length == 4)
                        {
                           
                            //TRUE indicates that the variable is bound to the function, FALSE indicates that no variable precedes the function reference
                            mwriter.Write(false);

                            //Then write the standard function header (native), which is another 32-bit signed integer
                            mwriter.Write(nativeTypeIdentifiers.IndexOf(command));
                            //Write how many arguments there are as another 32-bit signed integer
                            mwriter.Write(arg.Length);
                            //Write out the index value of each argument
                            foreach (object e in arg)
                            {
                                if (e.GetType() == typeof(int))
                                {
                                    //Not a literal string value prefixed with a length
                                    mwriter.Write(false);
                                    mwriter.Write((int)e);
                                }
                                else
                                {
                                    //A literal string value prefixed with a length
                                    mwriter.Write(true);
                                    mwriter.Write((string)e);
                                }
                            }

                   
                        }
                        else
                        {

                            //when compiled, should do something like:
                            //command.Invoke(null, arg);
                            //Check to see if variable is already allocated. If so; reference the position of the variable, if not, add the variable to the assembly
                            int identifier = 0;
                            
                            if (functionIdentifiers.Contains(parts[4]))
                            {
                                //Variable is already allocated. Simply reference it.
                                identifier = functionIdentifiers.IndexOf(parts[4]);
                            }
                            else
                            {
                                functionIdentifiers.Add(parts[4]);
                                identifier = functionIdentifiers.IndexOf(parts[4]);
                            }
                            //Write the variable identifier to the assembly and bind it to the native assembly call
                            //TRUE indicates that the variable is bound to the function, FALSE indicates that no variable precedes the function reference
                            mwriter.Write(true);
                            //Identifier is ALWAYS a 32-bit signed integer!
                            mwriter.Write(identifier);
                            //Then write the standard function header (native), which is another 32-bit signed integer
                            mwriter.Write(nativeTypeIdentifiers.IndexOf(command));
                            //Write how many arguments there are as another 32-bit signed integer
                            mwriter.Write(arg.Length);
                            //Write out the index value of each argument
                            foreach (object e in arg)
                            {
                                if (e.GetType() == typeof(int))
                                {
                                    //Not a literal string value prefixed with a length
                                    mwriter.Write(false);
                                    mwriter.Write((int)e);
                                }
                                else
                                {
                                    //A literal string value prefixed with a length
                                    mwriter.Write(true);
                                    mwriter.Write((string)e);
                                }
                            }
                        }
                   
                }
         
            }
            mwriter.Flush();
        }
        public object execXVARScript(StreamReader script)
        {
            while (!script.EndOfStream)
            {
                string bind = script.ReadLine();
                string[] parts = intellisplit(bind);
                List<string> stringparts = new List<string>(parts);
                string instruction = bind.Substring(0, bind.IndexOf(" "));
                int i = 0;
               //TODO: Functions
                //To begin a function:
                //begin functionname
                //To end a function
                //end functionname
                if (instruction == "callnative")
                {
                    if (priveledges == priveledgeLevel.Trusted)
                    {
                        //0 part is instruction type
                        //1 part is object type reference
                        //2 part is command
                        //3 part is parameters, seperated by commas (todo)
                        //4 part (optional) is optional variable output
                        Type objectRef = types[parts[1]];
                        MethodInfo command = objectRef.GetMethod(parts[2]);
                        object[] arg = null;

                        if (parts[3].Contains("\""))
                        {
                            arg = new object[] { parts[3].Replace("\"", "") };
                        }
                        else
                        {
                            if (parts[3] == "null")
                            {
                                arg = new object[0];
                            }
                            else
                            {
                                string[] vparams = parts[3].Split(","[0]);
                                List<object> vargs = new List<object>();
                                foreach (string et in vparams)
                                {
                                    vargs.Add(objects[et]);
                                }
                                arg = vargs.ToArray();
                            }
                        }
                            
                            if (parts.Length == 4)
                            {
                                
                                command.Invoke(null, arg);
                            
                            }
                            else
                            {
                                if (objects.Keys.Contains(parts[4]))
                                {
                                    objects[parts[4]] = command.Invoke(null, arg);
                                }
                                else
                                {
                                    objects.Add(parts[4], command.Invoke(null, arg));
                                }
                            }
                    }
                    else
                    {
                        throw new SecurityException("The application does not have the required priveledges to invoke a native script.");
                    }
                }
            }
            return null;
        }

       
    }
}
