using Fuse.Interop.FuseOpt;

namespace fuse.interop.tests
{
    internal class UIntSetter : Setter
    {
        private readonly uint _expectedValue;
        private uint _result;

        public UIntSetter(string template, uint expectedValue, string args) : base(template, args)
        {
            _expectedValue = expectedValue;
        }

        public override bool Success => _expectedValue == _result;

        public override void Init(FuseOptions options)
        {
            options.Add(Template, u => _result = u);
        }
    }
}