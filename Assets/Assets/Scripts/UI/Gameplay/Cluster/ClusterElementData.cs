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

        // Where the player currently put this piece. Deliberately NOT stored on Data: that
        // ClusterData instance is the level's own answer key (see UIGameplayPresenter), so
        // writing placement into it would overwrite the very thing we validate against.
        public int PlacementOrder = -1;
        public int PlacementGroup = -1;

        public readonly List<UILetterView> Letters = new();
        
        public int LetterCount => Letters?.Count ?? 0;
    }
}