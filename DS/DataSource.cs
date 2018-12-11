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
            new BE.TimePeriod(DayOfWeek.Sunday, new TimeSpan(09, 00, 00), new TimeSpan(18, 00, 00)),
            new BE.TimePeriod(DayOfWeek.Thursday, new TimeSpan(09, 00, 00), new TimeSpan(18, 00, 00)),
            //new BE.TimePeriod(DayOfWeek.Thursday, new TimeSpan(13, 00, 00), new TimeSpan(18, 00, 00)),
            new BE.TimePeriod(DayOfWeek.Monday, new TimeSpan(09, 00, 00), new TimeSpan(18, 00, 00)),
            new BE.TimePeriod(DayOfWeek.Tuesday, new TimeSpan(09, 00, 00), new TimeSpan(18, 00, 00)),
            new BE.TimePeriod(DayOfWeek.Wednesday, new TimeSpan(09, 00, 00), new TimeSpan(18, 00, 00)),
            new BE.TimePeriod(/*BE.WeekDays.ראשון*/ DayOfWeek.Monday, new TimeSpan(9, 00, 00), new TimeSpan(17, 00, 00))
            };

            testers = new List<BE.Tester>
        {
            new BE.Tester("10000001", "מוטי", "דגן", new DateTime(1965, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                "קרני שומרון", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 30000),
            new BE.Tester("10000002", "שמוליק", "קידן", new DateTime(1972, 05, 02), BE.Gender.זכר, "0510000002", "aaa@g.com",
                "מבוא הישיבה 1 בית אל", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                //new BE.Tester("1000g", "חיים", "בן יעקב", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                //    new BE.Address("רוטשילד", 26, "ירושלים"), 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),

                //new BE.Tester("1235", "משה", "בן דוד", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                //    new BE.Address("רוטשילד", 26, "בני ברק"), 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                //new BE.Tester("1000g", "דוד", "כהן" , new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                //    new BE.Address("רוטשילד", 26, "בני ברק"), 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                //new BE.Tester("1000g", "יעקב", "חיימי", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                //    new BE.Address("רוטשילד", 26, "בני ברק"), 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000),
                //new BE.Tester("1000g", "אברהם", "הלוי", new DateTime(1995, 09, 05), BE.Gender.זכר, "0510000003", "aaa@g.com",
                //    new BE.Address("רוטשילד", 26, "בני ברק"), 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours, 5000)
            };

            trainees = new List<BE.Trainee>
        {
            new BE.Trainee("12322222", "avrahamhh", "frankelll", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                "avraham224@gmail.com", "רבבה", BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38)
        };

            tests = new List<BE.Test>
        {
            new BE.Test("122121234", "12322222", new DateTime(2018, 12, 22), "פדואל")
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
