///////////////////////////////////////////////////////////////////////////////
// Project Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public void CreateProjectFramework472(DirectoryPath target, SolutionSetting setting)
{
	// Solution
	var projectTemplateFilePath = target.Combine("Source").GetFilePath("Project.Framework.4.7.2.csproj.template");
	if(System.IO.File.Exists(projectTemplateFilePath.FullPath))
	{
		return;
	}
	
	var sourceDirectory = target.Combine("Source");
	var mainProjectDirectory = target.Combine(setting.MainProjectName);
	if (!System.IO.Directory.Exists(mainProjectDirectory.FullPath))
	{
		System.IO.Directory.CreateDirectory(mainProjectDirectory.FullPath);
	}
	
	var projectFilePath = mainProjectDirectory.GetFilePath(setting.MainProjectName + ".csproj");
	if (System.IO.File.Exists(projectFilePath.FullPath))
	{
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
