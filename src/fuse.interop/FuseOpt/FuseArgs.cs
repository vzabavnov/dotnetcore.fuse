using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class FuseArgs : IDisposable, IList<string>
    {
        /** Argument count */
        internal int argc;

        /** Argument vector.  NULL terminated */
        internal IntPtr argv;

        /** Is 'argv' allocated? */
        internal int allocated;

        public FuseArgs()
        {
            
        }

        public FuseArgs(IReadOnlyList<string> args)
        {
            allocated = 1;

            argc = args.Count;

            argv = Marshal.AllocHGlobal(IntPtr.Size * (argc + 1));

            for (var i = 0; i < argc; i++)
            {
                var p = Marshal.StringToHGlobalAnsi(args[i]);
                Marshal.WriteIntPtr(argv, IntPtr.Size * i, p);
            }

            Marshal.WriteIntPtr(argv, argc * IntPtr.Size, IntPtr.Zero);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Clear();
        }

        /// <inheritdoc />
        public IEnumerator<string> GetEnumerator()
        {
            if (argv != IntPtr.Zero)
            {
                for (var i = 0; ; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    if (p == IntPtr.Zero)
                    {
                        break;
                    }

                    yield return Marshal.PtrToStringAnsi(p);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(string item)
        {
            Functions.FuseOptAddArg(this, item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (argv != IntPtr.Zero)
            {
                Functions.FuseOptFreeAgs(this);

                argv = IntPtr.Zero;
                argc = 0;
                allocated = 0;
            }
        }

        /// <inheritdoc />
        public bool Contains(string item)
        {
            return this.Any(s => s == item);
        }

        /// <inheritdoc />
        public void CopyTo(string[] array, int arrayIndex)
        {
            foreach (var s in this)
            {
                if (arrayIndex < array.Length)
                {
                    array[arrayIndex] = s;
                    arrayIndex++;
                }
                else
                {
                    break;
                }
            }
        }

        /// <inheritdoc />
        public bool Remove(string item)
        {
            var idx = IndexOf(item);
            if (idx != -1)
            {
                RemoveAt(idx);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public int Count => argc;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(string item)
        {
            int idx = 0;
            foreach (var s in this)
            {
                if (s == item)
                {
                    return idx;
                }

                idx++;
            }

            return -1;
        }

        /// <inheritdoc />
        public void Insert(int index, string item)
        {
            Functions.FuseOptInsertArg(this, index, item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            Marshal.FreeHGlobal(Marshal.ReadIntPtr(argv, index * IntPtr.Size));

            for (int i = index; i < argc; i++)
            {
                var p = Marshal.ReadIntPtr(argv, (i + 1) * IntPtr.Size);

                Marshal.WriteIntPtr(argv, i * IntPtr.Size, p);
            }

            argc--;
            argv = Marshal.ReAllocHGlobal(argv, new IntPtr(argc * IntPtr.Size));
        }

        /// <inheritdoc />
        public string this[int index]
        {
            get
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                return Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(argv, index * IntPtr.Size));
            }
            set
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                var off = index * IntPtr.Size;
                Marshal.FreeHGlobal(Marshal.ReadIntPtr(argv, off));
                Marshal.WriteIntPtr(argv, off, Marshal.StringToHGlobalAnsi(value));
            }
        }
    }
}
