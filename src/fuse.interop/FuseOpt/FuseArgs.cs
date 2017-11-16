using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Fuse.Interop.FuseOpt
{
    ///  <summary>
    ///  Argument list 
    ///  struct fuse_args {
    ///       /** Argument count */
    ///      int argc;
    ///      /** Argument vector.  NULL terminated */
    ///      char** argv;
    ///      /** Is 'argv' allocated? */
    ///      int allocated;
    ///  };
    ///  </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct FuseArgs
    {
        /** Argument count */
        internal int argc;

        /** Argument vector.  NULL terminated */
        internal IntPtr argv;

        /** Is 'argv' allocated? */
        internal int allocated;
    }
}
