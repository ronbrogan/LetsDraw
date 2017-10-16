namespace Core.Rendering.Enums
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
