using UI.Abstract;

namespace UI.ErrorLoading
{
    public sealed class UIErrorLoadingPresenter : UIPresenter<UIErrorLoadingView>
    {
        public UIErrorLoadingPresenter(UIErrorLoadingView view) : base(view) 
        {
        }
    }
}