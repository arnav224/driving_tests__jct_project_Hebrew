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
    /// <summary>
    /// implementatin of IDAL by XML files
    /// </summary>
    public class DAL_XML_imp : IDAL
    {
        static readonly string ProjectPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory.ToString()).FullName).FullName;//path of xml files
        XElement configRoot;
        private readonly string configPath = ProjectPath + "/Data/config.xml";
        XElement traineesRoot;
        private readonly string traineesPath = ProjectPath + "/Data/trainees.xml";
        private readonly string testersPath = ProjectPath + "/Data/testers.xml";
        private readonly string testsPath = ProjectPath + "/Data/tests.xml";
        public static List<BE.Trainee> trainees;
        public static List<BE.Tester> testers = new List<Tester>();
        public static List<BE.Test> tests = new List<Test>();
        bool traineesListChainged = true;
        private static string remeinderEmailHTML;
        public static string RemeinderEmailHTML
        {
            get
            {
                if (remeinderEmailHTML == null)
                    remeinderEmailHTML = File.ReadAllText(ProjectPath + "/Data/emails/TestRemeinder.html");
                return remeinderEmailHTML;
            }
        }
        private static string testAddedEmailHTML;
        public static string TestAddedEmailHTML
        {
            get
            {
                if (testAddedEmailHTML == null)
                    testAddedEmailHTML = File.ReadAllText(ProjectPath + "/Data/emails/AddedTest.html");
                return testAddedEmailHTML;
            }
        }
        private static string testCancelationEmailHTML;
        public static string TestCancelationEmailHTML
        {
            get
            {
                if (testCancelationEmailHTML == null)
                    testCancelationEmailHTML = File.ReadAllText(ProjectPath + "/Data/emails/TestCancelation.html");
                return testCancelationEmailHTML;
            }
        }
        private static string testUpdateResultsEmailHTML;
        public static string TestUpdateResultsEmailHTML
        {
            get
            {
                if (testUpdateResultsEmailHTML == null)
                    testUpdateResultsEmailHTML = File.ReadAllText(ProjectPath + "/Data/emails/UpdateTestResult.html");
                return testUpdateResultsEmailHTML;
            }
        }
        private static string appealEmailHTML;
        public static string AppealEmailHTML
        {
            get
            {
                if (appealEmailHTML == null)
                    appealEmailHTML = File.ReadAllText(ProjectPath + "/Data/emails/Appeal.html");
                return appealEmailHTML;
            }
        }

        /// <summary>
        /// DAL_XML_imp ctor
        /// </summary>
        internal DAL_XML_imp()
        {
            if (!File.Exists(traineesPath))
            {
                traineesRoot = new XElement("trainees");
                traineesRoot.Save(traineesPath);
            }
            if (!File.Exists(testersPath))
            {
                SaveToXML(new List<Tester>(), testersPath);
            }
            if (!File.Exists(testsPath))
            {
                SaveToXML(new List<Test>(), testsPath);
            }
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
        }

        /// <summary>
        /// Save Configuration To Xml
        /// </summary>
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

        /// <summary>
        /// dtor
        /// </summary>
        ~DAL_XML_imp()
        {
            traineesRoot.Save(traineesPath);
            SaveToXML<List<Tester>>(testers, testersPath);
            SaveToXML<List<Test>>(tests, testsPath);
            SaveConfigToXml();
        }

        /// <summary>
        /// Get Tester
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private BE.Tester GetTester(string ID)
        {
            return testers.FirstOrDefault(t => t.ID == ID);
        }

        /// <summary>
        /// Get Tester Copy
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public BE.Tester GetTesterCopy(string ID)
        {
            return GetTester(ID).Clone();
        }

        /// <summary>
        /// Get Test
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private BE.Test GetTest(int ID)
        {
            return tests.FirstOrDefault(t => t.TestID == ID);
        }

        /// <summary>
        /// Get Test Copy
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public BE.Test GetTestCopy(int ID)
        {
            return GetTest(ID).Clone();
        }

        /// <summary>
        /// Get Trainee from xml
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Trainee Copy
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public BE.Trainee GetTraineeCopy(string ID)
        {
            return GetTrainee(ID).Clone();
        }

        /// <summary>
        /// Add Test
        /// </summary>
        /// <param name="test"></param>
        public void AddTest(BE.Test test)
        {
            tests.Add(test.Clone());
            SaveToXML<List<Test>>(tests, testsPath);
        }

        /// <summary>
        /// Add Tester
        /// </summary>
        /// <param name="tester"></param>
        public void AddTester(BE.Tester tester)
        {
            BE.Tester ExsistTester = GetTester(tester.ID);
            if (ExsistTester != null)
                throw new Exception("הבוחן כבר קיים במערכת");
            testers.Add(tester.Clone());
            SaveToXML<List<Tester>>(testers, testersPath);
        }

        /// <summary>
        /// Add Trainee to xml
        /// </summary>
        /// <param name="trainee"></param>
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

        /// <summary>
        /// Get All Testers
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BE.Tester> GetAllTesters(Func<BE.Tester, bool> predicate = null)
        {
            if (predicate == null)
                return testers.AsEnumerable().Select(t => t.Clone());
            return testers.Where(predicate).Select(t => t.Clone());
        }

        /// <summary>
        /// Get All Tests
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BE.Test> GetAllTests(Func<BE.Test, bool> predicate = null)
        {
            if (predicate == null)
                return tests.AsEnumerable().Select(t => t.Clone());
            return tests.Where(predicate).Select(t => t.Clone());
        }

        /// <summary>
        /// Get All Trainees from xml.
        /// </summary>
        /// <param name="predicate"> Optional - Using predicate.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove Tester
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveTester(string ID)
        {
            BE.Tester tester = GetTester(ID);
            if (tester == null)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + ID);
            testers.Remove(tester);
            SaveToXML<List<Tester>>(testers, testersPath);
        }

        /// <summary>
        /// Remove Trainee from xml
        /// </summary>
        /// <param name="ID"></param>
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

        /// <summary>
        /// Update Test Result
        /// </summary>
        /// <param name="test"></param>
        public void UpdateTestResult(BE.Test test)
        {
            int indexTest = tests.FindIndex(t => t.TestID == test.TestID);
            if (indexTest == -1)
                throw new KeyNotFoundException("לא נמצא מבחן שמספרו " + test.TestID);
            tests[indexTest] = test.Clone();
            SaveToXML<List<Test>>(tests, testsPath);
        }

        /// <summary>
        /// Update Tester
        /// </summary>
        /// <param name="tester"></param>
        public void UpdateTester(BE.Tester tester)
        {
            int indexTester = testers.FindIndex(t => t.ID == tester.ID);
            if (indexTester == -1)
                throw new KeyNotFoundException("לא נמצא בוחן שמספרו " + tester.ID);
            testers[indexTester] = tester.Clone();
            SaveToXML<List<Tester>>(testers, testersPath);
        }

        /// <summary>
        /// Update Trainee to xml
        /// </summary>
        /// <param name="trainee"></param>
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

        /// <summary>
        /// Remove Test
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveTest(int ID)
        {
            BE.Test test = GetTest(ID);
            if (test == null)
                throw new KeyNotFoundException("לא נמצא טסט שמספרו " + ID);
            tests.Remove(test);
            SaveToXML<List<Test>>(tests, testsPath);
        }

        /// <summary>
        /// Get Email Temltate Test Remeinder
        /// </summary>
        /// <param name="TestID"></param>
        /// <param name="NoteToAdd"></param>
        /// <returns></returns>
        public string GetEmailTemltateTestRemeinder(int TestID, string NoteToAdd = "")
        {
            BE.Test test = GetTest(TestID);
            BE.Trainee trainee = GetTrainee(test.TraineeID);
            return RemeinderEmailHTML
            .Replace("@@Name@@", trainee.FirstName + (trainee.Gender == BE.Gender.זכר ? " היקר " : " היקרה "))
            .Replace("@@DATE@@", test.Time.ToString("dd/MM/yyyy"))
            .Replace("@@TIME@@", test.Time.ToString("HH:mm"))
            .Replace("@@ADDRESS@@", test.Address)
            .Replace("@@LINK@@", "https://www.google.com/maps/search/" + test.Address)
            .Replace("@@NOTES@@", NoteToAdd.Replace("\n", "<br>"))
            .Replace("@@TESTID@@", test.TestID.ToString());
            ;
        }

        /// <summary>
        /// Update Email Sending Time to Now
        /// </summary>
        /// <param name="testID"></param>
        public void UpdateEmailSendingTime(int testID)
        {
            Test test = GetTest(testID);
            test.RemeinderEmailSent = DateTime.Now;
        }

        /// <summary>
        /// Save To XML tamplate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="path"></param>
        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source); file.Close();
        }

        /// <summary>
        /// Load From XML tamplate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }

        /// <summary>
        /// Get Email Temltate HTML
        /// </summary>
        /// <param name="emailType"></param>
        /// <returns></returns>
        public string GetEmailTemltateHTML(EmailType emailType)
        {
            switch (emailType)
            {
                case EmailType.Reminder:
                    return RemeinderEmailHTML;
                case EmailType.TestAdded:
                    return TestAddedEmailHTML;
                case EmailType.TestCancelation:
                    return TestCancelationEmailHTML;
                case EmailType.TestUpdateResults:
                    return TestUpdateResultsEmailHTML;
                case EmailType.Appeal:
                    return AppealEmailHTML;
                default:
                    return "";
            }
        }
    }

}

