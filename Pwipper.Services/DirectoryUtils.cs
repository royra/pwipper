using System;
using System.IO;

namespace Pwipper.Services
{
    public static class DirectoryUtils
    {
        public class PathNotFoundException : Exception
        {
            public PathNotFoundException(string message)
                : base(message)
            {
            }
        }

        public static string SearchUp(string targetPath, DirectoryInfo startFromDirectory = null, bool throwIfNotExists = false)
        {
            startFromDirectory = startFromDirectory ?? new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var path = Path.Combine(startFromDirectory.FullName, targetPath);

            if (File.Exists(path))
                return path;

            if (startFromDirectory.Parent == null)
            {
                if (throwIfNotExists)
                    throw new PathNotFoundException(string.Format("Could not find {0}", targetPath));

                return null;
            }

            return SearchUp(targetPath, startFromDirectory.Parent, throwIfNotExists);
        }
    }
}