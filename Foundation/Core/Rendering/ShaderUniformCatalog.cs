namespace Foundation.Core.Rendering
{
    public class ShaderUniformCatalog
    {
        public int NormalMatrix { get; set; }

        public int ModelMatrix { get; set; }

        public int ViewMatrix { get; set; }

        public int ProjectionMatrix { get; set; }

        public int Alpha { get; set; }

        public int UseDiffuseMap { get; set; }

        public int DiffuseMap {get;set;}

        public int DiffuseColor { get; set; }
    }
}
