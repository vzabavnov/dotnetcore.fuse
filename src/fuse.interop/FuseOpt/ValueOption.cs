using System;
using Fuse.Interop.FuseOpt.Options;

namespace Fuse.Interop.FuseOpt
{
    internal class ValueOption : OptionInfo
    {
        private readonly int _initialValue;
        private readonly int _value;
        private readonly Action<int> _action;

        public ValueOption(string template, int initialValue, int value, Action<int> action) : base(template)
        {
            _initialValue = initialValue;
            _value = value;
            _action = action;
        }

        public override unsafe void Init(FuseOpt* opt, void* data, int offset)
        {
            base.Init(opt, data, offset);

            opt->value = _value;

            *(int*)((byte*)data + offset) = _initialValue;
        }

        public override unsafe void Process(FuseOpt* opt, void* data)
        {
            var value = *(int*)((byte*)data + opt->offset);
            if (value != 0)
            {
                _action(value);
            }
        }
    }
}