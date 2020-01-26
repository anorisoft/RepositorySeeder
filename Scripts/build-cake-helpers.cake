///////////////////////////////////////////////////////////////////////////////
// Build Cake Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

#load ./template-helpers.cake
#load ./solution-helpers.cake

public void CreateBuildFiles(DirectoryPath target, DirectoryPath templatePath, SolutionSetting setting)
{
	CreateBuildCakeFile(target, templatePath, setting);
	CopyFileFromTemplate(".appveyor.yml", target, templatePath, setting);
	CopyFileFromTemplate(".gitattributes", target, templatePath, setting);
	CopyFileFromTemplate(".gitignore", target, templatePath, setting);
	CopyFileFromTemplate("GitVersion.yml", target, templatePath, setting);	
	CopyFileFromTemplate("Run.cake", target, templatePath, setting);
	CopyFileFromTemplate("Run.cmd", target, templatePath, setting);
	CopyFileFromTemplate("RunAppVeyor.cmd", target, templatePath, setting);
	CopyFileFromTemplate("RunNuGet.cake", target, templatePath, setting);
	CopyFileFromTemplate("RunNuGet.cmd", target, templatePath, setting);
}

public void CreateBuildCakeFile(DirectoryPath target, DirectoryPath templatePath, SolutionSetting setting)
{
	var templateFilePath = templatePath.GetFilePath(fileName);
	if(!System.IO.File.Exists(templateFilePath.FullPath))
	{
		Debug("No template file {0}", templateFilePath.FullPath);
		throw new Exception(No template file {0}, filePath.FullPath);
	}
	
	var filePath = target.GetFilePath(fileName);
	if (System.IO.File.Exists(filePath.FullPath))
	{
		Information("Remove file {0}", filePath.FullPath);
		System.IO.File.Delete(filePath.FullPath);
		if (System.IO.File.Exists(filePath.FullPath))
		{
			Debug("Can't remove file {0}", filePath.FullPath);
			throw new Exception("Can't remove file {0}", filePath.FullPath);
		}
	}
	
	Debug("Read All Text to file {0}", templateFilePath.FullPath);
	var buildCakeString = System.IO.File.ReadAllText(templateFilePath.FullPath);
	
	var stringBuilder = new StringBuilder(buildCakeString);
	stringBuilder.Replace("%Solution_Name%", setting.SolutionName);
	stringBuilder.Replace("%Main_Project_Name%", setting.MainProjectName);
	
	Debug("Write All Text to file {0}", filePath.FullPath);
	System.IO.File.WriteAllText(filePath.FullPath, stringBuilder.ToString());
	if (!System.IO.File.Exists(filePath.FullPath))
	{
		Debug("Can't write file {0}", filePath.FullPath);
		throw new Exception("Can't write file {0}", filePath.FullPath);
	}
	Information("Add file: {0}", filePath.FullPath);
}