using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Core
{
    public interface IHudElement
    {
        void Draw();

        void Update();
        void Update(double deltaTime);

        void SetShader(int ProgramHandle);
    }
}
