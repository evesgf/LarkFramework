using UnityEngine;
using System.Collections;
using UnityEditor;
using SevenZip.Compression.LZMA;
using System.IO;
using System;

// 压缩或解压文件
public class CompressAndDecompress : Editor
{

    // 压缩Unity3D文件
    [MenuItem("Compress/Compress Unity3D File")]
    static void CompressUnity3DFile()
    {
        string path = EditorUtility.OpenFilePanel("Compress file", "", "unity3d");
        CompressFile(path);
    }

    // 压缩Lua文件
    [MenuItem("Compress/Compress Lua File")]
    static void CompressLuaFile()
    {
        string path = EditorUtility.OpenFilePanel("Compress file", "", "lua");
        CompressFile(path);
    }

    // 压缩文件
    static void CompressFile(string path)
    {
        if (path.Length != 0)
        {
            int lastDotIndex = path.LastIndexOf(".");
            string outputPath = path.Substring(0, lastDotIndex + 1) + "zip";
            CompressFileLZMA(path, outputPath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("压缩文件", "文件已压缩完成", "确认");
        }
    }

    // 解压Unity3D文件
    [MenuItem("Compress/Decompress Unity3D File")]
    static void DecompressUnity3DFile()
    {
        DecompressFile("unity3d");
    }

    // 解压Lua文件
    [MenuItem("Compress/Decompress Lua File")]
    static void DecompressLuaFile()
    {
        DecompressFile("lua");
    }

    static void DecompressFile(string extension)
    {
        string path = EditorUtility.OpenFilePanel("Decompress file", "", "zip");

        if (path.Length != 0)
        {
            int lastDotIndex = path.LastIndexOf(".");
            string outputPath = path.Substring(0, lastDotIndex + 1) + extension;
            DecompressFileLZMA(path, outputPath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("解压文件", "文件已解压完成", "确认");
        }
    }

    // 压缩文件夹中的文件
    [MenuItem("Compress/Compress Files In Directory")]
    static void CompressFilesInDirectory()
    {
        string path = EditorUtility.OpenFolderPanel("Compress Files In Directory", "", "");

        if (path.Length != 0)
        {
            DirectoryInfo source = new DirectoryInfo(path);
            DirectoryInfo target = new DirectoryInfo(path + "_compress");

            if (target.Exists)
            {
                target.Delete(true);
            }

            target.Create();
            target.Refresh();

            // 复制源文件夹到目标文件夹
            CopyAll(source, target);
            ListFiles((FileSystemInfo)target, true);
            EditorUtility.DisplayDialog("压缩文件夹", "文件夹中所有的文件已压缩完成", "确认");
        }
    }

    // 解压文件夹中的文件
    //[MenuItem("Compress/Decompress Files In Directory")]
    static void DecompressFilesInDirectory()
    {
        string path = EditorUtility.OpenFolderPanel("Decompress Files In Directory", "", "");

        if (path.Length != 0)
        {
            DirectoryInfo source = new DirectoryInfo(path);
            DirectoryInfo target = new DirectoryInfo(path + "_decompress");

            if (target.Exists)
            {
                target.Delete(true);
            }

            target.Create();
            target.Refresh();

            // 复制源文件夹到目标文件夹
            CopyAll(source, target);
            ListFiles(target, false);
            EditorUtility.DisplayDialog("解压文件夹", "文件夹中所有的文件已解压完成", "确认");
        }
    }

    // 使用LZMA算法压缩文件
    private static void CompressFileLZMA(string inFile, string outFile)
    {
        Encoder coder = new Encoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        coder.WriteCoderProperties(output);

        byte[] data = BitConverter.GetBytes(input.Length);

        output.Write(data, 0, data.Length);

        coder.Code(input, output, input.Length, -1, null);
        output.Flush();
        output.Close();
        input.Close();
    }

    // 使用LZMA算法解压文件
    private static void DecompressFileLZMA(string inFile, string outFile)
    {
        Decoder coder = new Decoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        byte[] properties = new byte[5];
        input.Read(properties, 0, 5);

        byte[] fileLengthBytes = new byte[8];
        input.Read(fileLengthBytes, 0, 8);
        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

        coder.SetDecoderProperties(properties);
        coder.Code(input, output, input.Length, fileLength, null);
        output.Flush();
        output.Close();
        input.Close();
    }

    // 复制源文件夹到目标文件夹
    public static void CopyAll(DirectoryInfo source, DirectoryInfo target, ArrayList extensions = null)
    {
        if (source.FullName.ToLower() == target.FullName.ToLower())
        {
            return;
        }

        // 检测目标文件是否存在，如果不存在，则创建
        if (!Directory.Exists(target.FullName))
        {
            Directory.CreateDirectory(target.FullName);
            target.Refresh();
        }

        // 复制文件至目标文件夹
        foreach (FileInfo fi in source.GetFiles())
        {
            if (extensions == null || extensions.Count == 0)
            {
                if (fi.Extension != ".meta")
                {
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
            }
            else
            {
                if (extensions.Contains(fi.Extension))
                {
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
            }

        }

        // 使用递归复制子文件夹
        foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
        {
            DirectoryInfo targetSubDir = target.CreateSubdirectory(sourceSubDir.Name);
            CopyAll(sourceSubDir, targetSubDir, extensions);
        }
    }

    // 遍历文件夹中的所有文件，压缩或解压缩
    public static void ListFiles(FileSystemInfo info, bool isCompress)
    {
        if (!info.Exists)
        {
            return;
        }

        DirectoryInfo dir = info as DirectoryInfo;

        // 不是目录
        if (dir == null)
        {
            return;
        }

        FileSystemInfo[] files = dir.GetFileSystemInfos();

        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;

            // 是文件
            if (file != null)
            {
                string fullName = file.FullName;

                // 如果是压缩文件
                if (isCompress)
                {
                    if (file.Extension == ".unity3d" || file.Extension == ".lua")
                    {
                        string outputPath = file.FullName.Replace(file.Extension, ".zip");
                        CompressFileLZMA(fullName, outputPath);
                        File.Delete(fullName);
                    }
                }
                // 如果是解压文件
                else
                {
                    if (file.Extension == ".zip")
                    {
                        string outputPath = file.FullName.Replace(file.Extension, ".unity3d");
                        DecompressFileLZMA(fullName, outputPath);
                        File.Delete(fullName);
                    }
                }

            }
            else
            {
                ListFiles(files[i], isCompress);
            }
        }
    }
}