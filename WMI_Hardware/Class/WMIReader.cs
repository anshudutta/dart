using System;
using System.Collections.Generic;
using System.Management;

namespace baileysoft.Wmi
{
    class WMIReader
    {
        public static IList<string> GetPropertyValues(Connection wmiConnection,
                                                      string selectQuery,
                                                      string className)
        {
            var connectionScope = wmiConnection.GetConnectionScope;
            var alProperties = new List<string>();
            var msQuery = new SelectQuery(selectQuery);
            var searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);

            try
            {
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    foreach (string property in XMLConfig.GetSettings(className))
                    {
                        try { alProperties.Add(property + ": " + item[property]); }
                        catch (SystemException) { /* ignore error */ }
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }
            
            return alProperties;
        }
    }
}
