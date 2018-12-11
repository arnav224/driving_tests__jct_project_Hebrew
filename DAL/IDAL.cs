using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IDAL
    {
        void AddTester(BE.Tester tester);
        void RemoveTester(string ID);
        void UpdateTester(BE.Tester tester);
        BE.Tester GetTesterCopy(string ID);

        void AddTrainee(BE.Trainee trainee);
        void RemoveTrainee(string ID);
        void UpdateTrainee(BE.Trainee trainee);
        BE.Trainee GetTraineeCopy(string ID);

        void AddTest(BE.Test test);
        void UpdateTest(BE.Test test);
        BE.Test GetTestCopy(int ID);

        IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null);
        IEnumerable<BE.Trainee> GetAllTrainees(Func<BE.Trainee, bool> predicate = null);
        IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null);

        //int Maps_DrivingDistance(string sourceAddress, string destinationAddress);
        //List<string> Maps_GetPlaceAutoComplete(string str);
        //bool SendingEmail(string recipients, string subject, string body);
    }
}
