using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.LFramework.ConstDefine;
using Assets.LFramework.Core;
using Assets.LFramework.Utility;
using UnityEngine;

namespace Assets.LFramework.Manager
{
    public class WWWLoadManager:Base
    {
        /// <summary>
        /// 加载的进度
        /// </summary>
        public float DownLoadProgress { get; set; }

        /// <summary>
        /// old
        /// </summary>
        private float oldProgress { get; set; }

        /// <summary>
        /// 开始下载资源
        /// </summary>
        public void StartDownLoadRes(Dictionary<string, string> urls, MonoBehaviour behaviour, Action finish)
        {
            behaviour.StartCoroutine(DownLoadSaveResource(GetNeedLoadUrls(urls), finish));
        }

        /// <summary>
        /// 开始加载本地的资源
        /// </summary>
        public void StartLoadRes(string url, MonoBehaviour behaviour, Action<byte[]> action)
        {
            behaviour.StartCoroutine(LoadLocalResource(url, action));
        }

        /// <summary>
        /// 下载并且保存资源，实时写入配置文件   
        /// </summary>
        private IEnumerator DownLoadSaveResource(Dictionary<string, string>urls,Action finish)
        {
            DownLoadProgress = 0;
            StreamWriter sw = File.AppendText(NetConst.NET_RES_PATH+"/config.bytes");
            foreach (KeyValuePair<string,string>pair in urls)
            {
                using (WWW www = new WWW(NetConst.URL_RES_HOST+pair.Value))
                {
                    while (!www.isDone)
                    {
                        oldProgress = www.progress;
                        yield return 1;
                        DownLoadProgress += (www.progress-oldProgress) / urls.Count;
                    }
                    yield return www;
                    if (www.isDone && www.error == null)
                    {
                        //加密资源
                        int index = pair.Value.LastIndexOf("/", StringComparison.Ordinal);
                        string fileName = pair.Value.Substring(index);
                        string resPath = NetConst.NET_RES_PATH+fileName;
                        File.WriteAllBytes(resPath, EncrytUtil.Encryption(www.bytes));//写入文件
                        //将加载的资源写入配置文件中
                        string line = pair.Key + "," + resPath;
                        sw.WriteLine(line);
                    }
                    else
                    {
                        Debug.LogError(www.error);
                    }
                }
            }
            sw.Close();
            sw.Dispose();
            urls.Clear();
            DownLoadProgress = 1;
            if (finish != null) finish();
        }

        /// <summary>
        /// 加载本地资源
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadLocalResource(string url,Action<byte[]> action)
        {
            string path = "file:///" + NetConst.NET_RES_PATH + url.Substring(url.LastIndexOf("/", StringComparison.Ordinal)+1);
            //Debug.Log("下载本地资源的路径是："+path);
            using (WWW www = new WWW(path))
            {
                yield return www;
                if (www.isDone && www.error == null)
                {
                    if (action != null) action(EncrytUtil.Decryption(www.bytes));
                }
                else
                {
                    Debug.Log(www.error);
                }
            }         
        }


        /// <summary>
        /// 校验资源并返回本次所需的加载资源的Urls
        /// </summary>
        private Dictionary<string,string> GetNeedLoadUrls(Dictionary<string, string> resUrls)
        {
            string configPath = NetConst.NET_RES_PATH+"/config.bytes";//资源目录配置文件
            string resPath = NetConst.NET_RES_PATH;
            Dictionary<string,string> needLoadUrls=new Dictionary<string, string>();
            Dictionary<string, string> localUrls = ReadResUrlConfig(configPath);//configPath=nodeid_nodeName/config.bytes
            if (localUrls == null)
            {
                foreach (KeyValuePair<string, string> pair in resUrls)
                {
                    if (!needLoadUrls.ContainsKey(pair.Key))
                    {
                        needLoadUrls.Add(pair.Key, pair.Value);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> pair in resUrls)
                {
                    if (localUrls.ContainsKey(pair.Key))
                    {
                        //obj[pair.key]=  /upload/08b68297-bff5-4867-a7bb-963a7c4854f4.jpg
                        int index = pair.Value.LastIndexOf("/", StringComparison.Ordinal);
                        string fileName = resPath + pair.Value.Substring(index);
                        if (localUrls[pair.Key] == fileName)//说明该资源没有变化，且已经下载
                        {
                            continue;
                        }
                        else//说明资源已经更新
                        {
                            //1.删除配置文件中的相关信息
                            string line = pair.Key + "," + localUrls[pair.Key];
                            RemoveConfigInfo(configPath, line);
                            //2.删除本地的原有资源
                            File.Delete(localUrls[pair.Key]);
                            needLoadUrls.Add(pair.Key, pair.Value);
                        }
                    }
                    else//该资源本地不存在
                    {
                        if (!needLoadUrls.ContainsKey(pair.Key))
                        {
                            needLoadUrls.Add(pair.Key, pair.Value);
                        }
                    }
                }              
            }
            return needLoadUrls;
        }

        /// <summary>
        /// 读取本地资源的Url配置文件
        /// </summary>
        private Dictionary<string, string> ReadResUrlConfig(string path)
        {
            if (!File.Exists(path)) return null;
            List<string> lines = new List<string>(File.ReadAllLines(path));//先读取到内存变量
            return (from line in lines where !string.IsNullOrEmpty(line) select line.Split(',')).ToDictionary(proArray => proArray[0], proArray => proArray[1]);
        }

        /// <summary>
        /// 移除配置文件中的相关信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        private void RemoveConfigInfo(string path, string value)
        {
            List<string> lines = new List<string>(File.ReadAllLines(path));
            lines.Remove(value);
            File.WriteAllLines(path, lines.ToArray());
        }
    }    
}