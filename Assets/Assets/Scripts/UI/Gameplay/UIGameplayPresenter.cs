using System.Collections.Generic;
using Core.Services.Models;
using Cysharp.Threading.Tasks;
using UI.Abstract;

namespace UI.Gameplay
{
    public class UIGameplayPresenter : UIPresenter<UIGameplayView>
    {
        private readonly List<ClusterData> _cachedClusters = new();
        
        public UIGameplayPresenter(UIGameplayView view) : base(view)
        {
        }

        public async UniTask Initialize(ProcessedLevelData levelData)
        {
            GetAllClustersFromLevel(levelData, _cachedClusters);
            
            _view.SetClusters(_cachedClusters);
            _view.SetupWordContainers(levelData.words.Length);
            
            await UniTask.CompletedTask;
        }
        
        private void GetAllClustersFromLevel(ProcessedLevelData levelData, List<ClusterData> output)
        {
            output.Clear(); 
            
            foreach (var wordEntry in levelData.words)
            {
                output.AddRange(wordEntry.clusters);
            }
        }
    }
}