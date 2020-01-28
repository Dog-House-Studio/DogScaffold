using UnityEngine;
using static DogScaffold.ServiceLocator;

namespace DogScaffold
{
    /// <summary>
    /// BaseService is an abstract base class meant
    /// for Services to inherit from to elimate any
    /// Service related boiler plate code.
    /// </summary>
    public abstract class BaseService<T> : MonoBehaviour, IService where T : IService
    {
        #region Main Methods
        public virtual void OnEnable()
        {
            RegisterService();
        }

        public virtual void OnDisable()
        {
            UnregisterService();
        }

        public void RegisterService()
        {
            string s = typeof(T).Name;
            Register(s, this);
        }

        public void UnregisterService()
        {
            string s = typeof(T).Name;
            Unregister(s, this);
        }
        #endregion
    }
}
