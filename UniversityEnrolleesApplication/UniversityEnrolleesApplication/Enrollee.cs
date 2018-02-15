using System.Collections.Generic;

namespace UniversityEnrolleesApplication
{
    enum RODDiplomaTypeEnum
    {
        FIRST = 1,
        SECOND = 2,
        THIRD = 3,
        NONE = 4
    }

    struct Enrollee
    {
        public Enrollee (bool _HasSchoolGoldMedal, string _RODDiplomaType, uint _RODSubjectID)
        {
            HasSchoolGoldMedal = _HasSchoolGoldMedal;
            RODSubjectID = _RODSubjectID;

            if (_RODDiplomaType == "First")
            {
                RODDiplomaType = RODDiplomaTypeEnum.FIRST;
            }
            else if (_RODDiplomaType == "Second")
            {
                RODDiplomaType = RODDiplomaTypeEnum.SECOND;
            }
            else if (_RODDiplomaType == "Third")
            {
                RODDiplomaType = RODDiplomaTypeEnum.THIRD;
            }
            else
            {
                RODDiplomaType = RODDiplomaTypeEnum.NONE;
            }

            CTsResults = new Dictionary <uint, uint> ();
            SchoolMarks = new Dictionary <uint, uint> ();
            Choices = new SortedList <uint, uint> ();

            SchoolMedianMarkPoints = 0;
        }

        public bool HasSchoolGoldMedal;
        public RODDiplomaTypeEnum RODDiplomaType;
        public uint RODSubjectID;

        public Dictionary <uint, uint> CTsResults;
        public Dictionary <uint, uint> SchoolMarks;
        public SortedList <uint, uint> Choices;

        // Runtime.
        public uint SchoolMedianMarkPoints;
    }
}
