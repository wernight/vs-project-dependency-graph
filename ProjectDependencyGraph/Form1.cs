using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Clifton.Collections.Generic;
using Clifton.Tools.Strings;

namespace ProjectDependencyGraph
{
    public partial class Form1 : Form
    {
        private bool _asTree = true;
        private List<string> _errors;
        private DiagnosticDictionary<string, Project> _projectMap;
        private List<Project> _projects;

        public Form1()
        {
            InitializeComponent();
        }

        protected void ParseSolution(string filename)
        {
            _errors = new List<string>();
            _projectMap = new DiagnosticDictionary<string, Project>("Project Map");
            _projects = new List<Project>();
            var sol = new Solution();
            sol.Read(filename);

            foreach (var kvp in sol.ProjectPaths)
            {
                var proj = new Project {Name = kvp.Key};
                proj.Read(kvp.Value);
                _projects.Add(proj);
                _projectMap[kvp.Key] = proj;
            }

            _projects.Sort();

            foreach (Project p in _projects)
            {
                foreach (string refProjName in p.ReferencedProjects.Keys)
                {
                    Project refProject;

                    if (_projectMap.TryGetValue(refProjName, out refProject))
                    {
                        p.Dependencies.Add(refProject);
                    }
                    else
                    {
                        _errors.Add("Could not find the project " + refProjName + " to add to the dependency list of " +
                                    p.Name);
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
            var tnRoot = new TreeNode("Projects");
            PopulateNewLevel(tnRoot, _projects);
            tvProjects.Nodes.Clear();
            tvProjects.Nodes.Add(tnRoot);
        }

        /// <summary>
        /// Given a project, what projects reference this project?
        /// </summary>
        protected void PopulateDependencyOf()
        {
            var tnRoot = new TreeNode("Projects");
            PopulateDependencyOfProjects(tnRoot, _projects);
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
                var tn = new TreeNode(p.Name + "  (" + p.TargetVersion + ")") {Tag = p};
                node.Nodes.Add(tn);

                if (_asTree)
                {
                    PopulateNewLevel(tn, p.Dependencies);
                }
                else
                {
                    // flatten the dependency hierarchy, removing duplicates
                    var sortedProjects = new List<Project>();
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
                var tn = new TreeNode(p.Name + "  (" + p.TargetVersion + ")") {Tag = p};
                node.Nodes.Add(tn);
            }
        }

        protected void PopulateDependencyOfProjects(TreeNode node, ICollection<Project> projects)
        {
            foreach (Project p in projects)
            {
                var tn = new TreeNode(p.Name + "  (" + p.TargetVersion + ")") {Tag = p};
                node.Nodes.Add(tn);
                var dependencies = new List<Project>();

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
                    var tn2 = new TreeNode(pdep.Name + "  (" + p.TargetVersion + ")") {Tag = pdep};
                    tn.Nodes.Add(tn2);
                }
            }
        }

        private void rbTree_CheckedChanged(object sender, EventArgs e)
        {
            _asTree = true;
            PopulateTreeView();
        }

        private void rbFlat_CheckedChanged(object sender, EventArgs e)
        {
            _asTree = false;
            PopulateTreeView();
        }

        private void btnLoadSolution_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                {
                    Filter = "sln files (*.sln)|*.sln",
                    RestoreDirectory = true
                };
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                // Set the form's caption (replacing previous filename)
                Text = StringHelpers.LeftOf(Text, '-').Trim() + " - " + Path.GetFileName(ofd.FileName);
                ParseSolution(ofd.FileName);
                PopulateTreeView();

                if (_errors.Count > 0)
                {
                    var sb = new StringBuilder();

                    foreach (string err in _errors)
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
            // Generate the digraph.
            bool simplified =
                MessageBox.Show("Render the simplified version skipping implied dependencies?", "Simplified graph?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes;

            string digraph = GenerateDigraph(simplified);

            Clipboard.SetText(digraph);

            // Find GraphViz
            string graphvizDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Graphviz2.22\bin");
            if (Directory.Exists(graphvizDir))
            {
                // Dotty
                string filename = Path.GetTempFileName() + ".dot";
                File.WriteAllText(filename, digraph);

                ExecuteWait(Path.Combine(graphvizDir, "dotty.exe"), filename);

                // Dot
                string outputImagePath = Path.Combine(Path.GetTempPath(), "ProjectDependencyGraph.png");
                if (File.Exists(outputImagePath))
                    File.Delete(outputImagePath);

                // Show the result with the default editor.
                ExecuteWait(Path.Combine(graphvizDir, "dot.exe"), "-Tpng " + filename + " -o " + outputImagePath);
                Process.Start(outputImagePath);
            }
            else
            {
                MessageBox.Show(
                    "GraphViz doesn't seem to be installed on your local machine under the expected folder: " +
                    Environment.NewLine + graphvizDir + Environment.NewLine + Environment.NewLine +
                    "The GraphViz source code has been copied to your clipboard. You may go to http://graphviz-dev.appspot.com/ to render it there.");
            }
        }

        /// <summary>
        /// Generate the digraph code.
        /// </summary>
        /// <param name="simplified">True to remove implied references: If A depends on B and C, and B also depends on C (directly or indirectly), then don't display the dependency of A on C.</param>
        /// <returns></returns>
        private string GenerateDigraph(bool simplified = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph G {");

            foreach (Project p in _projects)
            {
                sb.AppendFormat("  {0}", p.Name.Quote());
                if (p.OutputType != "Library")
                    sb.Append(" [style=filled]");
                sb.AppendLine(";");
            }

            foreach (Project p in _projects)
            {
                foreach (var r in p.ReferencedProjects.Keys)
                {
                    if (simplified)
                    {
                        // Given that 'p' depends on 'r', look if there is another dependency that depends
                        // directly or indirectly on 'r' too.
                        Project refProject;
                        if (_projectMap.TryGetValue(r, out refProject))
                        {
                            // Skip this referenced project.
                            if (p.Dependencies.Any(
                                dependency => dependency != refProject && dependency.References(refProject)))
                                continue;
                        }
                    }

                    sb.AppendFormat("  {0} -> {1};", p.Name.Quote(), r.Quote());
                    sb.AppendLine();
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void ExecuteWait(string fileName, string arguments)
        {
            using (Process process = Process.Start(fileName, arguments))
            {
                if (process == null)
                    throw new Exception("Failed to create a new process for: " + fileName);
                process.WaitForExit();
            }
        }
    }
}