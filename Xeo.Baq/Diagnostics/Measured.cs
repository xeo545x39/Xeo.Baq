using System;
using System.Diagnostics;

namespace Xeo.Baq.Diagnostics
{
    public static class Measured
    {
        public static void Go(Action action, out TimeSpan elapsed)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception was thrown during a measured block.", ex);
            }
            finally
            {
                stopwatch.Stop();
                elapsed = stopwatch.Elapsed;
            }
        }
    }
}