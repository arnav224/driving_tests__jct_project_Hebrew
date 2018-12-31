using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.ComponentModel;

namespace BE
{
    /// <summary>
    /// Driving student
    /// </summary>
    [Serializable]
    public class Trainee // :INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        
        //[field: NonSerialized]
        //public event PropertyChangedEventHandler PropertyChanged;
        ////public event PropertyChangedEventHandler PropertyChanged;

        #endregion

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

        public Gender Gender { get; set; }

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

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                // Note: Google Maps API is under development:
                // @ not valid address or if not in the local country. maby define in configuration.
                address = value;// = BE.Tools.Maps_GetPlaceAutoComplete(value)[0];
                //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddressAutoComplite"));
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

        //public List<string> AddressAutoComplite
        //{
        //    get { return BE.Tools.Maps_GetPlaceAutoComplete(address); }
        //}

        //public string TestItems = BE.Tools.Maps_GetPlaceAutoComplete(Address);

        public Vehicle Vehicle { get; set; }
        public GearBoxType gearBoxType { get; set; }
        private string drivingSchoolName;
        public string DrivingSchoolName
        {
            get { return drivingSchoolName; }
            set
            {
                Regex r = new Regex("^[a-zA-Zא-ת]([a-z1-9A-Zא-ת]| ){1,33}$");
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

        /// <summary>
        /// The student ask to match his gender to the gender of the tester
        /// </summary>
        public bool OnlyMyGender { get; set; }

        public Trainee(string ID, string FirstName, string LastName, DateTime birthDate, Gender Gender, string PhoneNumber,
            string MailAddress, string Address, Vehicle Vehicle, GearBoxType gearBoxType, 
            string DrivingSchoolName, string TeacherName, int NumOfDrivingLessons, bool OnlyMyGender = false)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.birthDate = birthDate;
            this.Gender = Gender;
            this.PhoneNumber = PhoneNumber;
            this.MailAddress = MailAddress;
            this.Address = Address;
            this.Vehicle = Vehicle;
            this.gearBoxType = gearBoxType;
            this.DrivingSchoolName = DrivingSchoolName;
            this.TeacherName = TeacherName;
            this.NumOfDrivingLessons = NumOfDrivingLessons;
            this.OnlyMyGender = OnlyMyGender;
        }
        public Trainee() { }

        public override string ToString()  
        {
            return "תעודת זהות: " + ID + ", מין: " + Gender + ", שם: " + FirstName + ' ' + LastName 
                + (birthDate != default(DateTime) ? ", " + ", תאריך לידה: " + birthDate.ToString("dd/MM/yyyy") : "") 
                + ", " + "טלפון: " + PhoneNumber + ", כתובת: " + Address 
                + ", סוג רכב: " + Vehicle + "-" + gearBoxType + ", בית ספר: "
                 + DrivingSchoolName + ", שם המורה: " + TeacherName + ", מספר שיעורים: " + numOfDrivingLessons + '.';
        }
    }
}
