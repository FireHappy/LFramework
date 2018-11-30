using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.LFramework.ConstDefine;
using Assets.LFramework.Singleton;
using Assets.LFramework.Utility;

namespace Assets.LFramework.Network.Data.Reader
{
    /// <summary>
    /// 用来区分数据
    /// </summary>
    public class NetConstValue
    {

    }


    /// <summary>
    /// 读取数据的类型
    /// </summary>
    public enum ReaderInfoType
    {

    }

    /// <summary>
    /// 网络数据读取工厂
    /// </summary>
    public class NetReaderFactory : Singleton<NetReaderFactory>
    {
        /// <summary>
        /// 存储从服务器得到的信息
        /// </summary>
        public Dictionary<Type, object> NetModelDict = new Dictionary<Type, object>();

        /// <summary>
        /// 所需加载的资源的Url，加载完成资源就会清空
        /// </summary>
        public Dictionary<string, string> NetResUrls = new Dictionary<string, string>();

        /// <summary>
        /// 数据缓存
        /// </summary>
        public Dictionary<string, object> ModelDict = new Dictionary<string, object>();

        /// <summary>
        /// 向字典中添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void AddInfoToFactory<T>(object obj)
        {
            Dictionary<Type, object> modelDict = Instance().NetModelDict;
            if (modelDict.ContainsKey(typeof(T)))
            {
                modelDict.Remove(typeof(T));
            }
            modelDict.Add(typeof(T), obj);
        }

        /// <summary>
        /// 从数据缓存中取得东东
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetInfoByName(string name)
        {
            object model = null;
            ModelDict.TryGetValue(name, out model);
            return model;
        }

        /// <summary>
        /// 使用Object可能会增加拆箱和装箱的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public object GetInfoByType<T>(string name = null)
        {
            object model;
            NetModelDict.TryGetValue(typeof(T), out model);
            if (!string.IsNullOrEmpty(name))
            {
                //将数据加入到本地缓存中
                if (ModelDict.ContainsKey(name))
                {
                    DebugUtil.Log("字典中已经存在key:" + name);
                    ModelDict.Remove(name);
                }
                ModelDict.Add(name, model);
                NetModelDict.Remove(typeof(T));
            }
            return model;
        }

        /// <summary>
        /// 清理缓存信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearCacheInfoByType<T>()
        {
            if (NetModelDict.ContainsKey(typeof(T)))
            {
                NetModelDict.Remove(typeof(T));
            }
            else
            {
                DebugUtil.Log("字典中不存在key:" + typeof(T));
            }
        }

        /// <summary>
        /// 清理所有的缓存信息
        /// </summary>
        public void CleanAllCacheInfo()
        {
            NetModelDict.Clear();
        }

        public void ReaderInfo(ReaderInfoType type, Dictionary<string, object> dict, List<object> list = null)
        {
            switch (type)
            {
               
            }
        }

        /// <summary>
        /// 校验资源并返回本次所需的加载资源的Urls
        /// </summary>
        /// <param name="obj">需要加载的资源列表</param>
        private void GetNeedLoadUrls(Dictionary<string, string> obj)
        {
            Dictionary<string, string> dict = ReadResUrlConfig(NetConst.NET_RES_CONFIG_PATH);
            if (dict == null)
            {
                foreach (KeyValuePair<string, string> pair in obj)
                {
                    if (!NetResUrls.ContainsKey(pair.Key))
                    {
                        NetResUrls.Add(pair.Key, pair.Value);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> pair in obj)
                {
                    if (dict.ContainsKey(pair.Key))
                    {
                        //obj[pair.key]=  /upload/08b68297-bff5-4867-a7bb-963a7c4854f4.jpg
                        int index = pair.Value.LastIndexOf("/", StringComparison.Ordinal);
                        string fileName = NetConst.NET_RES_PATH + pair.Value.Substring(index);
                        if (dict[pair.Key] == fileName)//说明该资源没有变化，且已经下载
                        {
                            //Debug.Log("原来资源的名字：" + dict[pair.Key] + "服务器资源的名字：" + fileName);
                            //do nothing
                        }
                        else//说明资源已经更新
                        {
                            //Debug.Log("原来资源的名字：" + dict[pair.Key] + "现在资源的名字：" + fileName);
                            //1.删除配置文件中的相关信息
                            string line = pair.Key + "," + dict[pair.Key];
                            RemoveConfigInfo(NetConst.NET_RES_CONFIG_PATH, line);
                            //2.删除本地的原有资源
                            File.Delete(dict[pair.Key]);
                            NetResUrls.Add(pair.Key, pair.Value);
                        }
                    }
                    else//该资源本地不存在
                    {
                        if (!NetResUrls.ContainsKey(pair.Key))
                        {
                            NetResUrls.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }
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