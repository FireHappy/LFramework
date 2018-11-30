using Assets.LFramework.Manager;
namespace Assets.LFramework.Core
{
    public class AppFacade:Facade
    {

        private UIManager uiManager;
        public UIManager UIManager
        {
            get { return uiManager; }
        }

        private NetManager netManager;       
        public NetManager NetManager
        {
            get { return netManager; }
        }

        private WWWLoadManager wwwLoadManager;

        public WWWLoadManager WWWLoadManager
        {
            get { return wwwLoadManager; }
        }


        private AnimationManager animationManager;

        public AnimationManager AnimationManager
        {
            get { return AnimationManager; }
        }

        private static AppFacade instance;

        public static AppFacade Instance()
        {
            return instance ?? (instance = new AppFacade());
        }

        /// <summary> 
        /// APP入口
        /// </summary>
        public void InitStart()
        {
            //向GameManager添加管理器           
            uiManager=AddManager<UIManager>();
            netManager=AddManager<NetManager>();
            wwwLoadManager = AddManager<WWWLoadManager>();
            animationManager = AddManager<AnimationManager>();
        }
    }
}