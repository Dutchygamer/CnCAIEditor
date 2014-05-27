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
        public static List<Trigger> TriggersList = new List<Trigger>();

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
            TriggersList.Clear();
            
            var splittedFileData = fileData.Split('[');

            //for each found object ('[xxx]')
            foreach (string dataBlock in splittedFileData)
            {
                //because we split on '[', we need to re-add it
                string fixedDataBlock = "[" + dataBlock;

                //if list definition or comment line, skip part
                if (fixedDataBlock.Contains("[TaskForces]") || 
                    fixedDataBlock.Contains("[ScriptTypes]") || 
                    fixedDataBlock.Contains("[TeamTypes]") || 
                    fixedDataBlock.StartsWith(";"))
                    continue;
                //else if its the AITriggerTypes part, decompile the triggers
                else if (fixedDataBlock.Contains("[AITriggerTypes]"))
                    DecompileTriggers(fixedDataBlock);
                //else decide what kind of object it is
                else
                    ReadDataAndAddToList(fixedDataBlock);
            }

            Console.WriteLine("THEY'RE DONE");
        }

        public static void DecompileTriggers(string data)
        {
            string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var line in lines)
                //TODO: regex fixen dat ie ook Triggers oppakt met <none> ipv target (en andersom indien Second Team aanwezig is)
                //old
                //@"^[\d\w\-]+=[\d\w\s\._/]+,[\d\w\-]+,<[\w]+>,\d,\d,[\w]+,[\d\w]{64},[\d]+.[\d]+,[\d]+.[\d]+,[\d]+.[\d]+,\d,[0],\d,\d,<[\w]+>,\d,\d,\d$"
                //(possible) new
                //@"^[\d\w\-]+=[\d\w\s\._/]+,[\d\w\-]+,<[\w]+>,\d,\d,[\w]+|<none>,[\d\w]{64},[\d]+.[\d]+,[\d]+.[\d]+,[\d]+.[\d]+,\d,[0],\d,\d,[\w]+|<none>,\d,\d,\d$"
                if (Regex.IsMatch(line, @"^[\d\w\-]+=[\d\w\s\._/]+,[\d\w\-]+,<[\w]+>,\d,\d,[\w]+,[\d\w]{64},[\d]+.[\d]+,[\d]+.[\d]+,[\d]+.[\d]+,\d,[0],\d,\d,<[\w]+>,\d,\d,\d$"))
                    TriggersList.Add(GenerateTrigger(line));
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
        /// Converts string data into Taskforce object
        /// </summary>
        private static Taskforce GenerateTaskforce(string data)
        {
            string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>();
            var unitlist = new List<string>();

            foreach (var line in lines)
            {
                //if a comment, skip
                if (line.StartsWith(";"))
                {
                    continue;
                }
                //if the first line, manually set to ID
                else if (line.StartsWith("["))
                {
                    var id = line.Trim(new Char[] { '[', ']' });
                    dictionary.Add("ID", id);
                }
                //if it starts with a number, add to unit list
                else if (Regex.IsMatch(line, @"^\d"))
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
                //if a comment, skip
                if (line.StartsWith(";"))
                {
                    continue;
                }
                //if the first line, manually set to ID
                else if (line.StartsWith("["))
                {
                    var id = line.Trim(new Char[] { '[', ']' });
                    dictionary.Add("ID", id);
                }
                //if it starts with a number, add to unit list
                else if (Regex.IsMatch(line, @"^\d"))
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

        /// <summary>
        /// Converts string data into Team object
        /// </summary>
        private static Team GenerateTeam(string data)
        {
            string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                //if a comment, skip
                if (line.StartsWith(";"))
                {
                    continue;
                }
                //if the first line, manually set to ID
                else if (line.StartsWith("["))
                {
                    var id = line.Trim(new Char[] { '[', ']' });
                    dictionary.Add("ID", id);
                }
                //else, it's a normal field we need to add to the dictionary
                else
                {
                    var keyvalue = line.Split('=');
                    dictionary.Add(keyvalue[0], keyvalue[1]);
                }
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
        /// Converts string data into Trigger object
        /// </summary>
        private static Trigger GenerateTrigger(string data)
        {
            var eins = data.Split('=');
            var zwei = eins[1].Split(',');

            var lines = new List<string>();
            lines.Add(eins[0]);
            lines.AddRange(zwei);

            //TODO: foutafhandeling indien gare waarden die niet naar int/float/bool geconvert kunnen worden

            var result = new Trigger
            {
                ID = lines[0],                                      //0981GEB0-G=
                Name = lines[1],                                    //HM_GDI emporca assisted vehicle attack,
                TeamID = lines[2],                                  //09GAESV1-G,
                //Team = lines[2],                                  
                Owner = lines[3],                                   //<all>,
                TechLevel = Int32.Parse(lines[4]),                  //9,
                //TriggerType = lines[5],                             //-1,
                TechTypeID = lines[6],                              //GAHPAD,
                //TechType = lines[6],
                TriggerValue = lines[7],                            //0100000003000000000000000000000000000000000000000000000000000000,
                WeigthedProbability = float.Parse(lines[8]),        //70.000000,
                MinWeigthedProbability = float.Parse(lines[9]),     //10.000000,
                MaxWeigthedProbability = float.Parse(lines[10]),    //80.000000,
                AvailableInSkirmish = StringToBool(lines[11]),        //1,
                //DummyValue = lines[12],                            //0,
                //SideOwner = lines[13],                              //1,
                IsBaseDefence = StringToBool(lines[14]),              //0,
                SupportTeamID = lines[15],                          //<none>,
                //SupportTeam = lines[15], 
                IsEasy = StringToBool(lines[16]),                     //0,
                IsMedium = StringToBool(lines[17]),                   //1,
                IsHard = StringToBool(lines[18])                      //1
            };

            return result;
        }

        private static bool StringToBool(string value)
        {
            return (value == "1") ? true : false;
        }
    }
}