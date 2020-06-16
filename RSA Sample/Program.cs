using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Sample
{
    public class RsaEnc
    {
        public static byte[] Encrypt(byte[] data, RSAParameters rsakey,bool doPadding)
        {

            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsakey);
                    var resultData = rsa.Encrypt(data, doPadding);
                    return resultData;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static byte[] Descrypt(byte[] data, RSAParameters rsakey, bool doPadding)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsakey);
                    var resultData = rsa.Decrypt(data, doPadding);
                    return resultData;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            Encoding encoding = new UTF8Encoding();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            byte[] planText;
            byte[] cypherText;

            Console.WriteLine("enter text to encrypt");
            var text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text))
            {
                planText = encoding.GetBytes(text);
                cypherText = RsaEnc.Encrypt(planText, rsa.ExportParameters(false), false);
                var encryptedText = encoding.GetString(cypherText);
                Console.WriteLine($"Encrypted Text: {encryptedText}");

                Console.WriteLine("Press enter to decrypt text");
                Console.ReadLine();

                byte[] decryptedText = RsaEnc.Descrypt(cypherText, rsa.ExportParameters(true), false);
                Console.WriteLine($"Decrypted Text: {encoding.GetString(decryptedText)}");
                Console.ReadLine();

            }


        }
    }
}
