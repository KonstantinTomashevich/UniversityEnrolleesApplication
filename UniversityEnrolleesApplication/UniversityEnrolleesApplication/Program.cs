using System;
using System.Collections.Generic;

namespace UniversityEnrolleesApplication
{
    class Program
    {
        static void Main (string [] args)
        {
            Dictionary <uint, Enrollee> enrollees;
            Dictionary <uint, Specialty> specialties;

            DataReader.ReadData (out enrollees, out specialties);
            CalculateEnrolleesSchoolMedianMarks (enrollees);
            ApplicationProcessor.Process (enrollees, specialties);
            Console.In.Read ();
        }

        private static void CalculateEnrolleesSchoolMedianMarks (Dictionary <uint, Enrollee> enrollees)
        {
            foreach (var enrolleeData in enrollees)
            {
                var enrollee = enrolleeData.Value;
                uint sum = 0;

                foreach (var mark in enrollee.SchoolMarks)
                {
                    sum += mark.Value;
                }

                enrollee.SchoolMedianMarkPoints =
                    Convert.ToUInt32 (Math.Round (sum * 10.0 / enrollee.SchoolMarks.Count));
            }
        }
    }
}