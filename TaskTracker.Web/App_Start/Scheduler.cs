namespace TaskTracker.Web
{

    using System;
    using System.Net;
    using System.Threading;



    /// <summary>
    /// This is the manager class that handles running the email operation on a background thread
    /// </summary>
    public class Scheduler : IDisposable
    {
        /// <summary>
        /// Determines the status fo the Scheduler
        /// </summary>        
        public bool Cancelled
        {
            get { return _Cancelled; }
            set { _Cancelled = value; }
        }
        private bool _Cancelled = false;


        /// <summary>
        /// The frequency of checks against hte POP3 box are 
        /// performed in Seconds.
        /// </summary>


        AutoResetEvent WaitHandle = new AutoResetEvent(false);

        object SyncLock = new Object();

        public Scheduler()
        {

        }

        /// <summary>
        /// Causes the processing to stop. If the operation is still
        /// active it will stop after the current message processing
        /// completes
        /// </summary>
        public void Stop()
        {
            lock (SyncLock)
            {
                if (Cancelled)
                    return;

                Cancelled = true;
                WaitHandle.Set();
            }
        }

        public void PingServer()
        {
            try
            {
                using (var http = new WebClient())
                {
                   string Result = http.DownloadString("");
                }
                //WebClient http = new WebClient();      
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}