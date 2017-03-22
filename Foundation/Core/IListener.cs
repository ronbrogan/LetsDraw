using System;

namespace Foundation.Core
{
    public interface IListener : IDisposable
    {
        void NotifyBeginFrame(double deltaTime);
        void NotifyDisplayFrame();
        void NotifyEndFrame(EventHandler ev);

        void NotifyResize(int width, int height, int prevWidth, int prevHeight);
    }
}
