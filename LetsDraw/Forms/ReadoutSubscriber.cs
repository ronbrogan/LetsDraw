using System;
using System.Net.Mime;
using System.Threading;
using System.Windows.Forms;
using Foundation.Core;
using Foundation.World;

namespace LetsDraw.Forms
{
    public class ReadoutSubscriber : ISceneChangeSubscriber
    {
        private Readout readout { get; set; }

        private Thread readoutThread { get; set; }

        private void ApplicationRunProc(object state)
        {
            Application.Run(state as Form);
        }

        public ReadoutSubscriber()
        {
            readout = new Readout();
            readout.CommandIssued += HandleReadoutCommand;

            readoutThread = new Thread(ApplicationRunProc);
            readoutThread.SetApartmentState(ApartmentState.STA);
            readoutThread.IsBackground = true;
            readoutThread.Start(readout);
        }

        private void HandleReadoutCommand(object sender, EventArgs e)
        {
            
        }

        public void Update(Scene scene)
        {
            if (!readout.IsHandleCreated || readout.IsDisposed || readout.Disposing)
                return;

            readout.Invoke(new Action(() => readout.UpdateData(scene)));
        }
    }
}
