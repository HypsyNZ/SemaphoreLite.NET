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

using SemaphoreLite;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        private static SemaphoreLight semaphoreLight = new SemaphoreLight();

        private static int testCountOne = 0;
        private static int testCountTwo = 0;

        private static void Main(string[] args)
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var callerRequiredToReleaseSemiphore = await semaphoreLight.IsTakenAsync(SomeAsyncTask, false).ConfigureAwait(false);
                    if (callerRequiredToReleaseSemiphore)
                    {
                        // You can also do work here before you release, This is also inside the Critical Section
                        semaphoreLight.Release();
                    }
                }
            });

            _ = Task.Run(() =>
            {
                while (true)
                {
                    var callerRequiredToReleaseSemiphore = semaphoreLight.IsTakenAsync(SomeOtherTaskAsync).Result;
                    if (callerRequiredToReleaseSemiphore)
                    {
                        // You can also do work here before you release, This is also inside the Critical Section
                        semaphoreLight.Release();
                    }
                }
            });

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write("One: " + testCountOne + "| Two: " + testCountTwo);
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            });

            Console.ReadLine();
        }

        private static Task SomeAsyncTask()
        {
            // Inside the Critical Section
            testCountOne++;
            return Task.CompletedTask;
        }

        private static Task SomeOtherTaskAsync()
        {
            // Inside the Critical Section
            testCountTwo++;
            return Task.CompletedTask;
        }
    }
}
