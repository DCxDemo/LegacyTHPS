namespace ThpsTrigEd
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.nodeInfo = new System.Windows.Forms.TextBox();
            this.visButton = new System.Windows.Forms.Button();
            this.nodesList = new System.Windows.Forms.ListBox();
            this.linksList = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.parentLinksList = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legacyTHPSDiscordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandListEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baddyScriptEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.railClusterList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.parsingModeBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nodeTypeBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toggleEditButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            // 
            // nodeInfo
            // 
            this.nodeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nodeInfo.Location = new System.Drawing.Point(248, 167);
            this.nodeInfo.Multiline = true;
            this.nodeInfo.Name = "nodeInfo";
            this.nodeInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.nodeInfo.Size = new System.Drawing.Size(449, 358);
            this.nodeInfo.TabIndex = 1;
            this.nodeInfo.Text = resources.GetString("nodeInfo.Text");
            // 
            // visButton
            // 
            this.visButton.Location = new System.Drawing.Point(592, 51);
            this.visButton.Name = "visButton";
            this.visButton.Size = new System.Drawing.Size(105, 23);
            this.visButton.TabIndex = 2;
            this.visButton.Text = "Visualize";
            this.visButton.UseVisualStyleBackColor = true;
            this.visButton.Click += new System.EventHandler(this.visButton_Click);
            // 
            // nodesList
            // 
            this.nodesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nodesList.FormattingEnabled = true;
            this.nodesList.Location = new System.Drawing.Point(12, 51);
            this.nodesList.Name = "nodesList";
            this.nodesList.Size = new System.Drawing.Size(230, 472);
            this.nodesList.TabIndex = 3;
            this.nodesList.SelectedIndexChanged += new System.EventHandler(this.nodesList_SelectedIndexChanged);
            // 
            // linksList
            // 
            this.linksList.FormattingEnabled = true;
            this.linksList.Location = new System.Drawing.Point(248, 51);
            this.linksList.Name = "linksList";
            this.linksList.Size = new System.Drawing.Size(166, 108);
            this.linksList.TabIndex = 4;
            this.linksList.SelectedIndexChanged += new System.EventHandler(this.linksList_SelectedIndexChanged);
            this.linksList.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(592, 80);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Search checksum";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // parentLinksList
            // 
            this.parentLinksList.FormattingEnabled = true;
            this.parentLinksList.Location = new System.Drawing.Point(420, 51);
            this.parentLinksList.Name = "parentLinksList";
            this.parentLinksList.Size = new System.Drawing.Size(166, 108);
            this.parentLinksList.TabIndex = 7;
            this.parentLinksList.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.legacyTHPSDiscordToolStripMenuItem,
            this.scriptingToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(881, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Import TRG...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // legacyTHPSDiscordToolStripMenuItem
            // 
            this.legacyTHPSDiscordToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.legacyTHPSDiscordToolStripMenuItem.Name = "legacyTHPSDiscordToolStripMenuItem";
            this.legacyTHPSDiscordToolStripMenuItem.Size = new System.Drawing.Size(127, 20);
            this.legacyTHPSDiscordToolStripMenuItem.Text = "LegacyTHPS Discord";
            this.legacyTHPSDiscordToolStripMenuItem.Click += new System.EventHandler(this.legacyTHPSDiscordToolStripMenuItem_Click);
            // 
            // scriptingToolStripMenuItem
            // 
            this.scriptingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commandListEditorToolStripMenuItem,
            this.baddyScriptEditorToolStripMenuItem});
            this.scriptingToolStripMenuItem.Name = "scriptingToolStripMenuItem";
            this.scriptingToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.scriptingToolStripMenuItem.Text = "Scripting";
            // 
            // commandListEditorToolStripMenuItem
            // 
            this.commandListEditorToolStripMenuItem.Name = "commandListEditorToolStripMenuItem";
            this.commandListEditorToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.commandListEditorToolStripMenuItem.Text = "Command list editor";
            this.commandListEditorToolStripMenuItem.Click += new System.EventHandler(this.commandListEditorToolStripMenuItem_Click);
            // 
            // baddyScriptEditorToolStripMenuItem
            // 
            this.baddyScriptEditorToolStripMenuItem.Enabled = false;
            this.baddyScriptEditorToolStripMenuItem.Name = "baddyScriptEditorToolStripMenuItem";
            this.baddyScriptEditorToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.baddyScriptEditorToolStripMenuItem.Text = "Baddy script editor";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // railClusterList
            // 
            this.railClusterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.railClusterList.FormattingEnabled = true;
            this.railClusterList.Location = new System.Drawing.Point(703, 51);
            this.railClusterList.Name = "railClusterList";
            this.railClusterList.Size = new System.Drawing.Size(166, 472);
            this.railClusterList.TabIndex = 11;
            this.railClusterList.SelectedIndexChanged += new System.EventHandler(this.listBox4_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(248, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "This node links to:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(417, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "This node is linked from:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(700, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Rail clusters (doesnt work):";
            // 
            // parsingModeBox
            // 
            this.parsingModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parsingModeBox.FormattingEnabled = true;
            this.parsingModeBox.Items.AddRange(new object[] {
            "THPS2",
            "THPS1",
            "MHPB"});
            this.parsingModeBox.Location = new System.Drawing.Point(592, 138);
            this.parsingModeBox.Name = "parsingModeBox";
            this.parsingModeBox.Size = new System.Drawing.Size(105, 21);
            this.parsingModeBox.TabIndex = 15;
            this.parsingModeBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(592, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Parsing mode:";
            // 
            // nodeTypeBox
            // 
            this.nodeTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nodeTypeBox.FormattingEnabled = true;
            this.nodeTypeBox.Items.AddRange(new object[] {
            "THPS2",
            "THPS1",
            "MHPB"});
            this.nodeTypeBox.Location = new System.Drawing.Point(70, 24);
            this.nodeTypeBox.Name = "nodeTypeBox";
            this.nodeTypeBox.Size = new System.Drawing.Size(172, 21);
            this.nodeTypeBox.TabIndex = 17;
            this.nodeTypeBox.SelectedValueChanged += new System.EventHandler(this.comboBox2_SelectedValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 21);
            this.button1.TabIndex = 18;
            this.button1.Text = "All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(248, 167);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(449, 358);
            this.propertyGrid1.TabIndex = 19;
            // 
            // toggleEditButton
            // 
            this.toggleEditButton.Location = new System.Drawing.Point(592, 167);
            this.toggleEditButton.Name = "toggleEditButton";
            this.toggleEditButton.Size = new System.Drawing.Size(105, 23);
            this.toggleEditButton.TabIndex = 20;
            this.toggleEditButton.Text = "Toggle edit pane";
            this.toggleEditButton.UseVisualStyleBackColor = true;
            this.toggleEditButton.Click += new System.EventHandler(this.button5_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 543);
            this.Controls.Add(this.toggleEditButton);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nodeTypeBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.parsingModeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.railClusterList);
            this.Controls.Add(this.parentLinksList);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.linksList);
            this.Controls.Add(this.nodesList);
            this.Controls.Add(this.visButton);
            this.Controls.Add(this.nodeInfo);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.TextBox nodeInfo;
        private System.Windows.Forms.Button visButton;
        private System.Windows.Forms.ListBox nodesList;
        private System.Windows.Forms.ListBox linksList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox parentLinksList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ListBox railClusterList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem legacyTHPSDiscordToolStripMenuItem;
        private System.Windows.Forms.ComboBox parsingModeBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox nodeTypeBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button toggleEditButton;
        private System.Windows.Forms.ToolStripMenuItem scriptingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandListEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem baddyScriptEditorToolStripMenuItem;
    }
}

