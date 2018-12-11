using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Factory
    {
        protected Factory() { }
        static IDAL instance = null;
        public static IDAL GetInstance()
        {
            if (instance == null)
                instance = new Dal_imp();
            return instance;
        }
    }
}
