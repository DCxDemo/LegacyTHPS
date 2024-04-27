namespace bon_tool
{
    partial class BonTextureReplacerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialListBox = new System.Windows.Forms.ListBox();
            this.bonMaterialControl1 = new bon_tool.BonMaterialControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openBONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.texturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceFromPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legacyTHPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialListBox
            // 
            this.materialListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.materialListBox.FormattingEnabled = true;
            this.materialListBox.Location = new System.Drawing.Point(3, 27);
            this.materialListBox.Name = "materialListBox";
            this.materialListBox.Size = new System.Drawing.Size(175, 368);
            this.materialListBox.TabIndex = 5;
            this.materialListBox.SelectedIndexChanged += new System.EventHandler(this.materialListBox_SelectedIndexChanged);
            // 
            // bonMaterialControl1
            // 
            this.bonMaterialControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bonMaterialControl1.Location = new System.Drawing.Point(184, 27);
            this.bonMaterialControl1.Material = null;
            this.bonMaterialControl1.Name = "bonMaterialControl1";
            this.bonMaterialControl1.Size = new System.Drawing.Size(490, 380);
            this.bonMaterialControl1.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openBONToolStripMenuItem,
            this.texturesToolStripMenuItem,
            this.legacyTHPSToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(677, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openBONToolStripMenuItem
            // 
            this.openBONToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.openBONToolStripMenuItem.Name = "openBONToolStripMenuItem";
            this.openBONToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.openBONToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(118, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // texturesToolStripMenuItem
            // 
            this.texturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractTexturesToolStripMenuItem,
            this.replaceFromPathToolStripMenuItem});
            this.texturesToolStripMenuItem.Name = "texturesToolStripMenuItem";
            this.texturesToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.texturesToolStripMenuItem.Text = "Textures";
            // 
            // extractTexturesToolStripMenuItem
            // 
            this.extractTexturesToolStripMenuItem.Name = "extractTexturesToolStripMenuItem";
            this.extractTexturesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.extractTexturesToolStripMenuItem.Text = "Extract all";
            this.extractTexturesToolStripMenuItem.Click += new System.EventHandler(this.extractTexturesToolStripMenuItem_Click);
            // 
            // replaceFromPathToolStripMenuItem
            // 
            this.replaceFromPathToolStripMenuItem.Name = "replaceFromPathToolStripMenuItem";
            this.replaceFromPathToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.replaceFromPathToolStripMenuItem.Text = "Mass replace";
            this.replaceFromPathToolStripMenuItem.Click += new System.EventHandler(this.replaceFromPathToolStripMenuItem_Click);
            // 
            // legacyTHPSToolStripMenuItem
            // 
            this.legacyTHPSToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.legacyTHPSToolStripMenuItem.Name = "legacyTHPSToolStripMenuItem";
            this.legacyTHPSToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.legacyTHPSToolStripMenuItem.Text = "LegacyTHPS";
            this.legacyTHPSToolStripMenuItem.Click += new System.EventHandler(this.legacyTHPSToolStripMenuItem_Click);
            // 
            // BonTextureReplacerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialListBox);
            this.Controls.Add(this.bonMaterialControl1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "BonTextureReplacerControl";
            this.Size = new System.Drawing.Size(677, 410);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox materialListBox;
        private BonMaterialControl bonMaterialControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openBONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem texturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractTexturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceFromPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legacyTHPSToolStripMenuItem;
    }
}
