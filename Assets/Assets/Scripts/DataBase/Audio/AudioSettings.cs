using Enums;
using UnityEngine;

namespace Gameplay.Data.Audio
{
    [CreateAssetMenu(menuName = "Audio/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        public SoundClip[] SoundClips;

        public AudioClip GetClip(ESoundType type)
        {
            foreach (var sound in SoundClips)
            {
                if (sound.Type == type)
                {
                    return sound.Clip;
                }
            }
            
            return null;
        }
    }
}