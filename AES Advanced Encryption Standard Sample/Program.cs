using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AES_Advanced_Encryption_Standard_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileRoot = AppDomain.CurrentDomain.BaseDirectory;
            string filePahtNote = fileRoot + "note.txt";
            string filePathCypher = fileRoot + "note.aes";
            string password = "12356";

            byte[] salt = GenerateSalt();
            FileStream fs = new FileStream(filePathCypher, FileMode.Create);
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CFB;
            fs.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
            FileStream fsIn = new FileStream(filePahtNote, FileMode.Open);

            byte[] buffer = new byte[108576];
            int read;

            try
            {
                while ((read=fsIn.Read(buffer,0,buffer.Length))>0)
                {
                    fsIn.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex)
            {

                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fs.Close();
                fsIn.Close();


            }

            Console.WriteLine("finish");
            Console.ReadLine();


        }

        public static byte[] GenerateSalt()
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }

            return data;

        }




    }
}
