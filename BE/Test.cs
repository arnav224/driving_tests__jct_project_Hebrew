using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BE
{
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
        //public BE.TimePeriod Time { get; set; }
        //public DateTime Time { get; set; }//@ + ctor + update func

        //private Address address;
        //public Address Address
        //{
        //    get { return address; }
        //    set
        //    {
        //        // @ if not in the local country. maby define in configuration.
        //        address = value;
        //    }
        //}

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


        public BE.Indices Indices { get; set; }
        public bool Passed { get; set; }
        public string TesterNotes { get; set; }
        /// <summary>
        /// time of last Remeinder Email sending
        /// </summary>
        public DateTime? RemeinderEmailSent { get; set; }
        public DateTime? SummaryEmailSent { get; set; }

        public Test(string TesterID, string TraineeID, DateTime Time, string Address)
        {
            TestID = BE.Configuration.NextTestID();
            this.testerID = TesterID;
            this.TraineeID = TraineeID;
            this.Time = Time;
            this.Address = Address;
            this.Indices = null;
            this.Passed = false;
            this.TesterNotes = "";
            RemeinderEmailSent = SummaryEmailSent = null;
        }
        public override string ToString()   //@
        {
            return null;
        }

    }
}
