//******************************************************************************************************
//  FileProcessorIndex.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/05/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using GSF.Collections;
using GSF.IO;
using log4net;

namespace openXDA.Nodes.Types.FileProcessing
{
    public class FileProcessorIndex
    {
        #region [ Members ]

        // Nested Types

        private class FileGroupEntry
        {
            #region [ Members ]

            // Fields
            private FileInfo[] m_fileGroup;

            #endregion

            #region [ Constructors ]

            public FileGroupEntry()
                : this(new HashSet<string>())
            {
            }

            public FileGroupEntry(HashSet<string> filePaths)
            {
                FilePaths = filePaths;

                FileGroup = filePaths
                    .Select(filePath => new FileInfo(filePath))
                    .ToArray();
            }

            #endregion

            #region [ Properties ]

            public FileInfo[] FileGroup
            {
                get => Interlocked.CompareExchange(ref m_fileGroup, null, null);
                private set => Interlocked.Exchange(ref m_fileGroup, value);
            }

            private HashSet<string> FilePaths { get; }

            #endregion

            #region [ Methods ]

            public void Add(string filePath)
            {
                if (!File.Exists(filePath))
                    return;

                if (!FilePaths.Add(filePath))
                    return;

                FileInfo[] fileGroup = FileGroup;
                int size = fileGroup.Length;

                Array.Resize(ref fileGroup, size + 1);
                fileGroup[size] = new FileInfo(filePath);
                FileGroup = fileGroup;
            }

            public void Prune()
            {
                int count = FilePaths.RemoveWhere(path => !File.Exists(path));

                if (count == 0)
                    return;

                FileGroup = FileGroup
                    .Where(fileInfo => FilePaths.Contains(fileInfo.FullName))
                    .ToArray();
            }

            #endregion
        }

        #endregion

        #region [ Constructors ]

        public FileProcessorIndex(string fileGroupingPattern) =>
            FileGroupingRegex = new Regex(fileGroupingPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region [ Properties ]

        private Regex FileGroupingRegex { get; }

        private HashSet<string> DirectoryIndex { get; }
            = new HashSet<string>();

        private Dictionary<string, FileGroupEntry> FileGroupIndex { get; }
            = new Dictionary<string, FileGroupEntry>();

        #endregion

        #region [ Methods ]

        public void Scan(string directory)
        {
            if (!DirectoryIndex.Add(directory))
                return;

            void HandleException(Exception ex) =>
                Log.Error(ex.Message, ex);

            var groupings = FilePath
                .EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly, HandleException)
                .GroupBy(GetFileGroupKey);

            foreach (var grouping in groupings)
            {
                string key = grouping.Key;
                HashSet<string> fileGroup = new HashSet<string>(grouping);
                FileGroupIndex.Add(key, new FileGroupEntry(fileGroup));
            }
        }

        public FileGroupInfo Index(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            Scan(directory);

            string key = GetFileGroupKey(filePath);
            FileGroupEntry fileGroupEntry = FileGroupIndex.GetOrAdd(key, _ => new FileGroupEntry());
            fileGroupEntry.Add(filePath);
            fileGroupEntry.Prune();

            FileInfo[] GetFileGroup() => fileGroupEntry.FileGroup;
            FileGroupInfo fileGroupInfo = new FileGroupInfo(key, GetFileGroup);
            return fileGroupInfo;
        }

        private string GetFileGroupKey(string filePath)
        {
            Match match = FileGroupingRegex.Match(filePath);

            if (!match.Success)
                return null;

            return match.Groups["FileGroupKey"]?.Value;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileProcessorIndex));

        #endregion
    }
}
