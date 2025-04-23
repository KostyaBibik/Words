using Enums;
using UnityEngine;

namespace Gameplay.Data.Audio
{
    [System.Serializable]
    public struct SoundClip
    {
        public ESoundType Type;
        public AudioClip Clip;
    }
}