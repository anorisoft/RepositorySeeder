///////////////////////////////////////////////////////////////////////////////
// Project Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli
public class VisualStudioProjectReference
{
	public VisualStudioProjectReference(string include)
	{
		this.Include = include;
	}
	public string Include {get; set;}
}


public class VisualStudioProject
{
	public VisualStudioProject(string projectName, Guid guid)
	{
		ProjectName = projectName;
		ProjectGuid = guid;
		AssemblyTitle = projectName;
		PackageId = projectName;
	}
	
	public VisualStudioProject(string projectName) : this(projectName, Guid.NewGuid()){}
	
	public List<VisualStudioProjectReference> ProjectReferences {get;} = new List<VisualStudioProjectReference>();
	
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
	public string TemplatePath {get; set;} = "";
}

public void CreateProject(DirectoryPath target, DirectoryPath templatePath, VisualStudioProject project)
{
	DirectoryPath projectDirectory = GetProjectDirectory(target, project);
	CreateProjectFile(projectDirectory, project, templatePath);
	CreateRepositoryStyleCopJson(projectDirectory, project, templatePath);
}

public DirectoryPath GetProjectDirectory(DirectoryPath target, VisualStudioProject project)
{
	DirectoryPath projectDirectory;
	
	if (string.IsNullOrEmpty(project.ProjectPath))
	{
		projectDirectory = target.Combine("Source").Combine(project.ProjectName);
	}
	else
	{
		projectDirectory = target.Combine(project.ProjectPath);
	}
	
	if (!System.IO.Directory.Exists(projectDirectory.FullPath))
	{
		Information("Project Directory {0} exists.", projectDirectory.FullPath);
		System.IO.Directory.CreateDirectory(projectDirectory.FullPath);
	}
	
	return projectDirectory;
}

public DirectoryPath GetTemplateDirectory(DirectoryPath target, VisualStudioProject project, string templatePath = "")
{
	DirectoryPath projectTemplateDirectory;
	if (!string.IsNullOrEmpty(project.TemplatePath))
	{
		projectTemplateDirectory = target.Combine(project.TemplatePath);
	}
	else 
	{
		projectTemplateDirectory = target.Combine("@Tools/SeedRepository/Template/Source");
	}
		
	if (!System.IO.Directory.Exists(projectTemplateDirectory.FullPath))
	{
		Information("Project Template Directory {0} exists.", projectTemplateDirectory.FullPath);
		System.IO.Directory.CreateDirectory(projectTemplateDirectory.FullPath);
	}
	
	return projectTemplateDirectory;
}

public void CreateProjectFile(DirectoryPath projectDirectory, VisualStudioProject setting, DirectoryPath templateDirectory)
{
	var projectTemplateFilePath = templateDirectory.GetFilePath("Project.csproj.template");
	if (!System.IO.File.Exists(projectTemplateFilePath.FullPath))
	{
		Error("Project Template File {0} not exists.", projectTemplateFilePath.FullPath);
		return;
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

public void CreateRepositoryStyleCopJson(DirectoryPath projectDirectory, VisualStudioProject setting, DirectoryPath templateDirectory)
{
	var stylecopTemplateFilePath = templateDirectory.GetFilePath("stylecop.json.template");
	
	if(!System.IO.File.Exists(stylecopTemplateFilePath.FullPath))
	{
		Information("stylecop.json Template File {0} not exists.", stylecopTemplateFilePath.FullPath);
		return;
	}
	
	var stylecopFilePath = projectDirectory.GetFilePath("stylecop.json");
	if (System.IO.File.Exists(stylecopFilePath.FullPath))
	{
		Information("stylecop.json File {0} exists.", stylecopFilePath.FullPath);
		return;
	}
	
	Information("Create stylecop.json File {0}", stylecopTemplateFilePath);
	var stylecopString = System.IO.File.ReadAllText(stylecopTemplateFilePath.FullPath);
	System.IO.File.WriteAllText(stylecopFilePath.FullPath, stylecopString.ToString());
	
}


public void CreateProjectFramework472(DirectoryPath target, DirectoryPath templatePath , VisualStudioSolution setting)
{
	// Solution
	var projectTemplateFilePath = templatePath.GetFilePath("Project.Framework.4.7.2.csproj.template");
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



