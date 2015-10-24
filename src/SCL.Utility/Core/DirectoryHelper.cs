using System;
using System.IO;

namespace Utility.Core
{
    public class DirectoryHelper
    {
        private const string APPLICATION_DIRECTORY = "YOUR_DIRECTORY_NAME";

        public static DirectoryInfo GetPyramidAppDirectory()
        {
            var directoryInfo =
                new DirectoryInfo(
                    StringHelper.EnsureDirectorySeperatorAtEnd(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + "Pyramid");
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            return directoryInfo;
        }
    }
}