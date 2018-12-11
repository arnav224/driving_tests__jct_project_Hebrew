using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public class Factory
    {
        protected Factory() { }
        static IBL instance = null;
        public static IBL GetInstance()
        {
            if (instance == null)
                instance = new BL_imp();
            return instance;
        }
    }

}
