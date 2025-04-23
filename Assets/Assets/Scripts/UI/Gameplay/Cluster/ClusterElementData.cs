using System.Collections.Generic;
using DataBase.Models;
using UnityEngine;

namespace UI.Gameplay.Elements
{
    public class ClusterElementData
    {
        public int OriginalSiblingIndex;
        public int GrabbedLetterIndex;
        public UIWordContainerView Container;
        public Transform OriginalParent;
        public Vector2 DragOffset;
        public ClusterData Data;
        
        public readonly List<UILetterView> Letters = new();
        
        public int LetterCount => Letters?.Count ?? 0;
    }
}