using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BE
{
    /// <summary>
    /// driving test
    /// </summary>
    [Serializable]
    public class Test
    {
        public int TestID { get; set; }

        private string testerID;
        public string TesterID
        {
            get { return testerID; }
            set
            {
                if (!Tools.ValidateID(value))
                    throw new Exception("תעודת זהות לא תקינה.");
                testerID = value;
            }
        }

        private string traineeID;
        public string TraineeID
        {
            get { return traineeID; }
            set
            {
                if (!Tools.ValidateID(value))
                    throw new Exception("תעודת זהות לא תקינה.");
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
                address = value;
            }
        }

        public bool? Passed { get; set; }

        public string TesterNotes { get; set; }

        private Dictionary<string, BE.Score> indices;
        [XmlIgnore]
        public Dictionary<string, BE.Score> Indices
        {
            get
            {
                if (indices == null)
                {
                    indices = new Dictionary<string, Score>
                    {
                        { "בטיחות", Score.עבר },
                        { "שליטה בהגה", Score.עבר },
                        { "שליטה בהילוכים", Score.עבר },
                        { "הסתכלות במראות", Score.עבר },
                        { "מתן זכות קדימה", Score.עבר },
                        { "מהירות", Score.עבר },
                        { "איתות", Score.עבר },
                        { "האצה והאטה בבטחה", Score.עבר },
                        { "ציות לתמרורים", Score.עבר },
                        { "שמירה רווח", Score.עבר },
                        { "בטיחות הולכי רגל", Score.עבר },
                        { "עקיפות", Score.עבר },
                        { "חנייה", Score.עבר },
                        { "פניות", Score.עבר },
                        { "השתלבות בתנועה", Score.עבר },
                        { "אביזרי בטיחות", Score.עבר },
                        { "נסיעה לאחור", Score.עבר },
                        { "שמירה על הימין", Score.עבר }
                    };
                }
                return indices;
            }
            set
            {
                indices = value;
            }
        }
        public string IndicesToString
        {
            get
            {
                if (indices == null)
                    return null;
                string result = "";
                foreach (var item in indices)
                {
                    result += item.Key + ',' + item.Value.ToString() + ';';
                }
                    return result.TrimEnd(';');
            }
            set
            {
                if (value != null)
                {
                    string[] pairs = value.Split(';');
                    Dictionary<string, Score> tempIndices = new Dictionary<string, Score>();
                    foreach (var item in pairs)
                    {
                        string[] pair = item.Split(',');
                        tempIndices.Add(pair[0], (Score)Enum.Parse(typeof(Score), pair[1]));
                    }
                    indices = tempIndices;
                }
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
