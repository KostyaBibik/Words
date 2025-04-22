using Enums;

namespace Core.Services
{
    public interface IAudioService
    {
        public void PlaySound(ESoundType type, float volumeScale = 1f);
    }
}