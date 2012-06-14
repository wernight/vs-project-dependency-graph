namespace ProjectDependencyGraph
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tvProjects = new System.Windows.Forms.TreeView();
            this.rbTree = new System.Windows.Forms.RadioButton();
            this.rbFlat = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadSolution = new System.Windows.Forms.Button();
            this.tvDependencyOf = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.ckSynchronize = new System.Windows.Forms.CheckBox();
            this.btnRender = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tvProjects
            // 
            this.tvProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvProjects.HideSelection = false;
            this.tvProjects.Location = new System.Drawing.Point(13, 82);
            this.tvProjects.Name = "tvProjects";
            this.tvProjects.Size = new System.Drawing.Size(289, 437);
            this.tvProjects.TabIndex = 0;
            this.tvProjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProjects_AfterSelect);
            // 
            // rbTree
            // 
            this.rbTree.AutoSize = true;
            this.rbTree.Checked = true;
            this.rbTree.Location = new System.Drawing.Point(168, 59);
            this.rbTree.Name = "rbTree";
            this.rbTree.Size = new System.Drawing.Size(47, 17);
            this.rbTree.TabIndex = 1;
            this.rbTree.TabStop = true;
            this.rbTree.Text = "Tree";
            this.rbTree.UseVisualStyleBackColor = true;
            this.rbTree.CheckedChanged += new System.EventHandler(this.rbTree_CheckedChanged);
            // 
            // rbFlat
            // 
            this.rbFlat.AutoSize = true;
            this.rbFlat.Location = new System.Drawing.Point(233, 59);
            this.rbFlat.Name = "rbFlat";
            this.rbFlat.Size = new System.Drawing.Size(69, 17);
            this.rbFlat.TabIndex = 2;
            this.rbFlat.Text = "Flattened";
            this.rbFlat.UseVisualStyleBackColor = true;
            this.rbFlat.CheckedChanged += new System.EventHandler(this.rbFlat_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Project Depends On:";
            // 
            // btnLoadSolution
            // 
            this.btnLoadSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSolution.Location = new System.Drawing.Point(528, 12);
            this.btnLoadSolution.Name = "btnLoadSolution";
            this.btnLoadSolution.Size = new System.Drawing.Size(105, 23);
            this.btnLoadSolution.TabIndex = 4;
            this.btnLoadSolution.Text = "&Load Solution";
            this.btnLoadSolution.UseVisualStyleBackColor = true;
            this.btnLoadSolution.Click += new System.EventHandler(this.btnLoadSolution_Click);
            // 
            // tvDependencyOf
            // 
            this.tvDependencyOf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvDependencyOf.HideSelection = false;
            this.tvDependencyOf.Location = new System.Drawing.Point(344, 82);
            this.tvDependencyOf.Name = "tvDependencyOf";
            this.tvDependencyOf.Size = new System.Drawing.Size(289, 437);
            this.tvDependencyOf.TabIndex = 5;
            this.tvDependencyOf.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDependencyOf_AfterSelect);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(344, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Project Is Dependency Of:";
            // 
            // ckSynchronize
            // 
            this.ckSynchronize.AutoSize = true;
            this.ckSynchronize.Location = new System.Drawing.Point(263, 18);
            this.ckSynchronize.Name = "ckSynchronize";
            this.ckSynchronize.Size = new System.Drawing.Size(114, 17);
            this.ckSynchronize.TabIndex = 7;
            this.ckSynchronize.Text = "<-- Synchronize -->";
            this.ckSynchronize.UseVisualStyleBackColor = true;
            // 
            // btnRender
            // 
            this.btnRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRender.Location = new System.Drawing.Point(528, 42);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(105, 23);
            this.btnRender.TabIndex = 8;
            this.btnRender.Text = "&Render";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 531);
            this.Controls.Add(this.btnRender);
            this.Controls.Add(this.ckSynchronize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tvDependencyOf);
            this.Controls.Add(this.btnLoadSolution);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbFlat);
            this.Controls.Add(this.rbTree);
            this.Controls.Add(this.tvProjects);
            this.Name = "Form1";
            this.Text = "VS2008 Project Dependency Graph";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView tvProjects;
		private System.Windows.Forms.RadioButton rbTree;
		private System.Windows.Forms.RadioButton rbFlat;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnLoadSolution;
		private System.Windows.Forms.TreeView tvDependencyOf;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox ckSynchronize;
		private System.Windows.Forms.Button btnRender;
	}
}

