using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LetsDraw.World;

namespace LetsDraw.Forms
{
    public partial class Readout : Form
    {
        Scene localScene { get; set; }

        public Readout()
        {
            InitializeComponent();
        }

        public event EventHandler CommandIssued;

        public void UpdateData(Scene scene)
        {
            localScene = scene;
            CamPosition.Text = scene.Camera.Position.ToString();
        }

        private void Readout_Load(object sender, EventArgs e)
        {

        }

        private void Readout_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var mesh in localScene.RenderQueue.MeshRegistry.Values.SelectMany(r => r))
            {
                mesh.Material.DiffuseMap = null;
                mesh.Material.DiffuseColor = new OpenTK.Vector3(0.5f);
            }
        }
    }
}
