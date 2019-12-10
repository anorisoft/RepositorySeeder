///////////////////////////////////////////////////////////////////////////////
// GLOBAL SEED
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli
// Description: The file makes it possible to centrally control the cake process of all repositories.

#addin nuget:?package=Cake.Json
#addin nuget:?package=Newtonsoft.Json&version=11.0.2

#load ./Tools/GlobalSettings/Addins.cake
#load ./Tools/SeedRepository/Scripts/seed-repository_1.0.cake

Environment.SetVariableNames();

BuildParameters.SetParameters(
	context: Context,
	shouldRunGitVersion: false,
	buildSystem: BuildSystem,
	title: "Seeder"
);

//BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

public class RepositoryTemplateSetting{
	public string Version {get; set;}
	public DateTime Created {get; set;}
}

var isAlreadySeeded = false;
float seedVersion = 0;

Task("InitializeSeeder")
	.Does(() => 
	{
		var target = BuildParameters.RootDirectoryPath;
		if (TryGetRepositorySettingExists(target, out var repositoryTemplateSetting)){
			Information("Repository is alresdy seeded!");
			isAlreadySeeded = true;
			Information("Version: {0}", repositoryTemplateSetting.Version);
			seedVersion = float.Parse(repositoryTemplateSetting.Version);
		}
	});
	
Task("Seed_1_0")
	.WithCriteria(!isAlreadySeeded)
	.Does(() => 
	{
		Seed_1_0();
	});

Task("Update_2_0")
	.WithCriteria(isAlreadySeeded)
	.WithCriteria(seedVersion < 2.0)
	.Does(() => 
	{
	});

Task("Seed")
	.IsDependentOn("InitializeSeeder")
	.IsDependentOn("Seed_1_0")
	.IsDependentOn("Update_2_0")
	.Does(() => 
	{
	});

Build.Run();
