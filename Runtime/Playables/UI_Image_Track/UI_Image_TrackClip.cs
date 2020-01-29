using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DogScaffold
{
    [Serializable]
    public class UI_Image_TrackClip : PlayableAsset, ITimelineClipAsset
    {
        //[HideLabel]
        public UI_Image_TrackBehaviour template = new UI_Image_TrackBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UI_Image_TrackBehaviour>.Create(graph, template);
            UI_Image_TrackBehaviour clone = playable.GetBehaviour();
            return playable;
        }
    }
}
