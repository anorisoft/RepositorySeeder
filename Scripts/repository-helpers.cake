// -----------------------------------------------------------------------
// <copyright file="repository-helpers.cake" company="Anorisoft">
// Copyright (c) Anorisoft. All rights reserved.
// </copyright>
// Repository Helpers
// The script contains the functions for creating and managing the repository.
// Author: Martin Egli
// -----------------------------------------------------------------------

/// <summary>
///     The Repository Setting Class
/// </summary>
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

/// <summary>
///     Get the git repository name of the current folder.
/// </summary>
/// <returns>
/// The repository name.
/// </returns>
public string GetMyRepositoryName()
{
	Debug("Begin GetMyRepositoryName")
	try
	{
		if (BuildParameters == null)
		{
			thwow new Exception("BuildParameters is null.");
		}
		
		if (BuildParameters.RootDirectoryPath == null)	
		{
			thwow new Exception("RootDirectoryPath is null.");
		}
		//ToDo Check Path
		Debug("Git Branch Current from {0}", BuildParameters.RootDirectoryPath)
		var branch = GitBranchCurrent(BuildParameters.RootDirectoryPath);
		if (branch == null)
		{
			thwow new Exception("No branch found.");
		}

		var remote = branch.Remotes.Where((r) => r.Name.ToLower() == "origin").FirstOrDefault();
		if(remote == null)
		{
			remote = branch.Remotes.FirstOrDefault();
		}
		Information("Remote Name: {0}", remote.Name);
		Information("Remote Url: {0}", remote.Url);
		
		var url = new System.Uri(remote.Url);
		Debug("Remote Url: {0}", remote.Url)
		var path = url.AbsolutePath;
		Debug("URL.AbsolutePath: {0}", url.AbsolutePath)
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
	finaly
	{
		Debug("End GetMyRepositoryName");
	}
}

/// <summary>
///     Set Repository Setting.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="setting">Repository setting.</param>
pulic void SetRepositorySetting(DirectoryPath target, RepositorySetting repositorySetting)
	Information("Repository Name: {0}", repositoryName);
	var repository = new Repository()
	{
		TemplateVersion = "1.0",
		TemplateType = "",
		Name = repositoryName,
		Created = DateTime.Now, 
		Url = remote.Url,
		Path = path
	};
	return repository;
}

/// <summary>
///     Create new RepositorySetting.
/// </summary>
/// <returns>
/// The RepositorySetting.
/// </returns>
public RepositorySetting CreateRepositorySetting(Repository repository)
{
	// .repository
	var repositorySetting = new RepositorySetting { Version = repository.TemplateVersion,
													Type = repository.TemplateType,
													Created = repository.Created, 
													Name = repository.Name};
	return repositorySetting;
}

public void SetRepositorySettingFile(DirectoryPath target, RepositorySetting repositorySetting)
{
	var repositorySettingFilePath = target.GetFilePath(".repository");
	Information("Set repositorySetting file {0}", repositorySettingFilePath);
	Context.SerializeJsonToPrettyFile<RepositorySetting>(repositorySettingFilePath, repositorySetting);
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

// <summary>
///     Create RepositoryFiles.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="setting">Repository setting.</param>
public void CreateRepositoryFiles(DirectoryPath target, DirectoryPath templatePath, RepositorySetting setting)
{
	CreateRepositoryReadMe(target, templatePath, setting);
	CreateRepositoryReleasesFile(target, templatePath, setting);
	CreateRepositoryLicenseFile(target, templatePath, setting);

	CreateRepositoryDirectories(target, templatePath, setting);
}

public void CreateRepositoryDirectories(DirectoryPath target, DirectoryPath templatePath, RepositorySetting setting)
{
	CreateDirectory("Source", target);
}

public void CreateDirectory(string directoryName, DirectoryPath target)
{
	var sourcePath = target.GetDirectoryPath(directoryName);
	if (Directory.Exists(sourcePath)) 
	{
		Debug("{0} Directory already exists {1}", directoryName, sourcePath.FullPath);
	}
	else
	{
		Directory.CreateDirectory(sourcePath.FullPath);
		if (Directory.Exists(sourcePath)) 
		{
			Debug("{0} Directory created {1}", directoryName, sourcePath.FullPath);
		}
		else
		{
			throw new Exception("Can't create {0} directory {1}", directoryName, sourcePath.FullPath);
		}
	}
}

// <summary>
///     Create repository ReadMe file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="setting">Repository setting.</param>
public void CreateRepositoryReadMe(DirectoryPath target, DirectoryPath templatePath, RepositorySetting setting)
{
	var readmeTemplateFilePath = templatePath.GetFilePath("README.md.template");
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

// <summary>
///     Create repository RELEASES file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="setting">Repository setting.</param>
public void CreateRepositoryReleasesFile(DirectoryPath target, DirectoryPath templatePath, RepositorySetting setting)
{
	var releasesTemplateFilePath = templatePath.GetFilePath("RELEASES.md.template");
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

// <summary>
///     Create repository LICENSE file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="setting">Repository setting.</param>
public void CreateRepositoryLicenseFile(DirectoryPath target, DirectoryPath templatePath, RepositorySetting setting)
{
	var licenseTemplateFilePath = templatePath.GetFilePath("LICENSE.template");
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
 