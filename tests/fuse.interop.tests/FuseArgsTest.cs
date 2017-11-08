using System;
using System.Linq;
using System.Runtime.InteropServices;
using Fuse.Interop.FuseOpt;
using Xunit;
using Xunit.Abstractions;

namespace fuse.interop.tests
{
    public class FuseArgsTest
    {
        private readonly ITestOutputHelper _helper;

        public FuseArgsTest(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void ConstructorTest()
        {
            var arg = new FuseArgs();
            Assert.Equal(0, arg.allocated);
            Assert.Equal(0, arg.argc);
            Assert.Equal(IntPtr.Zero, arg.argv);

            arg = new FuseArgs(new[] { "First", "Second" });
            Assert.Equal(1, arg.allocated);
            Assert.Equal(2, arg.argc);
            Assert.NotEqual(IntPtr.Zero, arg.argv);

            Assert.Equal("First", Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(arg.argv)));
            Assert.Equal("Second", Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(arg.argv, IntPtr.Size)));
            Assert.Equal(IntPtr.Zero, Marshal.ReadIntPtr(arg.argv, IntPtr.Size * 2));
        }

        [Fact]
        public void DisposeTest()
        {
            FuseArgs arg;
            using (arg = new FuseArgs(new[] { "First", "Second" }))
            {
                Assert.Equal(1, arg.allocated);
                Assert.Equal(2, arg.argc);
                Assert.NotEqual(IntPtr.Zero, arg.argv);
            }

            Assert.Equal(0, arg.allocated);
            Assert.Equal(0, arg.argc);
            Assert.Equal(IntPtr.Zero, arg.argv);

            using (arg = new FuseArgs(new[] { "First", "Second" }))
            {
                arg.Add("Third");
                Assert.Equal(1, arg.allocated);
                Assert.Equal(3, arg.argc);
                Assert.NotEqual(IntPtr.Zero, arg.argv);
            }

            Assert.Equal(0, arg.allocated);
            Assert.Equal(0, arg.argc);
            Assert.Equal(IntPtr.Zero, arg.argv);
        }

        [Fact]
        public void EnumeratorTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                Assert.Equal(2, arg.Count());
                Assert.Contains(arg, e => e == "First");
                Assert.Contains(arg, e => e == "Second");
            }
        }

        [Fact]
        public void AddTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                arg.Add("Third");
                Assert.Equal(3, arg.Count());
                Assert.Contains(arg, e => e == "First");
                Assert.Contains(arg, e => e == "Second");
                Assert.Contains(arg, e => e == "Third");
            }
        }

        [Fact]
        public void ClearTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                arg.Clear();

                Assert.Equal(0, arg.allocated);
                Assert.Equal(0, arg.argc);
                Assert.Equal(IntPtr.Zero, arg.argv);
            }
        }

        [Fact]
        public void ContainsTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                Assert.True(arg.Contains("First"));
                Assert.True(arg.Contains("Second"));
                Assert.False(arg.Contains("Third"));
            }
        }

        [Fact]
        public void CopyToTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                var arr = new string[arg.Count];
                arg.CopyTo(arr, 0);

                Assert.Equal(2, arr.Length);
                Assert.Equal("First", arr[0]);
                Assert.Equal("Second", arr[1]);
            }
        }

        [Fact]
        public void RemoveTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                var r = arg.Remove("First");
                Assert.True(r);

                Assert.Equal(0, arg.IndexOf("Second"));

                Assert.DoesNotContain("First", arg);

                _helper.WriteLine($"Count={arg.Count()}");
                
                int count = 0;
                string obj1 = null;
                foreach (var obj2 in arg)
                {
                    obj1 = obj2;
                    _helper.WriteLine($"iteraction {count}; value {obj2}");
                    ++count;
                }

                Assert.Equal(1, count);

                Assert.Single(arg);
                Assert.Contains("Second", arg);
            }
        }

        [Fact]
        public void IndexOfTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                Assert.Equal(0, arg.IndexOf("First"));
                Assert.Equal(1, arg.IndexOf("Second"));
                Assert.Equal(-1, arg.IndexOf("Third"));
            }
        }

        [Fact]
        public void InsertTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                arg.Insert(1, "Third");
                Assert.Equal(3, arg.Count);
                Assert.Equal(1, arg.IndexOf("Third"));
                Assert.Equal(0, arg.IndexOf("First"));
                Assert.Equal(2, arg.IndexOf("Second"));
            }
        }

        [Fact]
        public void RemoveAtTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second", "Third" }))
            {
                arg.RemoveAt(1);
                Assert.Equal(2, arg.Count);
                Assert.Equal(0, arg.IndexOf("First"));
                Assert.Equal(1, arg.IndexOf("Third"));
            }

        }

        [Fact]
        public void IndexerTest()
        {
            var arg = new FuseArgs(new[] { "First", "Second" });

            Assert.Equal("First", arg[0]);
            Assert.Equal("Second", arg[1]);

            var a = arg[0];
            arg[0] = arg[1];
            arg[1] = a;

            Assert.Equal("Second", arg[0]);
            Assert.Equal("First", arg[1]);

           arg.Dispose();
        }

        [Fact]
        public void CountTest()
        {
            using (var arg = new FuseArgs(new[] { "First", "Second" }))
            {
                Assert.Equal(arg.argc, arg.Count());
            }
        }
    }
}
