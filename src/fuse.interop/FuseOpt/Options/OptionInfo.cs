using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Fuse.Interop.FuseOpt.Options
{
    internal abstract class OptionInfo
    {
        public readonly string Template;

        protected OptionInfo(string template)
        {
            Template = template;
        }

        public virtual unsafe void Init(FuseOpt* opt, void* data, int offset)
        {
            opt->offset = (ulong)offset;
            opt->templ = Marshal.StringToHGlobalAnsi(Template);

            Trace.WriteLine($"OptionInfo.Init: offset={offset}, templ={Template}@{opt->templ}");
        }

        public abstract unsafe void Process(FuseOpt* opt, void* data);

        public virtual unsafe void Release(FuseOpt* opt, void* data)
        {
            Marshal.FreeHGlobal(opt->templ);
            Trace.WriteLine($"OptionInfo.Release: templ@{opt->templ}");
        }
    }
}