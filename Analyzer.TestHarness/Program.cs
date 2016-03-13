using System;
using System.Management;
using Analyzer.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            //Analyzer.Framework.Analyzer analyzer = Analyzer.Framework.Analyzer.GetInstance();

            //AnalyzerAttribute attr = analyzer.GetMemory(@"C:\Users");

            //foreach (FileMemoryList item in attr.MemoryList)
            //{
            //    Console.WriteLine("{0}, {1}", item.FileName, item.Memory);
            //}

            //Console.ReadLine();
            //List<FileMemoryList> list = attr.ArrangeMemoryList(1);
            //foreach (FileMemoryList item in list)
            //{
            //    Console.WriteLine("{0}, {1}", item.FileName, item.Memory);
            //}

            //Console.WriteLine("Path : {0}", attr.Path);
            ////Console.WriteLine("Path : {0}", attr.Name);
            //Console.WriteLine("Type : {0}", attr.Type);
            //Console.WriteLine("Total Memory : {0}", attr.TotalMemory);
            //Console.WriteLine("Used Memory : {0}", attr.UsedMemory);
            //Console.WriteLine("Free Memory : {0}", attr.FreeMemory);

            //Console.ReadLine();

            WqlObjectQuery objectQuery = new WqlObjectQuery("select * from win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(objectQuery);
            string s, os;
            foreach (ManagementObject MO in searcher.Get())
            {
                //s = MO["name"].ToString();
                //string[] split1 = s.Split('|');
                //os = split1[0];
                Console.WriteLine(MO.ToString());
            }

            Console.ReadLine();
        }
      
    }
}
