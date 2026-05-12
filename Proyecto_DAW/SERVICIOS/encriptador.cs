using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public class encriptador
    {
        #region CIFRADO IRREVERSIBLE SHA-256 (contraseña)
        public string GetSHA256(string strAcceso)
        {
            using (SHA256 sha256Acceso = SHA256.Create())
            {
                byte[] streamAcceso = sha256Acceso.ComputeHash(Encoding.UTF8.GetBytes(strAcceso));
                StringBuilder sbAcceso = new StringBuilder();
                foreach (byte bAcceso in streamAcceso)
                    sbAcceso.AppendFormat("{0:x2}", bAcceso);
                return sbAcceso.ToString();
            }
        }
        #endregion

        #region CIFRADO REVERSIBLE AES (dirección del cedente)
        private static readonly byte[] key = new byte[32] {
        0x2B, 0x7E, 0x15, 0x16, 0x28, 0xAE, 0xD2, 0xA6,
        0xAB, 0xF7, 0xCF, 0x40, 0x44, 0x35, 0x5C, 0xE5,
        0x10, 0x7D, 0xEC, 0x9A, 0x6D, 0x23, 0x3B, 0x91,
        0x1F, 0xA2, 0x02, 0x49, 0xD6, 0x7D, 0xB8, 0x65
        };

        private static readonly byte[] iv = new byte[16] {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
        0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
        };

        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        #endregion
    }
}
