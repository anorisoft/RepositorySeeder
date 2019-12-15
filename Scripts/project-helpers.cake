///////////////////////////////////////////////////////////////////////////////
// Project Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli
public class ProjectReference
{

	public ProjectReference(string include)
	{
		this.Include = include;
	}
	public string Include {get; set;}
}


public class ProjectSetting
{
	public ProjectSetting(string projectName, Guid guid)
	{
		ProjectName = projectName;
		ProjectGuid = guid;
		AssemblyTitle = projectName;
		PackageId = projectName;
	}
	
	public ProjectSetting(string projectName) : this(projectName, Guid.NewGuid()){}
	
	public List<ProjectReference> ProjectReferences {get;} = new List<ProjectReference>();
	
	public string ProjectName {get; set;} = "";
	public Guid ProjectGuid {get; set;}
	public string AssemblyTitle {get; set;} = "";
	public string Authors {get; set;} = "";
	public string PackageId {get; set;} = "";
	public string PackageLicenseUrl {get; set;} = "";
	public string PackageProjectUrl {get; set;} = "";
	public string Copyright {get; set;} = "";
	public string Description {get; set;} = "";
	public string Company {get; set;} = "";
	public string Product {get; set;} = "";
	public string PackageTags {get; set;} = "";
	public string RepositoryUrl {get; set;} = "";
	public string AssemblyOriginatorKeyFile {get; set;} = "";
	public string ProjectPath {get; set;} = "";
}
public void CreateProject(DirectoryPath target, ProjectSetting setting, string templatePath = "", string templateFile = "")
{
	CreateProjectFile(target, setting, templatePath);
	
}

