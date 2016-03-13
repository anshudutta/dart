using System;
using System.Collections.Generic;
using System.Text;

namespace baileysoft.Wmi
{
    interface IWMI
    {
        IList<string> GetPropertyValues();
    }
}
