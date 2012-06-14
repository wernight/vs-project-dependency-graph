using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Clifton.Collections.Generic;
using Clifton.Tools.Strings;

namespace ProjectDependencyGraph
{
	public partial class Form1 : Form
	{
		protected List<Project> projects;
		protected List<string> errors;
		protected DiagnosticDictionary<string, Project> projectMap;
		protected bool asTree = true;

		public Form1()
		{
			InitializeComponent();
		}

		protected void ParseSolution(string filename)
		{
			errors = new List<string>();
			projectMap = new DiagnosticDictionary<string, Project>("Project Map");
			projects = new List<Project>();
			Solution sol = new Solution();
			sol.Read(filename);

			foreach (KeyValuePair<string, string> kvp in sol.ProjectPaths)
			{
				Project proj = new Project();
				proj.Name = kvp.Key;
				proj.Read(kvp.Value);
				projects.Add(proj);
				projectMap[kvp.Key] = proj;
			}

			projects.Sort();

			foreach (Project p in projects)
			{
				foreach (string refProjName in p.ReferencedProjects.Keys)
				{

					Project refProject;

					if (projectMap.TryGetValue(refProjName, out refProject))
					{
						p.Dependencies.Add(refProject);
					}
					else
					{
						errors.Add("Could not find the project " + refProjName + " to add to the dependency list of " + p.Name);
					}
				}

				p.Dependencies.Sort();
			}
		}

		protected void PopulateTreeView()
		{
			PopulateDependsOn();
			PopulateDependencyOf();
		}

		/// <summary>
		/// Given a project, what projects does this project reference?
		/// </summary>
		protected void PopulateDependsOn()
		{
			TreeNode tnRoot = new TreeNode("Projects");
			PopulateNewLevel(tnRoot, projects);
			tvProjects.Nodes.Clear();
			tvProjects.Nodes.Add(tnRoot);
		}

		/// <summary>
		/// Given a project, what projects reference this project?
		/// </summary>
		protected void PopulateDependencyOf()
		{
			TreeNode tnRoot = new TreeNode("Projects");
			PopulateDependencyOfProjects(tnRoot, projects);
			tvDependencyOf.Nodes.Clear();
			tvDependencyOf.Nodes.Add(tnRoot);
		}

		/// <summary>
		/// Sets up initial project name and first level of dependencies.
		/// From there, child dependencies are either added hierarchically or flattened.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="projects"></param>
		protected void PopulateNewLevel(TreeNode node, ICollection<Project> projects)
		{
			foreach (Project p in projects)
			{
				TreeNode tn = new TreeNode(p.Name+"  ("+p.TargetVersion+")");
				tn.Tag = p;
				node.Nodes.Add(tn);

				if (asTree)
				{
					PopulateNewLevel(tn, p.Dependencies);
				}
				else
				{
					// flatten the dependency hierarchy, removing duplicates
					List<Project> sortedProjects = new List<Project>();
					PopulateSameLevel(p.Dependencies, sortedProjects);
					sortedProjects.Sort();
					PopulateTree(tn, sortedProjects);
				}
			}
		}

		protected void PopulateSameLevel(ICollection<Project> projects, List<Project> sortedProjects)
		{
			foreach (Project p in projects)
			{
				if (!(sortedProjects.Contains(p)))
				{
					sortedProjects.Add(p);
					PopulateSameLevel(p.Dependencies, sortedProjects);
				}
			}
		}

		protected void PopulateTree(TreeNode node, List<Project> sortedProjects)
		{
			foreach (Project p in sortedProjects)
			{
				TreeNode tn = new TreeNode(p.Name + "  (" + p.TargetVersion + ")");
				tn.Tag = p;
				node.Nodes.Add(tn);
			}
		}

		protected void PopulateDependencyOfProjects(TreeNode node, ICollection<Project> projects)
		{
			foreach (Project p in projects)
			{
				TreeNode tn = new TreeNode(p.Name + "  (" + p.TargetVersion + ")");
				tn.Tag = p;
				node.Nodes.Add(tn);
				List<Project> dependencies = new List<Project>();

				foreach (Project pdep in projects)
				{
					foreach (Project dep in pdep.Dependencies)
					{
						if (p.Name == dep.Name)
						{
							if (!dependencies.Contains(pdep))
							{
								dependencies.Add(pdep);
							}
						}
					}
				}

				foreach (Project pdep in dependencies)
				{
					TreeNode tn2 = new TreeNode(pdep.Name + "  (" + p.TargetVersion + ")");
					tn2.Tag = pdep;
					tn.Nodes.Add(tn2);
				}
			}
		}

		private void rbTree_CheckedChanged(object sender, EventArgs e)
		{
			asTree = true;
			PopulateTreeView();
		}

		private void rbFlat_CheckedChanged(object sender, EventArgs e)
		{
			asTree = false;
			PopulateTreeView();
		}

		private void btnLoadSolution_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "sln files (*.sln)|*.sln";
			ofd.RestoreDirectory = true;
			DialogResult res = ofd.ShowDialog();

			if (res == DialogResult.OK)
			{
				// Set the form's caption (replacing previous filename)
				Text = StringHelpers.LeftOf(Text, '-').Trim() + " - " + Path.GetFileName(ofd.FileName);
				ParseSolution(ofd.FileName);
				PopulateTreeView();

				if (errors.Count > 0)
				{
					StringBuilder sb = new StringBuilder();

					foreach (string err in errors)
					{
						sb.Append(err);
						sb.Append("\r\n");
					}

					MessageBox.Show(sb.ToString(), "There Are Errors", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void tvProjects_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (ckSynchronize.Checked)
			{
				object project = tvProjects.SelectedNode.Tag;

				foreach (TreeNode tn in tvDependencyOf.Nodes[0].Nodes)
				{
					if (tn.Tag == project)
					{
						tvDependencyOf.SelectedNode = tn;
						tn.Expand();
						break;
					}
				}
			}
		}

		private void tvDependencyOf_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (ckSynchronize.Checked)
			{
				object project = tvDependencyOf.SelectedNode.Tag;

				foreach (TreeNode tn in tvProjects.Nodes[0].Nodes)
				{
					if (tn.Tag == project)
					{
						tvProjects.SelectedNode = tn;
						tn.Expand();
						break;
					}
				}
			}
		}

		private void btnRender_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();
			sb.AppendLine("digraph G {");
	
			foreach (var p in projects)
			{
				if (p.ReferencedProjects.Count == 0)
				{
					sb.AppendFormat("  {0};{1}", p.Name.Quote(), Environment.NewLine);
				}
				else foreach (var r in p.ReferencedProjects)
				{
					sb.AppendFormat("  {0} -> {1};{2}", p.Name.Quote(), r.Key.Quote(), Environment.NewLine);
				}
			}

			sb.AppendLine("}");

			string filename = Path.GetTempFileName() + ".dot";
			File.WriteAllText(filename, sb.ToString());

			string progFilePath = Environment.GetEnvironmentVariable("ProgramFiles");
			string progFilex86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

			if (progFilex86 != null)
			{
				progFilePath = progFilex86;
			}

			Process.Start(progFilePath+@"\Graphviz2.22\bin\dotty.exe", filename);

			File.Delete(@"c:\temp\graph.png");
			var pr = Process.Start(progFilePath+@"\Graphviz2.22\bin\dot.exe", "-Tpng " + filename + @" -o c:\temp\graph.png");

			while (!pr.HasExited)
			{
				Thread.Sleep(100);
			}

			Process.Start(@"c:\temp\graph.png");
			Clipboard.SetText(sb.ToString());
		}
	}
}
