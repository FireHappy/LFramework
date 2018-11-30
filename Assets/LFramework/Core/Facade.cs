using UnityEngine;

namespace Assets.LFramework.Core
{
    public class Facade
    {
        private static GameObject m_GameManager;       

        private GameObject AppGameManager
        {
            get { return m_GameManager ?? (m_GameManager = GameObject.FindGameObjectWithTag("GameController")); }
        }

        /// <summary>
        /// 添加管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddManager<T>() where T : Component
        {
            AppGameManager.AddComponent<T>();
            return AppGameManager.GetComponent<T>();
        }      
    }
}