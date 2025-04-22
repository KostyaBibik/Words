using UI.Abstract;

namespace UI.Victory.Grid
{
    public class UIFinallyWordPresenter : UIPresenter<UIFinallyWordView>
    {
        public UIFinallyWordPresenter(UIFinallyWordView view) : base(view)
        {
        }

        public void UpdateData(string text) =>
            _view.UpdateText(text);
    }
}