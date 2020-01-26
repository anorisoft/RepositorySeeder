///////////////////////////////////////////////////////////////////////////////
// Build Cake Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public void CopyFileFromTemplate(string fileName, DirectoryPath target, DirectoryPath templatePath)
{
	CopyFileFromTemplate(fileName, fileName, target, templatePath);
}

public void CopyFileFromTemplate(string fileName, string templateFileName, DirectoryPath target, DirectoryPath templatePath)
{
	var templateFilePath = templatePath.GetFilePath(fileName);
	if(!System.IO.File.Exists(templateFilePath.FullPath))
	{
		Debug("No template file {0}", templateFilePath.FullPath);
		throw new Exception("No template file {0}", filePath.FullPath);
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

/// <summary>
///     Create repository ReadMe file.
/// </summary>
/// <param name="fileName">File name.</param>
/// <param name="templateFileName">Template filr name.</param>
/// <param name="replaceDictionary">Replace Dictionary.</param>
/// <param name="target">Target path.</param>
/// <param name="templatePath">Template path.</param>
public void CreateFileFromTemplate(string fileName, string templateFileName, IDictionary<string,string> replaceDictionary, DirectoryPath target, DirectoryPath templatePath)
{
	var templateFilePath = templatePath.GetFilePath(templateName);
	if(!System.IO.File.Exists(templateFilePath.FullPath))
	{
		Debug("No template file {0}", templateFilePath.FullPath);
		throw new Exception("No template file {0}", filePath.FullPath);
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
	
	Information("Create {0} file {1}", fileName, templateFilePath);
	var templateString = System.IO.File.ReadAllText(templateFilePath.FullPath);
	var stringBuilder = new StringBuilder(readmeString);
    foreach (var replace in replaceDictionary)
    {
        Debug("Replace {0} with {1}", replace.Key, replace.Value);
        stringBuilder.Replace(replace.Key, replace.Value);
    }
	Debug("Write All Text to {0}", filePath.FullPath);
    System.IO.File.WriteAllText(filePath.FullPath, stringBuilder.ToString());
	
    if (!System.IO.File.Exists(filePath.FullPath))
	{
		Debug("Can't copy file {0}", filePath.FullPath);
		throw new Exception("Can't copy file {0}", filePath.FullPath);
	}
	Information("Add file: {0}", filePath.FullPath);
}