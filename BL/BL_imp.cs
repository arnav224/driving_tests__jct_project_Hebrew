using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BE;

namespace BL
{
    public class BL_imp : IBL
    {
        DAL.IDAL IDAL;
        public BL_imp()
        {
            IDAL = DAL.Factory.GetInstance();

        }

        public void AddTest(string TraineeId, DateTime time, /*BE.Address*/ string address)
        {
            BE.Trainee trainee = IDAL.GetTraineeCopy(TraineeId);
            if (trainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + TraineeId);
            if (trainee.NumOfDrivingLessons < BE.Configuration.MinimumDrivingLessons)
                throw new Exception("אין אפשרות להוסיף מבחן לתלמיד שעשה פחות מ-." + BE.Configuration.MinimumDrivingLessons + " שיעורים.");

            BE.Test LastPreviusTest = null, FirstNextTest = null;
            foreach (var item in IDAL.GetAllTests(t => t.TraineeID == TraineeId))
            {
                if (item.Time < time && (LastPreviusTest == null || LastPreviusTest.Time < item.Time))
                    LastPreviusTest = item;
                else if (item.Time >= time && (FirstNextTest == null || LastPreviusTest.Time < item.Time))
                    FirstNextTest = item;
            }
            if (LastPreviusTest != null && (time - LastPreviusTest.Time).Days < BE.Configuration.MinimumDaysBetweenTests
                || FirstNextTest != null && (FirstNextTest.Time - time).Days < BE.Configuration.MinimumDaysBetweenTests)
                throw new Exception("לתלמיד זה קיים מבחן בהפרש של פחות משבעה ימים.");


            //GetTestersGroupByVehicle().Where(g => g.Key == trainee.Vehicle).Select(
            //    G =>
            //    from Tester tr in G
            //    where (Tools.Maps.Distance(tr.Address, address) < tr.MaxDistanceInMeters)
            //    select tr
            //    ).Any(t => true);

            //                                                                       where (Tools.Maps.Distance(t.Address, address) < t.MaxDistanceInMeters)
            //                                                                       select t;//Any(G => Tools.Maps.Distance(G..Address, address) < G.MaxDistanceInMeters)
            ////GetTestersGroupByVehicle().Where(g => g.Key == trainee.Vehicle).Where(t => K => Tools.Maps.Distance(k.Address, address) < k.MaxDistanceInMeters)//.Any(G => Tools.Maps.Distance(G..Address, address) < G.MaxDistanceInMeters)

            //if ((GetTestersGroupByVehicle().Where(g => g.Key == trainee.Vehicle).Select(g => (IEnumerable<Tester>)g)).First().Any(t => Tools.Maps.Distance(t.Address, address) < t.MaxDistanceInMeters))
            //{

            //}

            if (time < DateTime.Now)
                throw new Exception("מועד הטסט חלף");

            //@ נסיון
            //foreach (var item in GetAllTesters(time))
            //{
            //    Console.WriteLine(item.ToString() + '\n');
            //}

            BE.Tester tester = (from item in GetAllTesters(time)
                                where item.Vehicle == trainee.Vehicle
                                && BE.Tools.Maps_DrivingDistance(item.Address, address) < item.MaxDistanceInMeters
                                && (!trainee.OnlyMyGender || item.Gender == trainee.Gender)
                                && item.gearBoxType == trainee.gearBoxType
                                && NumOfTestsInWeek(item, time) < item.MaxTestsInWeek // @
                                select item).FirstOrDefault();


            //if (!GetAllTesters(time).Where(t=> t.Vehicle == trainee.Vehicle 
            //&& (!trainee.OnlyMyGender || t.Gender == trainee.Gender
            //&& t.gearBoxType == trainee.gearBoxType)).Any(t =>
            //    Tools.Maps.Distance(t.Address, address) < t.MaxDistanceInMeters))

            if (tester == null)
            {
                time.AddMinutes(-time.Minute);
                // חיפוש זמן פנוי - צריך לבדוק גם סוג רכב.     עדכון - כנראה בוצע
                while (!(from item in GetAllTesters(time)
                         where item.Vehicle == trainee.Vehicle
                         && (!trainee.OnlyMyGender || item.Gender == trainee.Gender)
                         && item.gearBoxType == trainee.gearBoxType
                         && BE.Tools.Maps_DrivingDistance(item.Address, address) < item.MaxDistanceInMeters
                         select item).Any() && time.Subtract(DateTime.Now).TotalDays < 30 * 3)
                {
                    time += new TimeSpan(0, 30, 0); //@
                    time = NextWorkTime(time);
                }
                if (time.Subtract(DateTime.Now).TotalDays >= 30 * 3)
                    throw new Exception("הזמן המבוקש תפוס. לא קיים זמן פנוי בשלושת החודשים הקרובים.");
                else
                    throw new Exception("הזמן המבוקש תפוס, אבל יש לנו זמן אחר להציע לך: " + time.ToString("MM/dd/yyyy HH:mm"));
            }

            BE.Test test = new BE.Test(tester.ID, trainee.ID, time, address);
            IDAL.AddTest(test);
        }

