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

IF NOT EXIST Tools\GlobalSettings (
	powershell write-host -fore Yellow "Adding Global Settings Submodule from git."
	git submodule add -f https://github.com/anorisoft/Cake.GlobalSettings.git Tools/GlobalSettings
	powershell write-host
)

IF NOT EXIST Tools\RepositorySeeder (
	powershell write-host -fore Yellow "Adding Repository Seeder Submodule from git."
	git submodule add -f https://github.com/anorisoft/RepositorySeeder.git Tools/RepositorySeeder
	powershell write-host
)

IF NOT EXIST "Tools\Resources" (
	powershell write-host -fore Yellow "Adding Resources Submodule from git.
	git submodule add -f https://github.com/anorisoft/Cake.Resources.git Tools/Resources
	powershell write-host
)

IF EXIST "Tools\Resources" (
	IF EXIST "Tools\GlobalSettings" (
		IF EXIST "Tools\GlobalSettings" (
			IF EXIST "Tools\RepositorySeeder" (
				COPY Tools\RepositorySeeder\seed-repository.cake
rem				powershell .\Tools\Resources\build.ps1 -Script Seed.cake -Verbosity Diagnostic -Target Seed
				powershell .\Tools\Resources\build.ps1 -Script seed-repository.cake -Target Seed
			) ELSE (
				powershell write-host -fore Red "File Tools\GlobalSettings\seed-repository.cake not exist."
			)
		) ELSE (
			powershell write-host -fore Red "Directory Tools\RepositorySeeder not exist."
		)
	) ELSE (
		powershell write-host -fore Red "Directory Tools\GlobalSettings not exist."
	)
) ELSE (
	powershell write-host -fore Red "Directory Tools\Resources not exist."
)
pause
