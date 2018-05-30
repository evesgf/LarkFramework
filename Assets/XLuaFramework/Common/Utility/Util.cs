using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Util
{
    public static string LuaPath
    {
        get
        {
            if (ResManager.UpdaeMode)
            {
                return DataPath;
            }
            else
            {
                return GetRelativePath() + "Assets/_Project/";
            }
        }
    }

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
                    return GetRelativePath() + "Assets/AssetsPackage/";
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

    /// <summary>
    /// 复制文件夹
    /// </summary>
    /// <param name="srcPath">目标文件夹</param>
    /// <param name="destPath">目标路径</param>
    public static void CopyDirectory(string srcPath, string destPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)     //判断是否文件夹
                {
                    if (!Directory.Exists(destPath +"/"+ i.Name))
                    {
                        Directory.CreateDirectory(destPath + "/" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }
                    CopyDirectory(i.FullName, destPath + "/" + i.Name);    //递归调用复制子文件夹
                }
                else
                {
                    File.Copy(i.FullName, destPath + "/" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
