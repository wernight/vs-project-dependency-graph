using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ProjectDependencyGraph
{
	public class Project : IComparable<Project>
	{
		protected Dictionary<string, string> referencedProjects;
		protected List<Project> dependencies;

		public string Name { get; set; }
		public string TargetVersion { get; set; }
		public Guid Guid { get; set; }
		
		public Dictionary<string, string> ReferencedProjects
		{
			get { return referencedProjects; }
		}

		public List<Project> Dependencies
		{
			get { return dependencies; }
		}

		public Project()
		{
			referencedProjects = new Dictionary<string, string>();
			dependencies = new List<Project>();
			TargetVersion = "v2.0";		// I assume this is the default.
		}


		// Example:
		// <Project ToolsVersion="3.5" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
		//      <ItemGroup>
		//			<ProjectReference Include="..\Cx.Attributes\Cx.Attributes.csproj">
		//				<Project>{EFDBD81C-64BE-47F3-905E-7618B61BD224}</Project>
		//				<Name>Cx.Attributes</Name>
		//			</ProjectReference>

		public void Read(string filename)
		{
			XDocument xdoc = XDocument.Load(filename);
			XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

			var tfw = xdoc.Root.Elements(ns + "PropertyGroup").Elements(ns + "TargetFrameworkVersion");

			if (tfw.Count() > 0)
			{
				TargetVersion = tfw.First().Value;
			}

			Guid = new Guid(xdoc.Root.Elements(ns + "PropertyGroup").Elements(ns + "ProjectGuid").First().Value);

			foreach (var projRef in from el in xdoc.Root.Elements(ns + "ItemGroup").Elements(ns + "ProjectReference")
									select new
									{
										Path = el.Attribute("Include").Value,
										Name = el.Element(ns + "Name").Value,
									})
			{
				string projPath = Path.GetDirectoryName(filename);
				projPath = Path.Combine(projPath, projRef.Path);
				referencedProjects.Add(projRef.Name, projPath);
			}
		}

		/// <summary>
		///  Required for sorting.
		/// </summary>
		public int CompareTo(Project other)
		{
			int ret = Name.CompareTo(other.Name);

			return ret;
		}
	}
}
