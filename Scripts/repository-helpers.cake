///////////////////////////////////////////////////////////////////////////////
// Repository Helpers
///////////////////////////////////////////////////////////////////////////////
// Author: Martin Egli

public string GetMyRepositoryName()
	{
		Information("Remote Name: {0}", remote.Name);
		Information("Remote Url: {0}", remote.Url);
		var url = new System.Uri(remote.Url);
		var path = url.AbsolutePath;
		var startIndex = path.LastIndexOf(@"/") + 1;
		var repositoryName = "";
		var index = path.LastIndexOf(@".git");
		if (index > startIndex)
		{
			var length = index - startIndex;
			repositoryName = path.Substring(startIndex, length);
		}
		else
		{
			var length = path.Length - startIndex;
			repositoryName = path.Substring(startIndex, length);
		}
		Information("Repository Name: {0}", repositoryName);
		return repositoryName;
	}