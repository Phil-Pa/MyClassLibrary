using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyClassLibrary.Windows
{
    public class ProcessTimeTracker
    {
        private readonly TimeSpan _timeLimit;

        private bool _stop;

        public event Action<Process>? ReachedTimeLimit;

        private readonly List<Process> _processes;

        private Task _backgroundTask;

        private readonly Dictionary<Process, TimeSpan> _usedProcessTime = new Dictionary<Process, TimeSpan>();

        public ProcessTimeTracker(TimeSpan timeLimit)
        {
            _timeLimit = timeLimit;
            _processes = Process.GetProcesses().ToList();

            foreach (var process in _processes)
            {
                try
                {
                    _usedProcessTime.Add(process, process.UserProcessorTime);
                }
                catch (Win32Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Start()
        {
            _stop = false;

            foreach (var process in _processes)
            {
                process.Refresh();
            }

            _backgroundTask = Task.Run(async () =>
            {
                while (!_stop)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Console.WriteLine("scanning processes...");

                    for (var index = 0; index < _processes.Count; index++)
                    {
                        var process = _processes[index];
                        process.Refresh();

                        var currentProcesses = Process.GetProcesses();

                        if (!currentProcesses.Contains(process))
                            _processes.Remove(process);

                        if (process.UserProcessorTime > _timeLimit + _usedProcessTime[process])
                        {
                            ReachedTimeLimit?.Invoke(process);
                        }
                    }
                }
            });
        }

        public void StopOnCondition(Func<bool> condition)
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (condition())
                {
                    _stop = true;
                }
            }
        }
    }
}
