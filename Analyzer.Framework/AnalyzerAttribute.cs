using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.Framework
{
    public class AnalyzerAttribute
    {
        /// <summary>
        /// Construtor never used, it's for future?
        /// </summary>
        public AnalyzerAttribute()
        {
            Initialize();
        }

        public AnalyzerAttribute(string path)
        {
            Path = path;
            Initialize();
            if (Utility.IsDrive(path))
                Type = ItemType.Drive;
            else if (Utility.IsDirectory(path))
                Type = ItemType.Directory;
            else
                Type = ItemType.File;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            MemoryList = new List<FileMemoryList>();
            InaccessibleList = new List<string>();
            TotalMemory = 0;
            FreeMemory = 0;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public ItemType Type { get; set; }

        public List<FileMemoryList> MemoryList { get; set; }
        public List<string> InaccessibleList { get; set; }
        public double UsedMemory { get; set; }
        public double FreeMemory { get; set; }

        public double TotalMemory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDetails()
        {
            try
            {
                var details = new Dictionary<string, string>();

                if (Type == ItemType.Drive)
                {
                    // Get Details for Drive
                    var drInfo = new DriveInfo(Path);

                    details.Add("Drive Type", drInfo.DriveType.ToString());
                    details.Add("Drive Format", drInfo.DriveFormat);
                    details.Add("Volume Label", drInfo.VolumeLabel);
                }
                else if (Type == ItemType.Directory)
                {
                    // Get details for Directory
                    var dirInfo = new DirectoryInfo(Path);

                    details.Add("Created", dirInfo.CreationTime.ToString("MM-dd-yyyy"));
                    details.Add("Last Accessed", dirInfo.LastAccessTime.ToString("MM-dd-yyyy"));
                    details.Add("Last Written", dirInfo.LastWriteTime.ToString("MM-dd-yyyy"));
                    details.Add("Root", dirInfo.Root.ToString());
                }
                else
                {
                    // Get details for file
                    var fileInfo = new FileInfo(Path);

                    details.Add("Created", fileInfo.CreationTime.ToString());
                    details.Add("Last Accessed", fileInfo.LastAccessTime.ToString());
                    details.Add("Last Written", fileInfo.LastWriteTime.ToString());
                    if (fileInfo.DirectoryName != null)
                        details.Add("Directory Name", fileInfo.DirectoryName);
                }

                return details;
            }
            catch (Exception ex)
            {
                // TODO: Error logger will be used
                return null;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void SortMemoryList()
        {
            var temp = new List<FileMemoryList>();
            foreach (FileMemoryList item in MemoryList)
            {
                if (item.Memory == 0)
                    InaccessibleList.Add(item.FileName);
                else
                    temp.Add(item);

                MemoryList = temp;
            }

            MemoryList.Sort();
            // bubble sort Memory List
            //for (int i = 0; i < MemoryList.Count; i++)
            //{
            //    for (int j = i + 1; j < MemoryList.Count - 1; j++)
            //    {
            //        if (MemoryList[i].Memory > MemoryList[j].Memory)
            //        {
            //            // swap
            //            string tempFileName = MemoryList[i].FileName;
            //            double tempMemory = MemoryList[i].Memory;
            //            MemoryList[i].FileName = MemoryList[j].FileName;
            //            MemoryList[i].Memory = MemoryList[j].Memory;
            //            MemoryList[j].FileName = tempFileName;
            //            MemoryList[j].Memory = tempMemory;
            //        }
            //    }
            //}


        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateTotalMemory()
        {
            if (Utility.IsAccessible(Path))
            {
                if (Type == ItemType.Drive)
                {
                    TotalMemory = new DriveInfo(Path).TotalSize / Utility.DivisionNumber;
                    FreeMemory = new DriveInfo(Path).TotalFreeSpace / Utility.DivisionNumber;
                    UsedMemory = TotalMemory - FreeMemory;
                }
                else
                {
                    UsedMemory = TotalMemory;
                }
            }

        }



        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<FileMemoryList> ArrangeMemoryList(int level)
        {
            // Minimum 10 % is always shown as "Others"

            CalculateTotalMemory();
            SortMemoryList();
            MemoryList.Reverse();
            int counter = 0;
            var temp = new List<FileMemoryList>();

            if (MemoryList.Count > 5)
            {
                double start = Math.Pow(TotalMemory, level);
                double end = Math.Pow(Convert.ToDouble(.1 * TotalMemory), level);
                double otherMemory = 0;

                foreach (FileMemoryList item in MemoryList)
                {
                    if (temp.Count <= 5)
                    {
                        temp.Add(new FileMemoryList
                                     {
                                         FileName = MemoryList[counter].FileName,
                                         Memory = MemoryList[counter].Memory
                                     });
                    }
                    else
                    {
                        if (item.Memory <= start && item.Memory >= end)
                        {
                            temp.Add(new FileMemoryList
                                         {
                                             FileName = MemoryList[counter].FileName,
                                             Memory = MemoryList[counter].Memory
                                         });
                        }
                        else
                            otherMemory += MemoryList[counter].Memory;
                    }
                    counter++;

                }

                temp.Add(new FileMemoryList { FileName = "Others", Memory = otherMemory });

            }
            else
            {
                for (int i = 0; i < MemoryList.Count; i++)
                {
                    temp.Add(new FileMemoryList
                                 {
                                     FileName = MemoryList[counter].FileName,
                                     Memory = MemoryList[counter].Memory
                                     //Memory = System.Math.Round((MemoryList[counter].Memory / TotalMemory) * 100,2)
                                 });

                    counter++;
                }
            }

            return temp;
        }
    }
}
