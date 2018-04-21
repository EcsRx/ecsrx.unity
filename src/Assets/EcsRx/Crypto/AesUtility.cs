using System;
using System.Security.Cryptography;
using System.IO;

namespace EcsRx.Crypto
{
    public class AesUtility
    {
        public static byte[] Encrption(byte[] input, string key, string iv)
        {
            MemoryStream msEncrypt = null;
            RijndaelManaged aesAlg = null;

            try
            {
                byte[] keys = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] ivs = System.Text.Encoding.UTF8.GetBytes(iv);
                aesAlg = new RijndaelManaged();

                aesAlg.Key = keys;
                aesAlg.IV = ivs;

                ICryptoTransform ict = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                msEncrypt = new MemoryStream();

                using (CryptoStream cts = new CryptoStream(msEncrypt, ict, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cts))
                    {
                        sw.Write(input);
                    }
                }
                return msEncrypt.ToArray();
            }
            finally
            {
                if (aesAlg != null)
                {
                    
                    //aesAlg.Dispose();
                    aesAlg.Clear();
                }
                msEncrypt?.Close();
            }

            //if (msEncrypt != null)
            //{
            //    byte[] content = msEncrypt.ToArray();

            //    sresult = Convert.ToBase64String(content);
            //}
        }

        public static string Decrption(byte[] input, string key, string iv)
        {
            string sresult = string.Empty;

            byte[] keys = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] ivs = System.Text.Encoding.UTF8.GetBytes(iv);

            //byte[] inputbytes = Convert.FromBase64String(input);

            RijndaelManaged rm = null;

            try
            {
                rm = new RijndaelManaged();
                rm.Key = keys;
                rm.IV = ivs;

                ICryptoTransform ict = rm.CreateDecryptor(rm.Key, rm.IV);

                using (MemoryStream ms = new MemoryStream(input))
                {
                    using (CryptoStream cs = new CryptoStream(ms, ict, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            sresult = sr.ReadToEnd();
                        }
                    }
                }

            }
            finally
            {
                if (rm != null)
                {
                    //rm.Dispose();
                    rm.Clear();
                }
            }

            return sresult;
        }
    }
}
