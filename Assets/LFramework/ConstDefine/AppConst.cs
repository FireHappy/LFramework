namespace Assets.LFramework.ConstDefine
{
    public class AppConst
    {
        /// <summary>
        /// 是否打印信息用于内部调试
        /// </summary>
        public const bool IS_DEBUG = true;

        /// <summary>
        /// UIPrefab的路径
        /// 资源的路径在Resources中
        /// </summary>
        public const string PANEL_PATH = "Prefab/UI/Panel";
        public const string ITEM_PATH = "Prefab/UI/Item";

        /// <summary>
        /// 资源加密的密钥
        /// </summary>
        public const string SECRET_KEY = "taixuesoftware";


        /// <summary>
        /// PlayerPref常量    
        /// </summary>
        public const string IS_REMENBER_PASSWORD = "0001";
        public const string USER_NAME = "0002";
        public const string PASS_WORD = "0003";      
    }   
}