using System.Linq;
using Cysharp.Threading.Tasks;
using DataBase.Models;
using UI.Abstract;

namespace UI.Gameplay
{
    public sealed class UIGameplayPresenter : UIPresenter<UIGameplayView>
    {
        public UIGameplayPresenter(UIGameplayView view) : base(view)
        {
        }

        public override void Initialize()
        {
            Hide();
        }

        public async UniTask Initialize(ProcessedLevelData levelData)
        {
            var clustersPanelPresenter = _view.ClustersPanel.Presenter;
            var wordGridPresenter = _view.WordGridView.Presenter;
            var clusters = GetAllClustersFromLevel(levelData);
            
            clustersPanelPresenter.UpdateData(clusters);
            wordGridPresenter.UpdateData(levelData.words.Length, 6);
            
            await UniTask.CompletedTask;
        }
        
        private ClusterData[] GetAllClustersFromLevel(ProcessedLevelData levelData) =>
            levelData.words
                .SelectMany(word => word.clusters)
                .ToArray();
    }
}