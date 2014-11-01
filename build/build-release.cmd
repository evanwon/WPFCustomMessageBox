:: http://msdn.microsoft.com/en-us/library/vstudio/ms164311.aspx

@ECHO OFF

SET msbuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\msbuild.exe"

:: delete existing build
del "..\source\WPFCustomMessageBox\bin\Release\" /q /s

IF '%1'=='' (SET configuration=Release) ELSE (SET configuration=%1)
IF '%2'=='' (SET platform="Any CPU") ELSE (SET platform=%2)

%msbuild% "../source/WPFCustomMessageBox.sln" /t:Rebuild /nologo /property:Platform=%platform% /property:Configuration=%configuration% /property:DebugSymbols=false /property:DebugType=None /property:AllowedReferenceRelatedFileExtensions=- /verbosity:minimal /flp:verbosity=normal;logfile=build-release.log 

IF NOT ERRORLEVEL 0 EXIT /B %ERRORLEVEL%