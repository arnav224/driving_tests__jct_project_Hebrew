using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Linq;
using BE;
using System.Xml.Serialization;

namespace DAL
{
    public class DAL_XML_imp : IDAL
    {
        XElement configRoot;
        private readonly string configPath = @"config.xml";
        XElement traineesRoot;
        private readonly string traineesPath = @"trainees.xml";
        private readonly string testersPath = @"testers.xml";
        private readonly string testsPath = @"tests.xml";
        public static List<BE.Trainee> trainees;
        public static List<BE.Tester> testers;
        public static List<BE.Test> tests;
        bool traineesListChainged = true;

        internal DAL_XML_imp()
        {
            //DS.DataSource.Initializer(); //todo זמני - צריך למחוק אחרי בניית DAL

            if (!File.Exists(traineesPath))
            {
                traineesRoot = new XElement("trainees");
                traineesRoot.Save(traineesPath);

                //todo זמני
                foreach (var item in DS.DataSource.trainees)
                {
                    AddTrainee(item);
                }
            }
            if (!File.Exists(testersPath))
                SaveToXML<List<Tester>>(new List<Tester>(), testersPath);
            if (!File.Exists(testsPath))
                SaveToXML<List<Test>>(new List<Test>(), testsPath);
            traineesRoot = XElement.Load(traineesPath);

            testers = LoadFromXML<List<Tester>>(testersPath);
            tests = LoadFromXML<List<Test>>(testsPath);
            if (!File.Exists(configPath))
            {
                SaveConfigToXml();
            }
            else
            {
                configRoot = XElement.Load(configPath);
                BE.Configuration.LastTestId = Convert.ToInt32(configRoot.Element("LastTestId").Value);
                BE.Configuration.MinimumTaineeAge = Convert.ToInt32(configRoot.Element("MinimumTaineeAge").Value);
                BE.Configuration.MinimumTesterAge = Convert.ToInt32(configRoot.Element("MinimumTesterAge").Value);
                BE.Configuration.MaximumTestsInWeek = Convert.ToInt32(configRoot.Element("MaximumTestsInWeek").Value);
                BE.Configuration.MaximumTesterAge = Convert.ToInt32(configRoot.Element("MaximumTesterAge").Value);
                BE.Configuration.MinimumDaysBetweenTests = Convert.ToInt32(configRoot.Element("MinimumDaysBetweenTests").Value);
                BE.Configuration.MinimumDrivingLessons = Convert.ToInt32(configRoot.Element("MinimumDrivingLessons").Value);
                BE.Configuration.PassingGrade = Convert.ToInt32(configRoot.Element("PassingGrade").Value);
                BE.Configuration.TestTimeSpanInMinutes = Convert.ToInt32(configRoot.Element("TestTimeSpanInMinutes").Value);
                BE.Configuration.WorkStartHour = Convert.ToInt32(configRoot.Element("WorkStartHour").Value);
                BE.Configuration.FridayWorkEndHour = Convert.ToInt32(configRoot.Element("FridayWorkEndHour").Value);
                BE.Configuration.SenderEmailAddress = configRoot.Element("SenderEmailAddress").Value;
                BE.Configuration.EmailServerPasword = configRoot.Element("EmailServerPasword").Value;
                BE.Configuration.GoogleMapsApiKey = configRoot.Element("GoogleMapsApiKey").Value;
                BE.Configuration.SMTP_Server = configRoot.Element("SMTP_Server").Value;
                BE.Configuration.SMTP_Port = Convert.ToInt32(configRoot.Element("SMTP_Port").Value);
                BE.Configuration.AutoSendingEmails = Convert.ToBoolean(configRoot.Element("AutoSendingEmails").Value);
                BE.Configuration.SendingEmails_DaysInAdvance = Convert.ToInt32(configRoot.Element("SendingEmails_DaysInAdvance").Value);
                BE.Configuration.AutoSendingEmailsAboutAddingAndCancalation = Convert.ToBoolean(configRoot.Element("AutoSendingEmailsAboutAddingAndCancalation").Value);
            }


            //todo טעינת תלמידים מDS זמני
            //foreach (var item in DS.DataSource.trainees)
            //{
            //    AddTrainee(item);
            //}
            //foreach (var item in DS.DataSource.testers)
            //{
            //    AddTester(item);
            //}
            //foreach (var item in DS.DataSource.tests)
            //{
            //    AddTest(item);
            //}

        }

