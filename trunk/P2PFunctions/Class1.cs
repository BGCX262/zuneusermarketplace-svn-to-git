using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PFunctions
{
    class VirtualPasswordDeriveBytes
    {
        byte[] saltedPassword;
        public VirtualPasswordDeriveBytes(byte[] password, byte[] salt)
        {
            saltedPassword = new byte[salt.Length];
            for (int pass = 0; pass < 100; pass++)
            {
                int Pposition = 0;
                int Sposition = 0;
                foreach (byte et in saltedPassword)
                {

                    try
                    {
                        saltedPassword[Sposition] = (byte)(password[Pposition] + salt[Sposition]);
                        salt[Sposition] = saltedPassword[Sposition];
                    }
                    catch (Exception)
                    {
                        //Sum is greater than 255. Take absolute value of answer

                        saltedPassword[Sposition] = (byte)Math.Abs((password[Pposition] - salt[Sposition]));
                        salt[Sposition] = saltedPassword[Sposition];
                    }

                    if (Pposition < password.Length)
                    {
                        Pposition += 1;
                    }
                    else
                    {
                        Pposition = 0;
                    }
                    if (Sposition < saltedPassword.Length)
                    {
                        Sposition += 1;
                    }
                    else
                    {
                        Sposition = 0;
                    }
                }
            }
        }
        public byte[] GetBytes(int number)
        {
            byte[] data = new byte[number];
            int cpos = 0;
            try
            {
                for (int i = 0; i < data.Length-1; i++)
                {
                    if (cpos < saltedPassword.Length)
                    {
                        cpos += 1;
                    }
                    else
                    {
                        cpos = 0;
                    }
                    data[i] = saltedPassword[cpos];

                }
            }
            catch (Exception)
            {
              //  System.Windows.Forms.MessageBox.Show("Ciphergen errorCPOS:"+cpos.ToString()+";salt:"+saltedPassword.Length+";data:"+data.Length.ToString()+";val="+data[0].ToString());
            }
            return data;
        }
    }
}
