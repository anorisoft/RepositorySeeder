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
		Information("Target: {0}", BuildParameters.Target);
		Information("Seed!");
		var source = new DirectoryPath("Tools/RepositoryTemplate/Template");
		Information("Source: {0}", source);
		var target = BuildParameters.RootDirectoryPath;
		Information("Target: {0}", target);
		CopyDirectory​(source, target);
		var repositorySettingFilePath = target.GetFilePath(".repository");
		Information("RepositorySetting: {0}", repositorySettingFilePath);
		if (System.IO.File.Exists(repositorySettingFilePath.FullPath)){
			Information("RepositorySetting: {0} is exists", repositorySettingFilePath);
			var repositoryTemplateSetting = Context.DeserializeJsonFromFile<RepositoryTemplateSetting>(repositorySettingFilePath);
			Information("Version: {0}", repositoryTemplateSetting.Version);
			Seed_1_0();
		}
		else
		{
			var repositoryTemplateSetting = new RepositoryTemplateSetting { Version = "1.0", Created = DateTime.Now};
			Context.SerializeJsonToPrettyFile<RepositoryTemplateSetting>(repositorySettingFilePath, repositoryTemplateSetting);
			GitCommit(target, "Seeder", "seeder@anorisoft.com", "Seeding by script");
		}
	});

Build.Run();