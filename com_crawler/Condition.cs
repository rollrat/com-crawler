// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace com_crawler
{
    public class Condition : ILazy<Condition>
    {
        Timer timer;
        Queue<long> memory_usage = new Queue<long>();
        Queue<double> cpu_usage = new Queue<double>();
        int processor_count = 0;
        long loop_count = 0;
        double last_processortime = 0;
        DateTime last_dt;

        public void Start()
        {
            processor_count = Environment.ProcessorCount;
            last_dt = DateTime.UtcNow;

            var proc = Process.GetCurrentProcess();
            last_processortime = proc.TotalProcessorTime.TotalMilliseconds;

            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Stop();
            timer.Dispose();
        }

        public long GetMemoryUsage()
        {
            var proc = Process.GetCurrentProcess();
            return proc.WorkingSet64;
        }

        public double GetLastMinuteMemoryUsage()
        {
            var cv = 60.0 / memory_usage.Count;
            return memory_usage.Sum() * cv / 60.0;
        }

        public double GetLastMinuteCPUUsage()
        {
            var cv = 60.0 / cpu_usage.Count;
            return cpu_usage.Sum() * cv / 60.0;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (loop_count > 59)
            {
                memory_usage.Dequeue();
                cpu_usage.Dequeue();
            }

            var proc = Process.GetCurrentProcess();
            memory_usage.Enqueue(proc.WorkingSet64);

            var cur = proc.TotalProcessorTime.TotalMilliseconds;
            var diff_cpu = cur - last_processortime;
            var diff_dt = DateTime.Now - last_dt;
            last_processortime = cur;
            cpu_usage.Enqueue(diff_cpu / (processor_count * diff_dt.TotalMilliseconds));

            loop_count += 1;
            last_dt = DateTime.UtcNow;
        }
    }
}
