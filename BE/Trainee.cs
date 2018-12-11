using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace BE
{
    [Serializable]
    public class Trainee
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
                Regex r = new Regex("^[a-zA-Zא-ת]{2,35}$");
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
                Regex r = new Regex("^[a-zA-Zא-ת]{2,35}$");
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

        public string MailAddress { get; set; }
        //private MailAddress mailAddress;
        //public MailAddress MailAddress
        //{
        //    get { return mailAddress; }
        //    set
        //    {
        //        try
        //        {
        //            mailAddress = value;
        //        }
        //        catch (Exception)
        //        {
        //            throw new Exception("כתובת המייל לא תקינה.");
        //        }
        //    }
        //}

        //public Address Address { get; set; }
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                
                // @ not valid address or if not in the local country. maby define in configuration.
                address = BE.Tools.Maps_GetPlaceAutoComplete(value)[0];
            }
        }

        public Vehicle Vehicle { get; set; }
        public GearBoxType gearBoxType { get; set; }
        private string drivingSchoolName;
        public string DrivingSchoolName
        {
            get { return drivingSchoolName; }
            set
            {
                Regex r = new Regex("^[a-zA-zא-ת]([a-zA-Z0-9]| ){1,34}$");
                if (!r.IsMatch(value))
                    throw new Exception("שם בית הספר לא תקין.");
                drivingSchoolName = value;
            }
        }
        private string teacherName;
        public string TeacherName
        {
            get { return teacherName; }
            set
            {
                Regex r = new Regex("^[a-zA-Zא-ת]{2,35}$");
                if (!r.IsMatch(value))
                    throw new Exception("שם צריך להכיל 2-35 אותיות.");
                teacherName = value;
            }
        }

        //public int NumOfDrivingLessons { get; set; }
        private int numOfDrivingLessons;
        public int NumOfDrivingLessons
        {
            get { return numOfDrivingLessons; }
            set
            {
                if (value < 0)
                    throw new Exception("מספר שיעורי נהיגה לא יכול להיות שלילי.");
                numOfDrivingLessons = value;
            }
        }

        public bool OnlyMyGender { get; set; }
        public Trainee(string ID, string FirstName, string LastName, DateTime birthDate, Gender Gender, string PhoneNumber,
            string MailAddress, string Address, Vehicle Vehicle, GearBoxType gearBoxType, string DrivingSchoolName, string TeacherName, int NumOfDrivingLessons, bool OnlyMyGender = false)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.birthDate = birthDate;
            this.Gender = Gender;
            this.PhoneNumber = PhoneNumber;
            this.MailAddress = MailAddress;
            //this.MailAddress = new MailAddress(MailAddress);
            this.Address = Address;
            this.Vehicle = Vehicle;
            this.DrivingSchoolName = DrivingSchoolName;
            this.TeacherName = TeacherName;
            this.NumOfDrivingLessons = NumOfDrivingLessons;
            this.OnlyMyGender = OnlyMyGender;
        }
        public override string ToString()   //@
        {
            //string result = "שם: " + FirstName + ' ' + LastName + ' ' + "תאריך לידה: " + BirthDate.ToString("MM/dd/yyyy HH:mm") + ' '
            //    + "טלפון:" + PhoneNumber + ' ' + "כתובת: " + Address + ' ' + "שנות נסיון: " + Experience + ' ' + "מקסימום טסטים בשבוע: "
            //    + MaxTestsInWeek + ' ' + "סוג רכב: " + Vehicle + ' ' + gearBoxType;
            //foreach (var item in WorkHours)
            //{
            //    result += item.ToString() + ' ';
            //}
            //return result;
            return null;
        }

    }
}
