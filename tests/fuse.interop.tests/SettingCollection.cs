using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace fuse.interop.tests
{
    internal class SettingCollection : IEnumerable<Setter>
    {
        readonly List<Setter> _setters = new List<Setter>();

        public IEnumerator<Setter> GetEnumerator()
        {
            return _setters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string template, int expectedValue, string args)
        {
            _setters.Add(new IntSetter(template, expectedValue, args));
        }

        public void Add(string template, string expectedValue, string args)
        {
            _setters.Add(new StringSetter(template, expectedValue, args));
        }

        public void Add(string template, uint expectedValue, string args)
        {
            _setters.Add(new UIntSetter(template, expectedValue, args));
        }

        public void Add(string template, string args)
        {
            _setters.Add(new FlagSetter(template, args));
        }

        public static implicit operator object[][](SettingCollection setters)
        {
            return setters.Select(e => new object[] {e}).ToArray();
        }
    }
}