
namespace Theblueway.Core.UI.PageNavigation
{
    public interface IPageNavigator<TNavigationParams>
    {
        public void YieldControl(TNavigationParams navigationParams);
        public void GainControl(TNavigationParams navigationParams);
        public void TakeBackControl(TNavigationParams navigationParams);
        public void ReturnControl(TNavigationParams navigationParams);
    }
}
