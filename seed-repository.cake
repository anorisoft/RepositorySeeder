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

Task("Seed")
	.Does(() => 
	{
		var target = BuildParameters.RootDirectoryPath;
		var repositorySettingFilePath = target.GetFilePath(".repository");
		if (System.IO.File.Exists(repositorySettingFilePath.FullPath)){
			Information("Repository is alresdy seeded!");
			var repositoryTemplateSetting = Context.DeserializeJsonFromFile<RepositoryTemplateSetting>(repositorySettingFilePath);
			Information("Version: {0}", repositoryTemplateSetting.Version);
		}
		else
		{
			Seed_1_0();
		}
	});

Build.Run();