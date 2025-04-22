using UI.Abstract;

namespace UI.ErrorLoading
{
    public sealed class UIErrorLoadingPresenter : UIPresenter<UIErrorLoadingView>
    {
        public UIErrorLoadingPresenter(UIErrorLoadingView view) : base(view) 
        {
        }

        public void UpdateText(string info) =>
            _view.UpdateText(info);
    }
}