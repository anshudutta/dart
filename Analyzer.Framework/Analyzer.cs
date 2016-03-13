using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Analyzer.Framework
{
    public class Analyzer
    {
        private static Analyzer _analyzer;
        private static readonly object SyncLock = new object();

        private Analyzer()
        {
            Utility.DivisionNumber = (1);
        }

        public static Analyzer GetInstance()
        {
            lock (SyncLock)
            {
                if (_analyzer == null)
                    _analyzer = new Analyzer();
            }
            return _analyzer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPathName"></param>
        /// <returns></returns>
        public AnalyzerAttribute GetMemory(string fullPathName)
        {
            try
            {
                if (Utility.IsAccessible(fullPathName))
                {
                    var dirInfo = new DirectoryInfo(fullPathName);
                    var attr = new AnalyzerAttribute(fullPathName);

                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        attr.MemoryList.Add(new FileMemoryList
                                                {
                                                    FileName = file.Name,
                                                    Memory = file.Length / Utility.DivisionNumber
                                                });
                        attr.TotalMemory += file.Length / Utility.DivisionNumber;
                    }

                    foreach (string fileName in Directory.GetDirectories(fullPathName))
                    {
                        double memory = GetMemory(fileName).TotalMemory;                        
                        attr.MemoryList.Add(new FileMemoryList()
                        {
                            FileName = fileName,                            
                            Memory = memory
                        });
                        attr.TotalMemory += memory;                       
                    }                                       
                    return attr;
                }
                return new AnalyzerAttribute(fullPathName)
                           {
                               MemoryList = new List<FileMemoryList> { new FileMemoryList { FileName = fullPathName, Memory = 0 } }
                           };
            }
            catch (Exception)
            {
                // TODO: Error logger will be used
            }
            return null;
        }

        public DataTable GetFolderGroups(string folderPath)
        {
            var aclTable = new DataTable();
            if (folderPath != null)
            {
                DataColumn[] dc = { new DataColumn("Identity"), new DataColumn("Control"), new DataColumn("Rights") };
                aclTable.Columns.AddRange(dc);
                FileSecurity fs = File.GetAccessControl(folderPath);
                AuthorizationRuleCollection arc = fs.GetAccessRules(true, true, typeof(NTAccount));

                foreach (FileSystemAccessRule fsar in arc)
                {

                    //ignore everyone
                    if (fsar.IdentityReference.Value.ToLower() == "everyone")
                        continue;
                    //ignore BUILTIN
                    if (fsar.IdentityReference.Value.ToLower().StartsWith("builtin"))
                        continue;
                    if (fsar.IdentityReference.Value.ToUpper() == @"NT AUTHORITY\SYSTEM")
                        continue;

                    DataRow row = aclTable.NewRow();

                    string group = fsar.IdentityReference.Value;
                    int nindex = group.IndexOf('\\');
                    if (nindex > 0)
                    {

                        row["Identity"] = group.Substring(nindex + 1, group.Length - nindex - 1);
                        //Debug.WriteLine(row["Identity"]);

                        row["Control"] = fsar.AccessControlType.ToString();
                        //Debug.WriteLine(row["AcceptControlType"]);

                        row["Rights"] = fsar.FileSystemRights.ToString();
                        //Debug.WriteLine(row["FileSystemRights"]);

                        aclTable.Rows.Add(row);
                    }
                }
            }

            return aclTable;

        }



    }
}
