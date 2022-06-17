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
        private const int ONE_MILLISECOND_IN_TICKS = 10000;

        private volatile bool _taken = false;
        private readonly object _lock = new object();
        protected readonly Stopwatch _sw = new Stopwatch();

        /// <summary>
        /// Returns True when the Caller has Taken the Semiphore and the Task has Ran to Completion inside the Critical Section
        /// <para>You <see href="MUST"/> call <see href="Release()"/> when you are done with your work or the Semiphore will block forever</para>
        /// </summary>
        /// <param name="theTask">The Task that will be completed if the Semiphore is Taken by the Caller</param>
        /// <param name="configureAwaiter">Configure the awaiter for the Task running inside the Critical Section</param>
        /// <returns>True if the Caller is required to <see href="Release()"/> the Semiphore for the next Caller</returns>
        public async Task<bool> IsTakenAsync(Func<Task> theTask, bool configureAwaiter = false)
        {
            await Task.Delay(1).ConfigureAwait(configureAwaiter);

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
        /// <para>READ BEFORE YOU START USING</para>
        /// Returns True when the Caller has Taken the Semiphore and the Task has Ran to Completion inside the Critical Section
        /// <para>You <see href="MUST"/> call <see href="Release()"/> when you are done with your work or the Semiphore will block forever</para>
        /// <para>This uses more CPU then probably necessary for most applications, Consider using <see href="IsTakenAsync()"/> instead</para>
        /// <para>This is NOT better than <see href="IsTakenAsync()"/>, it is just useful for certain applications</para>
        /// <para>It is not recommended to use this Method all the time, Only when you need something "Instantly"</para>
        /// <para>This method doesn't use <see href="Task.Delay(1)"/> or <see href="Delay()"/> and as a result it goes much faster but at the cost of CPU usage, Consider using <see href="IsTakenAsync()"/> instead</para>
        /// </summary>
        /// <param name="theTask">The Task that will be completed if the Semiphore is Taken by the Caller</param>
        /// <param name="configureAwaiter">Configure the awaiter for the Task running inside the Critical Section</param>
        /// <returns>True if the Caller is required to <see href="Release()"/> the Semiphore for the next Caller</returns>
        public async Task<bool> IsTakenAsyncNoDelay(Func<Task> theTask, bool configureAwaiter = false)
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
        /// <para>READ BEFORE YOU START USING</para>
        /// Returns True when the Caller has Taken the Semiphore and the Task has Ran to Completion inside the Critical Section
        /// <para>You <see href="MUST"/> call <see href="Release()"/> when you are done with your work or the Semiphore will block forever</para>
        /// <para>This is NOT better than <see href="IsTakenAsync()"/>, it is just useful for certain types of applications</para>
        /// <para>It is not recommended to use this Method all the time, Only when you need something "Instantly"</para>
        /// <para>This method doesn't use <see href="Task.Delay(1)"/> and uses custom made <see href="Delay()"/> instead and as a result it goes much faster but at the cost of CPU usage, Consider using <see href="IsTakenAsync()"/> instead</para>
        /// </summary>
        /// <param name="theTask">The Task that will be completed if the Semiphore is Taken by the Caller</param>
        /// <param name="configureAwaiter">Configure the awaiter for the Task running inside the Critical Section</param>
        /// <returns>True if the Caller is required to <see href="Release()"/> the Semiphore for the next Caller</returns>
        public async Task<bool> IsTakenAsyncUseFastDelay(Func<Task> theTask, bool configureAwaiter = false)
        {
            Delay();

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

        /// <summary>
        /// Creates a One Millisecond Delay from Operating System Ticks
        /// Ten Thousand Operating System Ticks is Approximately One Millisecond
        /// </summary>
        protected void Delay()
        {
            _sw.Restart();
            while (true)
            {
                if (_sw.ElapsedTicks > ONE_MILLISECOND_IN_TICKS)
                {
                    //Console.WriteLine(_sw.ElapsedTicks.ToString());
                    //Output: 10002 with timeout = 0
                    break;
                }
            }
        }
    }
}
