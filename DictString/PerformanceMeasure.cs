using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictString
{
    class PerformanceMeasure : IDisposable
    {
        private Process _currentProcess;
        private Stopwatch _stopWatch;
        private string _perfLogFile;
        private int _noMeasure;
        StringBuilder _result;
        double _processorTimeInit;

        public PerformanceMeasure(string perfLogFile, bool directStart)
        {
            this._result = new StringBuilder();

            this._perfLogFile = perfLogFile;
            this._currentProcess = Process.GetCurrentProcess();

            this._stopWatch = new Stopwatch();
            this._noMeasure = 0;

            string header = "NoMeasure;TimePassed[ms];CpuUsage[ms];CpuUsage[%]";
            _result.Append(header);
            _result.Append(System.Environment.NewLine);

            this._processorTimeInit = this._currentProcess.TotalProcessorTime.TotalMilliseconds;

            if (directStart)
                this.swStart();
        }

        public string WriteMeasure()
        {
            double cpuTime = this._currentProcess.TotalProcessorTime.TotalMilliseconds - this._processorTimeInit;
            double runTime = this._stopWatch.ElapsedMilliseconds;
            double cpuPercentage = cpuTime / runTime;

            //string header = "NoMeasure;TimePassed;RamUsage;CpuUsage";
            string line = String.Format("{0};{1};{2};{3}",
                this._noMeasure.ToString(),
                runTime.ToString(),
                cpuTime.ToString(),
                cpuPercentage);

            _result.Append(line);
            _result.Append(System.Environment.NewLine);
            _noMeasure++;
            return line;
        }

        public void swStart()
        {
            _stopWatch.Start();
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  // I'm not sure if this whole Dispose thingie is right -.-
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _currentProcess.Dispose();

            _disposed = true;
        }

        ~PerformanceMeasure()
        {
            Dispose(false);
            System.IO.File.WriteAllText(_perfLogFile, _result.ToString());
        }
    }
}
