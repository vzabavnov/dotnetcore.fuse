using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Fuse.Interop.FuseOpt.Options;

namespace Fuse.Interop.FuseOpt
{
    public class FuseOptions : IEnumerable<string>
    {
        readonly List<OptionInfo> _options = new List<OptionInfo>();

        public Func<string, int, int> ProcesingFunction { get; set; }

        /// <summary>
        /// Add text option. the <paramref template="template"/> must have format option '%s'
        /// </summary>
        /// <param name="template">The template for this option. it must contains %s</param>
        /// <param name="action"></param>
        public void Add(string template, Action<string> action)
        {
            if (template == null)
            {
                throw new NullReferenceException(nameof(template));
            }

            if (!template.Contains("%s"))
            {
                throw new ArgumentException("template must contains format '%s'", nameof(template));
            }

            Trace.WriteLine("Adding Text Option");
            _options.Add(new StringValueOption(template, action));
        }

        /// <summary>
        /// Add the flag option.
        /// when this options is match then the <paramref template="action"/> will be called with <paramref template="value"/> as an argument
        /// </summary>
        /// <param name="template">
        /// The template.
        /// the template must not have format parameter in it
        /// </param>
        /// <param name="action">the action</param>
        public void Add(string template, Action action)
        {
            if (template == null)
            {
                throw    new NullReferenceException(nameof(template));
            }

            if (action == null)
            {
                throw new NullReferenceException(nameof(action));
            }

            if (template.Contains("%"))
            {
                throw new ArgumentException("template must not contains format %", nameof(template));
            }

            _options.Add(new ValueOption(template, 0, 1, i => action()));
        }

        /// <summary>
        /// Add the int option. the <paramref template="template"/> must have format %i
        /// </summary>
        /// <param name="template"></param>
        /// <param name="action"></param>
        public void Add(string template, Action<int> action)
        {
            if (template == null)
            {
                throw new NullReferenceException(nameof(template));
            }

            if (action == null)
            {
                throw new NullReferenceException(nameof(action));
            }

            if (!template.Contains("%i"))
            {
                throw new ArgumentException("template must contain format %i", nameof(template));
            }

            Trace.WriteLine("Adding numberic OptiongOption");

            _options.Add(new ValueOption(template, 0, 0, action));
        }

        /// <summary>
        /// Add the int option. the <paramref template="template"/> must have format %i
        /// </summary>
        /// <param name="template"></param>
        /// <param name="action"></param>
        public void Add(string template, Action<uint> action)
        {
            if (template == null)
            {
                throw new NullReferenceException(nameof(template));
            }

            if (action == null)
            {
                throw new NullReferenceException(nameof(action));
            }

            if (!template.Contains("%u"))
            {
                throw new ArgumentException("template must contain format %u", nameof(template));
            }

            Trace.WriteLine("Adding numberic OptiongOption");

            _options.Add(new ValueOption(template, 0, -1, i => action(unchecked((uint) i))));
        }

        public void Add(string template, int initialValue, int value, Action<int> action)
        {
            if (template == null)
            {
                throw new NullReferenceException(nameof(template));
            }

            if (action == null)
            {
                throw new NullReferenceException(nameof(action));
            }

            if (!template.Contains("%"))
            {
                throw new ArgumentException("template must not contain format %", nameof(template));
            }

            _options.Add(new ValueOption(template, initialValue, value, action));
        }

        public void Add(string template, int value, Action<int> action)
        {
            if (template == null)
            {
                throw new NullReferenceException(nameof(template));
            }

            if (action == null)
            {
                throw new NullReferenceException(nameof(action));
            }

            if (template.Contains("%"))
            {
                throw new ArgumentException("template must contain format %u", nameof(template));
            }

            _options.Add(new ValueOption(template, 0, value, action));
        }

        public unsafe bool Parse(string[] argv)
        {
            Trace.WriteLine($"Parse args: {string.Join(", ", argv)}");

            var options = new FuseOpt[_options.Count + 1];

            var data = new IntPtr[_options.Count];

            fixed (FuseOpt* pOptions = options)
            fixed(IntPtr* pData = data)
            {
                int offset = 0;
                for (int i = 0; i < _options.Count; i++)
                {
                    _options[i].Init(pOptions + i, pData, offset);

                    offset += IntPtr.Size;
                }

                var args = new FuseArgs
                {
                    argc = argv.Length + 1,
                    allocated = 1,
                    argv = AllocateArgs(argv)
                };

                var pArgs = &args;

                try
                {
                    var fuseOptParse = Functions.FuseOptParse(pArgs, pData, pOptions, FuseOptProc);
                    if (fuseOptParse != -1)
                    {
                        for (int i = 0; i < _options.Count; i++)
                        {
                            _options[i].Process(pOptions + i, pData);
                        }

                        return true;
                    }

                    return false;
                }
                finally
                {
                    for (int i = 0; i < _options.Count; i++)
                    {
                        _options[i].Release(pOptions + i, pData);
                    }

                    ReleaseArgs(pArgs);
                }
            }
        }

        private unsafe IntPtr AllocateArgs(string[] argv)
        {
            var argsPtr = Marshal.AllocHGlobal(IntPtr.Size * (argv.Length + 2));
            var pArgs = (IntPtr*)argsPtr.ToPointer();
            pArgs[0] = Marshal.StringToHGlobalAnsi("");

            for (int i = 0; i < argv.Length; i++)
            {
                pArgs[i + 1] = Marshal.StringToHGlobalAnsi(argv[i]);
            }

            pArgs[argv.Length + 1] = IntPtr.Zero;

            return argsPtr;
        }

        private unsafe void ReleaseArgs(FuseArgs*pArg)
        {
            var pPtr = (IntPtr*)pArg->argv.ToPointer();

            for (int i = 0; ; i++)
            {
                var ptr = pPtr[i];
                if (ptr == IntPtr.Zero)
                {
                    break;
                }
                Marshal.FreeHGlobal(ptr);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _options.Select(e => e.Template).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private unsafe int FuseOptProc(void* data, string arg, int key, FuseArgs* outargs)
        {
            Trace.WriteLine($"FuseOptProc: arg={arg}, key={key}");

            if (ProcesingFunction != null)
            {
                return ProcesingFunction(arg, key);
            }

            return -1;
        }
    }

    public class FuseOptions<T> : FuseOptions
        where T: class 
    {
        public void Add(string template, int value, Expression<Func<T, int>> expression)
        {
            
        }

        public void Add(string template, Expression<Func<T, bool>> expression)
        {
            
        }

        public void Add(string template, Expression<Func<T, int>> expression)
        {
            
        }
        
        public void Add(string template, Expression<Func<T, string>> expression)
        {
            
        }

        public void Parse(string[] args, T data)
        {
            
        }
    }
}
