using System;
using System.Drawing;
using OpenTK;

namespace Foundation.Rendering.HUD
{
    public class FramesDisplay : TextDisplay
    {
        private int lastSecond;

        public FramesDisplay(Size screenSize, float originX, float originY, string text = "", int fontSize = 36, Brush color = null) : base(screenSize, originX, originY, text, fontSize, color)
        {
        }

        public FramesDisplay(Size screenSize, Vector2 origin, string text = "", int fontSize = 36, Brush color = null) : base(screenSize, origin, text, fontSize, color)
        {
        }

        public override void Update(double deltaTime)
        {
            if (lastSecond == DateTime.Now.Second)
                return;

            lastSecond = DateTime.Now.Second;
            RegenTexture = true;
            Text = (1 / deltaTime).ToString("0.0") + " fps";
        }

        public void Update(string textOverride)
        {
            Text = textOverride;
        }

    }
}
