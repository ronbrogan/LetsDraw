using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace LetsDrawATriangle.Core
{
    interface IListener : IDisposable
    {
        void NotifyBeginFrame();
        void NotifyDisplayFrame();
        void NotifyEndFrame();

        void NotifyResize(int width, int height, int prevWidth, int prevHeight);


        void NotifyKey(object sender, KeyboardKeyEventArgs e);
        void NotifyMouse(object sender, MouseMoveEventArgs e);
        void NotifyMouseDown(object sender, MouseButtonEventArgs e);
        void NotifyMouseUp(object sender, MouseButtonEventArgs e);
    }
}
