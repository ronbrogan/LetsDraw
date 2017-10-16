using System;
using System.Linq;
using System.Windows;
using Foundation.World;
using Foundation.Rendering.Models;
using Foundation.Managers;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Xml;
using Core.Primitives;
using Foundation;
using System.Numerics;
using Foundation.Rendering.Cameras;

namespace ShaderSleuth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Engine engine { get; set; }

        private Mesh TestMesh { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeScene();
        }
        
        private void InitializeScene()
        {
            engine = WinformsHost.CreateEngine();

            var shaderScene = new Scene();

            var testObject = new StaticScenery();
            TestMesh = PrimitiveGenerator.GenerateOctahedron(50);
            testObject.Meshes.Add(TestMesh);

            shaderScene.Scenery.Add(testObject);
            shaderScene.Camera = new FpCamera(new Vector3(0, 40, 40));
            shaderScene.Camera.Pitch = 0.5f;

            engine.LoadScene(shaderScene);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RandomShader_Click(object sender, RoutedEventArgs e)
        {
            var shaders = ShaderManager.AllShaders().Where(s => s != ShaderManager.GetShaderForMaterial(TestMesh.Material));

            var rand = new Random();
            var index = rand.Next(0, shaders.Count());

            TestMesh.ShaderOverride = shaders.Skip(index).Take(1).First();
        }

        private void GlslEditor_Initialized(object sender, EventArgs e)
        {
            var file = Path.Combine(Environment.CurrentDirectory, "SyntaxHighlighting", "glsl.xshd");

            using (var s = new StreamReader(file))
            using (XmlTextReader reader = new XmlTextReader(s))
            {
                ((TextEditor)sender).SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }

        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            int shaderHandle;

            var shaderGood = ShaderManager.TryCompileShader(vertexEditor.Text, fragmentEditor.Text, out shaderHandle);

            if (!shaderGood)
            {
                MessageBox.Show("Shader compilation failed.");
                return;
            }
                
            TestMesh.ShaderOverride = shaderHandle;
        }
    }
}
