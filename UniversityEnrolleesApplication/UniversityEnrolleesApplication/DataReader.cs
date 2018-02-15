using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace UniversityEnrolleesApplication
{
    static class DataReader
    {
        public static void ReadData (out Dictionary <uint, Enrollee> enrollees, out Dictionary <uint, Specialty> specialties)
        {
            var connection = new MySqlConnection ("server=localhost;database=Enrollees;uid=UEAAUser;password=UEAAUSERPASSWORD");
            connection.Open ();
            ReadEnrollees (out enrollees, connection);
            ReadCTsResults (enrollees, connection);
            ReadSchoolMarks (enrollees, connection);
            ReadChoices (enrollees, connection);

            ReadSpecialties (out specialties, connection);
            ReadRequiredCTs (specialties, connection);
            ReadSchoolMarksPriorities (specialties, connection);
            ReadAcceptedRODSubjects (specialties, connection);
            connection.Close ();
        }

        private static void ReadEnrollees (out Dictionary <uint, Enrollee> enrollees, MySqlConnection connection)
        {
            enrollees = new Dictionary <uint, Enrollee> ();           
            var readEnrollesCommand = new MySqlCommand ("select * from enrollees", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                Enrollee enrollee = new Enrollee (reader.GetBoolean ("HasSchoolGoldMedal"),
                    reader.GetString ("RODDiplomaType"), reader.GetUInt32 ("RODSubjectID"));
                enrollees [reader.GetUInt32 ("ID")] = enrollee;
            }

            reader.Close ();
        }

        private static void ReadCTsResults (Dictionary <uint, Enrollee> enrollees, MySqlConnection connection)
        {
            var readEnrollesCommand = new MySqlCommand ("select * from ctsresults", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                enrollees [reader.GetUInt32 ("BelongsTo")].CTsResults [reader.GetUInt32 ("SubjectID")] = reader.GetUInt32 ("Result");
            }

            reader.Close ();
        }

        private static void ReadSchoolMarks (Dictionary <uint, Enrollee> enrollees, MySqlConnection connection)
        {
            var readEnrollesCommand = new MySqlCommand ("select * from schoolmarks", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                enrollees[reader.GetUInt32 ("BelongsTo")].SchoolMarks [reader.GetUInt32 ("SubjectID")] = reader.GetUInt32 ("Mark");
            }

            reader.Close ();
        }

        private static void ReadChoices (Dictionary <uint, Enrollee> enrollees, MySqlConnection connection)
        {
            var readEnrollesCommand = new MySqlCommand ("select * from enrolleestospecialtiesconnection", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                enrollees [reader.GetUInt32 ("EnrolleeID")].Choices.Add (reader.GetUInt32 ("Priority"), reader.GetUInt32 ("SpecialtyID"));
            }

            reader.Close ();
        }

        private static void ReadSpecialties (out Dictionary <uint, Specialty> specialties, MySqlConnection connection)
        {
            specialties = new Dictionary <uint, Specialty> ();
            var readEnrollesCommand = new MySqlCommand ("select * from specialties", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                Specialty specialty = new Specialty (reader.GetString ("Name"),
                    reader.GetUInt32 ("Type"), reader.GetUInt32 ("MaxEnrollees"), reader.GetBoolean ("IsPedagogical"));
                specialties [reader.GetUInt32 ("ID")] = specialty;
            }

            reader.Close ();
        }

        private static void ReadRequiredCTs (Dictionary <uint, Specialty> specialties, MySqlConnection connection)
        {
            var readEnrollesCommand = new MySqlCommand ("select * from requiredcts", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                specialties [reader.GetUInt32 ("BelongsTo")].RequiredCTs.Add (reader.GetUInt32 ("Priority"),
                    new RequiredCT (reader.GetUInt32 ("SubjectID"), reader.GetBoolean ("UsedInPerExamComparision")));
            }

            reader.Close ();
        }

        private static void ReadSchoolMarksPriorities (Dictionary <uint, Specialty> specialties, MySqlConnection connection)
        {
            var readEnrollesCommand = new MySqlCommand ("select * from schoolmarkspriorities", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                specialties [reader.GetUInt32 ("BelongsTo")].SchoolMarksPriorities.Add (reader.GetUInt32 ("Priority"), reader.GetUInt32 ("SubjectID"));
            }

            reader.Close ();
        }

        private static void ReadAcceptedRODSubjects (Dictionary <uint, Specialty> specialties, MySqlConnection connection)
        {
            var readEnrollesCommand = new MySqlCommand ("select * from acceptedrodsubjects", connection);
            var reader = readEnrollesCommand.ExecuteReader ();

            while (reader.Read ())
            {
                specialties [reader.GetUInt32 ("BelongsTo")].AcceptedRODSubjects.Add (reader.GetUInt32 ("SubjectID"));
            }

            reader.Close ();
        }
    }
}