using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnCAIEditor
{
    public static class Controller
    {
        public static List<string> TaskforceList = new List<string>();
        public static List<string> TeamList = new List<string>();
        public static List<string> ScriptTypeList = new List<string>();

        //TODO: eerste versie gaat ervanuit dat de AI.ini in de juiste volgorde staat
        //juiste volgorde => lijst van TaskForces staat onder [TaskForces] (als voorbeeld)

        //Kan wellicht een goed voorbeeld zijn:
        //        StreamReader reader = File.OpenText("filename.txt");
        //string line;
        //while ((line = reader.ReadLine()) != null) {
        //    string[] items = line.Split('\t');
        //    int myInteger = int.Parse(items[1]); // Here's your integer.
        //    // Now let's find the path.
        //    string path = null;
        //    foreach (string item in items) {
        //        if (item.StartsWith("item\\") && item.EndsWith(".ddj")) {
        //            path = item;
        //        }
        //    }

        //    // At this point, `myInteger` and `path` contain the values we want
        //    // for the current line. We can then store those values or print them,
        //    // or anything else we like.
        //}

        //Martin adviseert ook:
        //http://en.wikipedia.org/wiki/Interpreter_pattern


        public static void ReadFileAsString(string fileData)
        {
            var splittedFileData = fileData.Split('[');

            //for each found object ('[xxx]')
            foreach (string line in splittedFileData)
            {
                //because we split on '[', we need to re-add it
                string fixedLine = "[" + line;

                //if list definition (excluding AITriggerTypes), skip part
                if (fixedLine.Contains("[TaskForces]") || fixedLine.Contains("[ScriptTypes]") || fixedLine.Contains("[TeamTypes]"))
                    continue;
                //else if its the AITriggerTypes part, do something different
                else if (fixedLine.Contains("[AITriggerTypes]"))
                {
                    Console.WriteLine("Triggers found!");
                    Console.WriteLine(fixedLine);
                }
                //else decide what kind of object it is
                else
                    ReadDataAndAddToList(fixedLine);
            }


            //TODO: object splitsen op enters
            //daarna via reflection variabelen koppelen
            //als in:
            //Name=H_GDI APC/engineer attack
            //zoek via reflection in Team class naar attribuut Name en geef deze de waarde "H_GDI APC/engineer attack"

            Console.WriteLine("TaskforceList");
            foreach (var wat in TaskforceList)
                Console.WriteLine(wat);

            Console.WriteLine("ScriptTypeList");
            foreach (var wat in ScriptTypeList)
                Console.WriteLine(wat);

            Console.WriteLine("TeamList");
            foreach (var wat in TeamList)
                Console.WriteLine(wat);

            Console.WriteLine("THEY'RE DONE");
        }
        
        /// <summary>
        /// Reads the string, decides what it is, and adds it to the correct list.
        /// </summary>
        public static void ReadDataAndAddToList(string data)
        {
            //if contains 'VeteranLevel=', it's a Team
            if (data.Contains("VeteranLevel="))
            {
                //Console.WriteLine("Team");
                //Console.WriteLine(data);
                TeamList.Add(data);
            }
            //if not, but contains 'Group=', it's a Taskforce
            else if (data.Contains("Group="))
            {
                //Console.WriteLine("Taskforce");
                //Console.WriteLine(data);
                TaskforceList.Add(data);
            }
            //if not, but contains 'Name=', it's a ScriptType
            else if (data.Contains("Name="))
            {
                //Console.WriteLine("ScriptType");
                //Console.WriteLine(data);
                ScriptTypeList.Add(data);
            }
        }
    }
}
