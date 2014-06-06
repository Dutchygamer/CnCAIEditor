using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnCAIEditor.Objects
{
    public class Trigger
    {
        //defaults
        public Trigger()
        {
            AvailableInSkirmish = true;
            DummyValue = 0;
            IsBaseDefence = false;
            SupportTeamID = "<none>";
        }

        public string ID { get; set; }

        public string Name { get; set; }

        //TODO: not sure if needed
        public string TeamID { get; set; }

        public Team Team { get; set; }

        public string Owner { get; set; }

        public int TechLevel { get; set; }

        public AITriggerType TriggerType { get; set; }

        public string TechTypeID { get; set; }

        //TODO: RulesObject maken
        //public RulesObject TechType {get; set;}

        public string TriggerValue { get; set; }

        public decimal WeigthedProbability { get; set; }

        public decimal MinWeigthedProbability { get; set; }

        public decimal MaxWeigthedProbability { get; set; }

        //TODO: either 0 or 1, else possibly change to int
        public bool AvailableInSkirmish { get; set; }

        //private because it's a dummy value that we need
        private int DummyValue { get; set; }

        public SideOwnership SideOwner { get; set; }

        //TODO: either 0 or 1, else possibly change to int
        public bool IsBaseDefence { get; set; }

        //TODO: not sure if needed
        public string SupportTeamID { get; set; }

        public Team SupportTeam { get; set; }
        
        //TODO: either 0 or 1, else possibly change to int
        public bool IsEasy { get; set; }
        
        //TODO: either 0 or 1, else possibly change to int
        public bool IsMedium { get; set; }
        
        //TODO: either 0 or 1, else possibly change to int
        public bool IsHard { get; set; }
    }

    public enum AITriggerType
    {
        Pool = -1,
        EnemyOwns = 0,
        AIOwns = 1,
        EnemyCriticalPower = 2,
        EnemyLowPower = 3,
        EnemyCredits = 4,
        //RA2 only
        AIIronCurtain = 5,
        AIChronosphere = 6,
        NeutralOwns = 7
    }

    public enum SideOwnership
    {
        None = 0,
        GDI = 1,
        Nod = 2,
        //RA2YR only
        Yuri = 3
    }
}

/*
 * //0981GEB0-G=
 * //HM_GDI emporca assisted vehicle attack,
 * //09GAESV1-G,
 * //<all>,
 * //9,
 * //-1,
 * //GAHPAD,
 * //0100000003000000000000000000000000000000000000000000000000000000,
 * //70.000000,
 * //10.000000,
 * //80.000000,
 * //1,
 * //0,
 * //1,
 * //0,
 * //<none>,
 * //0,
 * //1,
 * //1
 */