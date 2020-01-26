///////////////////////////////////////////////////////////////////////////////
// Build Cake Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

#load ./template-helpers.cake
#load ./solution-helpers.cake

public void CreateCakeBuildFiles(DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	CreateBuildCakeFile(target, templatePath, solution);
	CopyFileFromTemplate(".appveyor.yml", target, templatePath);
	CopyFileFromTemplate(".gitattributes", target, templatePath);
	CopyFileFromTemplate(".gitignore", target, templatePath);
	CopyFileFromTemplate("GitVersion.yml", target, templatePath);	
	CopyFileFromTemplate("Run.cake", target, templatePath);
	CopyFileFromTemplate("Run.cmd", target, templatePath);
	CopyFileFromTemplate("RunAppVeyor.cmd", target, templatePath);
	CopyFileFromTemplate("RunNuGet.cake", target, templatePath);
	CopyFileFromTemplate("RunNuGet.cmd", target, templatePath);
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