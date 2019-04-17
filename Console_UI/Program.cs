using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using BE;

namespace UI_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                //            BL.IBL bl = BL.Factory.GetInstance();
                //            //SortedSet<BE.TimePeriod> WorkHours1 = new SortedSet<BE.TimePeriod>
                //            //{            new BE.TimePeriod(DayOfWeek.Sunday, new TimeSpan(09, 00, 00), new TimeSpan(18, 00, 00)),
                //            //    new BE.TimePeriod(/*BE.WeekDays.ראשון*/ DayOfWeek.Tuesday, new TimeSpan(9, 00, 00), new TimeSpan(19, 00, 00))
                //            //};
                //            //SortedSet<BE.TimePeriod> WorkHours2 = new SortedSet<BE.TimePeriod>
                //            //{            new BE.TimePeriod(DayOfWeek.Sunday, new TimeSpan(09, 00, 00), new TimeSpan(12, 00, 00)),
                //            //new BE.TimePeriod(/*BE.WeekDays.ראשון*/ DayOfWeek.Sunday, new TimeSpan(9, 00, 00), new TimeSpan(10, 00, 00))
                //            //};
                //            //SortedSet<BE.TimePeriod> WorkHours3 = new SortedSet<BE.TimePeriod>
                //            //{            new BE.TimePeriod(DayOfWeek.Sunday, new TimeSpan(09, 00, 00), new TimeSpan(12, 00, 00)),
                //            //new BE.TimePeriod(/*BE.WeekDays.ראשון*/ DayOfWeek.Sunday, new TimeSpan(9, 00, 00), new TimeSpan(10, 00, 00))
                //            //};
                //            SortedSet<BE.TimePeriod> WorkHours1;
                //            SortedSet<BE.TimePeriod> WorkHours2;
                //            SortedSet<BE.TimePeriod> WorkHours3;
                //            SortedSet<BE.TimePeriod> WorkHours;
                //            WorkHours = new SortedSet<BE.TimePeriod>
                //            {
                //            new BE.TimePeriod(new TimeSpan(0, 09, 00, 00), new TimeSpan(0, 18, 00, 00)),
                //            new BE.TimePeriod(new TimeSpan(1, 09, 00, 00), new TimeSpan(1, 18, 00, 00)),
                //            new BE.TimePeriod(new TimeSpan(2, 09, 00, 00), new TimeSpan(2, 18, 00, 00)),
                //            new BE.TimePeriod(new TimeSpan(3, 09, 00, 00), new TimeSpan(3, 18, 00, 00)),
                //            new BE.TimePeriod(new TimeSpan(4, 09, 00, 00), new TimeSpan(4, 18, 00, 00)),
                //            new BE.TimePeriod(new TimeSpan(5, 09, 00, 00), new TimeSpan(5, 13, 30, 00)),
                //            };
                //            WorkHours1 = WorkHours2 = WorkHours3 = WorkHours;

                //            bl.AddTester(new BE.Tester("01223210", "מוטי", "דגן", new DateTime(1925, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                //                "בית הדפוס 12 ירושלים", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours1, 5000));
                //            bl.AddTester(new BE.Tester("012230210", "מוטי", "דגן", new DateTime(1965, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                //                "רעננה", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours2, 5000));
                //            bl.AddTester(new BE.Tester("011122310", "מוטי", "דגן", new DateTime(1965, 11, 05), BE.Gender.זכר, "0510000001", "aaa@g.com",
                //                "פתח תקווה", 5, 25, BE.Vehicle.פרטי, BE.GearBoxType.ידני, WorkHours3, 5000));

                //            //bl.AddTrainee(new BE.Trainee("122121534", "avrah1am1", "1frankel1", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                //            //    "avraham224@gmail.com", new BE.Address("Tal 1Hermon", 28, "Kedumim"), BE.Vehicle.דו_גלגלי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38));
                //            //bl.AddTrainee(new BE.Trainee("123232222", "avrah2am1", "f2rankel1", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                //            //    "avraham224@gmail.com", new BE.Address("Tal 2Hermon", 28, "Kedumim"), BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38));
                //            //bl.AddTrainee(new BE.Trainee("123242222", "avra3ham1", "3frankel1", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                //            //    "avraham224@gmail.com", new BE.Address("Tal3 Hermon", 28, "Kedumim"), BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38));
                //            //bl.AddTrainee(new BE.Trainee("123252222", "avra4ham1", "f4rankel1", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                //            //    "avraham224@gmail.com", new BE.Address("Tal4 Hermon", 28, "Kedumim"), BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38));
                //            //bl.AddTrainee(new BE.Trainee("126322222", "avr5aham1", "f5rankel1", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                //            //    "avraham224@gmail.com", new BE.Address("Tal5 Hermon", 28, "Kedumim"), BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38));
                //            //bl.AddTrainee(new BE.Trainee("123272222", "av6raham1", "f6rankel1", new DateTime(2001, 09, 05), BE.Gender.זכר, "0527560201",
                //            //    "avraham224@gmail.com", new BE.Address("Tal6 Hermon", 28, "Kedumim"), BE.Vehicle.פרטי, BE.GearBoxType.אוטומטי, "fun driving", "Shmulik", 38));
                //            //DateTime dateTime =
                //            //try
                //            //{
                //            //bl.AddTest( new Test("12322222", DateTime.Now.AddHours(0) /*new DateTime(2018, 11, 23)*/, "ראש העין");
                //            //bl.AddTest("12322222", DateTime.Now.AddHours(0) /*new DateTime(2018, 11, 23)*/, "רחובות");
                //            //}
                //            //catch (Exception e)
                //            //{

                //            //    Console.WriteLine(e.Message);
                //            //}




                //            Console.WriteLine(@"Select Action:
                //a - Add a Tester.
                //b - delete a Tester.
                //c - update tester.
                //d - add trainee
                //e - delete trainee.
                //f - update trainee.
                //g - add test
                //h - update test result.
                //i - print all testers.
                //j - print all tests.
                //k - print all trainee.
                //l - print all tests Who have not received a reminder yet
                //m - 
                //o - 
                //");
                //            //string ID, firstName, LastName, phone, email, Street, City;
                //            //int hostNumber, year, month, Day, expariance, maxTestsInweek, maxDistance;
                //            //Gender gender;
                //            //Vehicle vehicle;
                //            //GearBoxType gearBoxType;
                //            //SortedSet<BE.TimePeriod> workHours;
                //            char choice = Console.ReadLine()[0];
                //            switch (choice)
                //            {
                //                case 'a':
                //                    //                    ID =  Console.ReadLine();
                //                    //                    firstName = Console.ReadLine();
                //                    //                    LastName = Console.ReadLine();
                //                    //                    year = int.Parse(Console.ReadLine());
                //                    //                    month = int.Parse(Console.ReadLine());
                //                    //                    Day = int.Parse(Console.ReadLine());
                //                    //                    gender = Console.ReadLine() == "זכר" ? Gender.זכר : Gender.נקבה;
                //                    //                    phone = Console.ReadLine();
                //                    //                    email = Console.ReadLine();
                //                    //                    Street = Console.ReadLine();
                //                    //                    hostNumber = int.Parse(Console.ReadLine());
                //                    //                    City = Console.ReadLine();
                //                    //                    expariance = int.Parse(Console.ReadLine());
                //                    //                    maxTestsInweek = int.Parse(Console.ReadLine());




                //                    //                    Tester tester = new Tester(ID, firstName, LastName, new DateTime(year, month, Day), gender, phone, email, 
                //                    //                        new Address(Street, hostNumber, City), expariance, maxTestsInweek );
                //                    //);

                //                    break;

                //                case 'b':
                //                    break;

                //                case 'c':
                //                    break;

                //                case 'd':
                //                    break;

                //                case 'e':
                //                    break;

                //                case 'f':
                //                    break;

                //                case 'g':
                //                    break;

                //                case 'h':
                //                    BE.Test test = bl.GetAllTests(t => t.TraineeID == "12322222").FirstOrDefault();
                //                    //bl.UpdateTestResult(test.TestID, new BE.Indices(Score.עבר, Score.עבר, Score.עבר, Score.עבר));
                //                    break;

                //                case 'i':
                //                    foreach (Tester item in bl.GetAllTesters())
                //                    {
                //                        Console.WriteLine(item.ToString());
                //                    }
                //                    break;

                //                case 'j':
                //                    break;

                //                case 'k':
                //                    break;

                //                case 'l':
                //                    break;

                //                case 'm':
                //                    break;

                //                default:
                //                    break;
                //            }






                //bl.SendTestsRemindersLoop();
                //Console.WriteLine(Tools.Maps.GetDistanceGoogleMapsAPIXML(new BE.Address("טל חרמון", 26, "קדומים"), new BE.Address("הועד הלאומי", 21, "ירושלים")));
                //Console.WriteLine(Tools.Maps.GetDistanceGoogleMapsAPIXML(new BE.Address("Sussex+Drive+Ottawa", 24, "Bobcaygeon"),
                //    new BE.Address("Sussex+Drive+Ottawa", 28, "Bobcaygeon")));
            }





            Console.ReadKey();
        }
    }
}
