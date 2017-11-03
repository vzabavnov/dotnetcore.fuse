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
using System.Runtime.InteropServices;

namespace Fuse.Interop.FuseOpt
{
    /// <summary>
    ///     Option description
    ///     This structure describes a single option, and action associated
    ///     with it, in case it matches.
    ///     More than one such match may occur, in which case the action for
    ///     each match is executed.
    ///     There are three possible actions in case of a match:
    ///     i) An integer(int or unsigned) variable determined by 'offset' is
    ///     set to 'value'
    ///     ii) The processing function is called, with 'value' as the key
    ///     iii) An integer(any) or string (char///) variable determined by
    ///     'offset' is set to the value of an option parameter
    ///     'offset' should normally be either set to
    ///     - 'offsetof(struct foo, member)'  actions i) and iii)
    ///     - -1			      action ii)
    ///     The 'offsetof()' macro is defined in the stddef.h header.
    ///     The template determines which options match, and also have an
    ///     effect on the action.  Normally the action is either i) or ii), but
    ///     if a format is present in the template, then action iii) is
    ///     performed.
    ///     The types of templates are:
    ///     1) "-x", "-foo", "--foo", "--foo-bar", etc.These match only
    ///     themselves.Invalid values are "--" and anything beginning
    ///     with "-o"
    ///     2) "foo", "foo-bar", etc.These match "-ofoo", "-ofoo-bar" or
    ///     the relevant option in a comma separated option list
    ///     3) "bar=", "--foo=", etc.These are variations of 1) and 2)
    ///     which have a parameter
    ///     4) "bar=%s", "--foo=%lu", etc.Same matching as above but perform
    ///     action iii).
    ///     5) "-x ", etc.Matches either "-xparam" or "-x param" as
    ///     two separate arguments
    ///     6) "-x %s", etc.Combination of 4) and 5)
    ///     If the format is "%s", memory is allocated for the string unlike with
    ///     scanf().  The previous value(if non-NULL) stored at the this location is
    ///     freed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct FuseOpt
    {
        /// <summary>
        ///     Matching template and optional parameter formatting
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)] public string templ;

        /// <summary>
        ///     Offset of variable within 'data' parameter of fuse_opt_parse() or -1
        /// </summary>
        public ulong offset;

        /// <summary>
        ///     Value to set the variable to, or to be passed as 'key' to the processing function.	 Ignored if template has a
        ///     format
        /// </summary>
        public int value;

        /// <summary>
        ///     Key option.	In case of a match, the processing function will be called with the specified key.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static FuseOpt Key(string template, int key) =>
            new FuseOpt { templ = template, offset = ulong.MinValue, value = key };

        /// <summary>
        ///     Last option. An array of 'struct fuse_opt' must end with a NULL template value
        /// </summary>
        public static FuseOpt End = new FuseOpt { offset = 0, templ = null, value = 0 };
    }
}