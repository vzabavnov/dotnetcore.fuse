using System.Diagnostics;
using Fuse.Interop.FuseOpt;

namespace fuse.interop.tests
{
    internal class FlagSetter : Setter
    {
        private bool _result;

        public FlagSetter(string template, string args) : base(template, args)
        {
            Trace.WriteLine($"Creates Glag Setter: template: \"{template}\", args:\"{args}\"");
        }

        public override bool Success => _result;

        public override void Init(FuseOptions options)
        {
            options.Add(Template, () =>
            {
                Trace.WriteLine("FlagSetter.Action called");
                _result = true;
            });
        }
    }
}