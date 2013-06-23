using System;
using System.Collections.Generic;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Scenario 
    {
        public string Name { get; set; }
        public int Warmup { get; set; }
        public int Duration { get; set; }
        public int Cooldown { get; set; }
        public int ThrottleRps { get; set; }
        public Default Default { get; set; }
        public List<Transaction> Transaction { get; set; }


        public Scenario()
        {
            //-- Initialize with  default Settings
            Transaction = new List<Transaction>();
            Duration = 60;
            Warmup = 10;
            Cooldown = 10; 
            Default = new Default();
        }

        public void Optimize()
        {
            
        }
    }
}