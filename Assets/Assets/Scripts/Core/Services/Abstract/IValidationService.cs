using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Core.Services.Abstract
{
    public interface IValidationService
    {
        public UniTask Validate();
    }
}