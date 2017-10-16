using System.Diagnostics;

namespace WPFCommon.Misc
{

    public class DebugTraceListener : TraceListener
    {
        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
            if (Debugger.IsAttached)
                Debug.WriteLine(message);
        }
    }
}
