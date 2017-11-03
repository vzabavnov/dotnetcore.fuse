using System;
using System.Runtime.InteropServices;

namespace Fuse.Interop.FuseOpt
{
    /// <summary>
    /// Argument list 
    /// struct fuse_args {
    ///      /** Argument count */
    ///     int argc;
    ///
    ///     /** Argument vector.  NULL terminated */
    ///     char** argv;
    ///
    ///     /** Is 'argv' allocated? */
    ///     int allocated;
    /// };
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class FuseArgs
    {
        /** Argument count */
        public int argc;

        /** Argument vector.  NULL terminated */
        public IntPtr argv;

        /** Is 'argv' allocated? */
        public int allocated;
    }
}
