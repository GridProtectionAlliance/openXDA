::*******************************************************************************************************
::  UpdateDependencies.bat - Gbtc
::
::  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
::
::  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
::  the NOTICE file distributed with this work for additional information regarding copyright ownership.
::  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
::  not use this file except in compliance with the License. You may obtain a copy of the License at:
::
::      http://www.opensource.org/licenses/eclipse-1.0.php
::
::  Unless agreed to in writing, the subject software distributed under the License is distributed on an
::  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
::  License for the specific language governing permissions and limitations.
::
::  Code Modification History:
::  -----------------------------------------------------------------------------------------------------
::  08/16/2015 - Stephen C. Wills
::       Generated original version of source code.
::
::*******************************************************************************************************

@ECHO OFF

SET pwd="%CD%"
SET gwd="%LOCALAPPDATA%\Temp\openXDA"
SET git="%PROGRAMFILES(X86)%\Git\cmd\git.exe"
SET remote="git@github.com:GridProtectionAlliance/openXDA.git"
SET source="\\GPAWEB\NightlyBuilds\GridSolutionsFramework\Beta\Libraries\*.*"
SET target="Source\Dependencies\GSF"

ECHO.
ECHO Entering working directory...
IF EXIST %gwd% RMDIR /S /Q %gwd%
MKDIR %gwd%
CD /D %gwd%

ECHO.
ECHO Getting latest version...
%git% clone %remote% .

ECHO.
ECHO Updating dependencies...
XCOPY %source% %target% /E /U /Y

ECHO.
ECHO Committing updates to local repository...
%git% add %target%
%git% commit -m "Updated GSF dependencies."

ECHO.
ECHO Pushing changes to remote repository...
%git% push
CD /D %pwd%

ECHO.
ECHO Update complete