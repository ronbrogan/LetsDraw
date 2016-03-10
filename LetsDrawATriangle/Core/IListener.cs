using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDrawATriangle.Core
{
    interface IListener : IDisposable
    {
        void NotifyBeginFrame();
        void NotifyDisplayFrame();
        void NotifyEndFrame();

        void NotifyResize(int width, int height, int prevWidth, int prevHeight);
    }
}
