using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foundation.Core.Primitives;
using Foundation.Data.Enums;
using OpenTK;

namespace Foundation.Loaders
{
    public class MtlLoader
    {
        public List<Material> Materials = new List<Material>();

        public MtlLoader(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(l => l.Trim()).Where(l => !l.StartsWith("#")).Select(t => t.ReduceWhitespace().Replace('\t', ' '));

            Material currentMaterial = new Material(null);

            foreach (var parts in lines.Select(line => line.Split(' ')))
            {
                switch (parts[0])
                {
                    case "newmtl":
                        if(currentMaterial.MaterialName != null)
                        {
                            Materials.Add(currentMaterial);
                        }
                        currentMaterial = new Material(parts[1]);
                        break;

                    case "Ka":
                        currentMaterial.AmbientColor = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        break;

                    case "Kd":
                        currentMaterial.DiffuseColor = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        break;

                    case "Ks":
                        currentMaterial.SpecularColor = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        break;

                    case "Ke":
                        // Unused, Emissive color?
                        break;

                    case "d":
                        currentMaterial.Transparency = 1 - float.Parse(parts[1]);
                        break;

                    case "Tr":
                        currentMaterial.Transparency = float.Parse(parts[1]);
                        break;

                    case "illum":
                        currentMaterial.IlluminationModel = (IlluminationModel)int.Parse(parts[1]);
                        break;

                    case "tf":
                        currentMaterial.TransmissionFilter = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        break;

                    case "Ns":
                        currentMaterial.SpecularExponent = float.Parse(parts[1]);
                        break;

                    case "sharpness":
                        // Unused
                        break;

                    case "Ni":
                        currentMaterial.IndexOfRefraction = float.Parse(parts[1]);
                        break;

                    case "map_Ka":
                        currentMaterial.AmbientMap = ParseMap(Path.GetDirectoryName(filePath), string.Join(" ", parts.Skip(1)));
                        break;

                    case "map_Kd":
                        currentMaterial.DiffuseMap = ParseMap(Path.GetDirectoryName(filePath), string.Join(" ", parts.Skip(1)));
                        break;

                    case "map_Ks":
                        currentMaterial.SpecularMap = ParseMap(Path.GetDirectoryName(filePath), string.Join(" ", parts.Skip(1)));
                        break;

                    case "map_Ns":
                        currentMaterial.SpecularHighlightMap = ParseMap(Path.GetDirectoryName(filePath), string.Join(" ", parts.Skip(1)));
                        break;

                    case "map_d":
                        currentMaterial.AlphaMap = ParseMap(Path.GetDirectoryName(filePath), string.Join(" ", parts.Skip(1)));
                        break;

                    case "map_bump":
                    case "bump":
                        currentMaterial.BumpMap = ParseMap(Path.GetDirectoryName(filePath), string.Join(" ", parts.Skip(1)));
                        break;

                }
            }
            Materials.Add(currentMaterial);
        }

        private TextureMap ParseMap(string basePath, string mapOptions)
        {
            //var parts = mapOptions.Split(' ');
            var path = basePath;

            //if (parts.Length == 1)
            //{
            //    path = Path.Combine(path, parts[0]);
            //}
            //else
            //{
            //    path = Path.Combine(path, parts[parts.Length - 1]);
            //}

            path = Path.Combine(path, mapOptions);

            var map = new TextureMap(path);

            // TODO: Implement map options

            return map;
        }
    }
}
