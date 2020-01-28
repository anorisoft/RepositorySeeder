///////////////////////////////////////////////////////////////////////////////
// Nuspec SEED
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

#load ./solution-helpers.cake

public void CreateNuspec(DirectoryPath target, DirectoryPath template, VisualStudioSolution setting)
{
	// Nuspec
	var nuspecTemplateFilePath = template.Combine("Source").GetFilePath("Nuspec.template");
	if(!System.IO.File.Exists(nuspecTemplateFilePath.FullPath))
	{
		return;
	}
	
	var nuspecFilePath = target.Combine("Source").GetFilePath(setting.MainProjectName + ".nuspec");
	if (System.IO.File.Exists(nuspecFilePath.FullPath))
	{
		System.IO.File.Delete(nuspecFilePath.FullPath);
	}
	Information("Create Nuspec File {0}", nuspecFilePath);
	var nuspecString = System.IO.File.ReadAllText(nuspecTemplateFilePath.FullPath);
	var nuspecStringBuilder = new StringBuilder(nuspecString);
	nuspecStringBuilder.Replace("%Solution_Name%", setting.SolutionName);
	nuspecStringBuilder.Replace("%Main_Project_Name%", setting.MainProjectName);
	System.IO.File.WriteAllText(nuspecFilePath.FullPath, nuspecStringBuilder.ToString());
	
	if (System.IO.File.Exists(nuspecTemplateFilePath.FullPath))
	{
		Information("Remove Nuspec Template File {0}", nuspecTemplateFilePath.FullPath);
		//System.IO.File.Delete(nuspecTemplateFilePath.FullPath);
	}
}