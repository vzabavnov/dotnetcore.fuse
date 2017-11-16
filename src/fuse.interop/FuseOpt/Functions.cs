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
    internal static class Functions
    {
        /// <summary>
        /// Key value passed to the processing function if an option did not match any template
        /// </summary>
        public const int FuseOptKeyOpt = -1;

        /// <summary>
        /// Key value passed to the processing function for all non-options
        /// Non-options are the arguments beginning with a character other than '-' or all arguments after the special '--'
        /// option
        /// </summary>
        public const int FuseOptKeyNonopt = -2;

        /// <summary>
        /// Special key value for options to keep
        /// Argument is not passed to processing function, but behave as if the processing function returned 1
        /// </summary>
        public const int FuseOptKeyKeep = -3;

        /// <summary>
        /// Special key value for options to discard
        /// Argument is not passed to processing function, but behave as if the processing function returned zero
        /// </summary>
        public const int FuseOptKeyDiscard = -4;

        /// <summary>
        /// Processing function
        /// typedef int (*fuse_opt_proc_t)(void *data, const char *arg, int key, struct fuse_args *outargs);
        ///
        /// This function is called if
        ///    - option did not match any 'struct fuse_opt'
        ///    - argument is a non-option
        ///    - option did match and offset was set to -1
        ///
        /// The 'arg' parameter will always contain the whole argument or
        /// option including the parameter if exists.A two-argument option
        /// ("-x foo") is always converted to single argument option of the
        /// form "-xfoo" before this function is called.
        ///
        /// Options of the form '-ofoo' are passed to this function without the
        /// '-o' prefix.
        ///
        /// The return value of this function determines whether this argument
        /// is to be inserted into the output argument vector, or discarded.
        /// </summary>
        /// <param name="data">is the user data passed to the fuse_opt_parse() function</param>
        /// <param name="arg">is the whole argument or option</param>
        /// <param name="key">determines why the processing function was called</param>
        /// <param name="outargs">the current output argument list</param>
        /// <returns>-1 on error, 0 if arg is to be discarded, 1 if arg should be kept</returns>
        internal unsafe delegate int FuseOptProc(void* data, [MarshalAs(UnmanagedType.LPStr), In] string arg, int key, FuseArgs * outargs);

        /// <summary>
        /// Option parsing function 
        /// int fuse_opt_parse(struct fuse_args *args, void *data, const struct fuse_opt opts[], fuse_opt_proc_t proc);
        ///
        /// If 'args' was returned from a previous call to fuse_opt_parse() or it was constructed from
        ///
        /// A NULL 'args' is equivalent to an empty argument vector
        ///
        /// A NULL 'opts' is equivalent to an 'opts' array containing a single end marker
        ///
        /// A NULL 'proc' is equivalent to a processing function always  returning '1'
        /// 
        /// </summary>
        /// <param name="args">the input and output argument list </param>
        /// <param name="data">the user data</param>
        /// <param name="opts">the option description array</param>
        /// <param name="proc">the processing function </param>unsafe
        /// <returns>-1 on error, 0 on success</returns>
        [DllImport("libfuse", EntryPoint = "fuse_opt_parse")]
        internal static extern unsafe int FuseOptParse(FuseArgs* args, void* data, FuseOpt* opts, FuseOptProc proc);

        /// <summary>
        /// Add an option to a comma separated option list 
        /// int fuse_opt_add_opt(char **opts, const char *opt); 
        /// </summary>
        /// <param name="opts">a pointer to an option list, may point to a NULL value.</param>
        /// <param name="opt">the option to add </param>
        /// <returns>-1 on allocation error, 0 on success </returns>
        [DllImport("libfuse", EntryPoint = "fuse_opt_add_opt", CharSet = CharSet.Ansi)]
        internal static extern int FuseOptAddOpt(ref string opts, string opt);

        /// <summary>
        /// Add an argument to a NULL terminated argument vector 
        /// int fuse_opt_add_arg(struct fuse_args *args, const char *arg); 
        /// </summary>
        /// <param name="args">the structure containing the current argument list </param>
        /// <param name="arg">the new argument to add </param>
        /// <returns>-1 on allocation error, 0 on success </returns>
        [DllImport("libfuse", EntryPoint = "fuse_opt_add_arg")]
        internal static extern int FuseOptAddArg(IntPtr args, IntPtr arg);

        /// <summary>
        /// Add an argument at the specified position in a NULL terminated argument vector
        /// int fuse_opt_insert_arg(struct fuse_args *args, int pos, const char *arg); 
        /// </summary>
        /// <remarks>
        /// Adds the argument to the N-th position.This is useful for adding options at the 
        /// beginning of the array which must not come after the special '--' option.</remarks>
        /// <param name="args">the structure containing the current argument list </param>
        /// <param name="pos">the position at which to add the argument </param>
        /// <param name="arg">new argument to add </param>
        /// <returns></returns>
        [DllImport("libfuse", EntryPoint = "fuse_opt_insert_arg")]
        internal static extern int FuseOptInsertArg(IntPtr args, int pos, IntPtr arg);

        /// <summary>
        /// Free the contents of argument list 
        /// void fuse_opt_free_args(struct fuse_args *args); 
        /// </summary>
        /// <remarks>The structure itself is not freed</remarks>
        /// <param name="args">the structure containing the argument list </param>
        [DllImport("libfuse", EntryPoint = "fuse_opt_free_args")]
        internal static extern void FuseOptFreeAgs(IntPtr args);

        /// <summary>
        /// Check if an option matches 
        /// int fuse_opt_match(const struct fuse_opt opts[], const char *opt); 
        /// </summary>
        /// <param name="opts">the option description array </param>
        /// <param name="opt">the option to match </param>
        /// <returns>1 if a match is found, 0 if not </returns>
        [DllImport("libfuse")]
        internal static extern int FuseOptMatch(IntPtr opts, IntPtr opt);
    }
}
