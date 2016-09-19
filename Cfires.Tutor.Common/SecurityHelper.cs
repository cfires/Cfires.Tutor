using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cfires.Tutor.Common
{
    /// <summary>
    /// 加密算方法帮助类
    /// </summary>
    public sealed class SecurityHelper
    {
        private const string Key = "edc2f9b8e9435321";

        #region AES

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="inputString">被加密的明文</param>
        /// <param name="sKey">密钥</param>
        /// <returns>密文</returns>
        public static string EncryptAES(string inputString, string sKey = Key)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            byte[] encrypted;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                aesAlg.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(inputString);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="inputString">密钥</param>
        /// <param name="sKey">向量</param>
        /// <returns>明文</returns>
        public static string DecryptAES(string inputString, string sKey = Key)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            string plaintext = null;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                aesAlg.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(inputString)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        #endregion

        #region MD5

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <returns>密文</returns>
        public static string EncryptMD5(string inputString)
        {
            //创建MD5对象
            MD5 md5 = new MD5CryptoServiceProvider();

            //将字符串转为字节数组，加密
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            //将字节数组转换为16进制字符串
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }

        #endregion
    }
}
