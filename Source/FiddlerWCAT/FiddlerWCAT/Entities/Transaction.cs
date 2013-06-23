using System;
using System.Collections.Generic;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Transaction
    {
        public string Id { get; set; }
        public int Weight { get; set; }
        public List<Request> Request { get; set; }

        public Transaction()
        {
            Request = new List<Request>();
        }
    }

    
}