        private static DateTime NextWorkTime(DateTime time)
        {
            if (time.DayOfWeek >= DayOfWeek.Friday)
            {
                time.AddDays(DayOfWeek.Saturday - time.DayOfWeek + 1);
                time.AddHours(-time.Hour);
                time.AddMinutes(-time.Minute);
            }
            if (time.Hour < Configuration.WorkStartHour)
            {
                time.AddHours(Configuration.WorkStartHour - time.Hour);
                time.AddMinutes(-time.Minute);
            }
            if (time.Hour > Configuration.WorkEndHour)
            {
                time.AddHours(24 - time.Hour + Configuration.WorkStartHour);
                time.AddMinutes(-time.Minute);
            }
            return time;
        }

        public void AddTester(BE.Tester tester)
        {
            if (DateTime.Now.Year - tester.BirthDate.Year < BE.Configuration.MinimumTesterAge)
                throw new Exception("אין אפשרות להוסיף בוחן מתחת לגיל 40");
            IDAL.AddTester(tester);
        }

        public void AddTrainee(Trainee trainee)
        {
            BE.Trainee ExsistTrainee = IDAL.GetTraineeCopy(trainee.ID);
            if (ExsistTrainee != null)
                throw new Exception("התלמיד כבר קיים במערכת");
            IDAL.AddTrainee(trainee);
        }

        public IEnumerable<Tester> GetAllTesters(Func<Tester, bool> predicate = null)
        {
            return IDAL.GetAllTesters(predicate);
        }

        /// <summary>
        /// Find testers who are available for test on the given date.
        /// </summary>
        /// <param name="TestTime"></param>
        /// <returns></returns>
        public IEnumerable<BE.Tester> GetAllTesters(DateTime TestTime)
        {
            return from tester in IDAL.GetAllTesters()
                   where tester.WorkHours.AsEnumerable().Any(time =>
                   time.Day == TestTime.DayOfWeek
                         && time.Start <= TestTime.TimeOfDay
                         && time.End >= TestTime.TimeOfDay + BE.Configuration.TestTimeSpan) //work on the given time.
                         && !IDAL.GetAllTests(test => test.TesterID == tester.ID).Where(
                            test => (test.Time + BE.Configuration.TestTimeSpan > TestTime
                            && test.Time < TestTime + BE.Configuration.TestTimeSpan)).Any() // available for test on the given date.
                   select tester;


            //לא צריך אבל צריך לבדוק אם לשלב פונקציה אנונימית
            /*
            //לכל טסטר צריך:
            return IDAL.GetAllTesters().Where(delegate (Tester tr)
            {
                //לבדוק שעובד בשעה הזאת
                var a = tr.WorkHours.GetEnumerator();
                while (a.MoveNext() && (a.Current.Day < TestTime.DayOfWeek ||
                    (a.Current.Day == TestTime.DayOfWeek && a.Current.End < (TestTime.TimeOfDay))))
                    ;   //Get the Respectively time period of the tester
                if (a.Current.Day != TestTime.DayOfWeek || a.Current.Start > TestTime.TimeOfDay || a.Current.End < TestTime.TimeOfDay + BE.Configuration.TestTimeSpan)
                    return false;
                //לבדוק שהוא לא תפוס עם טסט אחר
                if (IDAL.GetAllTests(test => test.TesterID == tr.ID).Where(test => (test.Time + BE.Configuration.TestTimeSpan > TestTime && test.Time < TestTime + BE.Configuration.TestTimeSpan)).Any())
                    return false;
                return true;
            });
            */
        }



        public IEnumerable<Test> GetAllTests(Func<Test, bool> predicate = null)
        {
            return IDAL.GetAllTests(predicate);
        }

        public IEnumerable<Trainee> GetAllTrainees(Func<Trainee, bool> predicate = null)
        {
            return IDAL.GetAllTrainees(predicate);
        }

        public void RemoveTester(string ID)
        {
            BE.Tester tester = IDAL.GetTesterCopy(ID);
            if (tester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + ID);
            IDAL.RemoveTester(ID);
        }

        public void RemoveTrainee(string ID)
        {
            BE.Trainee trainee = IDAL.GetTraineeCopy(ID);
            if (trainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + ID);
            IDAL.RemoveTrainee(ID);
        }

        public void UpdateTestResult(int TestID, BE.Indices indices)
        {
            Test test = IDAL.GetTestCopy(TestID);
            if (test == null)
                throw new Exception("לא נמצא מבחן שמספרו " + TestID);
            //if (test.Time > DateTime.Now) // @@ להוציא מהערה
            //    throw new Exception("לא ניתן לעדכן תוצאות לטסט שעדיין לא התבצע.");

            // @ לבדוק את האינדיקטורים והאם עבר, 
            //if (indices.)
            //int sum = 0;
            foreach (var item in indices)
            {
                Console.WriteLine(item);
            }
            IDAL.UpdateTest(test);
        }

