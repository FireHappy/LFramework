using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.LFramework.Utility
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Util
    {
        #region 自动匹配组件

        private static void FindComponets(Transform transform, List<FieldInfo> infos, object obj)
        {
            foreach (Transform child in transform)
            {
                for (var i = 0; i < infos.Count; i++)
                {
                    var fieldName = infos[i].ToString().Split(' ')[1];
                    if (child.name == fieldName)
                    {
                        string[] names = infos[i].FieldType.ToString().Split('.');
                        string typeName = names[names.Length - 1];
                        infos[i].SetValue(obj, child.GetComponent(typeName));
                        infos.Remove(infos[i]);
                    }
                }
                if (child.childCount > 0)
                {
                    FindComponets(child, infos, obj);
                }
            }
        }

        /// <summary>
        /// 自动匹配组件
        /// </summary>
        public static void AutoFindComponent(Transform tsf, object obj, bool isPublic = true)
        {
            var type = obj.GetType();
            List<FieldInfo> infos;
            if (isPublic)
            {
                infos = type.GetFields().ToList(); //找到类中的所有的公共字段
            }
            else
            {
                infos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).ToList(); //找到类中的所有的私有字段
            }
            FindComponets(tsf, infos, obj);
            for (int i = 0; i < infos.Count; i++)
            {
                if (infos[i].GetValue(obj) == null)
                {
                    Debug.Log(string.Format("{0},{1},没有找到目标组件", obj, infos[i].ToString().Split(' ')[1]));
                }
            }
        }
        #endregion


        /// <summary>
        /// 将rectTsf归0
        /// </summary>
        /// <param name="tsf"></param>
        public static void SetRectTransformInfo(Transform tsf)
        {
            RectTransform rectTsf = tsf.GetComponent<RectTransform>();
            rectTsf.localScale = Vector3.one;
            rectTsf.offsetMax = Vector2.one;
            rectTsf.offsetMin = Vector2.zero;
            rectTsf.anchoredPosition = Vector2.zero;
            rectTsf.anchorMax = Vector2.one;
            rectTsf.anchorMin = Vector2.zero;
        }
      

        public static int Int(object o) {
            return Convert.ToInt32(o);
        }

        public static float Float(object o) {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o) {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max) {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max) {
            return UnityEngine.Random.Range(min, max);
        }

        public static long GetTime() {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component {
            if (go != null) {
                Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T Get<T>(Transform go, string subnode) where T : Component {
            if (go != null) {
                Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        public static T Get<T>(Component go, string subnode) where T : Component {
            return go.transform.Find(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component {
            if (go != null) {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++) {
                    if (ts[i] != null) Object.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode) {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(Transform go, string subnode) {
            Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode) {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode) {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string source) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++) {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file) {
            try {
                FileStream fs = new FileStream(file, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(i.ToString("x2"));
                }
                return sb.ToString();
            } catch (Exception ex) {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go) {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--) {
                Object.Destroy(go.GetChild(i).gameObject);
            }
        }


        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path) {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable {
            get {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi {
            get {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 通过图片数据得到精灵
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Sprite GetSprite(byte[] bytes)
        {
            Texture2D texture = new Texture2D(100, 100);
            texture.LoadImage(bytes);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        /// <summary>
        /// 判断一个点是否在一个矩形区域内
        /// </summary>
        /// <returns></returns>
        public static bool PointInRect(Vector2 rectCenterPoint,float rectHeight,float rectWidth,Vector2 targetPoint)
        {
            bool isInside = true;
            if (Mathf.Abs(targetPoint.x-rectCenterPoint.x)*2<=rectWidth&&Mathf.Abs(targetPoint.y-rectCenterPoint.y)*2<=rectHeight)
            {
                isInside = true;
            }
            else
            {
                isInside = false;
            }
            return isInside;
        }
    }    
}