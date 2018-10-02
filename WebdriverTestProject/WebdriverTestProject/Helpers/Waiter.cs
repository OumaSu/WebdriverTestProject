using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace WebdriverTestProject.Helpers
{
    public static class Waiter
    {
        private const int WaitTimeout = 1;
        private const int DefaultTimeout = 20000;

        public static void Wait(Func<bool> tryFunc, string actionDescription, int? timeout = null)
        {
            DoWait(tryFunc,
                () => throw new AssertionException(
                    $"Действие {(object) actionDescription} не выполнилось за {(object) GetActualTimeout(timeout)} мс"),
                timeout);
        }

        public static void DoWait(Func<bool> tryFunc, Action doIfWaitFails, int? timeout = null)
        {
            var stopwatch = Stopwatch.StartNew();
            while (!tryFunc())
            {
                Thread.Sleep(WaitTimeout);
                if (stopwatch.ElapsedMilliseconds >= GetActualTimeout(timeout))
                {
                    doIfWaitFails();
                    break;
                }
            }
        }

        private static int GetActualTimeout(int? timeout)
        {
            return timeout ?? DefaultTimeout;
        }
    }
}