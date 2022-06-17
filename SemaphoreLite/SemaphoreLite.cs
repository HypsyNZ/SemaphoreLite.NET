/*
*MIT License
*
*Copyright (c) 2022 S Christison
*
*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:
*
*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.
*
*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SemaphoreLite
{
    /// <summary>
    /// A Light Semiphore that only allows 1 Caller at a time, From anywhere
    /// </summary>
    public class SemaphoreLight
    {
        private volatile bool _taken = false;
        private readonly object _lock = new object();
        protected readonly Stopwatch _sw = new Stopwatch();

        /// <summary>
        /// Returns True when the Caller has Taken the Semiphore and the Task has Ran to Completion inside the Critical Section
        /// <para>You <see href="MUST"/> call <see href="Release()"/> when you are done with your work or the Semiphore will block forever</para>
        /// <para>If you use this in a Hard Loop or without any Kind of Delay it will use a lot of CPU</para>
        /// </summary>
        /// <param name="theTask">The Task that will be completed if the Semiphore is Taken by the Caller</param>
        /// <param name="configureAwaiter">Configure the awaiter for the Task running inside the Critical Section</param>
        /// <returns>True if the Caller is required to <see href="Release()"/> the Semiphore for the next Caller</returns>
        public async Task<bool> IsTakenAsync(Func<Task> theTask, bool configureAwaiter = false)
        {
            if (_taken)
            {
                return false;
            }

            lock (_lock)
            {
                if (_taken)
                {
                    return false;
                }

                _taken = true;
            }

            await theTask().ConfigureAwait(configureAwaiter);

            return true;
        }

        /// <summary>
        /// Releases the Semiphore so another Caller can Enter the Critical Section
        /// </summary>
        public void Release()
        {
            _taken = false;
        }
    }
}
