// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

using UnityEngine;

public sealed class CryptoHelper {

    private CryptoHelper(){
        
    }
    
    #region DES
    private static byte[] desKeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    public const int CRYPTO_KEY_LENGTH = 8;
    /// <summary>
    /// DES encryption
    /// </summary>
    /// <param name="encryptString">encrption string for</param>
    /// <param name="encryptKey">required: 8bit</param>
    /// <param name="errorMsg">for errorMsg</param>
    /// <returns>succeed: encrypted String; failed: source string</returns>
    public static string EncryptDES(string sourceString, string encryptKey, ErrorMsg errorMsg)
    {
        string result = sourceString;

        if (sourceString == null || encryptKey == null || encryptKey.Length < CRYPTO_KEY_LENGTH){
            errorMsg.Code = ErrorCode.IllegalParam;
            errorMsg.Msg = "string encrpt get illegal encryptString or encryptKey";
            return result;
        }

        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, CRYPTO_KEY_LENGTH));
            byte[] rgbIV = desKeys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            result = Convert.ToBase64String(mStream.ToArray());
            errorMsg.Code = ErrorCode.Succeed;
        }
        catch
        {
            errorMsg.Code = ErrorCode.Encrypt;
            LogHelper.Log(errorMsg);
        }
        return result;
    }
    
    /// <summary>
    /// DES decryption
    /// </summary>
    /// <param name="decryptString">encrypted string</param>
    /// <param name="decryptKey">decryptKey, 8bit, same as encryptKey</param>
    /// <param name="errorMsg">for errorMsg</param>
    /// <returns>succeed: decrypted string; failed: source string</returns>
    public static string DecryptDES(string sourceString, string encryptKey, ErrorMsg errorMsg)
    {
        string result = sourceString;

        if (sourceString == null || encryptKey == null || encryptKey.Length < CRYPTO_KEY_LENGTH){
            errorMsg.Code = ErrorCode.IllegalParam;
            errorMsg.Msg = "string encrpt get illegal decryptString or decryptKey";
            return result;
        }

        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
            byte[] rgbIV = desKeys;
            byte[] inputByteArray = Convert.FromBase64String(sourceString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            result = Encoding.UTF8.GetString(mStream.ToArray());
            errorMsg.Code = ErrorCode.Succeed;
        }
        catch
        {
            errorMsg.Code = ErrorCode.Decrypt;
            errorMsg.Msg = "DES encryptError";
            LogHelper.Log(errorMsg);
        }
        return result;
    }
    #endregion

    #region RSA
    public static byte[] EncryptRSA(string sourceString, string encryptKey, ErrorMsg errorMsg){
        byte[] result = Encoding.UTF8.GetBytes(sourceString);

        if (sourceString == null || encryptKey == null){
            errorMsg.Code = ErrorCode.IllegalParam;
            errorMsg.Msg = "string encrpt get illegal encryptString or encryptKey";
            return result;
        }
        
        try
        {
            CspParameters cspParams = new CspParameters();
            
            cspParams.KeyContainerName = encryptKey;
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(cspParams);

            result = provider.Encrypt(result, true);
            errorMsg.Code = ErrorCode.Succeed;
        }
        catch
        {
            errorMsg.Code = ErrorCode.Encrypt;
            errorMsg.Msg = "RSA encryptError";
            LogHelper.Log(errorMsg);
        }
        return result;
    }

    public string DecryptRSA(byte[] source, string encryptKey, ErrorMsg errorMsg){
        string result = ConvertHelper.BytesToString(source);
        
        if (result == null || encryptKey == null){
            errorMsg.Code = ErrorCode.IllegalParam;
            errorMsg.Msg = "string encrpt get illegal decryptString or decryptKey";
            return result;
        }
        
        try
        {
            CspParameters cspParams = new CspParameters();
            
            cspParams.KeyContainerName = encryptKey;
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(cspParams);

            result = System.Text.Encoding.UTF8.GetString(
                provider.Decrypt(source, true));    
            errorMsg.Code = ErrorCode.Succeed;
        }
        catch
        {
            errorMsg.Code = ErrorCode.Decrypt;
            errorMsg.Msg = "RSA encryptError";
            LogHelper.Log(errorMsg);
        }
        return result;
    }
    #endregion

    #region MD5
    //32 bit
    public string GetMD5_32(string s, string _input_charset) 
    { 
        MD5 md5 = new MD5CryptoServiceProvider(); 
        byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s)); 
        StringBuilder sb = new StringBuilder(32); 
        for (int i = 0; i < t.Length; i++) 
        { 
            sb.Append(t[i].ToString("x").PadLeft(2, '0')); 
        } 
        return sb.ToString(); 
    } 
    
    // 16 bit
    public static string GetMd5_16(string ConvertString) 
    { 
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); 
        string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8); 
        t2 = t2.Replace("-", ""); 
        return t2; 
    }
    #endregion
}