        public void UpdateTester(Tester tester)
        {
            BE.Tester ExistTester = IDAL.GetTesterCopy(tester.ID);
            if (ExistTester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + tester.ID);
            if (ExistTester.BirthDate != tester.BirthDate || ExistTester.Gender != tester.Gender
                || ExistTester.Experience != tester.Experience)
                throw new KeyNotFoundException("לא ניתן לשנות מידע בסיסי של בוחן");
            IDAL.UpdateTester(tester);
        }

        public void UpdateTrainee(Trainee trainee)
        {
            BE.Trainee ExistTrainee = IDAL.GetTraineeCopy(trainee.ID);
            if (ExistTrainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + trainee.ID);
            if (ExistTrainee.Gender != trainee.Gender || ExistTrainee.BirthDate != trainee.BirthDate
                || ExistTrainee.Vehicle != trainee.Vehicle || ExistTrainee.gearBoxType != trainee.gearBoxType
                || ExistTrainee.DrivingSchoolName != trainee.DrivingSchoolName || ExistTrainee.TeacherName != trainee.TeacherName)
                throw new KeyNotFoundException("לא ניתן לשנות מידע בסיסי של בוחן");
            IDAL.UpdateTrainee(trainee);
        }

        public IEnumerable<Tester> GetAllTesters(string address)
        {
            return IDAL.GetAllTesters(t => BE.Tools.Maps_DrivingDistance(t.Address, address) < t.MaxDistanceInMeters);
        }

        public IEnumerable<Test> GetAllTests(DateTime dateTime)
        {
            return IDAL.GetAllTests(t => t.Time.Year == dateTime.Year && t.Time.Month == dateTime.Month && t.Time.Day == dateTime.Day);
        }

        public IGrouping<Vehicle, Tester> GetTestersGroupByVehicle(bool sorted = false)
        {
            return (IGrouping<Vehicle, Tester>)from tester in IDAL.GetAllTesters()
                                               group tester by tester.Vehicle;  // @ 
        }

        public IGrouping<string, Trainee> GetTraineesGroupBySchool(bool sorted = false)
        {
            return (IGrouping<string, Trainee>)from trainee in IDAL.GetAllTrainees()
                                               group trainee by trainee.DrivingSchoolName; //@
        }

        public IGrouping<string, Trainee> GetTraineesGroupByTeacher(bool sorted = false)
        {
            return (IGrouping<string, Trainee>)from trainee in IDAL.GetAllTrainees()
                                               group trainee by trainee.TeacherName; //@
        }

        public IGrouping<int, Trainee> GetTraineesGroupByNumOfTests(bool sorted = false)
        {
            return (IGrouping<int, Trainee>)from trainee in IDAL.GetAllTrainees()
                                            group trainee by NumOfTests(trainee.ID); // @
        }

        public int NumOfTests(string TrayneeId)
        {
            return IDAL.GetAllTrainees(t => t.ID == TrayneeId).Count();
        }

        public bool PassedTest(string TrayneeId)
        {
            return IDAL.GetAllTests(test => test.Passed && test.TraineeID == TrayneeId).Any();
        }

        public void SendTestsRemindersLoop()
        {
            new Thread(() =>
            {
            while (true)
            {
                while (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Friday
                       || DateTime.Now.Hour < 8 || DateTime.Now.Hour > 21)    // Send only during working hours.
                    Thread.Sleep(100 * 60 * 60);
                foreach (var item in GetAllTests(t => t.RemeinderEmailSent == null && (t.Time > DateTime.Now) && ((t.Time - DateTime.Now).Days <= 3)))
                {
                    BE.Trainee trainee = IDAL.GetTraineeCopy(item.TraineeID);
                        try
                        {
                            BE.Tools.SendingEmail(trainee.MailAddress, "מועד הטסט שלך מתקרב", "asdfghjkl");
                            item.RemeinderEmailSent = DateTime.Now;
                        }
                        catch (Exception)
                        {
                            // @ else -                        
                        }
                    }
                }
            }).Start();
        }

        int NumOfTestsInWeek(Tester tester, DateTime testTime)
        {
            DateTime weekStart = testTime.Subtract(new TimeSpan((int)testTime.DayOfWeek, (int)testTime.Hour, (int)testTime.Minute, 0));
            DateTime weekEnd = testTime.AddDays(DayOfWeek.Saturday - testTime.DayOfWeek);
            return IDAL.GetAllTests(t => t.TesterID == tester.ID).Count(delegate (Test t)
             {
                 return t.Time > weekStart && t.Time < weekEnd;
            });
        }

        public void RemoveTest(string ID)
        {
            //BE.Test Existtest = IDAL.GetTestCopy(test.TestID);
            //if (Existtest == null)
            //    throw new KeyNotFoundException("לא נמצא מבחן שמספרו " + test.TestID);
            //if (Existtest.TraineeID != test.TraineeID || Existtest.TesterID != test.TesterID
            //    || Existtest.Time != test.Time || !Existtest.Address.Equals(test.Address))
            //    throw new KeyNotFoundException("לא ניתן לשנות מידע בסיסי של טסט");

            throw new NotImplementedException();
        }
    }
}
