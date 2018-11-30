namespace Assets.LFramework.Interface
{
    public interface IHttpCallBackListener
    {
        void OnResponse(string response);
        void OnFailed(string error);
    }
}