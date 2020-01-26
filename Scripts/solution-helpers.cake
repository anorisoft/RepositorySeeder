///////////////////////////////////////////////////////////////////////////////
// Solution Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public class SolutionSetting
{
	public SolutionSetting(string mainProjectName, string solutionName)
	{
		MainProjectName = mainProjectName;
		MainProjectGuid = Guid.NewGuid();
		UnitTestProjectName = mainProjectName+".UnitTest";
		UnitTestProjectGuid = Guid.NewGuid();
		SolutionName = solutionName;
		SolutionGuid = Guid.NewGuid();
		BuildItemsGuid = Guid.NewGuid();
		ToolsItemsGuid = Guid.NewGuid();
	}
	
	public SolutionSetting(string mainProjectName) : this(mainProjectName, mainProjectName){}
	
	public string MainProjectName {get; set;}
	public string UnitTestProjectName {get; set;}
	public string SolutionName {get; set;}
	public Guid MainProjectGuid {get; set;}
	public Guid UnitTestProjectGuid {get; set;}
	public Guid SolutionGuid {get; set;}
	public Guid BuildItemsGuid {get; set;}
	public Guid ToolsItemsGuid {get; set;}
}

public void CreateSolutionFiles(DirectoryPath target, DirectoryPath templatePath, SolutionSetting setting)
{
		CreateSolution16File(target, templatePath, setting); 

		var mainProjectSetting = new ProjectSetting(repositoryName, solutionSetting.MainProjectGuid);
		CreateProject(target, templatePath, mainProjectSetting);
		CreateNuspec(target, templatePath, solutionSetting);
}


public void CreateSolution16File(DirectoryPath target, DirectoryPath templatePath, SolutionSetting setting)
{
	// Solution
	var solutionTemplateFilePath = templatePath.Combine("Source").GetFilePath("Solution.16.template");
	if(!System.IO.File.Exists(solutionTemplateFilePath.FullPath))
	{
		return;
	}
	
	var solutionFilePath = target.Combine("Source").GetFilePath(setting.SolutionName + ".sln");
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

public void CreateSolutionUnitTest16(DirectoryPath target, SolutionSetting setting)
{
	// Solution
	var solutionTemplateFilePath = target.Combine("Source").GetFilePath("Solution.16.UnitTest.template");
	if(!System.IO.File.Exists(solutionTemplateFilePath.FullPath))
	{
		return;
	}
	
	var solutionFilePath = target.Combine("Source").GetFilePath(setting.SolutionName + ".sln");
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
	solutionStringBuilder.Replace("%UnitTest_Project_Name%", setting.UnitTestProjectName);
	solutionStringBuilder.Replace("%UnitTest_Project_GUID%", setting.UnitTestProjectGuid.ToString());
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
	
	var solutionFilePath = target.Combine("Source").GetFilePath(setting.SolutionName + ".sln");
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