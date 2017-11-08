using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fuse.Interop.FuseOpt
{
    public class FuseOptions : IEnumerable<string>
    {

        public void Add(string template, int value, Action<int> setter)
        {
            
        }

        public void Add(string template, Action<int> setter)
        {
            
        }

        public void Add(string template, Action<string> setter)
        {
            
        }


        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
