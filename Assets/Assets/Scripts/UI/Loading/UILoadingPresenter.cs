using Scripts.Enums;
using UI.Abstract;
using UniRx;
using VYandexTools.Localization.Scripts;

namespace UI.Loading
{
    public sealed class UILoadingPresenter : UIPresenter<UILoadingView>
    {
        private const string INITIALIZING_TEXT = "Initializing...";
        private const string ASSETS_LOADING_TEXT = "Loading assets...";
        private const string CONFIGS_LOADING_TEXT = "Loading configurations...";
        private const string CONFIGS_PROCESSING_TEXT = "Processing data...";
        private const string AUDIO_LOADING_TEXT = "Loading sounds...";
        private const string COMPLETED_TEXT = "Completed!";
        private const string FAILED_TEXT = "Loading error";

        private readonly CompositeDisposable _disposables = new();
        
        public UILoadingPresenter(UILoadingView view) : base(view) 
        {
        }
        
        public void TrackProgress(IReadOnlyReactiveProperty<ELoadPhase> currentPhase)
        {
            currentPhase
                .Subscribe(UpdateView)
                .AddTo(_disposables);
        }

        private void UpdateView(ELoadPhase phase)
        {
            var description = GetPhaseInfo(phase);
            _view.UpdateProgress(description);
        }

        private string GetPhaseInfo(ELoadPhase phase)
        {
            return phase switch
            {
                ELoadPhase.AssetsLoading => Loc.Text(LocalizationKey.loading_assets, ASSETS_LOADING_TEXT),
                ELoadPhase.ConfigsLoading => Loc.Text(LocalizationKey.loading_configs, CONFIGS_LOADING_TEXT),
                ELoadPhase.ConfigsProcessing => Loc.Text(LocalizationKey.loading_processing, CONFIGS_PROCESSING_TEXT),
                ELoadPhase.AudioLoading => Loc.Text(LocalizationKey.loading_audio, AUDIO_LOADING_TEXT),
                ELoadPhase.Completed => Loc.Text(LocalizationKey.loading_completed, COMPLETED_TEXT),
                ELoadPhase.Failed => Loc.Text(LocalizationKey.loading_failed, FAILED_TEXT),
                _ => Loc.Text(LocalizationKey.loading_initializing, INITIALIZING_TEXT)
            };
        }

        public override void Dispose() =>
            _disposables?.Dispose();
    }
}