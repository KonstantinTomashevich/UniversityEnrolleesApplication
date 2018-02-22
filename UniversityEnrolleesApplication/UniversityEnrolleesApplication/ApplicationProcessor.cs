using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UniversityEnrolleesApplication
{
    public static class ApplicationProcessor
    {
        public static void Process (Dictionary <uint, Enrollee> enrollees, Dictionary <uint, Specialty> specialties)
        {
            var toProcess = new Stack <Enrollee> ();
            foreach (var enrollee in enrollees)
            {
                toProcess.Push (enrollee.Value);
            }

            while (toProcess.Any ())
            {
                ApplyEnrollees (toProcess, specialties);
                CollectUnapplied (toProcess, specialties);
            }
        }

        private static void ApplyEnrollees (Stack <Enrollee> enrollees, Dictionary <uint, Specialty> specialties)
        {
            while (enrollees.Any ())
            {
                var enrollee = enrollees.Pop ();
                enrollee.AppliedIndex++;

                if (enrollee.AppliedIndex < 0)
                {
                    enrollee.AppliedIndex = 0;
                }

                if (enrollee.AppliedIndex < enrollee.Choices.Count)
                {
                    var specialty = specialties [enrollee.Choices.ElementAt (enrollee.AppliedIndex).Value];

                    int index;
                    for (index = 0; index < specialty.Enrollees.Count; index++)
                    {
                        if (CompareEnrollees.IsFirstEnrolleeBetter (specialty, enrollee, specialty.Enrollees [index]))
                        {
                            specialty.Enrollees.Insert (index, enrollee);
                            break;
                        }
                    }

                    if (index >= specialty.Enrollees.Count)
                    {
                        specialty.Enrollees.Add (enrollee);
                    }
                }
            }
        }
        
        private static void CollectUnapplied (Stack <Enrollee> enrollees, Dictionary <uint, Specialty> specialties)
        {
            foreach (var specialtyPair in specialties)
            {
                var specialty = specialtyPair.Value;
                while (specialty.Enrollees.Count > specialty.MaxEnrolles)
                {
                    int index = specialty.Enrollees.Count - 1;
                    enrollees.Push (specialty.Enrollees [index]);
                    specialty.Enrollees.RemoveAt (index);
                }
            }
        }
    }
}