//******************************************************************************************************
//  FTTImageGenerator.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/10/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using GSF;
using GSF.Data;
using GSF.IO;
using log4net;
using Random = GSF.Security.Cryptography.Random;

namespace FaultData.DataWriters.GTC
{
    public class FTTImageGenerator
    {
        #region [ Constructors ]

        public FTTImageGenerator(FTTOptions options)
        {
            CLIPath = options.CLIPath;
            URL = options.URL;
            QueryStringFormat = options.QueryStringFormat;
            QueryTimeout = options.QueryTimeout;
            IgnoreCertificateErrors = options.IgnoreCertificateErrors;

            ImageWidth = options.ImageWidth;
            ImageHeight = options.ImageHeight;
        }

        #endregion

        #region [ Properties ]

        private string CLIPath { get; }
        private string URL { get; }
        private string QueryStringFormat { get; }
        private TimeSpan QueryTimeout { get; }
        public bool IgnoreCertificateErrors { get; }

        private int ImageWidth { get; }
        private int ImageHeight { get; }

        #endregion

        #region [ Methods ]

        public Stream QueryToImageStream(params FTTRecord[] records) =>
            QueryToImageStream(records.AsEnumerable());

        public Stream QueryToImageStream(IEnumerable<FTTRecord> records)
        {
            UriBuilder builder = new UriBuilder(URL);

            IEnumerable<string> queryParts = new[] { builder.Query.TrimStart('?') }
                .Concat(records.Select(GetQueryString))
                .Where(queryPart => !string.IsNullOrEmpty(queryPart));

            builder.Query = string.Join("&", queryParts);

            string url = builder.ToString();
            return QueryToImageStreamAsync(url).GetAwaiter().GetResult();
        }

        private async Task<Stream> QueryToImageStreamAsync(string url)
        {
            StringBuilder outputData = new StringBuilder();
            StringBuilder errorData = new StringBuilder();

            try
            {
                string filePath = await Execute(url, outputData, errorData);
                MemoryStream memoryStream = new MemoryStream();

                using (FileStream fileStream = File.OpenRead(filePath))
                    await fileStream.CopyToAsync(memoryStream);

                File.Delete(filePath);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                return memoryStream;
            }
            catch (Exception ex)
            {
                string stdout = outputData.ToString().Trim();
                string stderr = errorData.ToString().Trim();

                string message = new StringBuilder()
                    .AppendLine("Standard Output:")
                    .AppendLine(stdout)
                    .AppendLine()
                    .AppendLine("Standard Error:")
                    .AppendLine(stderr)
                    .ToString();

                throw new Exception(message, ex);
            }
        }

        private async Task<string> Execute(string url, StringBuilder outputData, StringBuilder errorData)
        {
            string Escape(string cliArgument) =>
                Regex.Replace(cliArgument, @"(\\*)""", @"$1$1\""");

            string filePath = GenerateFilePath();

            using (Process process = new Process())
            {
                string[] fttArguments =
                {
                    Escape(url).QuoteWrap(),
                    IgnoreCertificateErrors.ToString(CultureInfo.InvariantCulture),
                    ImageWidth.ToString(CultureInfo.InvariantCulture),
                    ImageHeight.ToString(CultureInfo.InvariantCulture),
                    QueryTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture),
                    Escape(filePath).QuoteWrap()
                };

                process.StartInfo.FileName = CLIPath;
                process.StartInfo.Arguments = string.Join(" ", fttArguments);
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.EnableRaisingEvents = true;

                process.OutputDataReceived += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(args.Data))
                        return;

                    Log.Debug($"[FTT] {args.Data}");
                    outputData.AppendLine(args.Data);
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(args.Data))
                        return;

                    Log.Error($"[FTT] ERROR: {args.Data}");
                    errorData.AppendLine(args.Data);
                };

                TaskCreationOptions runContinuationsAsynchronously = TaskCreationOptions.RunContinuationsAsynchronously;
                TaskCompletionSource<object> exitPromise = new TaskCompletionSource<object>(runContinuationsAsynchronously);
                process.Exited += (sender, args) => exitPromise.SetResult(null);

                Log.Debug($"[FTT] Executing: \"{Escape(process.StartInfo.FileName)}\" {process.StartInfo.Arguments}");
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await exitPromise.Task;

                return filePath;
            }
        }

        private string GetQueryString(FTTRecord record, int index)
        {
            var parameters = new
            {
                index,
                station = record.StationName,
                line = record.LineKey,
                distance = record.Distance,
                eventTime = record.EventTime
            };

            return QueryStringFormat.Interpolate(parameters);
        }

        private string GenerateFilePath()
        {
            Environment.SpecialFolder programData = Environment.SpecialFolder.CommonApplicationData;
            string programDataPath = Environment.GetFolderPath(programData);
            string folderPath = Path.Combine(programDataPath, "FTTAPI", "Output");
            Directory.CreateDirectory(folderPath);

            byte[] buffer = new byte[5];
            Random.GetBytes(buffer);

            string ToHex(byte b) =>
                Convert.ToString(b, 16);

            string hex = string.Join("", buffer.Select(ToHex));
            string fileName = $"{hex}.jpg";
            return Path.Combine(folderPath, fileName);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FTTImageGenerator));

        // Static Methods
        public static Stream ConvertToFTTImageStream(AdoDataConnection connection, XElement fttElement)
        {
            string cliPathSetting = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'FTTInterop.CLIPath'");
            string queryTimeoutSetting = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'FTTInterop.QueryTimeout'");

            string cliPath = FilePath.GetAbsolutePath(cliPathSetting);

            TimeSpan queryTimeout = int.TryParse(queryTimeoutSetting, out int queryTimeoutSeconds)
                ? TimeSpan.FromSeconds(queryTimeoutSeconds)
                : TimeSpan.FromSeconds(60);

            string url = (string)fttElement.Attribute("url");
            string queryStringFormat = (string)fttElement.Attribute("queryStringFormat");
            string ignoreCertificateErrors = (string)fttElement.Attribute("ignoreCertificateErrors");
            string fttWidth = (string)fttElement.Attribute("width");
            string fttHeight = (string)fttElement.Attribute("height");

            if (!int.TryParse(fttWidth, out int imageWidth))
                throw new FormatException($"FTT width '{fttWidth}' is not an integer.");

            if (!int.TryParse(fttHeight, out int imageHeight))
                throw new FormatException($"FTT height '{fttHeight}' is not an integer.");

            FTTOptions options = new FTTOptions();
            options.CLIPath = cliPath;
            options.QueryTimeout = queryTimeout;
            options.URL = url;
            options.QueryStringFormat = queryStringFormat;
            options.IgnoreCertificateErrors = ignoreCertificateErrors.ParseBoolean();
            options.ImageWidth = imageWidth;
            options.ImageHeight = imageHeight;

            List<FTTRecord> fttRecords = fttElement
                .Elements("fttRecord")
                .Select(FTTRecord.ToRecord)
                .ToList();

            foreach (FTTRecord record in fttRecords)
                Validate(record);

            FTTImageGenerator generator = new FTTImageGenerator(options);
            return generator.QueryToImageStream(fttRecords);
        }

        private static void Validate(FTTRecord record)
        {
            if (!double.TryParse(record.Distance, out double _))
                throw new FormatException($"FTT distance '{record.Distance}' is not a number.");

            if (!DateTime.TryParse(record.EventTime, out DateTime _))
                throw new FormatException($"FTT eventTime '{record.EventTime}' is not a valid date/time.");
        }

        #endregion
    }
}
