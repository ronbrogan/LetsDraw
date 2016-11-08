using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Data.Enums
{
    public enum IlluminationModel
    {
        Color,
        ColorAndAmbient,
        Highlight,
        ReflectionAndRayTrace,
        TransparencyGlassRayTraceReflection,
        ReflectionFresnelAndRayTrace,
        TransparencyRefractionRayTraceReflection,
        TransparencyRefractionFresnelAndRayTraceReflection,
        Reflection,
        TransparencyGlass,
        CastShadowsOntoInvisible
    }
}
