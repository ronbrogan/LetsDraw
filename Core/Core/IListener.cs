using System;
using OpenTK;

namespace Core.Core
{
    public interface IListener : IDisposable
    {
        void NotifyBeginFrame(double deltaTime);
        void NotifyDisplayFrame();
        void NotifyEndFrame(GameWindow game);

        void NotifyResize(int width, int height, int prevWidth, int prevHeight);
    }
}
