using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace LetsDraw.Core
{
    public interface IListener : IDisposable
    {
        void NotifyBeginFrame(double deltaTime);
        void NotifyDisplayFrame();
        void NotifyEndFrame(GameWindow game);

        void NotifyResize(int width, int height, int prevWidth, int prevHeight);
    }
}
