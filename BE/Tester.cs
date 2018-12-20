using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
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
                Regex r = new Regex("^[0-9]{8,10}$");
                if (!r.IsMatch(value))
                    throw new Exception("תעודת זהות צריכה להכיל 8-10 ספרות.");
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
        public GearBoxType gearBoxType { get; set; }
        public readonly SortedSet<TimePeriod> WorkHours;

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
            this.gearBoxType = gearBoxType;
            this.WorkHours = WorkHours;
            this.MaxDistanceInMeters = MaxDistanceInMeters;
        }
        public Tester() { this.WorkHours = new SortedSet<TimePeriod>(); }

        public override string ToString()
        {
            string result = "שם: " + FirstName + ' ' + LastName + ", תאריך לידה: " + BirthDate.ToString("MM/dd/yyyy HH:mm")
                + ", מין: " + Gender + ", טלפון: " + PhoneNumber + ", כתובת מייל: " + MailAddress + ", כתובת: " + Address
                + ", שנות נסיון: " + Experience + ", מקסימום טסטים בשבוע: "
                + MaxTestsInWeek + ", סוג רכב: " + Vehicle + ' ' + gearBoxType + ", שעות עבודה: ";
            foreach (var item in WorkHours)
            {
                result += item.ToString() + ' ';
            }
            return result;
        }

    }
}
