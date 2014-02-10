using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnCAIEditor
{
    public static class Controller
    {
        public static void ReadFileAsStringArray(string[] fileData)
        {
            //var wat = fileData.Split('[');
            foreach(string line in fileData)
            {
                //if we get here, it's a definition of an object or a list.
                //lists are easily recognized as they always have the same name, on which we can filter
                //unfortunately, we can't tell which object it may be unless we know the next couple of lines
                //Taskforce is recognized by the Group= at the end.
                //Team is recognized by the crapload of tags it has.
                //Script is (not) recognized by the lack of anything unique (only has Name= and some numbers).
                //Trigger is not caught here due to being special definition.

                //Perhaps reading it as a complete string instead of seperate lines is better...
                if(!String.IsNullOrWhiteSpace(line) && line.Substring(0,1) == "[")
                {


                    Console.WriteLine(line);
                }
                //Console.WriteLine(line);

            }



            Console.Write(fileData);
        }

        public static void ReadFileAsString(string fileData)
        {
            var wat = fileData.Split('[');

            foreach (string line in wat)
            {
                string fixme = "[" + line;

                Console.WriteLine(fixme);
            }


            Console.Write(fileData);
        }
    }
}
