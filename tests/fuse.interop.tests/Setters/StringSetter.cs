using Fuse.Interop.FuseOpt;

namespace fuse.interop.tests
{
    internal class StringSetter : Setter
    {
        private readonly string _expectedValue;
        private string _result;

        public StringSetter(string template, string expectedValue, string args) : base(template, args)
        {
            _expectedValue = expectedValue;
        }

        public override bool Success => _result == _expectedValue;

        public override void Init(FuseOptions options)
        {
            options.Add(Template, s => _result = s);
        }
    }
}