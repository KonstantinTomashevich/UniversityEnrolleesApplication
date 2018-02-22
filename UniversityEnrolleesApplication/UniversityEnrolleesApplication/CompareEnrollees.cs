using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityEnrolleesApplication
{
    static class CompareEnrollees
    {
        public static bool IsFirstEnrolleeBetter (Specialty specialty, Enrollee first, Enrollee second)
        {
            // TODO: Not all BSU checks implemented.
            bool result;
            if (CheckIsBothCanChoiceSpecialty (specialty, first, second, out result) ||
                CheckRODs (specialty, first, second, out result) ||
                CheckSchoolGoldMedals (specialty, first, second, out result) ||
                CheckScores (specialty, first, second, out result) ||
                CheckPrimaryExams (specialty, first, second, out result) ||
                CheckSchoolCertificateMarks (specialty, first, second, out result))
            {
                return result;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckIsBothCanChoiceSpecialty (Specialty specialty, Enrollee first, Enrollee second,
            out bool resultOutput)
        {
            if (!CanEnrolleeChoiceSpecialty (specialty, first))
            {
                resultOutput = false;
                return true;
            }
            else if (!CanEnrolleeChoiceSpecialty (specialty, second))
            {
                resultOutput = true;
                return true;
            }
            else
            {
                resultOutput = false;
                return false;
            }
        }

        private static bool CheckRODs (Specialty specialty, Enrollee first, Enrollee second, out bool resultOutput)
        {
            if (first.RODDiplomaType != RODDiplomaTypeEnum.NONE && second.RODDiplomaType != RODDiplomaTypeEnum.NONE)
            {
                if (specialty.AcceptedRODSubjects.Contains (first.RODSubjectID) &&
                    specialty.AcceptedRODSubjects.Contains (second.RODSubjectID))
                {
                    resultOutput = first.RODDiplomaType < second.RODDiplomaType;
                    return true;
                }
                else if (specialty.AcceptedRODSubjects.Contains (first.RODSubjectID))
                {
                    resultOutput = true;
                    return true;
                }
                else if (specialty.AcceptedRODSubjects.Contains (second.RODSubjectID))
                {
                    resultOutput = false;
                    return true;
                }
            }

            else if (first.RODDiplomaType != RODDiplomaTypeEnum.NONE &&
                     specialty.AcceptedRODSubjects.Contains (first.RODSubjectID))
            {
                resultOutput = true;
                return true;
            }

            else if (second.RODDiplomaType != RODDiplomaTypeEnum.NONE &&
                     specialty.AcceptedRODSubjects.Contains (second.RODSubjectID))
            {
                resultOutput = false;
                return true;
            }

            resultOutput = false;
            return false;
        }

        private static bool CheckSchoolGoldMedals (Specialty specialty, Enrollee first, Enrollee second,
            out bool resultOutput)
        {
            if (specialty.IsPedagogical)
            {
                if (first.HasSchoolGoldMedal && !second.HasSchoolGoldMedal)
                {
                    resultOutput = true;
                    return true;
                }
                else if (!first.HasSchoolGoldMedal && second.HasSchoolGoldMedal)
                {
                    resultOutput = false;
                    return true;
                }
            }

            resultOutput = false;
            return false;
        }

        private static bool CheckScores (Specialty specialty, Enrollee first, Enrollee second, out bool resultOutput)
        {
            uint firstScore = CalculateEnrolleeScore (specialty, first);
            uint secondScore = CalculateEnrolleeScore (specialty, second);

            if (firstScore != secondScore)
            {
                resultOutput = firstScore > secondScore;
                return true;
            }
            else
            {
                resultOutput = false;
                return false;
            }
        }

        private static bool CheckPrimaryExams (Specialty specialty, Enrollee first, Enrollee second,
            out bool resultOutput)
        {
            var firstBestResultsInPriority = GetBestResultsInPriorities (specialty, first, false);
            var secondBestResultsInPriority = GetBestResultsInPriorities (specialty, second, false);

            foreach (var firstBestResult in firstBestResultsInPriority)
            {
                if (firstBestResult.Value != secondBestResultsInPriority [firstBestResult.Key])
                {
                    resultOutput = firstBestResult.Value > secondBestResultsInPriority [firstBestResult.Key];
                    return true;
                }
            }

            resultOutput = false;
            return false;
        }

        private static bool CheckSchoolCertificateMarks (Specialty specialty, Enrollee first, Enrollee second,
            out bool resultOutput)
        {
            foreach (var subject in specialty.SchoolMarksPriorities)
            {
                uint firstMark = first.SchoolMarks [subject.Value];
                uint secondMark = second.SchoolMarks [subject.Value];

                if (firstMark != secondMark)
                {
                    resultOutput = firstMark > secondMark;
                    return true;
                }
            }

            resultOutput = false;
            return false;
        }

        private static bool CanEnrolleeChoiceSpecialty (Specialty specialty, Enrollee enrollee)
        {
            if (enrollee.RODDiplomaType != RODDiplomaTypeEnum.NONE &&
                specialty.AcceptedRODSubjects.Contains (enrollee.RODSubjectID))
            {
                return true;
            }
            else
            {
                var bestResultsInPriority = GetBestResultsInPriorities (specialty, enrollee);
                foreach (var result in bestResultsInPriority)
                {
                    if (result.Value == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private static uint CalculateEnrolleeScore (Specialty specialty, Enrollee enrollee)
        {
            uint score = enrollee.SchoolMedianMarkPoints;

            var bestResultsInPriority = GetBestResultsInPriorities (specialty, enrollee);
            foreach (var result in bestResultsInPriority)
            {
                score += result.Value;
            }

            return score;
        }

        private static Dictionary <uint, uint> GetBestResultsInPriorities (Specialty specialty, Enrollee enrollee,
            bool addNotCompared = true)
        {
            var bestResultsInPriority = new Dictionary <uint, uint> ();
            foreach (var requiredCt in specialty.RequiredCTs)
            {
                if (requiredCt.Value.UsedInPerExamComparision || addNotCompared)
                {
                    if (!bestResultsInPriority.ContainsKey (requiredCt.Key) ||
                        bestResultsInPriority [requiredCt.Key] < enrollee.CTsResults [requiredCt.Value.SubjectID])
                    {
                        bestResultsInPriority [requiredCt.Key] = enrollee.CTsResults [requiredCt.Value.SubjectID];
                    }
                }
            }

            return bestResultsInPriority;
        }
    }
}