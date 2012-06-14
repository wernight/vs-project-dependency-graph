using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProjectDependencyGraph
{
    public class Project : IComparable<Project>
    {
        private readonly List<Project> _dependencies;
        private readonly Dictionary<string, string> _referencedProjects;

        public Project()
        {
            _referencedProjects = new Dictionary<string, string>();
            _dependencies = new List<Project>();
            TargetVersion = "v2.0"; // I assume this is the default.
        }

        public string Name { get; set; }
        public string TargetVersion { get; set; }
        public Guid Guid { get; set; }

        public Dictionary<string, string> ReferencedProjects
        {
            get { return _referencedProjects; }
        }

        public List<Project> Dependencies
        {
            get { return _dependencies; }
        }

        #region IComparable<Project> Members

        /// <summary>
        /// Required for sorting.
        /// </summary>
        public int CompareTo(Project other)
        {
            return Name.CompareTo(other.Name);
        }

        #endregion

        public void Read(string filename)
        {
            // Example:
            // <Project ToolsVersion="3.5" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
            //      <ItemGroup>
            //			<ProjectReference Include="..\Cx.Attributes\Cx.Attributes.csproj">
            //				<Project>{EFDBD81C-64BE-47F3-905E-7618B61BD224}</Project>
            //				<Name>Cx.Attributes</Name>
            //			</ProjectReference>
            XDocument xdoc = XDocument.Load(filename);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            IEnumerable<XElement> tfw = xdoc.Root.Elements(ns + "PropertyGroup").Elements(ns + "TargetFrameworkVersion");

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
                string projPath = Path.Combine(Path.GetDirectoryName(filename), projRef.Path);
                _referencedProjects.Add(projRef.Name, projPath);
            }
        }
    }
}