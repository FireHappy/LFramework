namespace Assets.LFramework.Core
{
    public interface IBase
    {
        void InitOnStart();
        void InitOnEnable();
        void InitOnAwake();
        void InitOnDisable();
        void InitOnDestroy();
    }
}