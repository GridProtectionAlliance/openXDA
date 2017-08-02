using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GSF;
using GSF.IO;
using GSF.IO.Checksums;

namespace WixFolderGen
{
    class Solution
    {
        public string ProjectName { get; set; }
        public string SolutionName { get; set; }
        public string RootFolderName { get; set; }
        public string SolutionRelativeRootFolder { get; set; }
        public string ApplicationFolder { get; set; }
        public int MaxWixIDLength { get; set; }
        public string ApplicationPath { get; set; }
        public string SourceFolder { get; set; }
        public string WebFeaturesDestinationFile { get; set; }
        public string WebFoldersDestinationFile { get; set; }
        public string WebFilesDestinationFile { get; set; }
        public string FileNamePostfix { get; set; }

        public Solution(string projectName, string rootFolderName, string applicationFolder, string fileNamePostfix)
        {
            ProjectName = projectName;
            RootFolderName = rootFolderName;
            ApplicationFolder = applicationFolder;
            SolutionRelativeRootFolder = applicationFolder + "\\" + ProjectName + "\\" + RootFolderName;
            MaxWixIDLength = 72;
            ApplicationPath = "..\\..\\..\\..\\..\\Source\\" + applicationFolder;
            SourceFolder = ApplicationPath + "\\" + ProjectName + "\\" + RootFolderName;
            FileNamePostfix = fileNamePostfix;
            WebFeaturesDestinationFile = "..\\..\\..\\..\\..\\Source\\Applications\\openXDA\\openXDASetup\\WebFeatures"+fileNamePostfix +".wxi";
            WebFoldersDestinationFile = "..\\..\\..\\..\\..\\Source\\Applications\\openXDA\\openXDASetup\\WebFolders" + fileNamePostfix + ".wxi";
            WebFilesDestinationFile = "..\\..\\..\\..\\..\\Source\\Applications\\openXDA\\openXDASetup\\WebFiles" + fileNamePostfix + ".wxi";
        }

    }
    class Program
    {
        static void Main()
        {
            List<Solution> solutions = new List<Solution>()
            {
                new Solution("openXDA", "wwwroot", "Applications\\openXDA", ""),
                new Solution("XDAAlarmCreationApp", "wwwrootXDAAlarm", "Tools", "XDAAlarm")
            };

            foreach(Solution solution in solutions)
            {
                List<string> folderList = GetFolderList(solution.SourceFolder);
                List<string> componentGroupRefTags = GetComponentRefTags(solution, folderList);
                List<string> directoryTags = GetDirectoryTags(solution, folderList);
                List<string> componentGroupTags = GetComponentGroupTags(solution, folderList);

                using (FileStream stream = File.Create(solution.WebFeaturesDestinationFile))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("<Include>");
                    writer.WriteLine("<Feature Id=\"WebFilesFeature" + solution.FileNamePostfix + "\" Title=\"Web Files\" Description=\"Web Files\">");

                    foreach (string tag in componentGroupRefTags)
                        writer.WriteLine("  " + tag);

                    writer.WriteLine("</Feature>");
                    writer.WriteLine("</Include>");
                }

                using (FileStream stream = File.Create(solution.WebFoldersDestinationFile))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("<Include>");
                    writer.WriteLine($"<Directory Id=\"{GetDirectoryID(solution, solution.RootFolderName)}\" Name=\"{solution.RootFolderName}\">");

                    foreach (string tag in directoryTags)
                        writer.WriteLine("  " + tag);

                    writer.WriteLine("</Directory>");
                    writer.WriteLine("</Include>");
                }

                using (FileStream stream = File.Create(solution.WebFilesDestinationFile))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("<Include>");

                    foreach (string tag in componentGroupTags)
                        writer.WriteLine(tag);

