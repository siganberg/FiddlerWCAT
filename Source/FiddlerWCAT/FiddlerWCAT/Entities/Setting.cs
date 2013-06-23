using System;

namespace FiddlerWCAT.Entities
{
    public class Settings
    {
        private static Settings _instance;
        private static readonly object PadLock = new Object();
        private static Settings Instance
        {
            get
            {
                if (_instance == null || _instance.HasChange())
                {
                    lock (PadLock)
                    {
                        if (_instance == null || _instance.HasChange())
                        {
                            _instance = new Settings();
                        }
                    }
                }
                return _instance;
            }
        }


        public string WCATHomeDirectory { get; set; }
        public int Duration { get; set; }
        public int Cooldown { get; set; }
        public int Warmup { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Settings()
        {

        }

        private bool HasChange()
        {
            //-- routine code to invalidate the singleton instance such as reading some configuration 
            return false;
        }
    }
}
