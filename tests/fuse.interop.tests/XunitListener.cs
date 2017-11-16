using System.Diagnostics;
using Xunit.Abstractions;

namespace fuse.interop.tests
{
    class XunitListener : TraceListener
    {
        private readonly ITestOutputHelper _output;

        public XunitListener(ITestOutputHelper output)
        {
            _output = output;
        }
        public override void Write(string message)
        {
            _output.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            _output.WriteLine(message);
        }
    }
}
