
using Assets.LFramework.ConstDefine;

namespace Assets.LFramework.Utility
{
    /// <summary>
    /// 打印的工具类
    /// </summary>
    public class DebugUtil
    {        
        public static void Log(object obj)  
        {
            if(AppConst.IS_DEBUG)
                UnityEngine.Debug.Log(obj.ToString());
        }

        public static void LogError(object obj)
        {
            if (AppConst.IS_DEBUG)
                UnityEngine.Debug.LogError(obj.ToString());
        }

        public static void LogWaring(object obj)
        {
            if (AppConst.IS_DEBUG)
                UnityEngine.Debug.LogWarning(obj.ToString());
        }
    }
}