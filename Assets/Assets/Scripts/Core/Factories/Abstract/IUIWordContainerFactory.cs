﻿using System.Collections.Generic;
using Core.Services.Models;
using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UI.Victory.Grid;
using UnityEngine;

namespace Core.Factories
{
    public interface IUIWordContainerFactory
    {
        public UILetterSlotView[] CreateLetterSlots(
            UILetterSlotView letterSlotPrefab,
            Transform parentLayer,
            int count
        );

        public UIWordContainerPresenter[] CreateWordContainers(
            UIWordContainerView containerPrefab,
            Transform parentLayer,
            int letterPerWorld,
            int count
        );
        
        public UIFinallyWordPresenter[] CreateFinallyWords(
            UIFinallyWordView elementPrefab,
            Transform parentLayer,
            IReadOnlyList<ClusterData> data);
    }
}