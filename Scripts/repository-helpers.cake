///////////////////////////////////////////////////////////////////////////////
// Repository Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public class RepositorySetting{
	public string Version {get; set;}
	public DateTime Created {get; set;}
	public string Name {get; set;}
	public string Type {get; set;}
}

public class Repository{
	public string TemplateVersion {get; set;}
	public string TemplateType {get; set;}
	public DateTime Created {get; set;}
	public string Name {get; set;}
	public string Url {get; set;}
	public string Path {get; set;}
}


public Repository GetMyRepository()
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
	var repository = new Repository()
	{
		TemplateVersion = "1.0",
		TemplateType = "",
		Name = repositoryName,
		Created = DateTime.Now, 
		Url = url,
		Path = path
	};
	return repository;
}

public RepositorySetting CreateRepositorySetting(Repository repository)
{
	// .repository
	var repositorySetting = new RepositorySetting { Version = repository.TemplateVersion,
													Type = repository.TemplateType,
													Created = repository.Created, 
													Name = repository.Name};
	return repositorySetting;
}

public void SetRepositorySetting(DirectoryPath target, RepositorySetting repositorySetting)
{
	var repositorySettingFilePath = target.GetFilePath(".repository");
	Information("Set repositorySetting file {0}", repositorySettingFilePath);
	Context.SerializeJsonToPrettyFile<RepositorySetting>(repositorySettingFilePath, repositorySetting);
}

public void CreateRepository(DirectoryPath target, Repository repository)
{
	CreateRepositoryReadMe(target, repository);
	CreateRepositoryReleases(target, repository);
	CreateRepositoryLicense(target, repository);
}

public void CreateRepositoryReadMe(DirectoryPath target, Repository repository)
{
	var readmeTemplateFilePath = target.GetFilePath("README.md.template");
	if(!System.IO.File.Exists(readmeTemplateFilePath.FullPath))
	{
		Information("README Template File {0} not exists.", readmeTemplateFilePath.FullPath);
		return;
	}
	
	var readmeFilePath = target.GetFilePath("README.md");
	if (System.IO.File.Exists(readmeFilePath.FullPath))
	{
		Information("README File {0} exists.", readmeFilePath.FullPath);
		return;
	}
	
	Information("Create README File {0}", readmeTemplateFilePath);
	var readmeString = System.IO.File.ReadAllText(readmeTemplateFilePath.FullPath);
	var readmeStringBuilder = new StringBuilder(readmeString);
	readmeStringBuilder.Replace("%RepositoryName%", repository.Name);
	readmeStringBuilder.Replace("%SolutionName%", repository.Name);
	readmeStringBuilder.Replace("%Created%", repository.Created.ToString());
	System.IO.File.WriteAllText(readmeFilePath.FullPath, readmeStringBuilder.ToString());
	
}

public void CreateRepositoryReleases(DirectoryPath target, Repository repository)
{
	var releasesTemplateFilePath = target.GetFilePath("RELEASES.md.template");
	if(!System.IO.File.Exists(releasesTemplateFilePath.FullPath))
	{
		Information("Releases Template File {0} not exists.", releasesTemplateFilePath.FullPath);
		return;
	}
	
	var releasesFilePath = target.GetFilePath("RELEASES.md");
	if (System.IO.File.Exists(releasesFilePath.FullPath))
	{
		Information("Releases File {0} exists.", releasesFilePath.FullPath);
		return;
	}
	
	Information("Create Releases File {0}", releasesTemplateFilePath);
	var releasesString = System.IO.File.ReadAllText(releasesTemplateFilePath.FullPath);
	var releasesStringBuilder = new StringBuilder(releasesString);
	releasesStringBuilder.Replace("%RepositoryName%", repository.Name);
	releasesStringBuilder.Replace("%SolutionName%", repository.Name);
	releasesStringBuilder.Replace("%Created%", repository.Created.ToString());
	System.IO.File.WriteAllText(releasesFilePath.FullPath, releasesStringBuilder.ToString());
	
}


public void CreateRepositoryLicense(DirectoryPath target, Repository repository)
{
	var licenseTemplateFilePath = target.GetFilePath("LICENSE.template");
	if(!System.IO.File.Exists(licenseTemplateFilePath.FullPath))
	{
		Information("License Template File {0} not exists.", licenseTemplateFilePath.FullPath);
		return;
	}
	
	var licenseFilePath = target.GetFilePath("LICENSE");
	if (System.IO.File.Exists(licenseFilePath.FullPath))
	{
		Information("License File {0} exists.", licenseFilePath.FullPath);
		return;
	}
	
	Information("Create License File {0}", licenseTemplateFilePath);
	var licenseString = System.IO.File.ReadAllText(licenseTemplateFilePath.FullPath);
	var licenseStringBuilder = new StringBuilder(licenseString);
	licenseStringBuilder.Replace("%RepositoryName%", repository.Name);
	licenseStringBuilder.Replace("%SolutionName%", repository.Name);
	licenseStringBuilder.Replace("%Created%", repository.Created.ToString());
	System.IO.File.WriteAllText(licenseFilePath.FullPath, licenseStringBuilder.ToString());
	
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