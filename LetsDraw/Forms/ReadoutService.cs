using LetsDraw.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetsDraw.Forms
{
    public static class ReadoutService
    {
        private static Readout readout { get; set; }

        private static Thread readoutThread { get; set; }

        private static void ApplicationRunProc(object state)
        {
            Application.Run(state as Form);
        }

        static ReadoutService()
        {
            readout = new Readout();
            readout.CommandIssued += HandleReadoutCommand;

            readoutThread = new Thread(ApplicationRunProc);
            readoutThread.SetApartmentState(ApartmentState.STA);
            readoutThread.IsBackground = true;
            readoutThread.Start(readout);
        }

        private static void HandleReadoutCommand(object sender, EventArgs e)
        {
            
        }

        public static void Update(Scene scene)
        {
            if (!readout.IsHandleCreated || readout.IsDisposed || readout.Disposing)
                return;

            readout.Invoke(new Action(() => readout.UpdateData(scene)));
        }
    }
}
