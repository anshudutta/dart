using System;
using System.Collections.Generic;
using System.Text;

namespace baileysoft.Wmi
{
    class Win32_PhysicalMedia : IWMI 
    {
             Connection WMIConnection;

        public Win32_PhysicalMedia(Connection WMIConnection)
        {
            this.WMIConnection = WMIConnection;
        }
        public IList<string> GetPropertyValues()
        {
            string className = System.Text.RegularExpressions.Regex.Match(
                                  this.GetType().ToString(), "Win32_.*").Value;

            return WMIReader.GetPropertyValues(WMIConnection,
                                               "SELECT * FROM " + className,
                                               className);
        }
    }
}
