using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

#pragma warning disable CS0649

namespace DogScaffold
{
    /// <summary>
    /// PlayableDirectorExtension is a script that extends
    /// the playable director. Other components can get information
    /// and event callbacks about the PlayableDirector through
    /// this component.
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayableDirectorExtension : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        //[BoxGroup("Events")]
        private UnityEvent m_onTimelineStopped;

        private PlayableDirector m_playableDirector;
        #endregion

        #region Main Methods
        private void OnEnable()
        {
            m_playableDirector = GetComponent<PlayableDirector>();

            m_playableDirector.stopped -= TimelineStopped;
            m_playableDirector.stopped += TimelineStopped;
        }

        public void Update()
        {
            #if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.P))
            {
                m_playableDirector?.Stop();
            }
            #endif
        }

        private void OnDisable()
        {
            m_playableDirector.stopped -= TimelineStopped;
        }

        private void TimelineStopped(PlayableDirector director)
        {
            m_onTimelineStopped?.Invoke();
        }
        #endregion
    }
}
#pragma warning restore CS0649
