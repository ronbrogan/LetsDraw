using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;

namespace Foundation.Managers
{
    public static class InputManager
    {
        public static HashSet<Key> DownKeys = new HashSet<Key>();

        private static Dictionary<Key, DateTime> pressedKeys = new Dictionary<Key, DateTime>();

        public static bool MouseDown = false;

        public static Vector2 MousePosition = new Vector2(0, 0);

        private static int DebounceMilliseconds = 100;


        private static void HandleKeyDown(Key key)
        {
            DownKeys.Add(key);

            if (pressedKeys.ContainsKey(key))
            {
                pressedKeys.Remove(key);
            }
        }

        private static void HandleKeyUp(Key key)
        {
            if(DownKeys.Contains(key))
                pressedKeys[key] = DateTime.Now.AddMilliseconds(DebounceMilliseconds);

            DownKeys.Remove(key);
        }

        public static IEnumerable<Key> PressedKeys
        {
            get
            {
                Key[] keys = new Key[pressedKeys.Count];
                pressedKeys.Keys.CopyTo(keys, 0);
                foreach (var key in keys)
                {
                    if (pressedKeys[key] > DateTime.Now)
                        continue;

                    yield return key;
                }
            }
        }

        internal static void ProcessKey(Key key)
        {
            if (!pressedKeys.ContainsKey(key))
                return;

            pressedKeys.Remove(key);
        }


        #region OpenTK Input
        // Support for OpenTK input
        public static void NotifyKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            HandleKeyDown(e.Key);
        }

        // Support for OpenTK input
        public static void NotifyKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            HandleKeyUp(e.Key);
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

        #endregion

        #region Winforms Input
        public static void NotifyKeyUp(object sender, KeyEventArgs e)
        {
            var state = Keyboard.GetState();

            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (state.IsKeyUp(key))
                    HandleKeyUp(key);
            }
        }

        public static void NotifyKeyDown(object sender, KeyEventArgs e)
        {
            var state = Keyboard.GetState();

            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (state.IsKeyUp(key))
                    continue;

                HandleKeyDown(key);
            }
        }

        public static void NotifyMouse(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MousePosition.X = e.X;
            MousePosition.Y = e.Y;
        }

        public static void NotifyMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDown = true;
        }

        public static void NotifyMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDown = false;
        }

        #endregion
    }
}
