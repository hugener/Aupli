namespace Sundew.Pi.TimerTester
{
    using System;
    using System.Diagnostics;
    using global::Pi;
    using global::Pi.System.Threading;
    using Timer = global::Pi.Timers.Timer;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Timer tester!");
            var threadFactory = new ThreadFactory(Board.Current, true);
            Console.WriteLine("Stopwatch is high res: " + Stopwatch.IsHighResolution + " - " + Stopwatch.Frequency);
            var stopwatch = new Stopwatch();
            using (var thread = threadFactory.Create())
            {
                thread.Sleep(TimeSpan.FromTicks(3 * 10));
                thread.Sleep(TimeSpan.FromMilliseconds(100));
                thread.Sleep(TimeSpan.FromMilliseconds(1));
                long elapsed;
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(150));
                elapsed = stopwatch.ElapsedTicks;
                Console.WriteLine($"Sleep: {elapsed / 1000_000}/150 ms");

                for (int i = 0; i < 10; i++)
                {
                    stopwatch.Restart();
                    thread.Sleep(TimeSpan.FromTicks(3 * 10));
                    elapsed = stopwatch.ElapsedTicks;
                    Console.WriteLine($"Sleep: {elapsed / 1000}/3 us");
                }

                for (int i = 0; i < 10; i++)
                {
                    stopwatch.Restart();
                    thread.Sleep(TimeSpan.FromTicks(6 * 10));
                    elapsed = stopwatch.ElapsedTicks;
                    Console.WriteLine($"Sleep: {elapsed / 1000}/6 us");
                }

                for (int i = 0; i < 10; i++)
                {
                    stopwatch.Restart();
                    thread.Sleep(TimeSpan.FromTicks(10 * 10));
                    elapsed = stopwatch.ElapsedTicks;
                    Console.WriteLine($"Sleep: {elapsed / 1000}/10 us");
                }

                for (int i = 0; i < 10; i++)
                {
                    stopwatch.Restart();
                    thread.Sleep(TimeSpan.FromTicks(10 * 10));
                    elapsed = stopwatch.ElapsedTicks;
                    Console.WriteLine($"Sleep: {elapsed / 1000}/100 us");
                }

                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(500));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 500 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(500));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 500 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(500));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 200 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(200));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 100 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(100));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 99 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(99));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 99 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(99));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 50 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(50));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 80 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(80));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 60 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(60));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 10 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(10));
                Console.WriteLine(stopwatch.Elapsed);
                Console.WriteLine("Sleep: 3 ms");
                stopwatch.Restart();
                thread.Sleep(TimeSpan.FromMilliseconds(3));
                Console.WriteLine(stopwatch.Elapsed);

                Console.WriteLine("Timer: 50/50 ms");
                var timer = Timer.Create();
                timer.Interval = TimeSpan.FromMilliseconds(50);
                var start = DateTime.Now;
                timer.Tick += (s, e) => OnTick(start);
                timer.Start(TimeSpan.FromMilliseconds(50));
                thread.Sleep(TimeSpan.FromMilliseconds(500));
                Timer.Dispose(timer);

                Console.WriteLine("Timer: 50/50 ms");
                timer = Timer.Create();
                timer.Interval = TimeSpan.FromMilliseconds(50);
                start = DateTime.Now;
                timer.Tick += (s, e) => OnTick(start);
                timer.Start(TimeSpan.FromMilliseconds(50));
                thread.Sleep(TimeSpan.FromMilliseconds(500));
                Timer.Dispose(timer);

                Console.WriteLine("Timer: 500/200ms");
                timer = Timer.Create();
                timer.Interval = TimeSpan.FromMilliseconds(200);
                start = DateTime.Now;
                timer.Tick += (s, e) => OnTick(start);
                timer.Start(TimeSpan.FromMilliseconds(500));
                thread.Sleep(TimeSpan.FromMilliseconds(2000));
                Timer.Dispose(timer);

                Console.WriteLine("Timer: 10000/1000ms");
                timer = Timer.Create();
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                start = DateTime.Now;
                timer.Tick += (s, e) => OnTick(start);
                timer.Start(TimeSpan.FromMilliseconds(10000));
                thread.Sleep(TimeSpan.FromMilliseconds(20000));
                Timer.Dispose(timer);
            }

            Console.WriteLine("Done");
        }

        private static void OnTick(DateTime start)
        {
            Console.WriteLine("Ticked: " + (DateTime.Now - start).TotalMilliseconds);
        }
    }
}
