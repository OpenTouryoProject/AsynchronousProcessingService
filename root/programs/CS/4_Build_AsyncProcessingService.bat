setlocal

@rem --------------------------------------------------
@rem Turn off the echo function.
@rem --------------------------------------------------
@echo off

@rem --------------------------------------------------
@rem Get the path to the executable file.
@rem --------------------------------------------------
set CURRENT_DIR="%~dp0"

@rem --------------------------------------------------
@rem Execution of the common processing.
@rem --------------------------------------------------
call %CURRENT_DIR%z_Common.bat

rem --------------------------------------------------
rem Build the batch Infrastructure(AsyncProcessingService)
rem --------------------------------------------------

..\nuget.exe restore "Frameworks\Infrastructure\AsyncProcessingService.sln"
%BUILDFILEPATH% %COMMANDLINE% "Frameworks\Infrastructure\AsyncProcessingService.sln"

pause

rem -------------------------------------------------------
endlocal