                    writer.WriteLine("</Include>");
                }

            }
        }

        private static List<string> GetFolderList(string path)
        {
            List<string> folderList = new List<string>();
            BuildFolderList(folderList, path, string.Empty);
            return folderList;
        }

        private static List<string> GetComponentRefTags(Solution solution, List<string> folderList)
        {
            return new[] { "" }
                .Concat(folderList)
                .Select(folder => $"<ComponentGroupRef Id=\"{GetComponentGroupID(solution, folder)}\" />")
                .ToList();
        }

        private static List<string> GetDirectoryTags(Solution solution, List<string> folderList)
        {
            List<string> directoryTags = new List<string>();

            List<string[]> brokenFolderList = folderList
                .Select(FilePath.RemovePathSuffix)
                .Select(folder => folder.Split(Path.DirectorySeparatorChar))
                .ToList();

            BuildDirectoryTags(solution, directoryTags, brokenFolderList, 0);

            return directoryTags;
        }

        private static List<string> GetComponentGroupTags(Solution solution, List<string> folderList)
        {
            List<string> componentGroupTags = new List<string>();

            componentGroupTags.Add($"<ComponentGroup Id=\"{GetComponentGroupID(solution, null)}\" Directory=\"{GetDirectoryID(solution, solution.RootFolderName)}\">");

            foreach (string file in Directory.EnumerateFiles(solution.SourceFolder))
            {
                string fileName = FilePath.GetFileName(file);
                string fileSource = $"$(var.SolutionDir){Path.Combine(solution.SolutionRelativeRootFolder, fileName)}";

                componentGroupTags.Add($"  <Component Id=\"{GetComponentID(solution, fileName)}\">");
                componentGroupTags.Add($"    <File Id=\"{GetFileID(solution, fileName)}\" Name=\"{fileName}\" Source=\"{fileSource}\" />");
                componentGroupTags.Add("  </Component>");
            }

            componentGroupTags.Add("</ComponentGroup>");

            foreach (string folder in folderList)
            {
                componentGroupTags.Add($"<ComponentGroup Id=\"{GetComponentGroupID(solution, folder)}\" Directory=\"{GetDirectoryID(solution, folder)}\">");

                foreach (string file in Directory.EnumerateFiles(Path.Combine(solution.SourceFolder, folder)))
                {
                    string fileName = FilePath.GetFileName(file);

                    if (!fileName.Equals("thumbs.db", System.StringComparison.OrdinalIgnoreCase))
                    {
                        string fileID = folder + "_" + fileName;
                        string fileSource = $"$(var.SolutionDir){Path.Combine(solution.SolutionRelativeRootFolder, folder, fileName)}";

                        componentGroupTags.Add($"  <Component Id=\"{GetComponentID(solution, fileID)}\">");
                        componentGroupTags.Add($"    <File Id=\"{GetFileID(solution, fileID)}\" Name=\"{fileName}\" Source=\"{fileSource}\" />");
                        componentGroupTags.Add("  </Component>");
                    }
                }

                componentGroupTags.Add("</ComponentGroup>");
            }

            return componentGroupTags;
        }

        private static void BuildFolderList(List<string> folderList, string path, string rootPath)
        {
            string name;

            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                name = FilePath.AddPathSuffix(rootPath + FilePath.GetLastDirectoryName(FilePath.AddPathSuffix(folder)));
                folderList.Add(name);
                BuildFolderList(folderList, folder, name);
            }
        }

        private static void BuildDirectoryTags(Solution solution, List<string> directoryTags, List<string[]> folderList, int level)
        {
            List<IGrouping<string, string[]>> groupings = folderList
                .Where(folder => folder.Length > level)
                .GroupBy(folder => string.Join(Path.DirectorySeparatorChar.ToString(), folder.Take(level + 1)))
                .OrderBy(grouping => grouping.Key)
                .ToList();

            foreach (IGrouping<string, string[]> grouping in groupings)
            {
                if (grouping.Count() == 1)
                {
                    string[] folder = grouping.First();
                    string name = FilePath.GetLastDirectoryName(FilePath.AddPathSuffix(string.Join(Path.DirectorySeparatorChar.ToString(), folder)));

                    directoryTags.Add($"{new string(' ', level * 2)}<Directory Id=\"{GetDirectoryID(solution, string.Join("", folder))}\" Name=\"{name}\" />");
                }
                else
                {
                    List<string[]> subfolderList = grouping
                        .Where(folder => folder.Length > level + 1)
                        .ToList();

                    string name = FilePath.GetLastDirectoryName(FilePath.AddPathSuffix(grouping.Key));

                    directoryTags.Add($"{new string(' ', level * 2)}<Directory Id=\"{GetDirectoryID(solution, grouping.Key)}\" Name=\"{name}\">");
                    BuildDirectoryTags(solution, directoryTags, subfolderList, level + 1);
                    directoryTags.Add($"{new string(' ', level * 2)}</Directory>");
                }
            }
        }

        private static string GetCleanID(string path, int limit,
            bool replaceDot = false,
            bool removeDot = false,
            bool replaceDirectorySeparatorChar = false,
            bool removeDirectorySeparatorChar = false,
            bool replaceSpaces = false,
            bool removeSpaces = false,
            bool removeUnderscores = false)
        {
            if (replaceDot)
                path = path.Replace('.', '_');

            if (removeDot)
                path = path.Replace(".", "");

            if (replaceDirectorySeparatorChar)
                path = path.Replace(Path.DirectorySeparatorChar, '_');

            if (removeDirectorySeparatorChar)
                path = path.Replace(Path.DirectorySeparatorChar.ToString(), "");

            if (replaceSpaces)
                path = path.ReplaceWhiteSpace('_');

            if (removeSpaces || !replaceSpaces)
                path = path.RemoveWhiteSpace();

            path = new Regex("[^a-zA-Z0-9_.]").Replace(path, "_");

            path = path.RemoveDuplicates("_").TrimEnd('_').TrimStart('_');

            if (removeUnderscores)
                path = path.Replace("_", "");

            if (path.Length > limit)
            {
                byte[] nameBytes = Encoding.Default.GetBytes(path);
                uint crc = nameBytes.Crc32Checksum(0, nameBytes.Length);
                string suffix = crc.ToString();
                path = path.Substring(0, limit - suffix.Length) + suffix;
            }

            return path;
        }

        private static string GetDirectoryID(Solution solution, string folderName)
        {
            string Prefix = solution.RootFolderName + "_";
            const string Suffix = "FOLDER";
            return Prefix + GetCleanID(folderName, solution.MaxWixIDLength - Suffix.Length, removeDirectorySeparatorChar: true, removeUnderscores: true).ToUpperInvariant() + Suffix;
        }

        private static string GetComponentGroupID(Solution solution, string folderName)
        {
            string Prefix = solution.RootFolderName + "_";
            string Suffix = "_Components";

            if (string.IsNullOrWhiteSpace(folderName))
                return solution.RootFolderName + Suffix;

            return Prefix + GetCleanID(folderName, solution.MaxWixIDLength - Prefix.Length - Suffix.Length, replaceDirectorySeparatorChar: true, replaceSpaces: true) + Suffix;
        }

        private static string GetComponentID(Solution solution,string fileName)
        {
            string Prefix = solution.RootFolderName + "_";
            string suffix = FilePath.GetExtension(fileName).Replace('.', '_');
            return Prefix + GetCleanID(FilePath.GetDirectoryName(fileName) + FilePath.GetFileNameWithoutExtension(fileName), solution.MaxWixIDLength - suffix.Length, removeSpaces: true, replaceDot: true) + suffix;
        }

        private static string GetFileID(Solution solution,string fileName)
        {
            string Prefix = solution.RootFolderName + "_";
            string suffix = FilePath.GetExtension(fileName);
            return Prefix + GetCleanID(FilePath.GetDirectoryName(fileName) + FilePath.GetFileNameWithoutExtension(fileName), solution.MaxWixIDLength - suffix.Length, removeSpaces: true) + suffix;
        }
    }
}
