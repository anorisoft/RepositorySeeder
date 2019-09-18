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
		var branch = GitBranchCurrent(BuildParameters.RootDirectoryPath);
		var remote = branch.Remotes.Where((r) => r.Name.ToLower() == "origin").FirstOrDefault();
		if(remote == null)
		{
			remote = branch.Remotes.FirstOrDefault();
		}
		
		
		Information("Remote Name: {0}", remote.Name);
		Information("Remote Url: {0}", remote.Url);
		var url = new System.Uri(remote.Url);
		var path = url.AbsolutePath;
		var startIndex = path.LastIndexOf(@"/") + 1;
		var length = path.LastIndexOf(@".") - startIndex;
		var repositoryName = path.Substring(startIndex, length);
		Information("Repository Name: {0}", repositoryName);
		
		var source = new DirectoryPath("Tools/SeedRepository/Template");
		Information("Source: {0}", source);
		
		var target = BuildParameters.RootDirectoryPath;
		Information("Target: {0}", target);
		CopyDirectory​(source, target);
		
		var repositorySettingFilePath = target.GetFilePath(".repository");
		Information("RepositorySetting: {0}", repositorySettingFilePath);
		
		var repositoryTemplateSetting = new RepositoryTemplateSetting { Version = "1.0", Created = DateTime.Now};
		Context.SerializeJsonToPrettyFile<RepositoryTemplateSetting>(repositorySettingFilePath, repositoryTemplateSetting);
		
		var solutionTemplateFilePath = source.Combine("Source").GetFilePath("Solution.template");
		var solutionFilePath = source.Combine("Source").GetFilePath("Solution.sln");
		
		if (System.IO.File.Exists(solutionTemplateFilePath.FullPath))
		{
			var solutionString = System.IO.File.ReadAllText(solutionTemplateFilePath.FullPath); 
			System.IO.File.WriteAllText(solutionFilePath.FullPath, solutionString);
			
			
//			GitCommit(target, "Seeder", "seeder@anorisoft.com", "Seeding by script");
		}
	}

