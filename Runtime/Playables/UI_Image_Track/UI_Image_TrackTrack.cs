using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace DogScaffold
{
    [TrackColor(0.9716981f, 0.9440051f, 0.3162602f)]
    [TrackClipType(typeof(UI_Image_TrackClip))]
    [TrackBindingType(typeof(Image))]
    public class UI_Image_TrackTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<UI_Image_TrackMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