        private void SaveConfigToXml()
        {
            configRoot = new XElement("config");
            try
            {
                configRoot.Add(new XElement("LastTestId", BE.Configuration.LastTestId),
                               new XElement("MinimumTaineeAge", BE.Configuration.MinimumTaineeAge),
                               new XElement("MinimumTesterAge", BE.Configuration.MinimumTesterAge),
                               new XElement("MaximumTestsInWeek", BE.Configuration.MaximumTestsInWeek),
                               new XElement("MaximumTesterAge", BE.Configuration.MaximumTesterAge),
                               new XElement("MinimumDaysBetweenTests", BE.Configuration.MinimumDaysBetweenTests),
                               new XElement("MinimumDrivingLessons", BE.Configuration.MinimumDrivingLessons),
                               new XElement("PassingGrade", BE.Configuration.PassingGrade),
                               new XElement("TestTimeSpanInMinutes", BE.Configuration.TestTimeSpanInMinutes),
                               new XElement("WorkStartHour", BE.Configuration.WorkStartHour),
                               new XElement("WorkEndHour", BE.Configuration.WorkEndHour),
                               new XElement("FridayWorkEndHour", BE.Configuration.FridayWorkEndHour),
                               new XElement("SenderEmailAddress", BE.Configuration.SenderEmailAddress),
                               new XElement("EmailServerPasword", BE.Configuration.EmailServerPasword),
                               new XElement("GoogleMapsApiKey", BE.Configuration.GoogleMapsApiKey),
                               new XElement("SMTP_Server", BE.Configuration.SMTP_Server),
                               new XElement("SMTP_Port", BE.Configuration.SMTP_Port),
                               new XElement("AutoSendingEmails", BE.Configuration.AutoSendingEmails),
                               new XElement("SendingEmails_DaysInAdvance", BE.Configuration.SendingEmails_DaysInAdvance),
                               new XElement("AutoSendingEmailsAboutAddingAndCancalation", BE.Configuration.AutoSendingEmailsAboutAddingAndCancalation));
                configRoot.Save(configPath);
            }
            catch (Exception)
            { }
        }

        ~DAL_XML_imp()
        {
            traineesRoot.Save(traineesPath);
            SaveToXML<List<Tester>>(testers, testersPath);
            SaveToXML<List<Test>>(tests, testsPath);
            SaveConfigToXml();
        }

        private BE.Tester GetTester(string ID)
        {

            return testers.FirstOrDefault(t => t.ID == ID);
        }

        public BE.Tester GetTesterCopy(string ID)
        {
            return GetTester(ID).Clone();
        }

        private BE.Test GetTest(int ID)
        {
            return tests.FirstOrDefault(t => t.TestID == ID);
        }

        public BE.Test GetTestCopy(int ID)
        {
            return GetTest(ID).Clone();
        }

        private BE.Trainee GetTrainee(string ID)
        {
            return (from trainee in traineesRoot.Elements().Where(t => t.Element("ID").Value == ID)
                    select
                 new Trainee(
                     trainee.Element("ID").Value,
                     trainee.Element("FirstName").Value,
                     trainee.Element("LastName").Value,
                     DateTime.Parse(trainee.Element("BirthDate").Value),
                     (Gender)Enum.Parse(typeof(Gender), trainee.Element("Gender").Value),
                     trainee.Element("PhoneNumber").Value,
                     trainee.Element("MailAddress").Value,
                     trainee.Element("Address").Value,
                     (Vehicle)Enum.Parse(typeof(Vehicle), trainee.Element("Vehicle").Value),
                     (GearBoxType)Enum.Parse(typeof(GearBoxType), trainee.Element("GearBoxType").Value),
                     trainee.Element("DrivingSchoolName").Value,
                     trainee.Element("TeacherName").Value,
                     Convert.ToInt32(trainee.Element("NumOfDrivingLessons").Value),
                     Convert.ToBoolean(trainee.Element("OnlyMyGender").Value)
                     )).FirstOrDefault();
        }

        public BE.Trainee GetTraineeCopy(string ID)
        {
            return GetTrainee(ID).Clone();
        }

        public void AddTest(BE.Test test)
        {
            tests.Add(test.Clone());
            SaveToXML<List<Test>>(tests, testsPath);
        }

