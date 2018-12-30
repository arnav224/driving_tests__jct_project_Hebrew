using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BE
{
    /// <summary>
    /// driving test
    /// </summary>
    [Serializable]
    public class Test
    {
        public int TestID { get; internal set; }

        private string testerID;
        public string TesterID
        {
            get { return testerID; }
            set
            {
                Regex r = new Regex("^[0-9]{8,10}$");
                if (!r.IsMatch(value))
                    throw new Exception("תעודת זהות צריכה להכיל 8-10 ספרות.");
                testerID = value;
            }
        }

        private string traineeID;
        public string TraineeID
        {
            get { return traineeID; }
            set
            {
                Regex r = new Regex("^[0-9]{8,10}$");
                if (!r.IsMatch(value))
                    throw new Exception("תעודת זהות צריכה להכיל 8-10 ספרות.");
                traineeID = value;
            }
        }

        public DateTime Time { get; set; }

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                // @ google maps - not valid address or if not in the local country. maby define in configuration.
                address = value;// BE.Tools.Maps_GetPlaceAutoComplete(value)[0];
            }
        }

        public bool? Passed { get; set; }

        public string TesterNotes { get; set; }

        private Dictionary<string, BE.Score> indices;

        public Dictionary<string, BE.Score> Indices
        {
            get
            {
                if (indices == null)
                {
                    indices = new Dictionary<string, Score>();
                    indices.Add("בטיחות", Score.עבר);
                    indices.Add("שליטה בהגה", Score.עבר);
                    indices.Add("שליטה בהילוכים", Score.עבר);
                    indices.Add("הסתכלות במראות", Score.עבר);
                    indices.Add("מתן זכות קדימה", Score.עבר);
                    indices.Add("מהירות", Score.עבר);
                    indices.Add("איתות", Score.עבר);
                    indices.Add("האצה והאטה בבטחה", Score.עבר);
                    indices.Add("ציות לתמרורים", Score.עבר);
                    indices.Add("שמירה רווח", Score.עבר);
                    indices.Add("בטיחות הולכי רגל", Score.עבר);
                    indices.Add("עקיפות", Score.עבר);
                    indices.Add("חנייה", Score.עבר);
                    indices.Add("פניות", Score.עבר);
                    indices.Add("השתלבות בתנועה", Score.עבר);
                    indices.Add("אביזרי בטיחות", Score.עבר);
                    indices.Add("נסיעה לאחור", Score.עבר);
                    indices.Add("שמירה על הימין", Score.עבר);
                }
                return indices;
            }
            set
            {
                indices = value;
            }
        }

        private BE.AppealTest appealTest;
        public BE.AppealTest AppealTest
        {
            get { return appealTest; }
            set { appealTest = value; }
        }

        /// <summary>
        /// time of last Remeinder Email sending
        /// </summary>
        public DateTime? RemeinderEmailSent { get; set; }
        public DateTime? SummaryEmailSent { get; set; }

        public Test()
        {
            TestID = BE.Configuration.NextTestID();
        }
        public override string ToString()
        {
            return "מספר סידורי: " + TestID + ", מספר בוחן: " + TesterID + ", מספר תלמיד: " + TraineeID 
                + ", מקום: " + Address + ", תאריך ושעה: " + Time.ToString("MM/dd/yyyy HH:mm") + ',' 
                + (Time > DateTime.Now ? " טרם התבצע הטסט." : (Passed != null && indices.Any(t=> t.Value != Score.עבר) ?
                " תוצאה: " + (Passed == true ? "עבר":"לא עבר") + ", הערות: " + TesterNotes + '.': ""));
        }
    }
}
