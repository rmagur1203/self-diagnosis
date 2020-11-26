using Java.Security;
using Java.Security.Spec;
using Java.Util;
using Javax.Crypto;
using Java.Interop;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDiagnosisLibrary
{
    public class Encrypt
    {
        public static async Task<string> encryptAsync(string value, string key)
        {
            //return encode(value, key);
            JObject obj = new JObject();
            obj.Add("cipherType", "RSA/ECB/PKCS1Padding");
            obj.Add("keyType", "publicKeyForEncryption");
            obj.Add("privateKey", key);
            obj.Add("publicKey", key);
            obj.Add("textToEncrypt", value);
            string data = await Http.PostJson("https://www.devglan.com/online-tools/rsa-encrypt", obj.ToString());
            JObject res = JObject.Parse(data);
            return res["encryptedOutput"].ToString();
        }
        /*
        public static String encode(String plainData, String stringPublicKey)
        {
            String encryptedData = null;
            try
            {
                KeyFactory keyFactory = KeyFactory.GetInstance("RSA");
                byte[] bytePublicKey = Base64.GetDecoder().Decode(Encoding.UTF8.GetBytes(stringPublicKey));
                X509EncodedKeySpec publicKeySpec = new X509EncodedKeySpec(bytePublicKey);
                var publicKey = keyFactory.GeneratePublic(publicKeySpec);
                Cipher cipher = Cipher.GetInstance("RSA");
                cipher.Init(CipherMode.EncryptMode, publicKey);
                byte[] byteEncryptedData = cipher.DoFinal(Encoding.UTF8.GetBytes(plainData));
                encryptedData = Base64.GetEncoder().EncodeToString(byteEncryptedData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return encryptedData;
        }
        */

        /*
        public static string encrypt(string value, string key)
        {
            var rsaParams = DecodeX509PublicKey(Convert.FromBase64String(key)).ExportParameters(false);
            return encrypt(value, rsaParams);
        }
        public static string encrypt(string value, RSAParameters parameters)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(value);
            byte[] encryptedData = rsa.Encrypt(dataToEncrypt, true);
            return Convert.ToBase64String(encryptedData);
        }

        public static string decrypt(string value, RSAParameters parameters)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            byte[] dataToDecrypt = System.Convert.FromBase64String(value);
            byte[] decryptedData = rsa.Decrypt(dataToDecrypt, true);
            string sDec = (new UTF8Encoding()).GetString(decryptedData, 0, decryptedData.Length);
            return sDec;
        }

        public static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
        {
            byte[] SeqOID = { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };

            MemoryStream ms = new MemoryStream(x509key);
            BinaryReader reader = new BinaryReader(ms);

            if (reader.ReadByte() == 0x30)
                ReadASNLength(reader); //skip the size
            else
                return null;

            int identifierSize = 0; //total length of Object Identifier section
            if (reader.ReadByte() == 0x30)
                identifierSize = ReadASNLength(reader);
            else
                return null;

            if (reader.ReadByte() == 0x06) //is the next element an object identifier?
            {
                int oidLength = ReadASNLength(reader);
                byte[] oidBytes = new byte[oidLength];
                reader.Read(oidBytes, 0, oidBytes.Length);
                if (oidBytes.SequenceEqual(SeqOID) == false) //is the object identifier rsaEncryption PKCS#1?
                    return null;

                int remainingBytes = identifierSize - 2 - oidBytes.Length;
                reader.ReadBytes(remainingBytes);
            }

            if (reader.ReadByte() == 0x03) //is the next element a bit string?
            {
                ReadASNLength(reader); //skip the size
                reader.ReadByte(); //skip unused bits indicator
                if (reader.ReadByte() == 0x30)
                {
                    ReadASNLength(reader); //skip the size
                    if (reader.ReadByte() == 0x02) //is it an integer?
                    {
                        int modulusSize = ReadASNLength(reader);
                        byte[] modulus = new byte[modulusSize];
                        reader.Read(modulus, 0, modulus.Length);
                        if (modulus[0] == 0x00) //strip off the first byte if it's 0
                        {
                            byte[] tempModulus = new byte[modulus.Length - 1];
                            Array.Copy(modulus, 1, tempModulus, 0, modulus.Length - 1);
                            modulus = tempModulus;
                        }

                        if (reader.ReadByte() == 0x02) //is it an integer?
                        {
                            int exponentSize = ReadASNLength(reader);
                            byte[] exponent = new byte[exponentSize];
                            reader.Read(exponent, 0, exponent.Length);

                            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                            RSAParameters RSAKeyInfo = new RSAParameters();
                            RSAKeyInfo.Modulus = modulus;
                            RSAKeyInfo.Exponent = exponent;
                            RSA.ImportParameters(RSAKeyInfo);

                            return RSA;
                        }
                    }
                }
            }
            return null;
        }
        public static int ReadASNLength(BinaryReader reader)
        {
            //Note: this method only reads lengths up to 4 bytes long as
            //this is satisfactory for the majority of situations.
            int length = reader.ReadByte();
            if ((length & 0x00000080) == 0x00000080) //is the length greater than 1 byte
            {
                int count = length & 0x0000000f;
                byte[] lengthBytes = new byte[4];
                reader.Read(lengthBytes, 4 - count, count);
                Array.Reverse(lengthBytes); //
                length = BitConverter.ToInt32(lengthBytes, 0);
            }
            return length;
        }
        */
    }
}
