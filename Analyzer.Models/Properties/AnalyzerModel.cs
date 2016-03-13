using System;
using System.Text;
using Analyzer.Framework;
using System.Collections.Generic;
using System.Data;

namespace Analyzer.Models
{
    public class AnalyzerModel
    {
        private Framework.Analyzer _analyzer;
        private AnalyzerAttribute _attr;

        public AnalyzerModel(string path)
        {
            _analyzer = Analyzer.Framework.Analyzer.GetInstance();
            _attr = _analyzer.GetMemory(Path);
            MemoryList = new List<ModelFileMemoryList>();

            Initialize();
        }

        private void Initialize()
        {
            //foreach (FileMemoryList item in _attr.MemoryList)
            foreach (FileMemoryList item in _attr.ArrangeMemoryList(1))
            {
                MemoryList.Add(
                    new ModelFileMemoryList(_attr.Path)
                    {
                        FileName = item.FileName,
                        ItemMemory = DefineMemory(item.Memory)
                        // MyType = item.MyType
                    });
            }

            Path = _attr.Path;
            Name = _attr.Name;
            ElementType = _attr.Type.ToString();
            TotalMemory = DefineMemory(_attr.TotalMemory);
            UsedMemory = DefineMemory(_attr.UsedMemory);
            History = _attr.GetDetails();
            InaccssibleList = _attr.InaccessibleList;

            AccessList = _analyzer.GetFolderGroups(Path);
        }

        public string Path { get; set; }
        public string Name { get; set; }
        public string ElementType { get; set; }
        public string TotalMemory { get; set; }
        public string UsedMemory { get; set; }
        public string FreeMemory { get; set; }
        public DataTable AccessList { get; set; }

        public Dictionary<string, string> History { get; set; }
        public List<string> InaccssibleList { get; set; }
        public List<ModelFileMemoryList> MemoryList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        private string DefineMemory(double memory)
        {
            try
            {
                int count = 1;
                string unit;
                do
                {
                    memory = Math.Round(memory / Math.Pow(1024, count), 2);
                    count++;

                } while (Math.Round(memory / 1024, 2) > 1);

                switch (count)
                {
                    case 0:
                        unit = "Bytes";
                        break;
                    case 1:
                        unit = "KB";
                        break;
                    case 2:
                        unit = "MB";
                        break;
                    default:
                        unit = "GB";
                        break;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(memory);
                stringBuilder.Append(" ");
                stringBuilder.Append(unit);
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                // TODO: Error logger will be used
            }
            return string.Empty;
        }

        public bool Serialize(object obj, Type type, string fileName)
        {
            return Utility.Serialize(obj, type, fileName);
        }
    }
}
