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
	
	CreateRepository(target, repositorySetting);
	
	var solutionSetting = new SolutionSetting(repository.Name);
	
	CreateBuildCake(target, solutionSetting);
	
	CreateSolution16(target, solutionSetting);
	
	var mainProjectSetting = new ProjectSetting(repository.Name,
		solutionSetting.MainProjectGuid)
	{
		Authors = "Martin Egli",
		Company = "Anori Soft"
		PackageLicenseUrl = repository.Url + "/LICENCE",
		PackageProjectUrl = repository.Url,
		RepositoryUrl = repository.Url,
		ProjectPath = "\source\" + repository.Name
	};
	
	CreateProject(target, mainProjectSetting);
	
	CreateNuspec(target, solutionSetting);
	
	Information("Comit Repository Seeding");
//			GitCommit(target, "Seeder", "seeder@anorisoft.com", "Seeding by script");
	
}

