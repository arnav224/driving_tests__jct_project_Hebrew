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
        
        internal BL_imp()
        {
            IDAL = DAL.Factory.GetInstance();
        }

        /// <summary>
        /// add Test to the DataBase
        /// </summary>
        /// <param name="test"></param>
        public void AddTest(BE.Test test)
        {
            if (test.Address == null || test.Address == "" || test.TraineeID == null || test.TraineeID == "")
                throw new Exception("חובה למלא את כל השדות");
            BE.Trainee trainee = IDAL.GetTraineeCopy(test.TraineeID);
            if (trainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + test.TraineeID);
            if (trainee.NumOfDrivingLessons < BE.Configuration.MinimumDrivingLessons)
                throw new Exception("אין אפשרות להוסיף מבחן לתלמיד שעשה פחות מ-." + BE.Configuration.MinimumDrivingLessons + " שיעורים.");

            BE.Test LastPreviusTest = null, FirstNextTest = null;
            foreach (var item in IDAL.GetAllTests(t => t.TraineeID == test.TraineeID))
            {
                if (item.Time < test.Time && (LastPreviusTest == null || LastPreviusTest.Time < item.Time))
                    LastPreviusTest = item;
                else if (item.Time >= test.Time && (FirstNextTest == null || LastPreviusTest.Time < item.Time))
                    FirstNextTest = item;
            }
            if (LastPreviusTest != null && (test.Time - LastPreviusTest.Time).Days < BE.Configuration.MinimumDaysBetweenTests
                || FirstNextTest != null && (FirstNextTest.Time - test.Time).Days < BE.Configuration.MinimumDaysBetweenTests)
                throw new Exception("לתלמיד זה קיים מבחן בהפרש של פחות משבעה ימים.");
            if (test.Time < DateTime.Now)
                throw new Exception("מועד הטסט חלף");
            if (test.Time != NextWorkTime(test.Time))
                throw new Exception("מועד הטסט מחוץ לשעות העבודה. \nשעות העבודה בימי השבוע הם: " + BE.Configuration.WorkStartHour + " עד " + BE.Configuration.WorkEndHour);

            BE.Tester tester = (from item in GetAllTesters(test.Time)
                                where item.Vehicle == trainee.Vehicle
                                && BE.Tools.Maps_DrivingDistance(item.Address, test.Address) < item.MaxDistanceInMeters
                                && (!trainee.OnlyMyGender || item.Gender == trainee.Gender)
                                && item.gearBoxType == trainee.gearBoxType
                                && NumOfTestsInWeek(item, test.Time) < item.MaxTestsInWeek // @
                                select item).FirstOrDefault();
            DateTime time = test.Time;
            if (tester == null)
            {
                time.AddMinutes(-time.Minute);
                while (!(from item in GetAllTesters(time)
                         where item.Vehicle == trainee.Vehicle
                         && (!trainee.OnlyMyGender || item.Gender == trainee.Gender)
                         && item.gearBoxType == trainee.gearBoxType
                         && BE.Tools.Maps_DrivingDistance(item.Address, test.Address) < item.MaxDistanceInMeters
                         select item).Any() && time.Subtract(DateTime.Now).TotalDays < 30 * 3)
                {
                    time += new TimeSpan(0, 15, 0);
                    time = NextWorkTime(time);
                }
                if (time.Subtract(DateTime.Now).TotalDays >= 30 * 3)
                    throw new Exception("הזמן המבוקש תפוס. לא קיים זמן פנוי בשלושת החודשים הקרובים.");
                else
                    throw new Exception("הזמן המבוקש תפוס, אבל יש לנו זמן אחר להציע לך: " + time.Day + '/' + time.Month + '/'
                        + time.Year + ' ' + time.Hour + ':' + time.Minute);// time.ToString("MM/dd/yyyy HH:mm"));
            }
            test.TesterID = tester.ID;
            IDAL.AddTest(test);
        }

        /// <summary>
        /// Next time of our testing institute
        /// </summary>
        /// <param name="time">Time to start checking from it</param>
        /// <returns></returns>
        private static DateTime NextWorkTime(DateTime time)
        {
            if (time.DayOfWeek >= DayOfWeek.Friday)
            {
                time = time.AddDays(DayOfWeek.Saturday - time.DayOfWeek + 1);
                time = time.AddHours(-time.Hour);
                time = time.AddMinutes(-time.Minute);
            }
            if (time.Hour < Configuration.WorkStartHour)
            {
                time = time.AddHours(Configuration.WorkStartHour - time.Hour);
                time = time.AddMinutes(-time.Minute);
            }
            if (time.Hour > Configuration.WorkEndHour)
            {
                time = time.AddHours(24 - time.Hour + Configuration.WorkStartHour);
                time = time.AddMinutes(-time.Minute);
            }
            return time;
        }

        /// <summary>
        /// add Tester to the DataBase
        /// </summary>
        /// <param name="tester"></param>
        public void AddTester(BE.Tester tester)
        {
            if (tester.Address == null || tester.Address == "" || tester.BirthDate == default(DateTime)
                || tester.FirstName == null || tester.FirstName == ""
                || tester.ID == null || tester.ID == "" || tester.LastName == null || tester.LastName == ""
                || tester.MailAddress == null || tester.MailAddress == ""
                || tester.PhoneNumber == null || tester.PhoneNumber == "" || tester.WorkHours == null)
                throw new Exception("חובה למלא את כל הפרטים");
            if (DateTime.Now.Year - tester.BirthDate.Year < BE.Configuration.MinimumTesterAge)
                throw new Exception("אין אפשרות להוסיף בוחן מתחת לגיל 40");
            IDAL.AddTester(tester);
        }

        /// <summary>
        /// add Trainee to the DataBase
        /// </summary>
        /// <param name="trainee"></param>
        public void AddTrainee(Trainee trainee)
        {
            if (trainee.Address == null || trainee.Address == "" || trainee.BirthDate == default(DateTime)
                || trainee.FirstName == null || trainee.FirstName == ""
                || trainee.LastName == null || trainee.LastName == "" || trainee.PhoneNumber == null || trainee.PhoneNumber == ""
                || trainee.TeacherName == null || trainee.TeacherName == ""
                || trainee.DrivingSchoolName == null || trainee.DrivingSchoolName == "")
                throw new Exception("חובה למלא את כל הפרטים");
            BE.Trainee ExsistTrainee = IDAL.GetTraineeCopy(trainee.ID);
            if (ExsistTrainee != null)
                throw new Exception("התלמיד כבר קיים במערכת");
            IDAL.AddTrainee(trainee);
        }

        /// <summary>
        /// Find testers who are available for test on the given date.
        /// </summary>
        /// <param name="TestTime">Date requested for test</param>
        /// <returns></returns>
        public IEnumerable<BE.Tester> GetAllTesters(DateTime TestTime)
        {
            return from tester in IDAL.GetAllTesters()
                   where tester.WorkHours.AsEnumerable().Any(time =>
                   time.Start.Days == (int)TestTime.DayOfWeek
                         && time.Start.Subtract(new TimeSpan(time.Start.Days, 0, 0, 0)) <= TestTime.TimeOfDay
                         && time.End.Subtract(new TimeSpan(time.Start.Days, 0, 0, 0)) >= TestTime.TimeOfDay + BE.Configuration.TestTimeSpan) //work on the given time.
                         && !IDAL.GetAllTests(test => test.TesterID == tester.ID).Where(
                            test => (test.Time + BE.Configuration.TestTimeSpan > TestTime
                            && test.Time < TestTime + BE.Configuration.TestTimeSpan)).Any() // available for test on the given date.
                   select tester;
        }

        /// <summary>
        /// Get All Testers
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all testers</param>
        /// <returns></returns>
        public IEnumerable<Tester> GetAllTesters(Func<Tester, bool> predicate = null)
        {
            return IDAL.GetAllTesters(predicate);
        }


        /// <summary>
        /// Get All Tests
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all tests</param>
        /// <returns></returns>
        public IEnumerable<Test> GetAllTests(Func<Test, bool> predicate = null)
        {
            return IDAL.GetAllTests(predicate);
        }

        public IEnumerable<Test> GetAllTests(string searchString, DateTime? FromTime, DateTime? ToTime, bool? passed)
        {
            int parsResult;
            int.TryParse(searchString, out parsResult);
            return IDAL.GetAllTests(t =>
                                    (FromTime == null || t.Time >= FromTime)
                                    && (ToTime == null || t.Time <= ToTime)
                                    && (searchString == null || t.TestID == parsResult
                                    || t.TesterID.Contains(searchString) || t.TraineeID.Contains(searchString)
                                    || t.Address.Contains(searchString))
                                    && (passed == null || passed == t.Passed));

        }
        public IEnumerable<Test> GetAllTests(string searchString, DateTime? FromTestTime, DateTime? ToTestTime, bool? passed, DateTime? FromAppealTime = null, DateTime? ToAppealTime = null)
        {
            int parsResult;
            int.TryParse(searchString, out parsResult);
            return IDAL.GetAllTests(t =>
                                    (FromTestTime == null || t.Time >= FromTestTime)
                                    && (ToTestTime == null || t.Time <= ToTestTime)
                                    && (searchString == null || t.TestID == parsResult
                                    || t.TesterID.Contains(searchString) || t.TraineeID.Contains(searchString)
                                    || t.Address.Contains(searchString))
                                    && (passed == null || passed == t.Passed)
                                    && (FromAppealTime == null || t.AppealTest.RequestTime >= FromAppealTime)
                                    && (ToAppealTime == null || t.AppealTest.RequestTime <= ToAppealTime)
                                    );

        }

        public IEnumerable<Test> GetAllAppealTests(string searchString, DateTime? FromAppealTime, DateTime? ToAppealTime, AppealStatus status)
        {
            int parsResult;
            int.TryParse(searchString, out parsResult);
            return IDAL.GetAllTests(t =>
                                    t.AppealTest != null
                                    && (searchString == null || t.TestID == parsResult
                                    || t.TesterID.Contains(searchString) || t.TraineeID.Contains(searchString)
                                    || t.AppealTest.TraineeNotes.Contains(searchString)
                                    || t.AppealTest.Decision.Contains(searchString))
                                    && (FromAppealTime == null || t.AppealTest.RequestTime >= FromAppealTime)
                                    && (ToAppealTime == null || t.AppealTest.RequestTime <= ToAppealTime)
                                    && (status == default(BE.AppealStatus) || t.AppealTest.appealStatus == status));
        }

        /// <summary>
        /// Get All Trainees
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all trainees</param>
        /// <returns></returns>
        public IEnumerable<Trainee> GetAllTrainees(Func<Trainee, bool> predicate = null)
        {
            return IDAL.GetAllTrainees(predicate);
        }

        /// <summary>
        /// Get All Trainees by filters
        /// </summary>
        /// <param name="predicate">Predicate for filtering or null to get all trainees</param>
        /// <returns></returns>
        public IEnumerable<Trainee> GetAllTrainees(string searchString, BE.Gender? gender, BE.GearBoxType? gearBoxType,
                                                        BE.Vehicle? vahicle, DateTime? FromTime, DateTime? ToTime, bool? passed)
        {
            return IDAL.GetAllTrainees(t =>
                (t.FirstName.Contains(searchString) || t.LastName.Contains(searchString) || (t.FirstName + ' ' + t.LastName).Contains(searchString)
                || (t.LastName + ' ' + t.FirstName).Contains(searchString) || t.ID.Contains(searchString) || t.Address.Contains(searchString)
                || t.MailAddress.Contains(searchString))

                && (gender == null || gender == t.Gender)

                && (gearBoxType == null || gearBoxType == t.gearBoxType) 

                && (vahicle == null || vahicle == t.Vehicle)

                && (FromTime == null || t.BirthDate >= FromTime) 

                && (ToTime == null || t.BirthDate <= ToTime)

                && (passed == null || PassedTest(t.ID) == passed));
        }

        /// <summary>
        /// Remove Tester from the DataBase
        /// </summary>
        /// <param name="ID">the ID fo Tester to remove</param>
        public void RemoveTester(string ID)
        {
            BE.Tester tester = IDAL.GetTesterCopy(ID);
            if (tester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + ID);
            IDAL.RemoveTester(ID);
        }

        /// <summary>
        /// Remove Trainee from the DataBase
        /// </summary>
        /// <param name="ID">the ID fo Trainee to remove</param>
        public void RemoveTrainee(string ID)
        {
            BE.Trainee trainee = IDAL.GetTraineeCopy(ID);
            if (trainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + ID);
            IDAL.RemoveTrainee(ID);
            var listToRemove = IDAL.GetAllTests(t => t.TraineeID == ID && t.Time > DateTime.Now).Select(t => t.TestID).ToArray();
            foreach (var item in listToRemove)
            {
                IDAL.RemoveTest(item);
            }
        }


        /// <summary>
        /// Update test results when done.
        /// </summary>
        /// <param name="test"></param>
        public void UpdateTestResult(BE.Test test)
        {
            if (test.Time > DateTime.Now) 
                throw new Exception("לא ניתן לעדכן תוצאות לטסט שעדיין לא התבצע.");
            int sum = 0;
            foreach (var pair in test.Indices)
            {
                sum += (int)pair.Value;
            }
            test.Passed = (100 * sum / (3 * test.Indices.Count) >= BE.Configuration.PassingGrade && !test.Indices.Any(pair => pair.Value == BE.Score.נכשל));
            IDAL.UpdateTestResult(test);
        }

        public void TestAppeal(Test test)
        {
            if (test.Time > DateTime.Now)
                throw new Exception("לא ניתן לעדכן תוצאות לטסט שעדיין לא התבצע.");
            IDAL.UpdateTestResult(test);
        }


        /// <summary>
        /// update address and time fo test
        /// </summary>
        /// <param name="test"></param>
        public void UpdateTest(Test test)
        {
            if (test.Time < DateTime.Now)
                throw new Exception("לא ניתן לעדכן פרטי טסט שכבר התבצע.");
            BE.Test ExistTest = IDAL.GetTestCopy(test.TestID);
                RemoveTest(ExistTest.TestID);
            try
            {
                AddTest(test);
            }
            catch (Exception ex)
            {
                IDAL.AddTest(ExistTest);
                throw ex;
            }
        }

        /// <summary>
        /// Update relevant properties of Tester
        /// </summary>
        /// <param name="tester"></param>
        public void UpdateTester(Tester tester)
        {
            BE.Tester ExistTester = IDAL.GetTesterCopy(tester.ID);
            if (ExistTester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + tester.ID);
            if (ExistTester.FirstName != tester.FirstName || ExistTester.LastName != tester.LastName
                ||ExistTester.BirthDate != tester.BirthDate || ExistTester.Gender != tester.Gender
                || ExistTester.Experience != tester.Experience)
                throw new KeyNotFoundException("לא ניתן לשנות מידע בסיסי של בוחן");
            IDAL.UpdateTester(tester);
        }

        /// <summary>
        /// Update relevant properties of Trainee
        /// </summary>
        /// <param name="trainee"></param>
        public void UpdateTrainee(Trainee trainee)
        {
            BE.Trainee ExistTrainee = IDAL.GetTraineeCopy(trainee.ID);
            if (ExistTrainee == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + trainee.ID);
            if (ExistTrainee.Gender != trainee.Gender || ExistTrainee.BirthDate != trainee.BirthDate
                || ExistTrainee.Vehicle != trainee.Vehicle || ExistTrainee.gearBoxType != trainee.gearBoxType
                || ExistTrainee.DrivingSchoolName != trainee.DrivingSchoolName || ExistTrainee.TeacherName != trainee.TeacherName)
                throw new KeyNotFoundException("לא ניתן לשנות מידע בסיסי של תלמיד");
            IDAL.UpdateTrainee(trainee);
        }

        /// <summary>
        /// Get All Testers Who are willing to travel to the proposed address
        /// </summary>
        /// <param name="address">The location of the test</param>
        /// <returns></returns>
        public IEnumerable<Tester> GetAllTesters(string address)
        {
            return IDAL.GetAllTesters(t => BE.Tools.Maps_DrivingDistance(t.Address, address) < t.MaxDistanceInMeters);
        }

        public IEnumerable<Tester> GetAllTesters(string searchString, Gender? gender, GearBoxType? 
                                                 gearBoxType, Vehicle? vahicle, DateTime? FromTime, DateTime? ToTime)
        {
            return IDAL.GetAllTesters(t =>
                (t.FirstName.Contains(searchString) || t.LastName.Contains(searchString) || (t.FirstName + ' ' + t.LastName).Contains(searchString)
                || (t.LastName + ' ' + t.FirstName).Contains(searchString) || t.ID.Contains(searchString) || t.Address.Contains(searchString)
                || t.MailAddress.Contains(searchString))

                && (gender == null || gender == t.Gender)

                && (gearBoxType == null || gearBoxType == t.gearBoxType)

                && (vahicle == null || vahicle == t.Vehicle)

                && (FromTime == null || t.BirthDate >= FromTime)

                && (ToTime == null || t.BirthDate <= ToTime));
        }

        /// <summary>
        /// Get All Testers Who are available at the time
        /// </summary>
        /// <param name="dateTime">Time of the test</param>
        /// <returns></returns>
        public IEnumerable<Test> GetAllTests(DateTime dateTime)
        {
            return IDAL.GetAllTests(t => t.Time.Year == dateTime.Year && t.Time.Month == dateTime.Month && t.Time.Day == dateTime.Day);
        }

        public IGrouping<Vehicle, Tester> GetTestersGroupByVehicle(bool sorted = false)
        {
            return (IGrouping<Vehicle, Tester>)from tester in IDAL.GetAllTesters()
                                               group tester by tester.Vehicle;
        }

        public IGrouping<string, Trainee> GetTraineesGroupBySchool(bool sorted = false)
        {
            return (IGrouping<string, Trainee>)from trainee in IDAL.GetAllTrainees()
                                               group trainee by trainee.DrivingSchoolName;
        }

        public IGrouping<string, Trainee> GetTraineesGroupByTeacher(bool sorted = false)
        {
            return (IGrouping<string, Trainee>)from trainee in IDAL.GetAllTrainees()
                                               group trainee by trainee.TeacherName; 
        }

        public IGrouping<int, Trainee> GetTraineesGroupByNumOfTests(bool sorted = false)
        {
            return (IGrouping<int, Trainee>)from trainee in IDAL.GetAllTrainees()
                                            group trainee by NumOfTests(trainee.ID); 
        }

        /// <summary>
        /// Number of registered student tests
        /// </summary>
        /// <param name="TrayneeId"></param>
        /// <returns></returns>
        public int NumOfTests(string TrayneeId)
        {
            return IDAL.GetAllTrainees(t => t.ID == TrayneeId).Count();
        }

        /// <summary>
        /// whether the student is successful in any test
        /// </summary>
        /// <param name="TrayneeId"></param>
        /// <returns></returns>
        public bool PassedTest(string TrayneeId)
        {
            return IDAL.GetAllTests(test => test.Passed != null && test.TraineeID == TrayneeId).Any();
        }

        // Note: In development
        public void SendTestsRemindersLoop()
        {
            new Thread(() =>
            {
                while (BE.Configuration.AutoSendingEmails)
                {
                    DateTime now = DateTime.Now;
                    if (now != NextWorkTime(now))
                        Thread.Sleep((NextWorkTime(now) - now).Milliseconds);
                    try
                    {
                        foreach (var item in GetAllTests(t => t.RemeinderEmailSent == null 
                                 && (t.Time > DateTime.Now) && ((t.Time - DateTime.Now).Days <= Configuration.SendingEmails_DaysInAdvance)))
                        {
                            if (!BE.Configuration.AutoSendingEmails)
                                break;
                            BE.Trainee trainee = IDAL.GetTraineeCopy(item.TraineeID);
                            try
                            {
                                if (BE.Tools.SendingEmail(trainee.MailAddress, "מועד הטסט שלך מתקרב", IDAL.GetEmailTemltateTestRemeinder(item.TestID)))
                                    UpdateEmailSendingTime(item.TestID, null, DateTime.Now);
                            }
                            catch (Exception)
                            { }
                        }
                    }
                    catch (Exception)
                    { }
                    Thread.Sleep(100 * 60 * 60);
                }
            }).Start();
        }

        /// <summary>
        /// The number of Test records of Tester in a given week
        /// </summary>
        /// <param name="tester"></param>
        /// <param name="testTime"></param>
        /// <returns></returns>
        int NumOfTestsInWeek(Tester tester, DateTime testTime)
        {
            DateTime weekStart = testTime.Subtract(new TimeSpan((int)testTime.DayOfWeek, (int)testTime.Hour, (int)testTime.Minute, 0));
            DateTime weekEnd = testTime.AddDays(DayOfWeek.Saturday - testTime.DayOfWeek);
            return IDAL.GetAllTests(t => t.TesterID == tester.ID).Count(delegate (Test t)
             {
                 return t.Time > weekStart && t.Time < weekEnd;
             });
        }

        /// <summary>
        /// Remove Test
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveTest(int ID)
        {
            BE.Test Existtest = IDAL.GetTestCopy(ID);
            if (Existtest == null)
                throw new KeyNotFoundException("לא נמצא מבחן שמספרו " + ID);
            IDAL.RemoveTest(ID);
        }

        public SortedSet<DateTime> avalibleDateTimes(Test test)
        {
            BE.Trainee trainee = IDAL.GetTraineeCopy(test.TraineeID);
            var result = new SortedSet<DateTime>();
            DateTime time = DateTime.Now.AddDays(2);
            time = time.AddMinutes(120 - time.Minute);
            while (time <= DateTime.Now.AddDays(BE.Configuration.AllowToAddTest_DaysInAdvance))
            {
                try
                {
                    test.Time = time;

                    BE.Test LastPreviusTest = null, FirstNextTest = null;
                    foreach (var item in IDAL.GetAllTests(t => t.TraineeID == test.TraineeID))
                    {
                        if (item.Time < test.Time && (LastPreviusTest == null || LastPreviusTest.Time < item.Time))
                            LastPreviusTest = item;
                        else if (item.Time >= test.Time && (FirstNextTest == null || LastPreviusTest.Time < item.Time))
                            FirstNextTest = item;
                    }
                    if (LastPreviusTest != null && (test.Time - LastPreviusTest.Time).Days < BE.Configuration.MinimumDaysBetweenTests
                        || FirstNextTest != null && (FirstNextTest.Time - test.Time).Days < BE.Configuration.MinimumDaysBetweenTests)
                        throw new Exception("לתלמיד זה קיים מבחן בהפרש של פחות משבעה ימים.");
                    if (test.Time != NextWorkTime(test.Time))
                        throw new Exception("מועד הטסט מחוץ לשעות העבודה. \nשעות העבודה בימי השבוע הם: " + BE.Configuration.WorkStartHour + " עד " + BE.Configuration.WorkEndHour);

                    BE.Tester tester = (from item in GetAllTesters(test.Time)
                                        where item.Vehicle == trainee.Vehicle
                                        && BE.Tools.Maps_DrivingDistance(item.Address, test.Address) < item.MaxDistanceInMeters
                                        && (!trainee.OnlyMyGender || item.Gender == trainee.Gender)
                                        && item.gearBoxType == trainee.gearBoxType
                                        && NumOfTestsInWeek(item, test.Time) < item.MaxTestsInWeek // @
                                        select item).FirstOrDefault();
                    if (tester == null)
                    {
                            throw new Exception("הזמן המבוקש תפוס");
                    }
                    result.Add(time);
                    time = time.AddDays(1);
                    time = time.AddHours(-time.Hour + BE.Configuration.WorkStartHour);
                }
                catch (Exception)
                {
                     time = time.AddMinutes(15);
                }
                time = NextWorkTime(time);
            }
            return result;
        }

        public SortedSet<DateTime> avalibleTimesInDay(Test test)
        {
            BE.Trainee trainee = IDAL.GetTraineeCopy(test.TraineeID);
            var result = new SortedSet<DateTime>();
            DateTime time = test.Time;
            DateTime timeFlag = test.Time;
            timeFlag = timeFlag.AddHours(-timeFlag.Hour);
            time = time.AddMinutes(120 - time.Minute);
            while (time <= timeFlag + new TimeSpan(1,0,0))
            {
                try
                {
                    test.Time = time;
                    if (test.Time != NextWorkTime(test.Time))
                        throw new Exception("מועד הטסט מחוץ לשעות העבודה. \nשעות העבודה בימי השבוע הם: " + BE.Configuration.WorkStartHour + " עד " + BE.Configuration.WorkEndHour);

                    BE.Tester tester = (from item in GetAllTesters(test.Time)
                                        where item.Vehicle == trainee.Vehicle
                                        && BE.Tools.Maps_DrivingDistance(item.Address, test.Address) < item.MaxDistanceInMeters
                                        && (!trainee.OnlyMyGender || item.Gender == trainee.Gender)
                                        && item.gearBoxType == trainee.gearBoxType
                                        && NumOfTestsInWeek(item, test.Time) < item.MaxTestsInWeek // @
                                        select item).FirstOrDefault();
                    if (tester == null)
                    {
                        throw new Exception("הזמן המבוקש תפוס");
                    }
                    result.Add(time);
                }
                catch (Exception)
                { }
                time = time.AddMinutes(15);
                time = NextWorkTime(time);
            }
            return result;
        }


        public void UpdateEmailSendingTime(int testID, DateTime? SummaryEmailSent = null, DateTime? RemeinderEmailSent = null)
        {
            IDAL.UpdateEmailSendingTime(testID, SummaryEmailSent, RemeinderEmailSent);
        }

        public string GetEmailTemltateTestRemeinder(int TestID, string NoteToAdd = "")
        {
            return IDAL.GetEmailTemltateTestRemeinder(TestID, NoteToAdd);
        }
    }
}
