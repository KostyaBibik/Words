using UI.Abstract;

namespace UI.Loading
{
    public sealed class UILoadingPresenter : UIPresenter<UILoadingView>
    {
        public UILoadingPresenter(UILoadingView view) : base(view) 
        {
        }
    }
}