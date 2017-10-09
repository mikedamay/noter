using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noter.Entities
{
    public class Note
    {
        public long Id { get; set; }
        public string Payload { get; set; }
        public Note(string payload)
        {
            Payload = payload;
        }
    }
}
