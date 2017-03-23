using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation.Core;
using Foundation.Core.Rendering;
using Foundation.Rendering.HUD;

namespace Foundation.Managers
{
    public class HudManager
    {
        private Dictionary<string, IHudElement> Elements;

        public HudManager(Size screenSize)
        {
            Elements = new Dictionary<string, IHudElement>();

            // Manually add test hud element here.
            var fpsReadout = new FramesDisplay(screenSize, -1, 1, "Test String", 72);
            fpsReadout.SetShader(ShaderManager.GetShader("HudShader"));
            Elements.Add("FpsReadout", fpsReadout);
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
            foreach(var elem in Elements.Values)
            {
                elem.Resize(width, height);
            }
        }

        public void Update(double deltaTime)
        {
            foreach (var model in Elements.Values)
            {
                model.Update(deltaTime);
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
