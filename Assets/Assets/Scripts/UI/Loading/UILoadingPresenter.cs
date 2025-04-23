using Scripts.Enums;
using UI.Abstract;
using UniRx;

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
                ELoadPhase.AssetsLoading => ASSETS_LOADING_TEXT,
                ELoadPhase.ConfigsLoading => CONFIGS_LOADING_TEXT,
                ELoadPhase.ConfigsProcessing => CONFIGS_PROCESSING_TEXT,
                ELoadPhase.AudioLoading => AUDIO_LOADING_TEXT,
                ELoadPhase.Completed => COMPLETED_TEXT,
                ELoadPhase.Failed => FAILED_TEXT,
                _ => INITIALIZING_TEXT
            };
        }

        public override void Dispose() =>
            _disposables?.Dispose();
    }
}