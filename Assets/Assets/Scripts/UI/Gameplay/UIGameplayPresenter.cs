using System.Linq;
using Core.Services;
using Cysharp.Threading.Tasks;
using DataBase.Models;
using UI.Abstract;
using UniRx;
using VYandexTools.Localization.Scripts;
using Zenject;

namespace UI.Gameplay
{
    public sealed class UIGameplayPresenter : UIPresenter<UIGameplayView>
    {
        [Inject] private readonly IWordProgressService _progressService;
        [Inject] private readonly IHintService _hintService;

        private readonly CompositeDisposable _hintDisposable = new();
        private readonly CompositeDisposable _progressDisposable = new();

        public UIGameplayPresenter(UIGameplayView view) : base(view)
        {
        }

        public override void Initialize()
        {
            _view.SetHintLabel(Loc.Text(LocalizationKey.gameplay_hint, "Подсказка"));

            SubscribeToHintButton();
            Hide();
        }

        public async UniTask Initialize(ProcessedLevelData levelData)
        {
            var clustersPanelPresenter = _view.ClustersPanel.Presenter;
            var wordGridPresenter = _view.WordGridView.Presenter;
            var clusters = GetAllClustersFromLevel(levelData);

            clustersPanelPresenter.UpdateData(clusters);
            wordGridPresenter.UpdateData(levelData.words.Length, 6);

            _view.CategoryText.text = levelData.category;
            _view.SetLevel(Loc.Format(LocalizationKey.gameplay_level_format, "Уровень {0}", levelData.id));

            TrackProgress(levelData);

            await UniTask.CompletedTask;
        }

        private void TrackProgress(ProcessedLevelData levelData)
        {
            // Containers are rebuilt for every level, so the previous subscriptions are dead.
            _progressDisposable.Clear();

            _progressService.Initialize(levelData);

            _progressService.SolvedCount
                .Subscribe(solved => _view.SetProgress(
                    Loc.Format(LocalizationKey.gameplay_progress_format, "{0} / {1}",
                        solved, _progressService.TotalCount)))
                .AddTo(_progressDisposable);
        }

        private void SubscribeToHintButton()
        {
            if (_view.HintBtn == null)
                return;

            _view.HintBtn.OnClick
                .AsObservable()
                .Subscribe(_ => _hintService.TryShowHint())
                .AddTo(_hintDisposable);
        }

        private ClusterData[] GetAllClustersFromLevel(ProcessedLevelData levelData) =>
            levelData.words
                .SelectMany(word => word.clusters)
                .ToArray();

        public override void Dispose()
        {
            _hintDisposable?.Dispose();
            _progressDisposable?.Dispose();

            base.Dispose();
        }
    }
}