public void CreateProjectFile(DirectoryPath target, ProjectSetting setting, string templatePath = "", string templateFile = "")
{
	// Solution
	FilePath projectTemplateFilePath;
	if (string.IsNullOrEmpty(templatePath))
	{
		if (string.IsNullOrEmpty(templateFile))
		{
			projectTemplateFilePath = target.Combine("Source").GetFilePath("Project.csproj.template");
		}
		else
		{
			projectTemplateFilePath = target.Combine("Source").GetFilePath(templateFile);
		}
	}
	else
	{
		if (string.IsNullOrEmpty(templateFile))
		{
			projectTemplateFilePath = target.Combine(templatePath).GetFilePath("Project.csproj.template");
		}
		else
		{
			projectTemplateFilePath = target.Combine(templatePath).GetFilePath(templateFile);
		}
	}
	
	if (!System.IO.File.Exists(projectTemplateFilePath.FullPath))
	{
		Information("Project Template File {0} not exists.", projectTemplateFilePath.FullPath);
		return;
	}

	DirectoryPath projectDirectory;
	
	if (string.IsNullOrEmpty(setting.ProjectPath))
	{
		projectDirectory = target.Combine("Source").Combine(setting.ProjectName);
	}
	else
	{
		projectDirectory = target.Combine(setting.ProjectPath).Combine(setting.ProjectName);
	}
	
	if (!System.IO.Directory.Exists(projectDirectory.FullPath))
	{
		Information("Project Directory {0} exists.", projectDirectory.FullPath);
		System.IO.Directory.CreateDirectory(projectDirectory.FullPath);
	}
	
	var projectFilePath = projectDirectory.GetFilePath(setting.ProjectName + ".csproj");
	if (System.IO.File.Exists(projectFilePath.FullPath))
	{
		Information("Project File {0} exists.", projectFilePath.FullPath);
		return;
	}
	
	Information("Create Project File {0}", projectFilePath);
	var projectString = System.IO.File.ReadAllText(projectTemplateFilePath.FullPath);
	var projectStringBuilder = new StringBuilder(projectString);
	projectStringBuilder.Replace("%ProjectName%", setting.ProjectName);
	projectStringBuilder.Replace("%ProjectGuid%", setting.ProjectGuid.ToString());
	projectStringBuilder.Replace("%AssemblyTitle%", setting.AssemblyTitle);
	projectStringBuilder.Replace("%Authors%", setting.Authors);
	projectStringBuilder.Replace("%PackageId%", setting.PackageId);
	projectStringBuilder.Replace("%PackageLicenseUrl%", setting.PackageLicenseUrl);
	projectStringBuilder.Replace("%PackageProjectUrl%", setting.PackageProjectUrl);
	projectStringBuilder.Replace("%Copyright%", setting.Copyright);
	projectStringBuilder.Replace("%Description%", setting.Description);
	projectStringBuilder.Replace("%Company%", setting.Company);
	projectStringBuilder.Replace("%Product%", setting.Product);
	projectStringBuilder.Replace("%PackageTags%", setting.PackageTags);
	projectStringBuilder.Replace("%RepositoryUrl%", setting.RepositoryUrl);
	projectStringBuilder.Replace("%AssemblyOriginatorKeyFile%", setting.AssemblyOriginatorKeyFile);
	projectStringBuilder.Replace("%ProjectPath%", setting.ProjectPath);
	
	var projectReferenceStringBuilder = new StringBuilder();
	foreach(var projectReference in setting.ProjectReferences)
	{
		projectReferenceStringBuilder.Append(@"    <ProjectReference Include=""");
		projectReferenceStringBuilder.Append(projectReference.Include);
		projectReferenceStringBuilder.Append(@""">");
		projectReferenceStringBuilder.Append("\r\n");
	}
	projectStringBuilder.Replace("%ProjectReferences%", projectReferenceStringBuilder.ToString());

	
	System.IO.File.WriteAllText(projectFilePath.FullPath, projectStringBuilder.ToString());
	
	if (System.IO.File.Exists(projectTemplateFilePath.FullPath))
	{
		Information("Remove Project Template File {0}", projectTemplateFilePath.FullPath);
		//System.IO.File.Delete(projectTemplateFilePath.FullPath);
	}
}



public void CreateProjectFramework472(DirectoryPath target, SolutionSetting setting)
{
	// Solution
	var projectTemplateFilePath = target.Combine("Source").GetFilePath("Project.Framework.4.7.2.csproj.template");
	if(!System.IO.File.Exists(projectTemplateFilePath.FullPath))
	{
		Information("Project Template File {0} not exists.", projectTemplateFilePath.FullPath);
		return;
	}
	
	var sourceDirectory = target.Combine("Source");
	var mainProjectDirectory = sourceDirectory.Combine(setting.MainProjectName);
	if (!System.IO.Directory.Exists(mainProjectDirectory.FullPath))
	{
		Information("Project Directory {0} exists.", mainProjectDirectory.FullPath);
		System.IO.Directory.CreateDirectory(mainProjectDirectory.FullPath);
	}
	
	var projectFilePath = mainProjectDirectory.GetFilePath(setting.MainProjectName + ".csproj");
	if (System.IO.File.Exists(projectFilePath.FullPath))
	{
		Information("Project File {0} exists.", projectFilePath.FullPath);
		return;
	}
	
	Information("Create Project File {0}", projectFilePath);
	var projectString = System.IO.File.ReadAllText(projectTemplateFilePath.FullPath);
	var projectStringBuilder = new StringBuilder(projectString);
	projectStringBuilder.Replace("%Solution_Name%", setting.SolutionName);
	projectStringBuilder.Replace("%Solution_GUID%", setting.SolutionGuid.ToString());
	projectStringBuilder.Replace("%Root_Namespace%", setting.MainProjectName);
	projectStringBuilder.Replace("%Assembly_Name%", setting.MainProjectName);
	projectStringBuilder.Replace("%Project_GUID%", setting.MainProjectGuid.ToString());
	projectStringBuilder.Replace("%Output_Path_Debug%", @"bin\Debug\");
	projectStringBuilder.Replace("%Output_Path_Release%", @"bin\Release\");
	System.IO.File.WriteAllText(projectFilePath.FullPath, projectStringBuilder.ToString());
	
	if (System.IO.File.Exists(projectTemplateFilePath.FullPath))
	{
		Information("Remove Project Template File {0}", projectTemplateFilePath.FullPath);
		//System.IO.File.Delete(projectTemplateFilePath.FullPath);
	}
}



