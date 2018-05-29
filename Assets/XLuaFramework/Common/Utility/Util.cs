using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Util
{
    public static string DataPath
    {
        get
        {
            string appName = Application.productName;
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + appName + "/";
            }
            else
            {
                if (ResManager.UpdaeMode)
                {
                    return "c:/" + appName + "/";
                }
                else
                {
                    return GetRelativePath()+ "Assets/" + ResManager.ABPath+"/";
                }
            }
        }
    }

    /// <summary>
    /// 获取绝对路径
    /// </summary>
    /// <returns></returns>
    public static string GetRelativePath()
    {
        //编辑器模式
        if (Application.isEditor)
            return System.Environment.CurrentDirectory.Replace("\\", "/") + "/";
        return null;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string Md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file("+file+") fail, error:" + ex.Message);
        }
    }
}
