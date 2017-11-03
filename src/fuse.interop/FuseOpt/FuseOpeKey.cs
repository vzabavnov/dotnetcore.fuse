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

namespace Fuse.Interop.FuseOpt
{
    internal enum FuseOpeKey
    {
        /// <summary>
        ///     Key value passed to the processing function if an option did not match any template
        /// </summary>
        Opt = -1,

        /// <summary>
        ///     Key value passed to the processing function for all non-options
        ///     Non-options are the arguments beginning with a character other than '-' or all arguments after the special '--'
        ///     option
        /// </summary>
        NonOpt = -2,

        /// <summary>
        ///     Special key value for options to keep
        ///     Argument is not passed to processing function, but behave as if the processing function returned 1
        /// </summary>
        Keep = -3,

        /// <summary>
        ///     Special key value for options to discard
        ///     Argument is not passed to processing function, but behave as if the processing function returned zero
        /// </summary>
        Discard = -4
    }
}