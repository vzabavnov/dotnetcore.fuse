// Copyright 2017 Vadim Zabavnov
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics;
using Fuse.Interop.FuseOpt;
using Xunit;
using Xunit.Abstractions;

namespace fuse.interop.tests
{
    public class FuseArgsTest : IDisposable
    {
        private readonly XunitListener _listener;

        public FuseArgsTest(ITestOutputHelper outputHelper)
        {
            _listener = new XunitListener(outputHelper);
            Trace.Listeners.Add(_listener);
        }

        public void Dispose()
        {
            Trace.Listeners.Remove(_listener);
        }

        public static object[][] Settings => new SettingCollection
        {
            { "-h", "-h"},
            { "--help",  "--help" },
            { "-M %u", 5u, "-M 5" },
            { "--maj=%u", 6u, "--maj=6" },
            { "-m %u", 7u, "-m 7" },
            { "--min=%u", 8u, "--min=8" },
            { "-n %s", "dev1", "-n dev1" },
            { "--name=%s", "dev2", "--name=dev2" },
            { "-steps=%i", 2, "-steps=2" }
        };

        [Theory]
        [MemberData(nameof(Settings))]
        public void ArgsParseTest(Setter setter)
        {
            var options = new FuseOptions();
            setter.Init(options);

            var result = options.Parse(setter.Args.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            Assert.True(result);
            Assert.True(setter.Success);
        }

        [Fact]
        public void OptionsParseTest()
        {
            const string args = "-o directport=22" 
                + ",ssh_command=cmd"
                + ",sftp_server=localhost"
                + ",max_read=12"
                + ",-1"
                + ",idmap=none"
                + ",idmap=user"
                + ",no_readahead";

            bool a1 = false, a2 = false;
            int idmap1 = 0, idmap2 = 0;
            var options = new FuseOptions
            {
                { "directport=%u", u => Assert.Equal(22u, u) },
                { "ssh_command=%s", s => Assert.Equal("cmd", s) },
                { "-1",  () => a1 = true },
                { "idmap=none", 1, i => idmap1 = i },
                { "idmap=user", 2, i => idmap2 = i },
                { "no_readahead", () => a2 = true }
            };

            var res = options.Parse(args.Split(' '));
            Assert.True(res);

            Assert.True(a1);
            Assert.True(a2);

            Assert.Equal(1, idmap1);
            Assert.Equal(2, idmap2);
        }
    }
}