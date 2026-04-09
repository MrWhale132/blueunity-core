
using System.Collections.Generic;

namespace Theblueway.Core.Runtime.UI.PageNavigation
{
    public class UIPageNavigationStack<TNavigationParams>
    {
        public List<IPageNavigator<TNavigationParams>> _navStack = new ();

        public void Push(IPageNavigator<TNavigationParams> navigator, TNavigationParams navigationParams)
        {
            if (_navStack.Count > 0)
            {
                _navStack[^1].YieldControl(navigationParams);
            }
            _navStack.Add(navigator);
            navigator.GainControl(navigationParams);
        }

        public void Pop(TNavigationParams navigationParams)
        {
            if(_navStack.Count > 0)
            {
                _navStack[^1].ReturnControl(navigationParams);
            }

            _navStack.RemoveAt(_navStack.Count - 1);

            if (_navStack.Count > 0)
            {
                _navStack[^1].TakeBackControl(navigationParams);
            }
        }
    }
}
