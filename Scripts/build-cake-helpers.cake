///////////////////////////////////////////////////////////////////////////////
// Build Cake Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

#load ./template-helpers.cake
#load ./solution-helpers.cake

public void CreateCakeBuildFiles(DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	CreateBuildCakeFile(target, templatePath, solution);
	CopyFileFromTemplate(".appveyor.yml", ".appveyor.yml.template", target, templatePath);
	CopyFileFromTemplate(".gitattributes", ".gitattributes.template", target, templatePath);
	CopyFileFromTemplate(".gitignore", ".gitignore.template", target, templatePath);
	CopyFileFromTemplate("GitVersion.yml", "GitVersion.yml.template", target, templatePath);	
	CopyFileFromTemplate("Run.cake", "Run.cake.template", target, templatePath);
	CopyFileFromTemplate("Run.cmd", "Run.cmd.template", target, templatePath);
	CopyFileFromTemplate("RunAppVeyor.cmd", "RunAppVeyor.cmd.template", target, templatePath);
	CopyFileFromTemplate("RunNuGet.cake", "RunNuGet.cake.template", target, templatePath);
	CopyFileFromTemplate("RunNuGet.cmd", "RunNuGet.cmd.template", target, templatePath);
}

public void CreateBuildCakeFile(DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	Information("Create BuildCake {0}", solution.SolutionName);
	var replaces = new Dictionary<string,string>();
	replaces.Add("%Solution_Name%", solution.SolutionName);
	replaces.Add("%Solution_GUID%", solution.SolutionGuid.ToString());
	replaces.Add("%Main_Project_Name%", solution.MainProjectName);
	replaces.Add("%Main_Project_GUID%", solution.MainProjectGuid.ToString());
	if (solution.UnitTestProject != null)
	{
		replaces.Add("%UnitTest_Project_Name%", solution.UnitTestProjectName);
		replaces.Add("%UnitTest_Project_GUID%", solution.UnitTestProjectGuid.ToString());
	}
	replaces.Add("%Build_Items_GUID%", solution.BuildItemsGuid.ToString());
	replaces.Add("%Tools_Items_GUID%", solution.ToolsItemsGuid.ToString());
	CreateFileFromTemplate("build.cake", "build.cake.template", replaces, target, templatePath);
}