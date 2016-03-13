using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analyzer.Framework
{
    public class DeviceIO
    {
        public DeviceIO(string path)
        {
            Path = path;
            if (System.IO.Path.GetFileName(path) == null ||
                System.IO.Path.GetFileName(path) == "")
                Name = path;
            else
                Name = System.IO.Path.GetFileName(path);
            Children = new List<DeviceIO>();

            if (Utility.IsDrive(path))
                ElementType = "Drive";
            else if (Utility.IsDirectory(path))
                ElementType = "Drirectory";
            else
                ElementType = "File";
            //this.Children = GetChildren(Path);
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public string ElementType { get; set; }

        public List<DeviceIO> Children { get; set; }

        public List<DeviceIO> GetChildren(string path)
        {
            try
            {
                //DeviceIO parent = new DeviceIO(Path) { Name = System.IO.Path.GetFileName(Path) };
                var children = new List<DeviceIO>();

                foreach (string name in Directory.GetDirectories(path))
                {
                    if (Utility.IsAccessible(name))
                        children.Add(new DeviceIO(name)
                        {
                            Name = System.IO.Path.GetFileName(name),
                            Path = path
                        });
                }

                // This is much more faster than the foreach variant
                // Tested with System.Diagnostics.Stopwatch
                // Number of childrens = 43
                // Foreach :
                // Seconds: 00:00:00.0059549
                // Ticks: 11602
                //
                // LINQ:
                // Seconds: 00:00:00.0030437
                // Ticks: 5930
                children.AddRange(Directory.GetFiles(path).Select(file => new DeviceIO(file)
                                                                              {
                                                                                  Name = System.IO.Path.GetFileName(file),
                                                                                  Path = path
                                                                              }));
                return children;
            }
            catch (Exception ex)
            {
                // TODO: Error logger will be used
            }
            return null;
        }

    }
}
