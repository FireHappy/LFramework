using System;
using UnityEngine;

namespace Assets.LFramework.Singleton
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance()
        {
            if (Singleton<T>._instance == null)
            {
                Singleton<T>._instance = Activator.CreateInstance<T>();
                if (Singleton<T>._instance == null)
                {
                    Debug.LogError("Failed to create the instance of " + typeof(T) + " as singleton!");
                }
            }
            return Singleton<T>._instance;
        }
        public static void Release()
        {
            if (Singleton<T>._instance != null)
            {
                Singleton<T>._instance = (T)((object)null);
            }
        }
    }

    public class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        private static GameObject m_container;

        public static T Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_container = new GameObject();
                    m_container.name = typeof(T).ToString();
                    m_instance = m_container.AddComponent(typeof(T)) as T;
                }
                return m_instance;
            }
        }
    }
}