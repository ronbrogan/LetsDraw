using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace Core.Managers
{
    public static class InputManager
    {
        public static List<Key> DownKeys = new List<Key>();

        public static bool MouseDown = false;

        public static Vector2 MousePosition = new Vector2(0, 0);

        public static void NotifyKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (DownKeys.Contains(e.Key))
                return;

            DownKeys.Add(e.Key);
        }

        public static void NotifyKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            DownKeys.Remove(e.Key);
        }

        public static void NotifyMouse(object sender, MouseMoveEventArgs e)
        {
            MousePosition.X = e.Position.X;
            MousePosition.Y = e.Position.Y;
        }

        public static void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown = true;
        }

        public static void NotifyMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseDown = false;
        }

    }
}
