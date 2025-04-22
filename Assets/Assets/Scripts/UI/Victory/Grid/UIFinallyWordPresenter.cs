using UI.Abstract;
using UnityEngine;

namespace UI.Victory.Grid
{
    public sealed class UIFinallyWordPresenter : UIPresenter<UIFinallyWordView>
    {
        public UIFinallyWordPresenter(UIFinallyWordView view) : base(view)
        {
        }

        public void UpdateData(string text) =>
            _view.UpdateText(text);

        public void Destroy() =>
            Object.Destroy(_view.gameObject);
    }
}