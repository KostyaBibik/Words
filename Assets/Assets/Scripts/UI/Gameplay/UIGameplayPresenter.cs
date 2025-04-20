using System.Linq;
using Core.Services.Models;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Gameplay.BottomPanel;
using Zenject;

namespace UI.Gameplay
{
    public class UIGameplayPresenter : UIPresenter<UIGameplayView>
    {
        private UIBottomPanelPresenter _bottomPanelPresenter;
        private UIWordGridPresenter _wordGridPresenter;

        public UIGameplayPresenter(UIGameplayView view) : base(view)
        {
        }

        [Inject]
        public void Construct(UIBottomPanelPresenter bottomPanelPresenter, UIWordGridPresenter wordGridPresenter)
        {
            _bottomPanelPresenter = bottomPanelPresenter;
            _wordGridPresenter = wordGridPresenter;
        }

        public async UniTask Initialize(ProcessedLevelData levelData)
        {
            var clusters = GetAllClustersFromLevel(levelData);
            
            _bottomPanelPresenter.UpdateData(clusters);
            _wordGridPresenter.UpdateData(levelData.words.Length, 6);
            
            await UniTask.CompletedTask;
        }
        
        private ClusterData[] GetAllClustersFromLevel(ProcessedLevelData levelData) =>
            levelData.words
                .SelectMany(word => word.clusters)
                .ToArray();
    }
}