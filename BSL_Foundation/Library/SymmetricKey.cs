
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class SymmetricKey
  {
    private const string _passPhrase = "JuNn@rKv1CencI0pA$sPh4@s3";
    private const string _saltValue = "jUnN@7k$AlTvA7()3";
    private const int _passwordIterations = 5;
    private const string _initVector = "JUnN@7k&^I(en@!0";
    private const int _keySize = 256;

    private SymmetricKey()
    {
    }

    internal static string Encrypt(string textToEncrypt)
    {
      HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
      return SymmetricKey.EncryptOrDecrypt("encrypt", textToEncrypt, "JuNn@rKv1CencI0pA$sPh4@s3", "jUnN@7k$AlTvA7()3", hashAlgorithm, 5, "JUnN@7k&^I(en@!0");
    }

    internal static string Decrypt(string cipherTextToDecrypt)
    {
      HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
      return SymmetricKey.EncryptOrDecrypt("decrypt", cipherTextToDecrypt, "JuNn@rKv1CencI0pA$sPh4@s3", "jUnN@7k$AlTvA7()3", hashAlgorithm, 5, "JUnN@7k&^I(en@!0");
    }

    internal static string Encrypt(string textToEncrypt, string passPhrase, string saltValue, HashAlgorithm hashAlgorithm, int passwordIterations, string initVector)
    {
      return SymmetricKey.EncryptOrDecrypt("encrypt", textToEncrypt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector);
    }

    internal static string Decrypt(string cipherTextToDecrypt, string passPhrase, string saltValue, HashAlgorithm hashAlgorithm, int passwordIterations, string initVector)
    {
      return SymmetricKey.EncryptOrDecrypt("decrypt", cipherTextToDecrypt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector);
    }

    private static string EncryptOrDecrypt(string operation, string textToEncryptOrDecrypt, string passPhrase, string saltValue, HashAlgorithm hashAlgorithm, int passwordIterations, string initVector)
    {
      if (!string.IsNullOrEmpty(textToEncryptOrDecrypt) && !string.IsNullOrEmpty(passPhrase) && (!string.IsNullOrEmpty(saltValue) && !string.IsNullOrEmpty(initVector)))
      {
        if (passwordIterations >= 1)
        {
          try
          {
            byte[] bytes1 = Encoding.ASCII.GetBytes(initVector);
            byte[] bytes2 = Encoding.ASCII.GetBytes(saltValue);
            string strHashName = "SHA1";
            byte[] bytes3 = new PasswordDeriveBytes(passPhrase, bytes2, strHashName, passwordIterations).GetBytes(32);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            if (operation == "encrypt")
            {
              byte[] bytes4 = Encoding.UTF8.GetBytes(textToEncryptOrDecrypt);
              ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes3, bytes1);
              MemoryStream memoryStream = new MemoryStream();
              CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
              cryptoStream.Write(bytes4, 0, bytes4.Length);
              cryptoStream.FlushFinalBlock();
              byte[] array = memoryStream.ToArray();
              memoryStream.Close();
              cryptoStream.Close();
              return Convert.ToBase64String(array);
            }
            byte[] buffer = Convert.FromBase64String(textToEncryptOrDecrypt);
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes3, bytes1);
            MemoryStream memoryStream1 = new MemoryStream(buffer);
            CryptoStream cryptoStream1 = new CryptoStream((Stream) memoryStream1, decryptor, CryptoStreamMode.Read);
            byte[] numArray = new byte[buffer.Length];
            int count = cryptoStream1.Read(numArray, 0, numArray.Length);
            memoryStream1.Close();
            cryptoStream1.Close();
            return Encoding.UTF8.GetString(numArray, 0, count);
          }
          catch
          {
            return "Invalid Text";
          }
        }
      }
      return "Invalid Parameter";
    }
  }
}
