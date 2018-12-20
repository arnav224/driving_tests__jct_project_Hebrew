using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public interface IBL
    {
        /// <summary>
        /// add Tester to the DataBase
        /// </summary>
        /// <param name="tester"></param>
        void AddTester(BE.Tester tester);
        /// <summary>
        /// Remove Tester from the DataBase
        /// </summary>
        /// <param name="ID">the ID fo Tester to remove</param>
        void RemoveTester(string ID);
        /// <summary>
        /// Update relevant properties of Tester
        /// </summary>
        /// <param name="tester"></param>
        void UpdateTester(BE.Tester tester);

        /// <summary>
        /// add Trainee to the DataBase
        /// </summary>
        /// <param name="trainee"></param>
        void AddTrainee(BE.Trainee trainee);
        /// <summary>
        /// Remove Trainee from the DataBase
        /// </summary>
        /// <param name="ID">the ID fo Trainee to remove</param>
        void RemoveTrainee(string ID);
        /// <summary>
        /// Update relevant properties of Trainee
        /// </summary>
        /// <param name="trainee"></param>
        void UpdateTrainee(BE.Trainee trainee);

        /// <summary>
        /// add Test to the DataBase
        /// </summary>
        /// <param name="test"></param>
        void AddTest(BE.Test test);
        /// <summary>
        /// update address and time fo test
        /// </summary>
        /// <param name="test"></param>
        void UpdateTest(BE.Test test);
        /// <summary>
        /// Update test results when done.
        /// </summary>
        /// <param name="TestID"></param>
        /// <param name="indices">@</param>
        void UpdateTestResult(BE.Test test);
        /// <summary>
        /// Remove Test
        /// </summary>
        /// <param name="ID"></param>
        void RemoveTest(int ID);

        /// <summary>
        /// Get All Testers
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all testers</param>
        /// <returns></returns>
        IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null);
        /// <summary>
        /// Find testers who are available for test on the given date.
        /// </summary>
        /// <param name="TestTime">Date requested for test</param>
        /// <returns></returns>
        IEnumerable<BE.Tester> GetAllTesters(DateTime dateTime);
        /// <summary>
        /// Get All Testers Who are willing to travel to the proposed address
        /// </summary>
        /// <param name="address">The location of the test</param>
        /// <returns></returns>
        IEnumerable<BE.Tester> GetAllTesters(string address);
        IEnumerable<BE.Tester> GetAllTesters(string searchString, BE.Gender? gender, BE.GearBoxType? gearBoxType,
                                             BE.Vehicle? vahicle, DateTime? FromTime, DateTime? ToTime);
        /// <summary>
        /// Get All Tests
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all tests</param>
        /// <returns></returns>
        IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null);
        /// <summary>
        /// Get All Testers Who are available at the time
        /// </summary>
        /// <param name="dateTime">Time of the test</param>
        /// <returns></returns>
        IEnumerable<BE.Test> GetAllTests(DateTime dateTime);
        //IEnumerable<BE.Test> GetAllTests(string searchString, BE.GearBoxType? gearBoxType,
        //                                                BE.Vehicle? vahicle, DateTime? FromTime, DateTime? ToTime, bool? passed);
        /// <summary>
        /// Get All Trainees
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all trainees</param>
        /// <returns></returns>
        IEnumerable<BE.Trainee> GetAllTrainees(Func<BE.Trainee, bool> predicate = null);
        IEnumerable<BE.Trainee> GetAllTrainees(string searchString, BE.Gender? gender, BE.GearBoxType? gearBoxType,
                                                        BE.Vehicle? vahicle, DateTime? FromTime, DateTime? ToTime, bool? passed);
        IGrouping<BE.Vehicle, BE.Tester> GetTestersGroupByVehicle(bool sorted = false);
        IGrouping<string, BE.Trainee> GetTraineesGroupBySchool(bool sorted = false);
        IGrouping<string, BE.Trainee> GetTraineesGroupByTeacher(bool sorted = false);
        IGrouping<int, BE.Trainee> GetTraineesGroupByNumOfTests(bool sorted = false);
        /// <summary>
        /// Number of registered student tests
        /// </summary>
        /// <param name="TrayneeId"></param>
        /// <returns></returns>
        int NumOfTests(string TrayneeId);
        /// <summary>
        /// whether the student is successful in any test
        /// </summary>
        /// <param name="TrayneeId"></param>
        /// <returns></returns>
        bool PassedTest(string TrayneeId);

        void SendTestsRemindersLoop();
    }
}
