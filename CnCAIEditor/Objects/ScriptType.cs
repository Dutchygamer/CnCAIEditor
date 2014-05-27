using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnCAIEditor.Objects
{
    public class ScriptType
    {
        public string ID { get; set; }
        public string Name { get; set; }

        //TODO: List van specifieke Action objecten en volgorde ID
        public List<string> Actions { get; set; }
    }
}