using System;
using System.Collections.Generic;
using System.IO;
using Clifton.Tools.Strings;

namespace ProjectDependencyGraph
{
	public class Solution
	{
		protected Dictionary<string, string> projectPaths;

		public Dictionary<string, string> ProjectPaths 
		{
			get { return projectPaths; }
		}

		public Solution()
		{
			projectPaths = new Dictionary<string, string>();
		}

		public void Read(string filename)
		{
            // Example:
            // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "NumericKeypadComponent", "NumericKeypadComponent\NumericKeypadComponent.csproj", "{05D03020-4604-4CE9-8F99-E1D93ADEDF15}"
            // EndProject
            string solPath = Path.GetDirectoryName(filename);
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("Project"))
                    {
                        string projName = StringHelpers.Between(line, '=', ',');
                        projName = projName.Replace('"', ' ').Trim();
                        string projPath = StringHelpers.RightOf(line, ',');
                        projPath = StringHelpers.Between(projPath, '"', '"');
                        projPath = projPath.Replace('"', ' ').Trim();

                        // virtual solution folders appear as projects but don't end with .csproj
                        // gawd, imagine what happens if someone creates a foo.csproj virt. solution folder!
                        if (projPath.EndsWith(".csproj"))
                        {
                            // assume relative paths.  Probably not a good assumption
                            projPath = Path.Combine(solPath, projPath);

                            // we don't allow projects with the same name, even if different paths.
                            projectPaths.Add(projName, projPath);
                        }
                    }
                }
            }
		}
	}
}
