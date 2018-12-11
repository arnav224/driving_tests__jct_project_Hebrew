using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public interface IBL
    {
        void AddTester(BE.Tester tester);
        void RemoveTester(string ID);
        void UpdateTester(BE.Tester tester);
        //p BE.Tester GetTester(int ID);

        void AddTrainee(BE.Trainee trainee);
        void RemoveTrainee(string ID);
        void UpdateTrainee(BE.Trainee trainee);

        void AddTest(string TraineeId, DateTime time, string address);
        void UpdateTestResult(int TestID, BE.Indices indices);
        void RemoveTest(string ID);//הוספה שלנו

        IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null);
        IEnumerable<BE.Tester> GetAllTesters(DateTime dateTime);
        IEnumerable<BE.Tester> GetAllTesters(string address);
        IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null);
        IEnumerable<BE.Test> GetAllTests(DateTime dateTime);
        IEnumerable<BE.Trainee> GetAllTrainees(Func<BE.Trainee, bool> predicate = null);
        IGrouping<BE.Vehicle, BE.Tester> GetTestersGroupByVehicle(bool sorted = false);
        IGrouping<string, BE.Trainee> GetTraineesGroupBySchool(bool sorted = false);
        IGrouping<string, BE.Trainee> GetTraineesGroupByTeacher(bool sorted = false);
        IGrouping<int, BE.Trainee> GetTraineesGroupByNumOfTests(bool sorted = false);
        int NumOfTests(string TrayneeId);
        bool PassedTest(string TrayneeId);
        //int NumOfTestsInWeek(this BE.Tester tester, DateTime dateTime);

        void SendTestsRemindersLoop();
    }
}
