using UnityEngine;
using UnityEngine.Events;

namespace DogScaffold
{
    /// <summary>
    /// InvokeInSeconds will invoke a unity event
    /// in a specied amount of time. This can be 
    /// great for quickly timing things together
    /// in the Inspector.
    /// </summary>
    public class InvokeInSeconds : MonoBehaviour 
    {
        #region Private Variables
        [SerializeField]
        private float m_time = 0f;

        [SerializeField]
        private UnityEvent m_onTimingFinished = default(UnityEvent);

        private float m_timePassed = 0f;
        private TimingState m_state = TimingState.IDLE;
        #endregion

        #region Main Methods
        public void BeginTiming()
        {
            if (m_state != TimingState.IDLE) return;
            Reset();
            m_state = TimingState.TIMING;
        }

        private void Update()
        {
            if (m_state != TimingState.TIMING) return;
            m_timePassed += Time.deltaTime;
            CheckTimePassedThreshhold();
        }

        #endregion

        #region Utility Methods
        private void Reset()
        {
            m_timePassed = 0f;
        }

        private void CheckTimePassedThreshhold()
        {
            if (m_timePassed < m_time) return;
            m_timePassed = 0f;
            m_state = TimingState.IDLE;
            m_onTimingFinished?.Invoke();
        }
        #endregion
    }

    public enum TimingState
    {
        IDLE,
        TIMING
    }
}
