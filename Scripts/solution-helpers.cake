///////////////////////////////////////////////////////////////////////////////
// Solution Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

pulic class SolutionSetting
{
	public string MainProjectName {get; set;}
	public string SolutionName {get; set;}
	public Guid MainProjectGuid {get; set;}
	public Guid SolutionGuid {get; set;}
	public Guid BuildItemsGuid {get; set;}
	public Guid ToolsItemsGuid {get; set;}
}

public void CreateSolution16(DirectoryPath target, SolutionSetting setting)
{
	// Solution
	var solutionTemplateFilePath = target.Combine("Source").GetFilePath("Solution.16.template");
	if(!System.IO.File.Exists(solutionTemplateFilePath.FullPath))
	{
		return;
	}
	
	var solutionFilePath = target.Combine("Source").GetFilePath(repositoryName + ".sln");
	if (System.IO.File.Exists(solutionFilePath.FullPath))
	{
		System.IO.File.Delete(solutionFilePath.FullPath);
	}
	
	Information("Create Solution File {0}", solutionFilePath);
	var solutionString = System.IO.File.ReadAllText(solutionTemplateFilePath.FullPath);
	var endTag = "EndSolutionTemplate";
	var solutionStringIndex = solutionString.IndexOf(endTag) + endTag.Length + 2;
	var solutionStringBuilder = new StringBuilder(solutionString.Substring(solutionStringIndex, solutionString.Length - solutionStringIndex));
	solutionStringBuilder.Replace("%Solution_Name%", setting.SolutionName);
	solutionStringBuilder.Replace("%Solution_GUID%", setting.SolutionGuid.ToString());
	solutionStringBuilder.Replace("%Main_Project_Name%", setting.MainProjectName);
	solutionStringBuilder.Replace("%Main_Project_GUID%", setting.MainProjectGuid.ToString());
//	solutionStringBuilder.Replace("%Solution_Project_GUID%", Guid.NewGuid().ToString());
	solutionStringBuilder.Replace("%Build_Items_GUID%", setting.BuildItemsGuid.ToString());
	solutionStringBuilder.Replace("%Tools_Items_GUID%", setting.ToolsItemsGuid.ToString());
	System.IO.File.WriteAllText(solutionFilePath.FullPath, solutionStringBuilder.ToString());
	
	if (System.IO.File.Exists(solutionTemplateFilePath.FullPath))
	{
		Information("Remove Solution Template File {0}", solutionTemplateFilePath.FullPath);
		//System.IO.File.Delete(solutionTemplateFilePath.FullPath);
	}
}

public void CreateSolution15(DirectoryPath target, SolutionSetting setting)
{
	// Solution
	var solutionTemplateFilePath = target.Combine("Source").GetFilePath("Solution.15.template");
	if(!System.IO.File.Exists(solutionTemplateFilePath.FullPath))
	{
		return;
	}
	
	var solutionFilePath = target.Combine("Source").GetFilePath(repositoryName + ".sln");
	if (System.IO.File.Exists(solutionFilePath.FullPath))
	{
		System.IO.File.Delete(solutionFilePath.FullPath);
	}
	
	Information("Create Solution File {0}", solutionFilePath);
	var solutionString = System.IO.File.ReadAllText(solutionTemplateFilePath.FullPath);
	var endTag = "EndSolutionTemplate";
	var solutionStringIndex = solutionString.IndexOf(endTag) + endTag.Length + 2;
	var solutionStringBuilder = new StringBuilder(solutionString.Substring(solutionStringIndex, solutionString.Length - solutionStringIndex));
	solutionStringBuilder.Replace("%Solution_Name%", setting.SolutionName);
	solutionStringBuilder.Replace("%Solution_GUID%", setting.SolutionGuid.ToString());
	solutionStringBuilder.Replace("%Main_Project_Name%", setting.MainProjectName);
	solutionStringBuilder.Replace("%Main_Project_GUID%", setting.MainProjectGuid.ToString());
//	solutionStringBuilder.Replace("%Solution_Project_GUID%", Guid.NewGuid().ToString());
	solutionStringBuilder.Replace("%Build_Items_GUID%", setting.BuildItemsGuid.ToString());
	solutionStringBuilder.Replace("%Tools_Items_GUID%", setting.ToolsItemsGuid.ToString());
	System.IO.File.WriteAllText(solutionFilePath.FullPath, solutionStringBuilder.ToString());
	
	if (System.IO.File.Exists(solutionTemplateFilePath.FullPath))
	{
		Information("Remove Solution Template File {0}", solutionTemplateFilePath.FullPath);
		//System.IO.File.Delete(solutionTemplateFilePath.FullPath);
	}
}