        public void AddTester(BE.Tester tester)
        {
            BE.Tester ExsistTester = GetTester(tester.ID);
            if (ExsistTester != null)
                throw new Exception("הבוחן כבר קיים במערכת");
            testers.Add(tester.Clone());
            SaveToXML<List<Tester>>(testers, testersPath);
        }

        public void AddTrainee(BE.Trainee trainee)
        {
            if (GetTrainee(trainee.ID) != null)
                throw new Exception("התלמיד כבר קיים במערכת");
            try
            {
                XElement t = new XElement("Trainee");
                t.Add(new XElement("ID", trainee.ID),
                      new XElement("FirstName", trainee.FirstName),
                      new XElement("LastName", trainee.LastName),
                      new XElement("BirthDate", trainee.BirthDate.ToString()),
                      new XElement("Gender", trainee.Gender.ToString()),
                      new XElement("PhoneNumber", trainee.PhoneNumber),
                      new XElement("MailAddress", trainee.MailAddress.ToString()),
                      new XElement("Address", trainee.Address),
                      new XElement("Vehicle", trainee.Vehicle.ToString()),
                      new XElement("GearBoxType", trainee.GearBoxType.ToString()),
                      new XElement("DrivingSchoolName", trainee.DrivingSchoolName),
                      new XElement("TeacherName", trainee.TeacherName),
                      new XElement("NumOfDrivingLessons", trainee.NumOfDrivingLessons.ToString()),
                      new XElement("OnlyMyGender", trainee.OnlyMyGender.ToString()));
                traineesRoot.Add(t);
                traineesRoot.Save(traineesPath);
                traineesListChainged = true;
            }
            catch (Exception)
            { }
        }

        public IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null)
        {
            if (predicate == null)
                return testers.AsEnumerable().Select(t => t.Clone());
            return testers.Where(predicate).Select(t => t.Clone());
        }

