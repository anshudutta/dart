using System;

namespace Analyzer.Framework
{
    public class FileMemoryList : IComparable<FileMemoryList>
    {
        /// <summary>
        /// This empty constructor is redundant
        /// Needed for future plans ?
        /// </summary>
        public FileMemoryList()
        { }


        public string FileName { get; set; }
        public double Memory { get; set; }

        public int CompareTo(FileMemoryList other)
        {
            return Memory.CompareTo(other.Memory);
        }
    }
}