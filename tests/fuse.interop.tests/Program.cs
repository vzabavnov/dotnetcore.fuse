using System;
using System.Runtime.InteropServices;
using Fuse.Interop.FuseOpt;

namespace fuse.interop.tests
{

    class Program
    {
        [DllImport("fuse.harness")]
        internal static extern long fuse_args_parse(FuseArgs args);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var a = new FuseArgs {argc = 5, argv = new IntPtr(15)};
            var result = fuse_args_parse(a);

            Console.WriteLine(result);
        }
    }
}
