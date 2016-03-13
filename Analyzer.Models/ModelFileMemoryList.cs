using Analyzer.Framework;

namespace Analyzer.Models
{
    public class ModelFileMemoryList : FileMemoryList
    {
        private string myType;

        public ModelFileMemoryList(string path)
        {
            this.Path = path;
        }
        public string ItemMemory { get; set; }
        public string Path { get; set; }

        public string MyType
        {
            get
            {
                if (FileName != null)
                {
                    if (System.IO.Path.HasExtension(FileName))
                    {
                        return System.IO.Path.GetExtension(FileName);
                    }
                    if (Utility.IsDrive(FileName))
                        return "Drive";
                    return Utility.IsDirectory(FileName) ? "Directory" : "file";
                }
                return "Undefined";
            }
            set
            {
                myType = value;
            }
        }
    }
}