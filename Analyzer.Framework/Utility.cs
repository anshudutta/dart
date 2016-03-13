using System;
using System.Xml.Serialization;
using System.IO;

namespace Analyzer.Framework
{
    public static class Utility
    {
        public static double DivisionNumber;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsAccessible(string fileName)
        {
            try
            {
                var dirInfo = new DirectoryInfo(fileName);
                dirInfo.GetFiles();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Error logger will be used
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static bool IsDirectory(string pathName)
        {
            try
            {
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(pathName);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return true;
                return false;
            }
            catch (Exception)
            {
                // TODO: Error logger will be used
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static bool IsDrive(string pathName)
        {
            try
            {
                var d = new DirectoryInfo(pathName);
                if (d.Parent == null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                // TODO: Error logger will be used
            }
            return false;
        }
        public static bool Serialize(object obj, Type type, string fileName)
        {
            try
            {
                TextWriter textWriter = new StreamWriter(@fileName);
                var serializer = new XmlSerializer(type);
                serializer.Serialize(textWriter, obj);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }



    public enum ItemType
    {
        Drive,
        Directory,
        File
    }
}
