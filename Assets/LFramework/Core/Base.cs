using UnityEngine;

namespace Assets.LFramework.Core
{
    public abstract class Base:MonoBehaviour,IBase
    {

        #region UnityEvent

        //Unity事件

        private void Awake()
        {
            InitOnAwake();
        }

        private void OnEnable()
        {
            InitOnEnable();
        }

        private void Start()
        {
            InitOnStart();
        }

        private void OnDisable()
        {
            InitOnDisable();
        }

        private void OnDestroy()
        {
            InitOnDestroy();
        }

        protected virtual void Update()
        {

        }

        #endregion

        public virtual void InitOnStart()
        {
            
        }

        public virtual void InitOnEnable()
        {
            
        }

        public virtual void InitOnAwake()
        {
            
        }

        public virtual void InitOnDisable()
        {
            
        }

        public virtual void InitOnDestroy()
        {
            
        }
    }
}