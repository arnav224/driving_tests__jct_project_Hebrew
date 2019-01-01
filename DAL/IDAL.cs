using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IDAL
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
        /// Get deep clone of Tester by Id.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        BE.Tester GetTesterCopy(string ID);

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
        /// Get deep clone of Trainee by Id.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        BE.Trainee GetTraineeCopy(string ID);

        /// <summary>
        /// add Test to the DataBase
        /// </summary>
        /// <param name="test"></param>
        void AddTest(BE.Test test);
        /// <summary>
        /// Remove Test
        /// </summary>
        /// <param name="ID"></param>
        void RemoveTest(int ID);
        /// <summary>
        /// Update test results when done.
        /// </summary>
        /// <param name="test"></param>
        void UpdateTestResult(BE.Test test);
        /// <summary>
        /// Get deep clone of Test by Id.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        BE.Test GetTestCopy(int ID);

        /// <summary>
        /// Get All Testers
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all testers</param>
        /// <returns></returns>
        IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null);
        /// <summary>
        /// Get All Trainees
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all trainees</param>
        /// <returns></returns>
        IEnumerable<BE.Trainee> GetAllTrainees(Func<BE.Trainee, bool> predicate = null);
        /// <summary>
        /// Get All Tests
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all tests</param>
        /// <returns></returns>
        IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null);

        string GetEmailTemltateTestRemeinder(int TestID, string NoteToAdd = "");
        void UpdateEmailSendingTime(int testID, DateTime? SummaryEmailSent = null, DateTime? RemeinderEmailSent = null);

    }
}
