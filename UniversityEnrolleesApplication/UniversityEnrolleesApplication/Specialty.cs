using System.Collections.Generic;

namespace UniversityEnrolleesApplication
{
    struct RequiredCT
    {
        public RequiredCT (uint _SubjectID, bool _UsedInPerExamComparision)
        {
            SubjectID = _SubjectID;
            UsedInPerExamComparision = _UsedInPerExamComparision;
        }

        public uint SubjectID;
        public bool UsedInPerExamComparision;
    }

    struct Specialty
    {
        public Specialty (string _Name, uint _Type, uint _MaxEnrollees, bool _IsPedagogical)
        {
            Name = _Name;
            Type = _Type;
            MaxEnrolles = _MaxEnrollees;
            IsPedagogical = _IsPedagogical;

            RequiredCTs = new SortedList <uint, RequiredCT> ();
            SchoolMarksPriorities = new SortedList <uint, uint> ();
            AcceptedRODSubjects = new List <uint> ();
        }

        public string Name;
        public uint Type;
        public uint MaxEnrolles;
        public bool IsPedagogical;

        public SortedList <uint, RequiredCT> RequiredCTs;
        public SortedList <uint, uint> SchoolMarksPriorities;
        public List <uint> AcceptedRODSubjects;
    }
}
