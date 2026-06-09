using UnityEngine;

namespace Nullbytes
{
    public abstract class InjectableMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            TryInject();
        }

        protected void TryInject()
        {
            if (EntryPoint.Instance?.GameManager?.Container != null)
            {
                Injector.Inject(this, EntryPoint.Instance.GameManager.Container);
            }
            else
            {
                Logger.Log(this.GetType(), LogType.Error, "TryInject Error");
            }
        }
    }
}