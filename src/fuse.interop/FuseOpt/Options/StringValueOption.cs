using System;
using System.Runtime.InteropServices;
using Fuse.Interop.FuseOpt.Options;

namespace Fuse.Interop.FuseOpt
{
    internal class StringValueOption : OptionInfo
    {
        private readonly Action<string> _action;

        public StringValueOption(string template, Action<string> action) : base(template)
        {
            _action = action;
        }

        public override unsafe void Init(FuseOpt* opt, void* data, int offset)
        {
            base.Init(opt, data, offset);

            opt->value = 0;

            *(IntPtr*)((byte*)data + offset) = IntPtr.Zero;
        }

        public override unsafe void Process(FuseOpt* opt, void* data)
        {
            var ptr = *(IntPtr*)((byte*)data + opt->offset);
            if (ptr != IntPtr.Zero)
            {
                var str = Marshal.PtrToStringAnsi(ptr);
                _action(str);
            }
        }

        public override unsafe void Release(FuseOpt* opt, void* data)
        {
            var ptr = *(IntPtr*)((byte*)data + opt->offset);
            if (ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);
            }

            base.Release(opt, data);
        }
    }
}