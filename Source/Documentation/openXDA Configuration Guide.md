# openXDA Configuration

# Service Configuration

Apart from the connection string in the **openXDA.exe.config** file, configuration options for the service are located in the database in a table called **Setting**. These are the configuration options that can be defined in the Setting table.

- **DbTimeout**
  - Default: 120
  - Amount of time each database query is given to complete, in seconds.

- **WatchDirectories**
  - Default: Watch
  - Semi-colon separated list of directories where fault records can be discovered by the service.

- **ResultsPath**
  - Default: Results
  - Directory to which the results of fault analysis will be written.

- **FilePattern**
  - Default: (?<AssetKey>[^\\]+)\\[^\\]+$
  - Regular expression pattern that defines how files are associated with their meters.
  - A capture group for **AssetKey** must be specified.
  - See [http://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx](http://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx) for more information about regular expression in .NET.

- **DefaultMeterTimeZone**
  - Default: UTC
  - The time zone identifier for the time zone used by meters in the system unless explicitly configured otherwise.
  - See [https://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx](https://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx) for the list of Windows time zone identifiers.


- **XDATimeZone**
  - Default: Local time zone
  - The time zone identifier for the time zone used by openXDA when storing time data in the openXDA database.
  - Supplying an empty string (which is the default value for this setting) will default to the local time zone of the system on which openXDA is running.

- **WaitPeriod**
  - Default: 10.0
  - The amount of time, in seconds, between the time a file is processed by the system and the time an email should be produced by the system.
  - Increasing the wait period will allow openXDA more time to gather details about an event from multiple meters and provide a single email result.
  - Shortening the wait period will decrease the amount of time between the even and the email notification, but may result in redundancy between emails when new information arrives from another meter.

- **TimeTolerance**
  - Default: 0.5
  - The maximum distance, in seconds, between a meter's clock and real time.
  - Adjust this parameter to increase or decrease the tolerance used by openXDA when aligning events that occurred at approximately the same time. The system will attempt to determine whether two events could have occurred at the same time based on their relative positions in time, taking into account this time tolerance.

- **MaxTimeOffset**
  - Default: 0.0
  - The maximum number of hours beyond the current system time before the time of the event record indicates that the data is unreasonable.
  - Events with data that is considered unreasonable will be excluded from fault analysis.
  - The default value of 0.0 disables this setting so that openXDA processes files regardless of how far in the future the timestamps are.


- **MinTimeOffset**
  - Default: 0.0
  - The maximum number of hours prior to the current system time before the time of the record indicates that the data is unreasonable.
  - Events with data that is considered unreasonable will be excluded from fault analysis.
  - The default value of 0.0 disables this setting so that openXDA processes files regardless of how old the timestamps are.

- **MaxFileDuration**
  - Default: 0.0
  - The maximum duration, in seconds, of the files processed by openXDA. Files with a larger duration will be skipped by openXDA's file processing engine.
  - The default value, 0.0, disables this setting so that openXDA processes all files regardless of the duration.

- **MaxFileCreationTimeOffset**
  - Default: 0.0
  - The maximum number of hours prior to the current system time before the file creation time indicates that the data should not be processed.
  - Use this setting when the file creation time closely coincides with the time of the data in the file to automatically skip old files and not process them.
  - The default value, 0.0, disables this setting so that openXDA processes all files regardless of the creation time.

- **SystemFrequency**
  - Default: 60.0
  - The frequency, in Hz, of the electrical system being analyzed by openXDA.

- **MaxVoltage**
  - Default: 2.0
  - The per-unit threshold at which the voltage exceeds engineering reasonableness.
  - Events with data that exceeds engineering reasonableness will be excluded from fault analysis.

- **MaxCurrent**
  - Default: 1000000.0
  - The threshold, in amps, at which the current exceeds engineering reasonableness.
  - Events with data that exceeds engineering reasonableness will be excluded from fault analysis.


- **FaultLocation.PrefaultTrigger**
  - Default: 5.0
  - The threshold at which the ratio between RMS current and prefault RMS current indicates faulted conditions.
  - If the ratio exceeds this threshold, the cycle is considered to have been recorded during a fault, but only if the fault suppression algorithm indicates that there is no reason to believe otherwise.

- **FaultLocation.MaxFaultDistanceMultiplier**
  - Default: 1.05
  - The multiplier applied to the line length to determine the maximum value allowed for fault distance before the results are considered invalid.

- **FaultLocation.MinFaultDistanceMultiplier**
  - Default: -0.05
  - The multiplier applied to the line length to determine the minimum value allowed for fault distance before the results are considered invalid.

- **Breakers.OpenBreakerThreshold**
  - Default: 20.0
  - The maximum current, in amps, at which the breaker can be considered open.
  - This threshold is compared against the amplitude (peak) of the sine wave fitted to each cycle of the current waveform using linear regression. A flat, noisy signal should produce a very low amplitude sine wave.

- **Breakers.LateBreakerThreshold**
  - Default: 1.0
  - The maximum number of cycles that a breaker operation's timing can exceed the configured breaker speed before being considered late.

- **LengthUnits**
  - Default: miles
  - The units of measure to use for lengths.
  - This value is only applied to human-readable exports and does not affect the fault calculations.


- **COMTRADEMinWaitTime**
  - Default: 15.0
  - The minimum amount of time, in seconds, to wait for additional data files after the system detects the existence of a .d00 COMTRADE file.
  - The best way to ensure that all data files are present before openXDA attempts to process them is to copy the data files first, then copy the .cfg file last.

- **ProcessingThreadCount**
  - Default: 0
  - The number of threads used for processing meter data concurrently.
  - Values less than zero indicate that the system should use as many threads as there are logical processors in the system.

- **MaxQueuedFileCount**
  - Default: 10
  - The number of files that can be queued on meter data processing threads before the system starts blocking the file processing thread.
  - Values less than or equal to zero will be set to one.

- **FileWatcherEnumerationStrategy**
  - ooDefault: ParallelSubdirectories
  - ooStrategy used for enumeration of files in the file watcher.
    -  **Sequential** processes all watch directories sequentially on a single thread using a depth-first recursive search.
    -  **ParallelWatchDirectories** processes all watch directories in parallel on their own thread using a depth-first recursive search.
    -  **ParallelSubdirectories** processes each directory on its own thread, including subdirectories.
    -  **None** disables file enumeration.

- **FileWatcherMaxFragmentation**
  - Default: 10
  - The maximum amount of fragmentation allowed before compacting the list of processed files in the file watcher.
  - The amount of fragmentation is measured as the number of files that have been removed from the watch directory since the start of the program or the last compact operation.


- **FileWatcherInternalThreadCount**
  - Default: 0
  - The number of threads used internally to the file processor.
  - Values less than zero indicate that the file processor should use as many threads as there are logical processors in the system.

- **FileWatcherBufferSize**
  - Default: 8192
  - The size, in bytes, of the internal buffer used by the watchers of each of the configured watch directories.
  - This buffer is used to store information about the files involved in file system events. Small buffers may overflow causing file events to be missed by the system. Large buffers will use up large amounts of non-paged memory space which could cause system performance degradation or system errors.
  - This value can be between 4 KB and 64 KB. On Windows systems, use a multiple of 4 KB for better performance.

-  **FileShares**
  - A double semicolon-separated list of connection strings that define the credentials required for the service to connect to a file share.
  - Each connection string should contain 3 settings: Name, Username, and Password.
    -  **Name** : The name of the file share (\server\share).
    -  **UserName** : The name of the user to log in as (DOMAIN\USERNAME).
    -  **Password** : The password of the user to log in as.
  - Alternatively, each file share can be defined as three separate settings in the Setting table.
    -  **Name**                                 **Value**
    - FileShares.1.Name                \\server1\share
FileShares.1.Username                DOMAIN1\USER1
FileShares.1.Password                user1pwd
    - FileShares.2.Name                \\server2\share
FileShares.2.Username                DOMAIN2\USER2
FileShares.2.Password                user2pwd
    - Etc.

- **Email.SMTPServer**
  - The hostname or IP address of the SMTP server used for sending emails when a fault is detected.


- **Email.FromAddress**
  - Default: openXDA@gridprotectionalliance.org
  - The email address placed on the From line of the emails sent when a fault is detected.

- **Email.Username**
  - The username used to authenticate to the SMTP server.
  - Remove this field from system settings or leave it blank if no authentication is required.

- **Email.Password**
  - The password used to authenticate to the SMTP server.
  - Remove this field from system settings if no authentication is required.

- **Email.EnableSSL**
  - Default: False
  - Flag that determines whether to enable SSL when establishing communications with the SMTP server.

- **Historian.Server**
  - Default: 127.0.0.1
  - The hostname and port of the openHistorian 2.0 server to be used for archiving trending data. (e.g. 127.0.0.1:38402).

- **Historian.InstanceName**
  - Default: XDA
  - The instance name of the historian instance to be used for archiving trending data.
