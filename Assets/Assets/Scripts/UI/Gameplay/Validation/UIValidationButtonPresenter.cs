using Assets.Scripts.Core.Services.Abstract;
using UI.Abstract;
using UniRx;

namespace UI.Gameplay.Validation
{
    public class UIValidationButtonPresenter : UIPresenter<UIValidationButtonView>
    {
        private readonly IValidationService _validationService;

        public UIValidationButtonPresenter(UIValidationButtonView view, IValidationService validationService) : base(view)
        {
            _validationService = validationService;

            _view.OnValidateClicked
                .Subscribe(_ => _validationService.Validate())
                .AddTo(_view);
        }
    }
}