using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine;

namespace DogScaffold
{
    public class UI_Image_TrackMixerBehaviour : PlayableBehaviour
    {
        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Image trackBinding = playerData as Image;

            if (!trackBinding)
                return;

            int inputCount = playable.GetInputCount();
            Color finalColor = new Color(0f, 0f, 0f, 0f);

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<UI_Image_TrackBehaviour> inputPlayable = (ScriptPlayable<UI_Image_TrackBehaviour>)playable.GetInput(i);
                UI_Image_TrackBehaviour input = inputPlayable.GetBehaviour();

                // Use the above variables to process each frame of this playable.
                finalColor += input.ImageColor * inputWeight;
            }

            trackBinding.color = finalColor;
        }
    }
}
