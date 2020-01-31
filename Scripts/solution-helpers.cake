///////////////////////////////////////////////////////////////////////////////
// Solution Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public class VisualStudioSolution
{
	public VisualStudioSolution(VisualStudioProject mainProject, VisualStudioProject unitTestProject = null, string solutionName = null, string solutionType = "Default")
	{
		MainProject = mainProject;
		MainProjectName = mainProject.ProjectName;
		MainProjectGuid = mainProject.ProjectGuid;

		if (unitTestProject != null)
		{
			UnitTestProject = unitTestProject;
			UnitTestProjectName = unitTestProject.ProjectName;
			UnitTestProjectGuid = unitTestProject.ProjectGuid;
		}

		if (solutionName == null)
		{
			SolutionName = mainProject.ProjectName;
		}
		else
		{
			SolutionName = solutionName;
		}
		
		SolutionGuid = Guid.NewGuid();
		BuildItemsGuid = Guid.NewGuid();
		ToolsItemsGuid = Guid.NewGuid();
	}
	
	public VisualStudioProject MainProject {get; set;} = null;
	public VisualStudioProject UnitTestProject {get; set;} = null;
	public string MainProjectName {get; set;}
	public string UnitTestProjectName {get; set;}
	public string SolutionName {get; set;}
	public string SolutionType {get; set;}
	public Guid MainProjectGuid {get; set;}
	public Guid UnitTestProjectGuid {get; set;}
	public Guid SolutionGuid {get; set;}
	public Guid BuildItemsGuid {get; set;}
	public Guid ToolsItemsGuid {get; set;}
}

public void CreateVisualStudioSolution16Files(
	DirectoryPath target, 
	DirectoryPath templatePath, 
	VisualStudioSolution solution,
	Repository repository)
{
	var solutionDirectoryPath = target.Combine(repository.SourceFolder);
	Information("Source Path {0}", solutionDirectoryPath);

	var solutionTemplateDirectoryPath = templatePath.Combine("VisualStudio");
		
	if (solution.UnitTestProject == null)
	{
		CreateSolution16File(solutionDirectoryPath, solutionTemplateDirectoryPath, solution); 
	}
	else
	{
		var testsPath = target.Combine(repository.TestsFolder);
		CreateSolutionUnitTest16File(solutionDirectoryPath, solutionTemplateDirectoryPath, solution);
		CreateProject(testsPath, solutionTemplateDirectoryPath, solution.UnitTestProject);
	}

	CreateProject(solutionDirectoryPath, solutionTemplateDirectoryPath, solution.MainProject);
	CreateNuspec(solutionDirectoryPath, solutionTemplateDirectoryPath, solution);

	CreateSolutionFile("README.md", "Solution.README.md.template", solutionDirectoryPath, solutionTemplateDirectoryPath, solution);

	CopyFileFromTemplate("stylecop.json", "stylecop.json.template", solutionDirectoryPath, solutionTemplateDirectoryPath);
	CopyFileFromTemplate("CodeMaid.config", "CodeMaid.config.template", solutionDirectoryPath, solutionTemplateDirectoryPath);
	CopyFileFromTemplate("public.snk", "public.snk.template", solutionDirectoryPath, solutionTemplateDirectoryPath);
	CopyFileFromTemplate(solution.SolutionName + ".v3.ncrunchsolution", "Solution.16.v3.ncrunchsolution.template", solutionDirectoryPath, solutionTemplateDirectoryPath);
	CopyFileFromTemplate(solution.SolutionName + ".ruleset", "Solution.16.ruleset.template", solutionDirectoryPath, solutionTemplateDirectoryPath);
	CopyFileFromTemplate(solution.SolutionName + ".DotSettings", "Solution.16.DotSettings.template", solutionDirectoryPath, solutionTemplateDirectoryPath);
}

public void CreateSolution16File(DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	Information("Create Solution 16");
	CreateSolutionFile("Solution.16.template", target, templatePath, solution);
}

public void CreateSolutionUnitTest16File(DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	Information("Create Solution 16 with Unitest");
	CreateSolutionFile("Solution.16.UnitTest.template", target, templatePath, solution);
}

public void CreateSolution15File(DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	Information("Create Solution 15");
	CreateSolutionFile("Solution.15.template", target, templatePath, solution);
}

public void CreateSolutionFile(string solutionTemplateFile, DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	CreateSolutionFile(solution.SolutionName + ".sln", solutionTemplateFile, target, templatePath, solution);
}


public void CreateSolutionFile(string targetFile, string templateFile, DirectoryPath target, DirectoryPath templatePath, VisualStudioSolution solution)
{
	Information("Create {0}", targetFile);
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
	CreateFileFromTemplate(targetFile, templateFile, replaces, target, templatePath);
}

