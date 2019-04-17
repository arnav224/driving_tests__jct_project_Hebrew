using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    /// <summary>
    /// get the singelton instance of BL
    /// </summary>
    static public class Factory
    {
        static IDAL instance = null;
        public static IDAL GetInstance()
        {
            if (instance == null)
                instance = new DAL_XML_imp();
            return instance;
        }
    }
}
