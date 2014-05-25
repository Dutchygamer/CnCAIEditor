using CnCAIEditor.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CnCAIEditor
{
    public static class Controller
    {
        public static List<Taskforce> TaskforceList = new List<Taskforce>();
        public static List<Team> TeamList = new List<Team>();
        public static List<ScriptType> ScriptTypeList = new List<ScriptType>();

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
            //clear all lists when reading new file
            TaskforceList.Clear();
            TeamList.Clear();
            ScriptTypeList.Clear();
            
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
                    //Console.WriteLine(fixedLine);
                }
                //else decide what kind of object it is
                else
                    ReadDataAndAddToList(fixedLine);
            }

            //Console.WriteLine("TaskforceList");
            //foreach (var wat in TaskforceList)
            //    Console.WriteLine(wat);

            //Console.WriteLine("ScriptTypeList");
            //foreach (var wat in ScriptTypeList)
            //    Console.WriteLine(wat);

            //Console.WriteLine("TeamList");
            //foreach (var wat in TeamList)
            //    Console.WriteLine(wat);

            Console.WriteLine("THEY'RE DONE");
        }

        /// <summary>
        /// Reads the string, decides what it is, and adds it to the correct list.
        /// </summary>
        public static void ReadDataAndAddToList(string data)
        {
            try
            {
                //if contains 'VeteranLevel=', it's a Team
                if (data.Contains("VeteranLevel="))
                {
                    TeamList.Add(GenerateTeam(data));
                }
                //if not, but contains 'Group=', it's a Taskforce
                else if (data.Contains("Group="))
                {
                    TaskforceList.Add(GenerateTaskforce(data));
                }
                //if not, but contains 'Name=', it's a ScriptType
                else if (data.Contains("Name="))
                {
                    ScriptTypeList.Add(GenerateScriptTypes(data));
                }
            }
            catch (Exception e)
            {
                var moo = "mpp";
            }
        }

        /// <summary>
        /// Converts string data into Team object
        /// </summary>
        private static Team GenerateTeam(string data)
        {
            string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                //if the first line, skip
                if (line.StartsWith("[") || line.StartsWith(";"))
                    continue;
                //split into key and value, and add to dictionary
                var keyvalue = line.Split('=');
                dictionary.Add(keyvalue[0], keyvalue[1]);
            }

            var result = new Team();
            //loop trough all fields of Team and find matching value in dictionary
            foreach (var field in typeof(Team).GetProperties())
            {
                string value;
                if(dictionary.TryGetValue(field.Name, out value))
                    field.SetValue(result, value);
            }

            return result;
        }

        /// <summary>
        /// Converts string data into Taskforce object
        /// </summary>
        private static Taskforce GenerateTaskforce(string data)
        {
            string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>();
            var unitlist = new List<string>();

            foreach (var line in lines)
            {
                //if the first line or a comment, skip
                if (line.StartsWith("[") || line.StartsWith(";"))
                    continue;
                //if it starts with a number, add to unit list
                if (Regex.IsMatch(line, @"^\d"))
                {
                    var keyvalue = line.Split('=');
                    //if it contains an actual value, add to unit list
                    if (!String.IsNullOrEmpty(keyvalue[1]))
                        unitlist.Add(keyvalue[1]);
                }
                //else, it's a normal field we need to add to the dictionary
                else
                {
                    var keyvalue = line.Split('=');
                    dictionary.Add(keyvalue[0], keyvalue[1]);
                }
            }

            //loop trough all fields of Taskforce and find matching value in dictionary
            var result = new Taskforce();
            foreach (var field in typeof(Taskforce).GetProperties())
            {
                //if its the Members field, give it the unitlist
                if (field.Name == "Members")
                    field.SetValue(result, unitlist);
                //else give it the value from the dictionary
                else
                {
                    string value;
                    if (dictionary.TryGetValue(field.Name, out value))
                        field.SetValue(result, value);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts string data into ScriptType object
        /// </summary>
        private static ScriptType GenerateScriptTypes(string data)
        {
            string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>();
            var unitlist = new List<string>();

            foreach (var line in lines)
            {
                //if the first line or a comment, skip
                if (line.StartsWith("[") || line.StartsWith(";"))
                    continue;
                //if it starts with a number, add to unit list
                if (Regex.IsMatch(line, @"^\d"))
                {
                    var keyvalue = line.Split('=');
                    //if it contains an actual value, add to unit list
                    if (!String.IsNullOrEmpty(keyvalue[1]))
                        unitlist.Add(keyvalue[1]);
                }
                //else, it's a normal field we need to add to the dictionary
                else
                {
                    var keyvalue = line.Split('=');
                    dictionary.Add(keyvalue[0], keyvalue[1]);
                }
            }

            //loop trough all fields of Taskforce and find matching value in dictionary
            var result = new ScriptType();
            foreach (var field in typeof(ScriptType).GetProperties())
            {
                //if its the Members field, give it the unitlist
                if (field.Name == "Actions")
                    field.SetValue(result, unitlist);
                //else give it the value from the dictionary
                else
                {
                    string value;
                    if (dictionary.TryGetValue(field.Name, out value))
                        field.SetValue(result, value);
                }
            }

            return result;
        }

        //TODO: Trigger object
        //TODO: Trigger generate function
    }
}
