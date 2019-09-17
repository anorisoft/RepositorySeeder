///////////////////////////////////////////////////////////////////////////////
// GLOBAL SEED
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli
// Description: The file makes it possible to centrally control the cake process of all repositories.

public void Seed_1_0()
	{
		Information("Start Seed 1.0");
		Information("Target: {0}", BuildParameters.Target);
		Information("Seed!");
		var source = new DirectoryPath("Tools/SeedRepository/Template");
		Information("Source: {0}", source);
		var target = BuildParameters.RootDirectoryPath;
		Information("Target: {0}", target);
		CopyDirectory​(source, target);
		var repositorySettingFilePath = target.GetFilePath(".repository");
		Information("RepositorySetting: {0}", repositorySettingFilePath);
		
		var repositoryTemplateSetting = new RepositoryTemplateSetting { Version = "1.0", Created = DateTime.Now};
		Context.SerializeJsonToPrettyFile<RepositoryTemplateSetting>(repositorySettingFilePath, repositoryTemplateSetting);
		
		GitCommit(target, "Seeder", "seeder@anorisoft.com", "Seeding by script");
	}

