using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation.Core;
using Foundation.Core.Rendering;
using Foundation.Rendering.HUD;
using OpenTK.Input;

namespace Foundation.Managers
{
    public class HudManager
    {
        private Dictionary<string, IHudElement> Elements { get; set; }
        private Size screenSize { get; set; }

        private FramesDisplay framesDisplay { get; set; }

        public HudManager(Size ScreenSize)
        {
            screenSize = ScreenSize;
            Elements = new Dictionary<string, IHudElement>();
            framesDisplay = new FramesDisplay(screenSize, -1, 1, "Test String", 72);
            framesDisplay.SetShader(ShaderManager.GetShader("HudShader"));
        }

        public void Draw()
        {
            foreach (var elem in Elements.Values)
            {
                elem.Draw();
            }
        }

        public void Resize(int width, int height)
        {
            screenSize = new Size(width, height);

            foreach(var elem in Elements.Values)
            {
                elem.Resize(width, height);
            }
        }

        public void Update(double deltaTime)
        {
            foreach (var key in InputManager.PressedKeys)
            {
                switch(key)
                {
                    case Key.F3:
                        // This is a terminal press
                        InputManager.ProcessKey(key);
                        ToggleFpsReadout();
                        break;
                }
            }

            foreach (var model in Elements.Values)
            {
                model.Update(deltaTime);
            }
        }

        private void ToggleFpsReadout()
        {
            if (Elements.ContainsKey("FpsReadout"))
            {
                Elements.Remove("FpsReadout");
            }
            else
            {
                framesDisplay.Resize(screenSize.Width, screenSize.Height);
                Elements.Add("FpsReadout", framesDisplay);
            }
        }

        public void DeleteElement(string name)
        {
            if (!Elements.ContainsKey(name))
                return;

            Elements.Remove(name);
        }

        public IHudElement GetElement(string elem)
        {
            if (!Elements.ContainsKey(elem))
                return null;

            return Elements[elem];
        }
    }
}
