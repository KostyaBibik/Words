using System.Linq;
using Core.Services.Models;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Gameplay.ClustersPanel;
using Zenject;

namespace UI.Gameplay
{
    public sealed class UIGameplayPresenter : UIPresenter<UIGameplayView>
    {
        private UIClustersPanelPresenter _clustersPanelPresenter;
        private UIWordGridPresenter _wordGridPresenter;

        public UIGameplayPresenter(UIGameplayView view) : base(view)
        {
        }

        [Inject]
        public void Construct(UIClustersPanelPresenter clustersPanelPresenter, UIWordGridPresenter wordGridPresenter)
        {
            _clustersPanelPresenter = clustersPanelPresenter;
            _wordGridPresenter = wordGridPresenter;
        }

        public async UniTask Initialize(ProcessedLevelData levelData)
        {
            var clusters = GetAllClustersFromLevel(levelData);
            
            _clustersPanelPresenter.UpdateData(clusters);
            _wordGridPresenter.UpdateData(levelData.words.Length, 6);
            
            await UniTask.CompletedTask;
        }
        
        private ClusterData[] GetAllClustersFromLevel(ProcessedLevelData levelData) =>
            levelData.words
                .SelectMany(word => word.clusters)
                .ToArray();
    }
}