using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlueberryEarth5.Services
{
    public class JobExecutedEventArgs : EventArgs { }

    public class PeriodicExecutor : IDisposable
    {
        public event EventHandler<JobExecutedEventArgs> JobExecuted;
        void OnJobExecuted()
        {
            JobExecuted?.Invoke(this, new JobExecutedEventArgs());
        }

        AppState appState;
        public PeriodicExecutor(AppState appState)
        {
            this.appState = appState;
        }

        Timer _updateTimer, _drawTimer;
        bool _updateRunning, _drawRunning;

        public void StartExecuting()
        {
            if (!_updateRunning)
            {
                // Initiate a Timer
                _updateTimer = new Timer
                {
                    Interval = 1001  // ms
                };
                _updateTimer.Elapsed += OnUpdateTimerTrigger;
                _updateTimer.AutoReset = true;
                _updateTimer.Enabled = true;

                _updateRunning = true;
            }
            if (!_drawRunning)
            {
                // Initiate a Timer
                _drawTimer = new Timer
                {
                    Interval = 111  // ms
                };
                _drawTimer.Elapsed += OnDrawTimerTrigger;
                _drawTimer.AutoReset = true;
                _drawTimer.Enabled = true;

                _drawRunning = true;
            }
        }
        void OnUpdateTimerTrigger(object source, ElapsedEventArgs e)
        {
            // Execute required job
            var musherCount = appState.GetResourceValue(ResourceType.BlueberryMusher);
            appState.ModifyResourceValue(ResourceType.BlueberryMush, 1 * musherCount);
            
            // Notify any subscribers to the event
            OnJobExecuted();
        }

        void OnDrawTimerTrigger(object source, ElapsedEventArgs e)
        {
            appState.RedrawIfNeeded();
            // call draw subscribers event if needed?
        }

        public void Dispose()
        {
            if (_updateRunning)
            {
                // Clear up the timer
                _updateTimer.Dispose();
            }
            if (_drawRunning)
            {
                _drawTimer.Dispose();
            }
        }
    }
}
