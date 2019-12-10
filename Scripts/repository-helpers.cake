///////////////////////////////////////////////////////////////////////////////
// Repository Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public class RepositorySetting{
	public string Version {get; set;}
	public DateTime Created {get; set;}
	public string Name {get; set;}
}


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

public RepositorySetting CreateRepositorySetting(DirectoryPath target, string repositoryName)
{
	// .repository
	var repositorySettingFilePath = target.GetFilePath(".repository");
	Information("Create repositorySetting file {0}", repositorySettingFilePath);
	
	var repositorySetting = new RepositorySetting { Version = "1.0",
																	Created = DateTime.Now, 
																	Name = repositoryName};
	Context.SerializeJsonToPrettyFile<RepositorySetting>(repositorySettingFilePath, repositorySetting);
	return repositorySetting;
}

public bool IsRepositorySettingExists(DirectoryPath target)
{
	var repositorySettingFilePath = target.GetFilePath(".repository");
	return System.IO.File.Exists(repositorySettingFilePath.FullPath);
}

public bool TryGetRepositorySetting(DirectoryPath target, out RepositorySetting setting)
{
	setting = null;
	try
		{
		var repositorySettingFilePath = target.GetFilePath(".repository");
		if (!System.IO.File.Exists(repositorySettingFilePath.FullPath))
		{
			return false;
		}
		setting = Context.DeserializeJsonFromFile<RepositorySetting>(repositorySettingFilePath);
		if (setting == null)
		{
			return false;
		}
		return true;
	}
	catch
	{
		return false;
	}
}

public bool CopyTemplates(DirectoryPath target)
{
	var source = new DirectoryPath("Tools/SeedRepository/Template");
	Information("Source: {0}", source);
	CopyDirectory​(source, target);
	return true;
}