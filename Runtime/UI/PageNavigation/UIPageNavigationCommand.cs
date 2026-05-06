
using System;

namespace Theblueway.Core.UI.PageNavigation
{
    public class UIPageNavigationCommand<TNavigationParams>
    {
        public Action<TNavigationParams> _command;

        public UIPageNavigationCommand(Action<TNavigationParams> command)
        {
            _command = command;
        }

        public void Execute(TNavigationParams navigationParams)
        {
            _command?.Invoke(navigationParams);
        }
    }
}
