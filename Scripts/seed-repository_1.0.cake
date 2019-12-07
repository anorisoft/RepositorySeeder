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
		var repositoryName = "";
		var index = path.LastIndexOf(@".git");
		if (index > startIndex)
		{
			var length = index - startIndex;
			repositoryName = path.Substring(startIndex, length);
		}
		else
		{
			var length = path.Length - startIndex;
			repositoryName = path.Substring(startIndex, length);
		}
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
		
		
		var solutionTemplateFilePath = target.Combine("Source").GetFilePath("Solution.template");
		if(!System.IO.File.Exists(solutionTemplateFilePath.FullPath))
		{
			return;
		}
		
		var solutionFilePath = target.Combine("Source").GetFilePath(repositoryName + ".sln");
		if (System.IO.File.Exists(solutionFilePath.FullPath))
		{
			System.IO.File.Delete(solutionFilePath.FullPath);
		}
		
		Information("Create Solution File {0}", solutionFilePath);
		var solutionString = System.IO.File.ReadAllText(solutionTemplateFilePath.FullPath);
		var endTag = "EndSolutionTemplate";
		var solutionStringIndex = solutionString.IndexOf(endTag) + endTag.Length + 2;
		var solutionStringBuilder = new StringBuilder(solutionString.Substring(solutionStringIndex, solutionString.Length - solutionStringIndex));
		solutionStringBuilder.Replace("%Solution_Name%", repositoryName);
		solutionStringBuilder.Replace("%Main_Project_Name%", repositoryName);
		solutionStringBuilder.Replace("%Main_Project_GUID%", Guid.NewGuid().ToString());
		solutionStringBuilder.Replace("%Solution_Project_GUID%", Guid.NewGuid().ToString());
		solutionStringBuilder.Replace("%Build_Items_GUID%", Guid.NewGuid().ToString());
		solutionStringBuilder.Replace("%Tools_Items_GUID%", Guid.NewGuid().ToString());
		solutionStringBuilder.Replace("%Solution_GUID%", Guid.NewGuid().ToString());
		System.IO.File.WriteAllText(solutionFilePath.FullPath, solutionStringBuilder.ToString());
		
		if (System.IO.File.Exists(solutionTemplateFilePath.FullPath))
		{
			Information("Remove Solution Template File {0}", solutionTemplateFilePath.FullPath);
			//System.IO.File.Delete(solutionTemplateFilePath.FullPath);
		}
		
		
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

