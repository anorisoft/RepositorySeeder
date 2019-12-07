///////////////////////////////////////////////////////////////////////////////
// Repository Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public string GetMyRepositoryName()
{
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
	return repositoryName;
}

public void CreateRepositorySetting(DirectoryPath target)
{
	// .repository
	var repositorySettingFilePath = target.GetFilePath(".repository");
	Information("RepositorySetting: {0}", repositorySettingFilePath);
	
	var repositoryTemplateSetting = new RepositoryTemplateSetting { Version = "1.0", Created = DateTime.Now};
	Context.SerializeJsonToPrettyFile<RepositoryTemplateSetting>(repositorySettingFilePath, repositoryTemplateSetting);
	
}