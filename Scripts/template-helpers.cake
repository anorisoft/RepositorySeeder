///////////////////////////////////////////////////////////////////////////////
// Build Cake Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public void CopyFileFromTemplate(string fileName, DirectoryPath target, DirectoryPath templatePath, SolutionSetting setting){
{
	CopyFileFromTemplate(fileName, fileName, target, templatePath, setting);
}

public void CopyFileFromTemplate(string fileName, string templateFileName, DirectoryPath target, DirectoryPath templatePath, SolutionSetting setting){
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

	Debug("Copy file {0}", filePath.FullPath);
	System.IO.File.Copy(templateFilePath, filePath);
	if (!System.IO.File.Exists(filePath.FullPath))
	{
		Debug("Can't copy file {0}", filePath.FullPath);
		throw new Exception("Can't copy file {0}", filePath.FullPath);
	}
	Information("Add file: {0}", filePath.FullPath);
}