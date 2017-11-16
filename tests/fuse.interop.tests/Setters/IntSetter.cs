using Fuse.Interop.FuseOpt;

namespace fuse.interop.tests
{
    internal class IntSetter : Setter
    {
        private readonly int _expectedValue;
        private int _result;

        public IntSetter(string template, int expectedValue, string args) : base(template, args)
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