using System;
using System.Management;

namespace baileysoft.Wmi
{
    class Connection
    {
        ManagementScope _connectionScope;
        ConnectionOptions _options;

        #region "properties"
        public ManagementScope GetConnectionScope
        {
            get { return _connectionScope; }
        }
        public ConnectionOptions GetOptions
        {
            get { return _options; }
        }
        #endregion
              
        #region "static helpers"
        public static ConnectionOptions SetConnectionOptions()
        {
            var options = new ConnectionOptions
                              {
                                  Impersonation = ImpersonationLevel.Impersonate,
                                  Authentication = AuthenticationLevel.Default,
                                  EnablePrivileges = true
                              };
            return options;
        }

        public static ManagementScope SetConnectionScope(string machineName,
                                                   ConnectionOptions options)
        {
            var connectScope = new ManagementScope
                                   {Path = new ManagementPath(@"\\" + machineName + @"\root\CIMV2"), Options = options};

            try
            {
                connectScope.Connect();
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
            return connectScope;
        }
        #endregion

        #region "constructors"
        public Connection()
        {
            EstablishConnection(null, null, null, Environment.MachineName);
        }

        public Connection(string userName,
                          string password,
                          string domain,
                          string machineName)
        {
            EstablishConnection(userName, password, domain, machineName);
        }
        #endregion

        #region "private helpers"
        private void EstablishConnection(string userName, string password, string domain, string machineName)
        {
            _options = SetConnectionOptions();
            if (domain != null || userName != null)
            {
                _options.Username = domain + "\\" + userName;
                _options.Password = password;
            }
            _connectionScope = SetConnectionScope(machineName, _options);
        }
        #endregion
      
   }
}
