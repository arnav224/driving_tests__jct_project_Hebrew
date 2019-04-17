using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Xml.Serialization;

namespace BE
{
    /// <summary>
    /// Driving Tester
    /// </summary>
    [Serializable]
    public class Tester
    {
        private string id;
        public string ID
        {
            get { return id; }
            set
            {
                if (!Tools.ValidateID(value))
                    throw new Exception("תעודת זהות לא תקינה.");
                id = value;
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                Regex r = new Regex("^([^20]|[a-zA-Zא-ת]){2,35}$");
                if (!r.IsMatch(value))
                    throw new Exception("שם צריך להכיל 2-35 אותיות.");
                firstName = value;
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                Regex r = new Regex("^([^20]|[a-zA-Zא-ת]){2,35}$");
                if (!r.IsMatch(value))
                    throw new Exception("שם צריך להכיל 2-35 אותיות.");
                lastName = value;
            }
        }

        private DateTime birthDate;
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                if (value > DateTime.Now || DateTime.Now.Year - value.Year > 120)
                    throw new Exception("גיל לא תקין");
                birthDate = value;
            }
        }

        public Gender Gender { get; set; }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                Regex r = new Regex("(^0(5|7)[0-9]-{0,1}[0-9]{7}$)|(^0(2|3|4|7|8|9)-{0,1}[0-9]{7}$)"/*"^0[0-9]{1,2}-{0,1}[0-9]{7}$"*/);
                if (!r.IsMatch(value))
                    throw new Exception("מספר הטלפון לא תקין.");
                phoneNumber = value;
            }
        }

        private string mailAddress;
        public string MailAddress
        {
            get { return mailAddress; }
            set
            {
                try
                {
                    MailAddress m = new MailAddress(value);
                }
                catch (Exception)
                {
                    throw new Exception("כתובת המייל לא תקינה.");
                }
                mailAddress = value;
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                // @ google maps - not valid address or if not in the local country. maby define in configuration.
                address = value; // = BE.Tools.Maps_GetPlaceAutoComplete(value)[0];
            }
        }

        private DateTime experiencedSince;
        public DateTime ExperiencedSince
        {
            get
            {
                return experiencedSince;
            }
            set
            {
                if (value > DateTime.Now || DateTime.Now.Year - value.Year > 75)
                    throw new Exception("שנת תחילת עבודה לא תקינה");
                experiencedSince = value;
            }
        }
        /// <summary>
        /// years of experience
        /// </summary>
        public int Experience
        {
            get
            {
                return DateTime.Now.Year - experiencedSince.Year;
            }
        }

        private int maxTestsInWeek;
        public int MaxTestsInWeek
        {
            get { return maxTestsInWeek; }
            set
            {
                if (value < 0 || value > BE.Configuration.MaximumTestsInWeek)
                    throw new Exception("מספר מקסימום טסטים לשבוע לא תקין.");
                maxTestsInWeek = value;
            }
        }

        public Vehicle Vehicle { get; set; }
        public GearBoxType GearBoxType { get; set; }

        private SortedSet<TimePeriod> workHours;
        [XmlIgnore]
        public SortedSet<TimePeriod> WorkHours
        {
            get
            {
                if (workHours == null)
                    workHours = new SortedSet<TimePeriod>();
                return workHours;
            }
            set
            {
                if (value.Count != 0)
                {
                    SortedSet<BE.TimePeriod> tempTimePeriods = new SortedSet<TimePeriod>();
                    TimeSpan previousStart = value.ElementAt(0).Start;
                    TimeSpan previousEnd = value.ElementAt(0).End;
                    foreach (var item in value)
                    {
                        if (item.Start.CompareTo(previousEnd) <= 0)
                        {
                            if (item.End.CompareTo(previousEnd) > 0)
                                previousEnd = item.End;
                        }
                        else
                        {
                            tempTimePeriods.Add(new TimePeriod(previousStart, previousEnd));
                            previousStart = item.Start;
                            previousEnd = item.End;
                        }
                    }
                    tempTimePeriods.Add(new TimePeriod(previousStart, previousEnd));
                    workHours = tempTimePeriods;
                }
            }
        }

        public string WorkHoursToSring
        {
            get
            {
                string result = "";
                foreach (var item in workHours)
                {
                    result += item.Start.Days.ToString() + ',' + item.Start.Hours.ToString() + ',' + item.Start.Minutes.ToString() + ',' + item.End.Hours.ToString() + ',' + item.End.Minutes.ToString() + ';';
                }
                return result.TrimEnd(';');
            }
            set
            {
                string[] timePeriods = value.Split(';');
                SortedSet<BE.TimePeriod> tempTimePeriods = new SortedSet<TimePeriod>();
                foreach (var item in timePeriods)
                {
                    int[] timePeriod = (from str in item.Split(',') select Convert.ToInt32(str)).ToArray();
                    tempTimePeriods.Add(new TimePeriod(new TimeSpan(timePeriod[0], timePeriod[1], timePeriod[2], 0),
                                        new TimeSpan(timePeriod[0], timePeriod[3], timePeriod[4], 0)));
                }
                workHours = tempTimePeriods;
            }
        }

        /// <summary>
        /// Maximum distance the Tester agrees to move from his home to the Test
        /// </summary>
        private int maxDistanceInMeters;
        public int MaxDistanceInMeters
        {
            get { return maxDistanceInMeters; }
            set
            {
                if (value < 0)
                    throw new Exception("המרחק המקסימלי לא תקין.");
                maxDistanceInMeters = value;
            }
        }



        public Tester(string ID, string FirstName, string LastName, DateTime birthDate,
             Gender Gender, string PhoneNumber, string MailAddress, string Address,
            int Experience, int MaxTestsInWeek, Vehicle Vehicle, GearBoxType gearBoxType, SortedSet<TimePeriod> WorkHours, int MaxDistanceInMeters)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.BirthDate = birthDate;
            this.Gender = Gender;
            this.PhoneNumber = PhoneNumber;
            this.MailAddress = MailAddress;
            this.Address = Address;
            this.ExperiencedSince = DateTime.Now.AddYears(-Experience);
            this.MaxTestsInWeek = MaxTestsInWeek;
            this.Vehicle = Vehicle;
            this.GearBoxType = gearBoxType;
            this.WorkHours = WorkHours;
            this.MaxDistanceInMeters = MaxDistanceInMeters;
        }
        public Tester() { this.WorkHours = new SortedSet<TimePeriod>(); }

        public override string ToString()
        {
            string result = "שם: " + FirstName + ' ' + LastName +  ", ת\"ז:" + ID + (birthDate != default(DateTime) ? ", תאריך לידה: " + birthDate.ToString("dd/MM/yyyy") : "")
                + ", מין: " + Gender + ", טלפון: " + PhoneNumber + ", כתובת מייל: " + MailAddress + ", כתובת: " + Address
                + ", שנות נסיון: " + Experience + ", מקסימום טסטים בשבוע: "
                + MaxTestsInWeek + ", סוג רכב: " + Vehicle + '-' + GearBoxType + ", שעות עבודה: ";
            foreach (var item in WorkHours)
            {
                result += item.ToString() + ", ";
            }
            return result + '.';
        }

    }
}
