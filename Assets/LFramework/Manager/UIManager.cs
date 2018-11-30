using System.Collections.Generic;
using Assets.LFramework.ConstDefine;
using Assets.LFramework.Core;
using UnityEngine;

namespace Assets.LFramework.Manager
{
    public class UIManager:Base
    {
        public Transform UIParent;

        public override void InitOnAwake()
        {
            UIParent = GameObject.FindGameObjectWithTag("UIParent").transform;
            if (UIParent == null)
            {
                Debug.LogError("场景中找不到UIParent");
            }           
        }

        public override void InitOnStart()
        {
            //创建LoginUI
            //CreatePanel<LoginPanel>();
        }

        /// <summary>
        /// 创建Item
        /// </summary>
        public T CreateItem<T>(Transform parent)    
        {
            GameObject go;
            go = Resources.Load<GameObject>(AppConst.ITEM_PATH + "/" + typeof (T).Name);
            if (go != null)
            {
                go.SetActive(true);
                Transform tsf = Instantiate(go,parent).transform;
                tsf.gameObject.name = typeof (T).Name;
                return tsf.GetComponent<T>();
            }
            else
            {
                Debug.LogWarning("找不到加载的路径：" + AppConst.ITEM_PATH+ "/" + typeof(T).Name);
                return default(T);
            }
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialog">是否是对话框,如果是对话框则不会隐藏上层界面</param>
        /// <returns></returns>
        public T CreatePanel<T>(bool dialog=false)  
        {
            if (dialog == false&&UIParent.childCount>=1)
            {
                //将之前的界面设置未false                
                Transform tsf= UIParent.GetChild(UIParent.childCount - 1);
                if(tsf!=null)tsf.gameObject.SetActive(false);
            }
            GameObject go;
            go = Resources.Load<GameObject>(AppConst.PANEL_PATH + "/" + typeof (T).Name);
            if (go != null)
            {
                go.SetActive(true);
                Transform tsf = Instantiate(go,UIParent).transform;
                tsf.gameObject.name = typeof(T).Name;
                return tsf.GetComponent<T>();
            }
            else
            {
                Debug.LogWarning("找不到加载的路径：" + AppConst.PANEL_PATH + "/" +typeof(T).Name);
                return default(T);
            }
        }


        /// <summary>
        /// 退出UI
        /// </summary>
        public void Exit()
        {
            if (UIParent.childCount>0)
            {
                DestroyImmediate(UIParent.GetChild(UIParent.childCount-1).gameObject);
            }
            //将上一个界面的UI设置为true
            Debug.Log(UIParent.childCount);
            Transform ui = FindTopChild(UIParent);
            if (ui != null)
            {
                ui.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 退出UI
        /// </summary>
        /// <param name="uiname"></param> 
        public void Exit(string uiname)
        {
            Transform tsf = UIParent.Find(uiname);
            if (tsf != null)
            {
                DestroyImmediate(tsf.gameObject);
            }
            Debug.Log(UIParent.childCount);
            //将上一个界面的UI设置为true
            Transform ui = FindTopChild(UIParent);
            if (ui!=null)
            {
                ui.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 找到位于栈顶的UI
        /// </summary>
        /// <param name="tsf"></param>
        /// <returns></returns>
        private Transform FindTopChild(Transform tsf)
        {
            if (tsf.childCount > 0)
            {
                return tsf.GetChild(tsf.childCount - 1);
            }
            return null;
        }

        /// <summary>
        /// 退出所有的UI
        /// </summary>
        public void AllExit()
        {
            foreach (Transform tsf in UIParent)
            {
                Destroy(tsf.gameObject);
            }
        }

        /// <summary>
        /// 从Parent中获得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Transform GetUIFromParent<T>()
        {
            Transform tsf = UIParent.Find(typeof(T).Name + "(Clone)");
            return tsf;
        }
    }
}