        public IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null)
        {
            if (predicate == null)
                return tests.AsEnumerable().Select(t => t.Clone());
            return tests.Where(predicate).Select(t => t.Clone());
        }

        public IEnumerable<BE.Trainee> GetAllTrainees(Func<BE.Trainee, bool> predicate = null)
        {
            try
            {
                if (traineesListChainged)
                {
                    trainees = (from trainee in traineesRoot.Elements()
                                select
                             new Trainee(trainee.Element("ID").Value,
                                         trainee.Element("FirstName").Value,
                                         trainee.Element("LastName").Value,
                                         DateTime.Parse(trainee.Element("BirthDate").Value),
                                         (Gender)Enum.Parse(typeof(Gender), trainee.Element("Gender").Value),
                                         trainee.Element("PhoneNumber").Value,
                                         trainee.Element("MailAddress").Value,
                                         trainee.Element("Address").Value,
                                         (Vehicle)Enum.Parse(typeof(Vehicle), trainee.Element("Vehicle").Value),
                                         (GearBoxType)Enum.Parse(typeof(GearBoxType), trainee.Element("GearBoxType").Value),
                                         trainee.Element("DrivingSchoolName").Value,
                                         trainee.Element("TeacherName").Value,
                                         Convert.ToInt32(trainee.Element("NumOfDrivingLessons").Value),
                                         Convert.ToBoolean(trainee.Element("OnlyMyGender").Value)
                                 )).ToList();
                    traineesListChainged = false;
                }
            }
            catch (Exception)
            {
                throw new Exception("בעיה בקובץ התלמידים");
            }
            if (predicate == null)
                return trainees.AsEnumerable().Select(t => t.Clone());
            return trainees.Where(predicate).Select(t => t.Clone());
        }

        public void RemoveTester(string ID)
        {
            BE.Tester tester = GetTester(ID);
            if (tester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + ID);
            testers.Remove(tester);
            SaveToXML<List<Tester>>(testers, testersPath);
        }

        public void RemoveTrainee(string ID)
        {
            XElement TraineeElement = (from t in traineesRoot.Elements()
                                       where t.Element("ID").Value == ID
                                       select t).FirstOrDefault();
            if (TraineeElement == null)
                throw new KeyNotFoundException("לא נמצא תלמיד שמספרו " + ID);
            try
            {
                TraineeElement.Remove();
                traineesListChainged = true;
                traineesRoot.Save(traineesPath);
            }
            catch (Exception)
            {
            }
        }

        public void UpdateTestResult(BE.Test test)
        {
            int indexTest = tests.FindIndex(t => t.TestID == test.TestID);
            if (indexTest == -1)
                throw new KeyNotFoundException("לא נמצא מבחן שמספרו " + test.TestID);
            tests[indexTest] = test.Clone();
            SaveToXML<List<Test>>(tests, testsPath);
        }

        public void UpdateTester(BE.Tester tester)
        {
            int indexTester = testers.FindIndex(t => t.ID == tester.ID);
            if (indexTester == -1)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + tester.ID);
            testers[indexTester] = tester.Clone();
            SaveToXML<List<Tester>>(testers, testersPath);
        }

        public void UpdateTrainee(BE.Trainee trainee)
        {
            try
            {
                XElement t = (from item in traineesRoot.Elements()
                              where item.Element("ID").Value == trainee.ID
                              select item).FirstOrDefault();
                t.Element("FirstName").Value = trainee.FirstName;
                t.Element("LastName").Value = trainee.LastName;
                t.Element("BirthDate").Value = trainee.BirthDate.ToString();
                t.Element("Gender").Value = trainee.Gender.ToString();
                t.Element("PhoneNumber").Value = trainee.PhoneNumber;
                t.Element("MailAddress").Value = trainee.MailAddress.ToString();
                t.Element("Address").Value = trainee.Address;
                t.Element("Vehicle").Value = trainee.Vehicle.ToString();
                t.Element("GearBoxType").Value = trainee.GearBoxType.ToString();
                t.Element("DrivingSchoolName").Value = trainee.DrivingSchoolName;
                t.Element("TeacherName").Value = trainee.TeacherName;
                t.Element("NumOfDrivingLessons").Value = trainee.NumOfDrivingLessons.ToString();
                t.Element("OnlyMyGender").Value = trainee.OnlyMyGender.ToString();
                traineesRoot.Save(traineesPath);
                traineesListChainged = true;
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("שגיאה בעדכון התלמיד " + trainee.ID);
            }
        }

        public void RemoveTest(int ID)
        {
            BE.Test test = GetTest(ID);
            if (test == null)
                throw new KeyNotFoundException("לא נמצא טסט שמספרו " + ID);
            tests.Remove(test);
            SaveToXML<List<Test>>(tests, testsPath);
        }

        public string GetEmailTemltateTestRemeinder(int TestID, string NoteToAdd = "")
        {
            BE.Test test = GetTest(TestID);
            BE.Trainee trainee = GetTrainee(test.TraineeID);

            string messege = '!' + trainee.FirstName + (trainee.Gender == BE.Gender.זכר ? " היקר " : " היקרה ") + @"<p> רק רצינו להזכיר לך שמועד הטסט שלך מתקרב</p>"
+ @".הטסט שלך יתקיים בתאריך <b>" + test.Time.ToString("dd/MM/yyyy") + "</b> בשעה <b>" + test.Time.ToString("mm:HH") + "</b><br>" +
".המיקום שנקבע לטסט הוא: <b>" + test.Address + "</b>"
+ "<p></p>!בהצלחה";
            string URL = "https://www.google.com/maps/search/" + test.Address/*.Replace(' ', '+').Replace(',', '%')*/;

            return RemeinderEmailHTML
                .Replace("@@Text@@", messege + "<p>" + NoteToAdd.Replace("\n", "<br>") + "</p>")
                .Replace("@@LINK@@", URL);
        }

        public void UpdateEmailSendingTime(int testID, DateTime? SummaryEmailSent = null, DateTime? RemeinderEmailSent = null)
        {
            Test test = GetTest(testID);
            if (SummaryEmailSent != null)
                test.SummaryEmailSent = SummaryEmailSent;
            if (RemeinderEmailSent != null)
                test.RemeinderEmailSent = RemeinderEmailSent;
        }

        private static string remeinderEmailHTML;
        public static string RemeinderEmailHTML
        {
            get
            {
                if (remeinderEmailHTML == null)
                    remeinderEmailHTML = File.ReadAllText("emails/TestRemeinder.html");
                return remeinderEmailHTML;
            }
        }

        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source); file.Close();
        }

        public static T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }
    }

}

