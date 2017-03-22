using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.World;

namespace Core.Core
{
    public interface ISceneChangeSubscriber
    {
        void Update(Scene scene);
    }
}
