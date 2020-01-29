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
	public string SourceFolder {get; set;} = "Source";
	public string DocumentationFolder {get; set;} = "Documentation";
	public string TestsFolder {get; set;} = "Tests";
	public string ToolsFolder {get; set;} = "Tools";
}

/// <summary>
///     Get the git repository name of the current folder.
/// </summary>
/// <returns>
/// The repository name.
/// </returns>

public Repository GetMyRepository()
{
	Debug("Begin GetMyRepository");
	try
	{
		if (BuildParameters.RootDirectoryPath == null)	
		{
			throw new Exception("RootDirectoryPath is null.");
		}
		
		//ToDo Check Path
		Debug("Git Branch Current from {0}", BuildParameters.RootDirectoryPath);
		var branch = GitBranchCurrent(BuildParameters.RootDirectoryPath);
		if (branch == null)
		{
			throw new Exception("No branch found.");
		}

		var remote = branch.Remotes.Where((r) => r.Name.ToLower() == "origin").FirstOrDefault();
		if(remote == null)
		{
			remote = branch.Remotes.FirstOrDefault();
		}
		Information("Remote Name: {0}", remote.Name);
		Information("Remote Url: {0}", remote.Url);
		
		var url = new System.Uri(remote.Url);
		Debug("Remote Url: {0}", remote.Url);
		var path = url.AbsolutePath;
		Debug("URL.AbsolutePath: {0}", url.AbsolutePath);
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
			Url = remote.Url,
			Path = path
		};
		return repository;
	}
	finally
	{
		Debug("End GetMyRepositoryName");
	}
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

public void SaveRepositorySettingFile(DirectoryPath target, RepositorySetting repositorySetting)
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
/// <param name="repository">Repository setting.</param>
public void CreateRepositoryFiles(DirectoryPath target, DirectoryPath templatePath, Repository repository)
{
	CreateRepositoryReadMe(target, templatePath, repository);
	CreateRepositoryReleasesFile(target, templatePath, repository);
	CreateRepositoryLicenseFile(target, templatePath, repository);

	CreateRepositoryDirectories(target, templatePath, repository);
}

// <summary>
///     Create repository ReadMe file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="repository">Repository setting.</param>
public void CreateRepositoryDirectories(DirectoryPath target, DirectoryPath templatePath, Repository repository)
{
	CreateDirectory(repository.SourceFolder, target);
	CreateDirectory(repository.DocumentationFolder, target);
	CreateDirectory(repository.TestsFolder, target);
	CreateDirectory(repository.ToolsFolder, target);
}

// <summary>
///     Create repository ReadMe file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="repository">Repository setting.</param>
public void CreateRepositoryReadMe(DirectoryPath target, DirectoryPath templatePath, Repository repository)
{
	Information("Create README File {0}", templatePath);
	var replaces = new Dictionary<string, string>();
	replaces.Add("%RepositoryName%", repository.Name);
	replaces.Add("%SolutionName%", repository.Name);
	replaces.Add("%Created%", repository.Created.ToString());
	CreateFileFromTemplate("README.md", "README.md.template", replaces, target, templatePath);
}

// <summary>
///     Create repository RELEASES file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="repository">Repository setting.</param>
public void CreateRepositoryReleasesFile(DirectoryPath target, DirectoryPath templatePath, Repository repository)
{
	Information("Create Releases File {0}", templatePath);
	var replaces = new Dictionary<string, string>();
	replaces.Add("%RepositoryName%", repository.Name);
	replaces.Add("%SolutionName%", repository.Name);
	replaces.Add("%Created%", repository.Created.ToString());
	CreateFileFromTemplate("RELEASES.md", "RELEASES.md.template", replaces, target, templatePath);
}

// <summary>
///     Create repository LICENSE file.
/// </summary>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
/// <param name="repository">Repository setting.</param>
public void CreateRepositoryLicenseFile(DirectoryPath target, DirectoryPath templatePath, Repository repository)
{
	Information("Create License File {0}", templatePath);
	var replaces = new Dictionary<string, string>();
	replaces.Add("%RepositoryName%", repository.Name);
	replaces.Add("%SolutionName%", repository.Name);
	replaces.Add("%Created%", repository.Created.ToString());
	CreateFileFromTemplate("LICENSE", "LICENSE.template", replaces, target, templatePath);
}
 