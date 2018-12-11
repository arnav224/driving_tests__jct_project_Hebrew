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

    [Serializable]
    public struct TimePeriod : IComparable<TimePeriod>
    {
        public TimePeriod(DayOfWeek Day, TimeSpan Start, TimeSpan End)
        {
            //if (Start.Hours < 7)
            //    throw new Exception("זמן ההתחלה מוקדם מדי.");
            //if (End.Hours > 21)
            //    throw new Exception("זמן הסיום מאוחר מדי.");
            if (Start > End)
                throw new Exception("זמן סיום לא יכול להיות לפני זמן התחלה.");
            this.Day = Day;
            this.Start = Start;
            this.End = End;
        }
        public DayOfWeek Day { get; private set; }
        //public WeekDays Day { get; private set; }
        public TimeSpan Start { get; private set; }
        public TimeSpan End { get; private set; }

        public override string ToString()
        {
            return Day.ToString() + ' ' + Start + " עד " + End;
        }

        public int CompareTo(TimePeriod other)
        {
            if (Day != other.Day)
                return Day.CompareTo(other.Day);
            return this.Start.CompareTo(other.Start);
        }
    }

    [Serializable]
    public class Indices :IEnumerable
    {

        public Indices(Score DistanceKeeping, Score ReverseParking, Score mirrors, Score signals)
        {
            this.DistanceKeeping = DistanceKeeping;
            this.ReverseParking = ReverseParking;
            this.mirrors = mirrors;
            this.signals = signals;
        }
        Score DistanceKeeping;
        Score ReverseParking;
        Score mirrors;
        Score signals;

        public IEnumerator GetEnumerator()
        {
            yield return DistanceKeeping;
        }
    }

    public enum Score
    {
        עבר, רע, רע_מאוד, נכשל
    }

    public enum Gender
    {
        זכר, נקבה
    }

    public enum Vehicle
    {
        פרטי, דו_גלגלי, משאית_בינונית, משאית_כבדה
    }

    public enum GearBoxType
    {
        ידני, אוטומטי
    }

}