///////////////////////////////////////////////////////////////////////////////
// GLOBAL SEED
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli
// Description: The file makes it possible to centrally control the cake process of all repositories.

#load ./repository-helpers.cake
#load ./nuspec-helpers.cake
#load ./solution-helpers.cake
#load ./project-helpers.cake

public void Seed_1_0()
	{
		Information("Start Seed 1.0");
		Information("Target: {0}", BuildParameters.Target);
		Information("Seed!");
		
		var repositoryName = GetMyRepositoryName();
		
		var source = new DirectoryPath("Tools/SeedRepository/Template");
		Information("Source: {0}", source);
		
		var target = BuildParameters.RootDirectoryPath;
		Information("Target: {0}", target);
		CopyDirectory​(source, target);
		
		var repositorySetting = CreateRepositorySetting(target, repositoryName);
		
		var solutionSetting = new SolutionSetting()
		{
			MainProjectName = repositoryName,
			MainProjectGuid = Guid.NewGuid(),
			SolutionName = repositoryName,
			SolutionGuid = Guid.NewGuid(),
			BuildItemsGuid = Guid.NewGuid(),
			ToolsItemsGuid = Guid.NewGuid()
		};
		
		CreateBuildCake(target, solutionSetting);
		
		CreateSolution16(target, solutionSetting);
		
		// Nuspec
		var nuspecTemplateFilePath = target.Combine("Source").GetFilePath("Nuspec.template");
		if(!System.IO.File.Exists(nuspecTemplateFilePath.FullPath))
		{
			return;
		}
		
		var nuspecFilePath = target.Combine("Source").GetFilePath(repositoryName + ".nuspec");
		if (System.IO.File.Exists(nuspecFilePath.FullPath))
		{
			System.IO.File.Delete(nuspecFilePath.FullPath);
		}
		Information("Create Nuspec File {0}", nuspecFilePath);
		var nuspecString = System.IO.File.ReadAllText(nuspecTemplateFilePath.FullPath);
		var nuspecStringBuilder = new StringBuilder(nuspecString);
		nuspecStringBuilder.Replace("%Solution_Name%", repositoryName);
		nuspecStringBuilder.Replace("%Main_Project_Name%", repositoryName);
		nuspecStringBuilder.Replace("%Main_Project_GUID%", Guid.NewGuid().ToString());
		nuspecStringBuilder.Replace("%Solution_Project_GUID%", Guid.NewGuid().ToString());
		nuspecStringBuilder.Replace("%Build_Items_GUID%", Guid.NewGuid().ToString());
		nuspecStringBuilder.Replace("%Tools_Items_GUID%", Guid.NewGuid().ToString());
		nuspecStringBuilder.Replace("%Solution_GUID%", Guid.NewGuid().ToString());
		System.IO.File.WriteAllText(nuspecFilePath.FullPath, nuspecStringBuilder.ToString());
		
		if (System.IO.File.Exists(nuspecTemplateFilePath.FullPath))
		{
			Information("Remove Nuspec Template File {0}", nuspecTemplateFilePath.FullPath);
			//System.IO.File.Delete(nuspecTemplateFilePath.FullPath);
		}
		
		Information("Comit Repository Seeding");
//			GitCommit(target, "Seeder", "seeder@anorisoft.com", "Seeding by script");
		
	}

