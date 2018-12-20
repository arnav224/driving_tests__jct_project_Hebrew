using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DS
{
    static public class DataSource
    {
        static SortedSet<BE.TimePeriod> WorkHours;
        public static List<BE.Tester> testers;
        public static List<BE.Trainee> trainees;
        public static List<BE.Test> tests;

        static public void Initializer()
        {
            WorkHours = new SortedSet<BE.TimePeriod>
            {
            new BE.TimePeriod(new TimeSpan(0, 09, 00, 00), new TimeSpan(0, 18, 00, 00)),
            new BE.TimePeriod(new TimeSpan(1, 09, 00, 00), new TimeSpan(1, 18, 00, 00)),
            new BE.TimePeriod(new TimeSpan(2, 09, 00, 00), new TimeSpan(2, 18, 00, 00)),
            new BE.TimePeriod(new TimeSpan(3, 09, 00, 00), new TimeSpan(3, 18, 00, 00)),
            new BE.TimePeriod(new TimeSpan(4, 09, 00, 00), new TimeSpan(4, 18, 00, 00)),
            new BE.TimePeriod(new TimeSpan(5, 09, 00, 00), new TimeSpan(5, 13, 30, 00)),
            };

            testers = new List<BE.Tester>
        {
                new BE.Tester("10000001", "מוטי", "דגן", new DateTime(1965, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                    "קרני שומרון", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 30000),
                new BE.Tester("10000002", "שמוליק", "קידן", new DateTime(1972, 05, 02), BE.Gender.זכר, "0510000002", "aaa@g.com",
                    "מבוא הישיבה 1 בית אל", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                new BE.Tester("10076543", "חיים", "יעקב", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                     "קריית מלאכי", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),

                new BE.Tester("335344444", "משה", "בן דוד", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                    "שם", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                new BE.Tester("100456678", "דוד", "כהן" , new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                    "פתח תקווה", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                new BE.Tester("100055876", "יעקב", "חיימי", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                    "באר שבע", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                new BE.Tester("100555444", "אברהם", "הלוי", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                    "בני ברק", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000)
            };

            trainees = new List<BE.Trainee>
            {
                new BE.Trainee("12322222", "מושיקו", "הלוי", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                    "avraham224@gmail.com", "רבבה", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "הסולל", "מיכאלה", 38)
            };


            //SortedSet<BE.TimePeriod> WorkHours1 = new SortedSet<BE.TimePeriod>
            //{            new BE.TimePeriod( new TimeSpan(0, 09, 00, 00), new TimeSpan(0, 18, 00, 00)),
            //    new BE.TimePeriod( new TimeSpan(9, 00, 00), new TimeSpan(19, 00, 00))
            //};
            //SortedSet<BE.TimePeriod> WorkHours2 = new SortedSet<BE.TimePeriod>
            //{            new BE.TimePeriod( new TimeSpan(09, 00, 00), new TimeSpan(12, 00, 00)),
            //new BE.TimePeriod( new TimeSpan(9, 00, 00), new TimeSpan(10, 00, 00))
            //};
            //SortedSet<BE.TimePeriod> WorkHours3 = new SortedSet<BE.TimePeriod>
            //{            new BE.TimePeriod( new TimeSpan(09, 00, 00), new TimeSpan(12, 00, 00)),
            //new BE.TimePeriod(new TimeSpan(9, 00, 00), new TimeSpan(10, 00, 00))
            //};


            SortedSet<BE.TimePeriod> WorkHours1;
            SortedSet<BE.TimePeriod> WorkHours2;
            SortedSet<BE.TimePeriod> WorkHours3;
            //SortedSet<BE.TimePeriod> WorkHours;
            //WorkHours = new SortedSet<BE.TimePeriod>
            //{
            //new BE.TimePeriod(new TimeSpan(0, 08, 30, 00), new TimeSpan(0, 18, 00, 00)),
            //new BE.TimePeriod(new TimeSpan(1, 08, 30, 00), new TimeSpan(1, 18, 00, 00)),
            //new BE.TimePeriod(new TimeSpan(2, 08, 30, 00), new TimeSpan(2, 18, 00, 00)),
            //new BE.TimePeriod(new TimeSpan(3, 08, 30, 00), new TimeSpan(3, 18, 00, 00)),
            //new BE.TimePeriod(new TimeSpan(4, 08, 30, 00), new TimeSpan(4, 18, 00, 00)),
            //new BE.TimePeriod(new TimeSpan(5, 08, 30, 00), new TimeSpan(5, 13, 30, 00)),
            //};
            WorkHours1 = WorkHours2 = WorkHours3 = WorkHours;

            testers.Add(new BE.Tester("01223210", "מוטי", "דגן", new DateTime(1925, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                "בית הדפוס 12 ירושלים", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours1, 30000));
            testers.Add(new BE.Tester("012230210", "מוטי", "דגן", new DateTime(1965, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                "רעננה", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours2, 500000));
            testers.Add(new BE.Tester("011122310", "מוטי", "דגן", new DateTime(1965, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                "פתח תקווה", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours3, 50000));

            trainees = new List<BE.Trainee>();
            trainees.Add(new BE.Trainee("122121534", "משה", "גונן", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                "avraham224@gmail.com", "תל דפנה", BE.Vehicle.דו_גלגלי, BE.GearBoxType.אוטומטי, "הדרכים", "שמוליק", 38));
            trainees.Add(new BE.Trainee("123232222", "דוד", "הכהן", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                "avraham224@gmail.com", "עכו", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "סוללים לך עתיד", "שמשון", 38));
            trainees.Add(new BE.Trainee("123242222", "חיים", "אבירם", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                "avraham224@gmail.com", "כפר יונה", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "רמזור", "דנה", 38));
            trainees.Add(new BE.Trainee("123252222", "יענקי", "שפירא", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                "avraham224@gmail.com", "גבעת אולגה", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "שבתאי", "רחל", 38));
            trainees.Add(new BE.Trainee("126322222", "ירמיהו", "מלאכי", new DateTime(2001, 09, 05), BE.Gender.נקבה, "0527560201",
                "avraham224@gmail.com", "המסגר 32 רעננה", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "עוז", "תרצה", 38));
            trainees.Add(new BE.Trainee("123272222", "ישעיהו", "רוסקוביץ'", new DateTime(2001, 09, 05), BE.Gender.נקבה, "0527560201",
                "avraham224@gmail.com", "רבי לוי יצחק יקנעם", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "יחדיו", "דניאל", 38));

            //try
            //{
            //    bl.AddTest("12322222", DateTime.Now.AddHours(0) /*new DateTime(2018, 11, 23)*/, "ראש העין");
            //    bl.AddTest("12322222", DateTime.Now.AddHours(0) /*new DateTime(2018, 11, 23)*/, "רחובות");
            //}
            //catch (Exception e)
            //{

            //    Console.WriteLine(e.Message);
            //}

            tests = new List<BE.Test>()
            {
                new BE.Test() {Address = "ירושלים", TesterID = "10000001", Time = new DateTime(2018, 10, 22, 10, 30, 0), TraineeID = "122121534", },
                new BE.Test() {Address = "ירושלים", TesterID = "10000002", Time = new DateTime(2019, 01, 25, 10, 30, 0), TraineeID = "122121534", },
                new BE.Test() {Address = "באר שבע", TesterID = "10076543", Time = new DateTime(2019, 01, 12, 07, 30, 0), TraineeID = "123232222", },
                new BE.Test() {Address = "ירושלים", TesterID = "335344444", Time = new DateTime(2019, 01, 01, 10, 30, 0), TraineeID = "123242222", },
                new BE.Test() {Address = "ירושלים", TesterID = "100456678", Time = new DateTime(2019, 02, 4, 10, 30, 0), TraineeID = "123272222", },
            };


        }



        //static List<BE.TimePeriod>[] WorkHours = new List<BE.TimePeriod>[5];
        //static List<BE.TimePeriod> Sunday = new List<BE.TimePeriod> { new BE.TimePeriod(BE.WeekDays.ראשון ,new TimeSpan(09, 00, 00), new TimeSpan(15, 00, 00)) };

        //static bool[,] WorkHours = new bool[5, 6]
        //{
        //        { false, true, true, true, true, true  },
        //        { false, true, true, true, true, true  },
        //        { false, true, true, true, true, true  },
        //        { false, true, true, true, true, true  },
        //        { false, true, true, true, true, true  }};

    }


    //public DataSource()
    //{
    //    bool[,] WorkHours = new bool[5, 6] 
    //    { 
    //        { false, true, true, true, true, true  },
    //        { false, true, true, true, true, true  },
    //        { false, true, true, true, true, true  },
    //        { false, true, true, true, true, true  },
    //        { false, true, true, true, true, true  }};
    //    testers.Add(new BE.Tester(315390740, "avraham", "frankel", new DateTime(1995, 09, 05),
    //        BE.Gender.זכר, "0527560202", new BE.Address("Tal Hermon", 26, "Kedumim"), 5, 25, BE.Vehicle.פרטי, WorkHours, 5000));

    //    trainees.Add(new BE.Trainee(315390741, "avraham1", "frankel1", new DateTime(2001, 09, 05),
    //        BE.Gender.זכר, "0527560201", new BE.Address("Tal Hermon", 28, "Kedumim"), BE.Vehicle.פרטי, "fun driving", "Shmulik", 38));

    //    tests.Add(new BE.Test(1234, 315390740, 315390741, new DateTime(2018, 12, 05), new BE.Address("havaad haleumi", 21, "jerusalem"),
    //        new BE.Indices(true, true, true, true), true, "כל הכבוד! תקפיד לא לדרוס חתולות"));
    //}

}
