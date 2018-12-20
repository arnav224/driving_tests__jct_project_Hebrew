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

        public Dictionary<string, BE.Score> Indices;
        public bool Passed { get; set; }
        public string TesterNotes { get; set; }
        /// <summary>
        /// time of last Remeinder Email sending
        /// </summary>
        public DateTime? RemeinderEmailSent { get; set; }
        public DateTime? SummaryEmailSent { get; set; }

        public Test()
        {
            TestID = BE.Configuration.NextTestID();
            Indices = Configuration.DefultIndices();
        }
    }
}
