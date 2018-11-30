using System;
using System.IO;
using Assets.LFramework.Utility;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Assets.LFramework.Editor
{
    /// <summary>
    /// 加密的编辑器工具类
    /// </summary>
    public class EnCription :ScriptableWizard {

        //1.在外部找到需要加密的文件夹
        //2.读取文件夹中的所有资源，并记录文件夹的路径
        //3.读取文件夹中的资源
        //4.将文件转化为字节
        //5.加密所有资源
        //6.根据之前的资源目录将文件夹写入到StreamAsset文件中

        [MenuItem("Assets/EnCription")]
        static void CreatEnCriptionWindow()
        {
            DisplayWizard<EnCription>("资源加密","EnCription");
        }

        public string ResPath = @"E:/UnityProjects/IAClient/Assets/Resources/StreamRes";//目标资源的路径
        public string OutPath = @"E:/UnityProjects/IAClient/Assets/StreamingAssets/StreamRes";//资源输出的路径
        public string PathNode = "StreamRes";//路径的节点

        private const string resPathKey = "EnCription.ResPath";
        private const string outPathKey = "EnCription.OutPath";
        private const string pathNodeKey = "EnCription.PathNode";

        private int count = 0;//计数器
        void OnEnable()
        {
            ResPath = EditorPrefs.GetString(resPathKey,ResPath);
            OutPath = EditorPrefs.GetString(outPathKey,OutPath);
            PathNode = EditorPrefs.GetString(pathNodeKey,PathNode);
        }

        void OnWizardCreate()
        {
            Debug.Log("资源加密");
            EnCriptionFile(ResPath,OutPath,PathNode);
        }

        void OnWizardUpdate()
        {
            EditorPrefs.SetString(resPathKey,ResPath);
            EditorPrefs.SetString(outPathKey,OutPath);
            EditorPrefs.SetString(pathNodeKey,PathNode);
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        static void EnCriptionFile(string resPath,string outPath,string pathNode)  
        {
            if (string.IsNullOrEmpty(resPath))
            {
                Debug.LogError("资源路径为空");
                return;
            }
            DirectoryInfo dirInfo=new DirectoryInfo(resPath);
            if (!dirInfo.Exists)
            {
                Debug.LogError("不存在路径：" + resPath);
                return;
            }
            else//资源路径存在
            {
                FileSystemInfo[] fileSystemInfos = dirInfo.GetFileSystemInfos();
                for (int i = 0; i < fileSystemInfos.Length; i++)
                {
                    FileInfo fileInfo = fileSystemInfos[i] as FileInfo;
                    if (fileInfo != null)//文件
                    {
                        byte[] bytes = File.ReadAllBytes(fileInfo.FullName);//GetBytes(systemInfo.FullName);
                        if (bytes != null)
                        {
                            EncrytUtil.Encryption(bytes);
                            WirteFileInfo(fileInfo, outPath, pathNode, bytes);
                        }
                    }
                    else//文件夹
                    {
                        OpResByDirects(fileSystemInfos[i], outPath, pathNode);  
                    }    
                }      
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 读取资源
        /// </summary>
        static void OpResByDirects(FileSystemInfo fileSystemInfo,string outPath,string pathNode)
        {
            if (!fileSystemInfo.Exists||fileSystemInfo.Extension==".meta")
            {
                return;
            }
            DirectoryInfo directoryInfo=new DirectoryInfo(fileSystemInfo.FullName);
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            foreach (var systemInfo in fileSystemInfos)
            {
                FileInfo fileInfo=systemInfo as FileInfo;
                if (fileInfo == null)//文件夹
                {
                    OpResByDirects(systemInfo,outPath,pathNode);
                }
                else//文件
                {
                    //将文件转化为字节
                    byte[] bytes = File.ReadAllBytes(fileInfo.FullName);//GetBytes(systemInfo.FullName);
                    if (bytes != null)
                    {
                        EncrytUtil.Encryption(bytes);
                        WirteFileInfo(fileInfo, outPath, pathNode, bytes);
                    }     
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        static void WirteFileInfo(FileInfo fileInfo ,string outPath,string pathNode,byte[]bytes)
        {
            if(fileInfo.Extension==".meta")return;
            int index = fileInfo.FullName.IndexOf(pathNode); Debug.Log("PathNode:"+pathNode);//res
            Debug.Log("fileInfo.FullName.Substring(index):" + fileInfo.FullName.Substring(index));//Resources\Res\Audio\2_Audio.ogg
            string finalPath = outPath + "/" + fileInfo.FullName.Substring(index);
            string filePath = finalPath.Remove(finalPath.LastIndexOf(@"\"));
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            if (!directoryInfo.Exists) directoryInfo.Create();
            File.WriteAllBytes(finalPath, bytes);
            Debug.Log("finalPath:" + finalPath);//E:/UnityProjects/IAClient/Assets/StreamingAssets/Res/Resources\Res\2_Audio.ogg
            Debug.Log("filePath:" + fileInfo);//E:\UnityProjects\IAClient\Assets\Resources\Res\2_Audio.ogg
        }

        /// <summary>
        /// 将文件转化为字节数组(用File.GetByte代替)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static byte[] GetBytes(string filePath)
        {
            byte[] bytes;
            using (FileStream fs=new FileStream(filePath,FileMode.Open))
            {
                try
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    return bytes;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    return null;
                }   
            }   
        }
    }
}
