using Enums;
using Gameplay.Data.Audio;

namespace Core.Services
{
    public interface IAudioService
    {
        public void PlaySound(ESoundType type, float volumeScale = 1f);
        public void SwapSoundsActiveStatus();
        void SetSettings(AudioSettings settings);
    }
}