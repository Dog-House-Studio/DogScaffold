using UnityEngine;
using UnityEngine.Events;

namespace DogScaffold
{
    /// <summary>
    /// OnEnableDisable is a component helper script
    /// that will invoke a unity event in the OnEnable
    /// and OnDisable method of this MonoBehaviour.
    /// </summary>
    public class OnEnableDisable : MonoBehaviour 
    {
        #region Private Variables
        [SerializeField]
        private UnityEvent m_onEnable = default(UnityEvent);

        [SerializeField]
        private UnityEvent m_onDisable = default(UnityEvent);
        #endregion

        #region Main Methods
        private void OnEnable() => m_onEnable?.Invoke();

        private void OnDisable() => m_onDisable?.Invoke();
        #endregion
    }
}
