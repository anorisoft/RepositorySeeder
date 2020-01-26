///////////////////////////////////////////////////////////////////////////////
// GLOBAL SEED
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli
// Description: The file makes it possible to centrally control the cake process of all repositories.

#load ./repository-helpers.cake
#load ./nuspec-helpers.cake
#load ./solution-helpers.cake
#load ./project-helpers.cake
#load ./build-cake-helpers.cake

public void Seed_1_0()
{
	Information("Start Seed 1.0");
	Information("Target: {0}", BuildParameters.Target);
	Information("Seed!");
	
	var repository = GetMyRepository();
	var target = BuildParameters.RootDirectoryPath;
	Information("Target: {0}", target);
	
	CopyTemplates(target);
	
	var repositorySetting = CreateRepositorySetting(repository);
	
	SetRepositorySetting(target, repositorySetting);
	
	CreateRepository(target, repository);
	
	var solutionSetting = new SolutionSetting(repository.Name);
	
	CreateBuildCake(target, solutionSetting);
	
	CreateSolution16(target, solutionSetting);
	
	var mainProjectSetting = new ProjectSetting(repository.Name,
		solutionSetting.MainProjectGuid)
	{
		Information("Start Seed 1.0");
		Information("Target: {0}", BuildParameters.Target);
		Information("Seed!");
		Debug(DateTime.Now);
		
		Debug("Get Repository Name");
		var repositoryName = GetMyRepositoryName();
		Debug("Repository Name: {0}", repositoryName);

		Debug("Get Target Path");
		var target = BuildParameters.RootDirectoryPath;
		Information("Target: {0}", target);
		
		var templatePath = new DirectoryPath(@"Tools/SeedRepository/Template");
	    Information("Template: {0}", templatePath);
				
		// CopyTemplates(target);
		
		Debug("Create Repository Setting");
		var repositorySetting = CreateRepositorySetting(repositoryName);
		
		Debug("Set Repository Setting");
		SetRepositorySetting(target, repositoryName);
		
		Debug("Create Repository Files");
		CreateRepositoryFiles(target, templatePath, repositorySetting);
		
		var solutionSetting = new SolutionSetting(repositoryName);
		
		CreateBuildFiles(target, templatePath, solutionSetting);
		
		CreateSolutionFiles(target, templatePath, solutionSetting);
				
		Information("Comit Repository Seeding");
//			GitCommit(target, "Seeder", "seeder@anorisoft.com", "Seeding by script");
	
}

