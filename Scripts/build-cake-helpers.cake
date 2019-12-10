///////////////////////////////////////////////////////////////////////////////
// Build Cake Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public void CreateBuildCake(DirectoryPath target, RepositorySetting repositorySetting)
{
	var repositoryName = repositorySetting.Name;
	// build.cake
	var buildCakeTemplateFilePath = target.GetFilePath("build.cake.template");
	if(!System.IO.File.Exists(buildCakeTemplateFilePath.FullPath))
	{
		return;
	}
	
	var buildCakeFilePath = target.GetFilePath("build.cake");
	if (System.IO.File.Exists(buildCakeFilePath.FullPath))
	{
		Information("Remove build.cake Template File {0}", buildCakeFilePath.FullPath);
		System.IO.File.Delete(buildCakeFilePath.FullPath);
	}
	
	Information("Create build.cake File {0}", buildCakeFilePath);
	var buildCakeString = System.IO.File.ReadAllText(buildCakeTemplateFilePath.FullPath);
	var buildCakeStringBuilder = new StringBuilder(buildCakeString);
	buildCakeStringBuilder.Replace("%Solution_Name%", repositoryName);
	buildCakeStringBuilder.Replace("%Main_Project_Name%", repositoryName);
	System.IO.File.WriteAllText(buildCakeFilePath.FullPath, buildCakeStringBuilder.ToString());
	
	if (System.IO.File.Exists(buildCakeTemplateFilePath.FullPath))
	{
		Information("Remove build.cake Template File {0}", buildCakeTemplateFilePath.FullPath);
		//System.IO.File.Delete(buildCakeTemplateFilePath.FullPath);
	}
}