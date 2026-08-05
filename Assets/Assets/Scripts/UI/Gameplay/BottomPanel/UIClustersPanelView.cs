using System;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Gameplay.Elements;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.ClustersPanel
{
    public sealed class UIClustersPanelView : UIView, IClusterDropZone
    {
        [SerializeField] private ClusterPanelSettings _clusterPanelSettings;
        [Tooltip("Optional stagger played once the clusters have been spawned.")]
        [SerializeField] private UI.Juice.UIStaggerReveal _clustersReveal;

        public ClusterPanelSettings ClusterPanelSettings => _clusterPanelSettings;

        private readonly Subject<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            _onTryDrop = new();

        public UIClustersPanelPresenter Presenter { get; private set; }

        public IObservable<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            OnTryDrop => _onTryDrop;

        public void Initialize(UIClustersPanelPresenter presenter) =>
            Presenter = presenter;

        public void PlayIntro()
        {
            if (_clustersReveal != null)
                _clustersReveal.Play();
        }

        public async UniTask<bool> TryDrop(UIClusterElementView cluster, PointerEventData eventData)
        {
            var taskSource = new UniTaskCompletionSource<bool>();
            _onTryDrop.OnNext((cluster, eventData, taskSource));
            return await taskSource.Task;
        }

        private void OnDestroy()
        {
            _onTryDrop?.OnCompleted();
        }
    }
}