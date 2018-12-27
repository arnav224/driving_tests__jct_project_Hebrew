using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using BE;

namespace DAL
{
    public class Dal_imp : IDAL
    {
        internal Dal_imp()
        {
            DS.DataSource.Initializer();
        }

        private BE.Tester GetTester(string ID)
        {
            return DS.DataSource.testers.FirstOrDefault(t => t.ID == ID);
        }

        public BE.Tester GetTesterCopy(string ID)
        {
            BE.Tester tester = DS.DataSource.testers.FirstOrDefault(t => t.ID == ID);
            if (tester == null)
                return null;
            return tester.Clone();
        }

        private BE.Test GetTest(int ID)
        {
            return DS.DataSource.tests.FirstOrDefault(t => t.TestID == ID);
        }

        public BE.Test GetTestCopy(int ID)
        {
            BE.Test test = DS.DataSource.tests.FirstOrDefault(t => t.TestID == ID);
            if (test == null)
                return null;
            return test.Clone();
        }

        private BE.Trainee GetTrainee(string ID)
        {
            return DS.DataSource.trainees.FirstOrDefault(t => t.ID == ID);
        }

        public BE.Trainee GetTraineeCopy(string ID)
        {
            BE.Trainee trainee = DS.DataSource.trainees.FirstOrDefault(t => t.ID == ID);
            if (trainee == null)
                return null;
            return trainee.Clone();
        }

        public void AddTest(BE.Test test)
        {
            DS.DataSource.tests.Add(test.Clone());
        }

        public void AddTester(BE.Tester tester)
        {
            BE.Tester ExsistTester = GetTester(tester.ID);
            if (ExsistTester != null)
                throw new Exception("הבוחן כבר קיים במערכת");
            DS.DataSource.testers.Add(tester.Clone());
        }

        public void AddTrainee(BE.Trainee trainee)
        {
            BE.Trainee ExsistTrainee = GetTrainee(trainee.ID);
            if (ExsistTrainee != null)
                throw new Exception("התלמיד כבר קיים במערכת");
            DS.DataSource.trainees.Add(trainee.Clone());
        }

        public IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null)
        {
            if (predicate == null)
                return DS.DataSource.testers.AsEnumerable().Select(t => t.Clone());
            return DS.DataSource.testers.Where(predicate).Select(t => t.Clone());
        }

        public IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null)
        {
            if (predicate == null)
                return DS.DataSource.tests.AsEnumerable().Select(t => t.Clone());
            return DS.DataSource.tests.Where(predicate).Select(t => t.Clone());
        }

        public IEnumerable<BE.Trainee> GetAllTrainees(Func<BE.Trainee, bool> predicate = null)
        {
            if (predicate == null)
                return DS.DataSource.trainees.AsEnumerable().Select(t => t.Clone());
            return DS.DataSource.trainees.Where(predicate).Select(t => t.Clone());
        }

        public void RemoveTester(string ID)
        {
            BE.Tester tester = GetTester(ID);
            if (tester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + ID);
            DS.DataSource.testers.Remove(tester);
        }

        public void RemoveTrainee(string ID)
        {
            BE.Trainee trainee = GetTrainee(ID);
            if (trainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + ID);
            DS.DataSource.trainees.Remove(trainee);
        }

        public void UpdateTestResult(BE.Test test)
        {
            int indexTest = DS.DataSource.tests.FindIndex(t => t.TestID == test.TestID);
            if (indexTest == -1)
                throw new KeyNotFoundException("לא נמצא מבחן שמספרו " + test.TestID);
            DS.DataSource.tests[indexTest] = test.Clone();
        }

        public void UpdateTester(BE.Tester tester)
        {
            int indexTester = DS.DataSource.testers.FindIndex(t => t.ID == tester.ID);
            if (indexTester == -1)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + tester.ID);
            DS.DataSource.testers[indexTester] = tester.Clone();
        }

        public void UpdateTrainee(BE.Trainee trainee)
        {
            int indexTrainee = DS.DataSource.trainees.FindIndex(t => t.ID == trainee.ID);
            if (indexTrainee == -1)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + trainee.ID);
            DS.DataSource.trainees[indexTrainee] = trainee.Clone();
        }

        public void RemoveTest(int ID)
        {
            BE.Test test = GetTest(ID);
            if (test == null)
                throw new KeyNotFoundException("לא נמצא טסט שמספרו " + ID);
            DS.DataSource.tests.Remove(test);
        }
    }
}
