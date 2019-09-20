@echo off
rem ***************************************************************************
rem * Repository seeder script
rem ***************************************************************************
powershell write-host -fore Cyan  ***************************************************************************
powershell write-host -fore Cyan "* Repository seeder script"
powershell write-host -fore Cyan  ***************************************************************************
powershell write-host


IF NOT EXIST "Tools" (
	powershell write-host -fore Cyan "Directory Tools not exist."
) ELSE (
	powershell write-host -fore Cyan "Directory Tools exist."
)

IF NOT EXIST Tools\RepositorySeeder (
	powershell write-host -fore Yellow "Adding Repository Seeder Submodule from git."
	git submodule add -f https://github.com/anorisoft/RepositorySeeder.git Tools/RepositorySeeder
	powershell write-host
	goto :UPDATENO
) ELSE (
	set /P c=Are you sure you want to update seeder repository [y/N]?
	if /I "%c%" EQU "Y" goto :UPDATEYES
	if /I "%c%" EQU "N" goto :UPDATENO
	goto :UPDATENO
)

:UPDATEYES
	powershell write-host -fore Yellow "Updating Repository Seeder Submodule from git."
	git submodule update -f --init Tools/RepositorySeeder
:UPDATENO


IF EXIST "Tools\RepositorySeeder" (
  COPY Tools\RepositorySeeder\seed-repository*.cmd
) ELSE (
  powershell write-host -fore Red "Directory Tools\RepositorySeeder not exist."
)

:DELETCHOICE
set /P c=Are you sure you want to delet seeder.cmd file [y/N]?
if /I "%c%" EQU "Y" goto :YES
if /I "%c%" EQU "N" goto :NO
goto :NO


:YES

echo "Deleting seeder.cmd"
DEL /f seeder.cmd
pause
exit

:NO
exit

