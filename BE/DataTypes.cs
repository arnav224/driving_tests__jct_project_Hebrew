using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;

namespace BE
{
    //[Serializable]
    //public struct Address
    //{
    //    public Address(string street, int HostNumber, string City)
    //    {
    //        this.street = street;
    //        this.HostNumber = HostNumber;
    //        this.City = City;
    //    }
    //    public string street;
    //    public int HostNumber;
    //    public string City;
    //    public override string ToString()
    //    {
    //        return street + ' ' + HostNumber + ", " + City;
    //    }
    //    public string ToGoogleMapsFormat()
    //    {
    //        return City.Replace(' ', '+') + '+' + "ON|" + HostNumber.ToString() + '+' + street.Replace(' ', '+');
    //    }
    //}

    public enum WeekDays
    {
        ראשון, שני, שלישי, רביעי, חמישי, שישי, שבת
    }

    /// <summary>
    /// Period of time of Tester
    /// </summary>
    [Serializable]
    public struct TimePeriod : IComparable<TimePeriod>
    {
        public TimePeriod(TimeSpan Start, TimeSpan End)
        {
            if (Start.Days != End.Days)
                throw new Exception("יש להוסיף זמנים לכל יום בנפרד.");
            if (Start.Days > 5 || Start.Hours > 24 || End.Hours > 24 || Start.Minutes > 60 || End.Minutes > 60 || Start == End)
                throw new Exception("זמן לא תקין.");
            if (Start > End)
                throw new Exception("זמן סיום לא יכול להיות לפני זמן התחלה.");
            if (Start.Hours < Configuration.WorkStartHour)
                throw new Exception("זמן תחילת העבודה לא יכול להיות לפני השעה " + Configuration.WorkStartHour);
            if (Start.Days < 5 && End.Hours > Configuration.WorkEndHour)
                throw new Exception("זמן סיום העבודה לא יכול להיות אחרי השעה " + Configuration.WorkEndHour);
            if (Start.Days == 5 && End.Hours > Configuration.FridayWorkEndHour)
                throw new Exception("ביום שישי זמן סיום העבודה לא יכול להיות אחרי השעה " + Configuration.FridayWorkEndHour);
            this.Start = Start;
            this.End = End;
        }
        public TimeSpan Start { get; private set; }
        public TimeSpan End { get; private set; }

        public override string ToString()
        {
            return ((BE.WeekDays)Start.Days).ToString() + ' ' + Start.ToString(@"hh\:mm") + " עד " + End.ToString(@"hh\:mm");
        }

        public int CompareTo(TimePeriod other)
        {
            if (this.Start != other.Start)
                return this.Start.CompareTo(other.Start);
            if (this.End != other.End)
                return this.End.CompareTo(other.End);
            return 0;
        }
    }

    [Serializable]
    public class AppealTest
    {
        public DateTime RequestTime;
        public DateTime DecisionTime;
        public AppealStatus appealStatus;
        public string TraineeNotes;
        public string Decision;
    }

    public enum AppealStatus
    {
        ממתין, התקבל, נדחה
    }

    /// <summary>
    /// A possible score for a test
    /// </summary>
    public enum Score
    {
        נכשל, רע_מאוד, רע, עבר
    }

    public enum Gender
    {
        זכר, נקבה
    }

    /// <summary>
    /// car type
    /// </summary>
    public enum Vehicle
    {
        פרטי, דו_גלגלי, משאית_בינונית, משאית_כבדה
    }

    /// <summary>
    /// Gear Box Type
    /// </summary>
    public enum GearBoxType
    {
        ידני, אוטומטי
    }

}