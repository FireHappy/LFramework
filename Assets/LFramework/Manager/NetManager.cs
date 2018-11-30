using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.LFramework.ConstDefine;
using Assets.LFramework.Core;
using Assets.LFramework.Interface;
using Assets.LFramework.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.LFramework.Manager
{
    public class NetManager:Base
    {   
        private string sessionId = "";	    
    
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="code">请求类型</param>
        /// <param name="postData"></param>
        /// <param name="listener"></param>
        public void SendHttpRequest(RequestCode code,Dictionary<string,object> postData  ,IHttpCallBackListener listener)
        {
            string url = NetConst.URL_HOST +code.ToString().ToLower();
            url += code.ToString().ToLower();
            StartCoroutine(Post(url,postData,listener));
        }

        /// <summary>
        /// 返回一个字典PostData
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetPostData() 
        {
            Dictionary<string, object> postData = new Dictionary<string, object> {{"sessionid", sessionId}};
            return postData;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <returns></returns>
        private IEnumerator Post(string url,Dictionary<string,object> postData  ,IHttpCallBackListener listener)
        {
            string temp = url;
            if (postData.ContainsKey("sessionid"))
            {
                string sessionid = (string)postData["sessionid"];
                temp += ";jsessionid=" + sessionid;
            }
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string json = JsonConvert.SerializeObject(postData, Formatting.Indented, jSetting);//过滤掉空值
            Encoding encoding = new UTF8Encoding();
            Debug.Log("请求的URl地址是:"+temp);
            using (WWW www = new WWW(temp, encoding.GetBytes(json)))
            {              
                yield return www;
                if (www.isDone && www.error == null)
                {                    
                    listener.OnResponse(www.text);
                }
                else
                {
                    listener.OnFailed(www.error);
                }
            }
        }

        /// <summary>
        /// 服务器返回错误参数提示调用
        /// </summary>
        /// <param name="resCode"></param>
        public void OnResponse(string resCode)
        {           
            switch (resCode)
            {
                case NetConst.FUCTION_NO_OPEN://学校未开通该功能
                    break;
                case NetConst.PARAMS_WRONG://参数错误
                    break;
                case NetConst.USERNAME_PASSWORD_WRONG://用户名或密码错误
                    break;
            }
        }
    }
}