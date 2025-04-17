using UI.Abstract;

namespace UI.Gameplay
{
    public sealed class UIGameplayPresenter : UIPresenter<UIGameplayView>
    {
        public UIGameplayPresenter(UIGameplayView view) : base(view) 
        {
        }
    }
}