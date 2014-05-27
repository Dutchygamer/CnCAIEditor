using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnCAIEditor.Objects
{
    public class Taskforce
    {
        public string ID { get; set; }

        public string Name { get; set; }

        //TODO: List van specifieke Member objecten en hoeveelheid
        public List<string> Members { get; set; }

        public string Group { get; set; }
    }
}
