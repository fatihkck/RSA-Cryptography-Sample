using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RSA_Public_Private_Key_Sample
{

    public class RsaEnc
    {
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RsaEnc()
        {
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
        }

        public string PublicKeyString()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }

        public string PrivateString()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _privateKey);
            return sw.ToString();
        }

        public string Encrypt(string plainText)
        {
            csp.ImportParameters(_publicKey);

            var data = Encoding.Unicode.GetBytes(plainText);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }


        public string Decrypt(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            csp.ImportParameters(_privateKey);
            var plainText = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plainText);

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RsaEnc rs = new RsaEnc();
            string cypher = string.Empty;
            Console.WriteLine($"PublicKey: \n {rs.PublicKeyString()}\n");
            Console.WriteLine($"PrivateKey: \n {rs.PrivateString()}\n");

            Console.WriteLine("Enter your text to encrypt");
            var text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text))
            {
                cypher = rs.Encrypt(text);
                Console.WriteLine($"Cypher Text : {cypher}");
            }

            Console.WriteLine("Press enter to descrypt");
            Console.ReadLine();
            var plainText = rs.Decrypt(cypher);
            Console.WriteLine($"Decrypted Text : \n{plainText}");

            Console.ReadLine();

        }
    }
}
