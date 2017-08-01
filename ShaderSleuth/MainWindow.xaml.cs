using Foundation.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
//using InputManager = Foundation.Managers.InputManager;
using Foundation.World;
using Foundation.Rendering.Models;
using Foundation.Core.Rendering;
using Foundation.World.Cameras;
using Foundation.Managers;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Xml;

namespace ShaderSleuth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GLControl glControl { get; set; }
        private Engine engine { get; set; }
        private DateTime lastMeasure { get; set; }

        private Mesh TestMesh { get; set; }

        private int msaaSamples = 8;


        public MainWindow()
        {
            InitializeComponent();
            InitializeGlControl();
        }
        
        private void InitializeGlControl()
        {
            glControl = new GLControl(new GraphicsMode(32, 24, 0, msaaSamples));
            glControl.CreateControl();
            glControl.MakeCurrent();

            engine = new Engine(glControl.Size);

            SetupEngine();
            BindEngineToControl();

            glControl.Dock = DockStyle.Fill;
            glControl.Paint += glControl_Paint;

            lastMeasure = DateTime.Now;
            engine.Start();

            this.WinformsHost.Child = glControl;

            var shaderScene = new Scene();

            var testObject = new StaticScenery();
            TestMesh = PrimitiveGenerator.GenerateOctahedron(50);
            testObject.Meshes.Add(TestMesh);

            shaderScene.Scenery.Add(testObject);
            shaderScene.Camera = new FpCamera(new Vector3(0, 40, 40));
            shaderScene.Camera.Pitch = 0.5f;
            //shaderScene.Skybox = new Skybox();

            engine.LoadScene(shaderScene);
        }

        private void SetupEngine()
        {
            glControl.VSync = false;

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        private void BindEngineToControl()
        {
            glControl.Resize += engine.Resize;
            engine.SwapBuffers += Engine_SwapBuffers;

            glControl.KeyDown += InputManager.NotifyKeyDown;
            glControl.KeyUp += InputManager.NotifyKeyUp;
            glControl.MouseMove += InputManager.NotifyMouse;
            glControl.MouseDown += InputManager.NotifyMouseDown;
            glControl.MouseUp += InputManager.NotifyMouseUp;
        }

        private void Engine_SwapBuffers(object sender, EventArgs e)
        {
            glControl.SwapBuffers();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            // Rendering in dispatch queue to allow UI updates
            WinformsHost.Dispatcher.InvokeAsync(() =>
            {
                var now = DateTime.Now;
                var elapsed = (now - lastMeasure).TotalSeconds;
                if (elapsed <= 0d)
                    elapsed = 0.000001;

                engine.Render(sender, new FrameEventArgs(elapsed));

                // Immediately invalidate state, force repaint ASAP
                ((GLControl)sender).Invalidate();

                lastMeasure = now;

            }, DispatcherPriority.Render);
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
                System.Windows.MessageBox.Show("Shader compilation failed.");
                return;
            }
                

            TestMesh.ShaderOverride = shaderHandle;


        }
    }
}
