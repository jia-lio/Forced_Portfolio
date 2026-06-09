namespace Nullbytes
{
    public interface ISubManager
    {
        void Initialize(ManagerContext context);
        void CleanUp();
        void ManualUpdate();
        void ManualLateUpdate();
        void ManualFixedUpdate();
    }
}