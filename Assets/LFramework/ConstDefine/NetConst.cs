
using UnityEngine;

namespace Assets.LFramework.ConstDefine
{
    /// <summary>
    /// 网络的配置参数
    /// </summary>
    public class NetConst
    {
        public static readonly string URL_HOST = "http://a.zjtaixue.com/api/";//服务器地址
        public static readonly string URL_RES_HOST = "http://a.zjtaixue.com/api/";//资源的ip地址    
        public static readonly string NET_RES_PATH = Application.streamingAssetsPath + "/"; //网络资源的缓存路径
        public static readonly string NET_RES_CONFIG_PATH = "";//资源的配置文件地址

        /// <summary>
        /// 服务器协议
        /// </summary>
        public const string SUCCESS = "1000";//成功
        public const string USERNAME_PASSWORD_WRONG = "1001";//用户名或者密码错误
        public const string PARAMS_WRONG = "1002";//参数错误
        public const string FUCTION_NO_OPEN = "1003";//功能未开通

        /// <summary>
        /// 服务器数据缓存的分区
        /// </summary>
        public const string TASK_BOOK = "taskbook";
        public const string GUIDE_BOOK = "guidebook";
        public const string VIDEO = "video";
        public const string SHAPER = "shaper";
        public const string ICONURL = "icon";
        public const string YMJ_DRAWING = "ymj_drawing";
        public const string GJXL_DRAWING = "gjxl_drawing";
    }
}