using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversityEnrolleesApplication
{
    class Program
    {
        static void Main (string[] args)
        {
            Dictionary <uint, Enrollee> enrollees;
            Dictionary <uint, Specialty> specialties;

            DataReader.ReadData (out enrollees, out specialties);
            Console.In.Read ();
        }
    }
}
