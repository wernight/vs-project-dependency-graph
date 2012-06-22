using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProjectDependencyGraph
{
    public class Project : IComparable<Project>
    {
        public Project()
        {
            ReferencedProjects = new Dictionary<string, string>();
            Dependencies = new List<Project>();
            TargetVersion = "v2.0"; // I assume this is the default.
        }

        public string Name { get; set; }
        public string TargetVersion { get; set; }
        public Guid Guid { get; set; }
        public string OutputType { get; set; }

        /// <summary>
        /// Dictionary of key the project name and value the project path.
        /// </summary>
        public Dictionary<String, String> ReferencedProjects { get; private set; }

        /// <summary>
        /// Referenced projects (when found).
        /// </summary>
        public List<Project> Dependencies { get; private set; }

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

            OutputType = xdoc.Root.Elements(ns + "PropertyGroup").Elements(ns + "OutputType").First().Value;

            foreach (var projRef in from el in xdoc.Root.Elements(ns + "ItemGroup").Elements(ns + "ProjectReference")
                                    select new
                                        {
                                            Path = el.Attribute("Include").Value,
                                            Name = el.Element(ns + "Name").Value,
                                        })
            {
                string projPath = Path.Combine(Path.GetDirectoryName(filename), projRef.Path);
                ReferencedProjects.Add(projRef.Name, projPath);
            }
        }

        /// <summary>
        /// Tell if a project depends directly or indirectly on another project.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool References(Project other)
        {
            foreach (Project referencedProject in Dependencies)
            {
                if (referencedProject == other)
                    return true;

                // Recurse (look for indirect dependencies).
                if (referencedProject.References(other))
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
                return true;
            if (ReferenceEquals(obj, null))
                return false;
            Project other = obj as Project;
            if (other == null)
                return false;

            if (Guid != other.Guid)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}