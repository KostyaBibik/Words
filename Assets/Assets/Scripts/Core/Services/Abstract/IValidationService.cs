using Cysharp.Threading.Tasks;
using UniRx;

namespace Assets.Scripts.Core.Services.Abstract
{
    public interface IValidationService
    {
        public UniTask Validate();
        public IReadOnlyReactiveProperty<bool> ValidationStatus { get; }
    }
}