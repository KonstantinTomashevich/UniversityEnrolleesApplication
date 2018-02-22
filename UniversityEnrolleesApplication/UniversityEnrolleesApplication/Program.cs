using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

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
            UpdateTableWithResults (enrollees);
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

        private static void UpdateTableWithResults (Dictionary <uint, Enrollee> enrollees)
        {
            var connection =
                new MySqlConnection ("server=localhost;database=Enrollees;uid=UEAAUser;password=UEAAUSERPASSWORD");
            connection.Open ();
            
            var command = connection.CreateCommand ();
            command.CommandType = CommandType.Text;
                
            foreach (var enrolleePair in enrollees)
            {
                for (int index = 0; index < enrolleePair.Value.Choices.Count; index++)
                {
                    command.CommandText = "update enrolleestospecialtiesconnection " +
                                          "set IsAttended = " + (index == enrolleePair.Value.AppliedIndex
                                              ? "b'1' "
                                              : "b'0' ") +
                                          "where EnrolleeID = " + enrolleePair.Key +
                                          " and SpecialtyID = " + enrolleePair.Value.Choices.ElementAt (index).Value + ";";
                    command.Prepare ();
                    command.ExecuteNonQuery ();
                }
            }
            
            connection.Close ();
        }
    }
}