using System.Collections.Generic;

namespace UniversityEnrolleesApplication
{
    struct Enrollee
    {
        public Enrollee (bool _HasSchoolGoldMedal, string _RODDiplomaType, uint _RODSubjectID)
        {
            HasSchoolGoldMedal = _HasSchoolGoldMedal;
            RODDiplomaType = _RODDiplomaType;
            RODSubjectID = _RODSubjectID;

            CTsResults = new Dictionary <uint, uint> ();
            SchoolMarks = new Dictionary <uint, uint> ();
            Choices = new SortedList <uint, uint> ();
        }

        public bool HasSchoolGoldMedal;
        public string RODDiplomaType;
        public uint RODSubjectID;

        public Dictionary <uint, uint> CTsResults;
        public Dictionary <uint, uint> SchoolMarks;
        public SortedList <uint, uint> Choices;
    }
}
