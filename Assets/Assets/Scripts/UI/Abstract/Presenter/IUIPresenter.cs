namespace UI.Abstract
{
    public interface IUIPresenter
    {
        void Show(bool instant = true);
        void Hide(bool instant = true);
    }
}