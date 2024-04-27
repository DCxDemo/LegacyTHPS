namespace ThpsFontEd
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importerTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFNT1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByCharToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getCharsetSpacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setButtonsCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unswizzleVadruToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLowercaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBBGRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepButtonsOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add1pxAlphaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rearrangeGlyphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bleed1pxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleX2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legacyThpsDiscordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tHPSFontsHuntToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.fontBox = new System.Windows.Forms.PropertyGrid();
            this.infoBox = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.versionbox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.newFromTrueTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(412, 417);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(439, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // previewBox
            // 
            this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox.Location = new System.Drawing.Point(412, 27);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(445, 333);
            this.previewBox.TabIndex = 3;
            this.previewBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.legacyThpsDiscordToolStripMenuItem,
            this.tHPSFontsHuntToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(863, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFromTrueTypeToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importerTestToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveFNT1ToolStripMenuItem,
            this.exportPNGToolStripMenuItem,
            this.exportBitmapToolStripMenuItem,
            this.exportCharsetToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open font...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importerTestToolStripMenuItem
            // 
            this.importerTestToolStripMenuItem.Name = "importerTestToolStripMenuItem";
            this.importerTestToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importerTestToolStripMenuItem.Text = "Import charset";
            this.importerTestToolStripMenuItem.Click += new System.EventHandler(this.importerTestToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // saveFNT1ToolStripMenuItem
            // 
            this.saveFNT1ToolStripMenuItem.Name = "saveFNT1ToolStripMenuItem";
            this.saveFNT1ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveFNT1ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveFNT1ToolStripMenuItem.Text = "Save font...";
            this.saveFNT1ToolStripMenuItem.Click += new System.EventHandler(this.saveFNT1ToolStripMenuItem_Click);
            // 
            // exportPNGToolStripMenuItem
            // 
            this.exportPNGToolStripMenuItem.Name = "exportPNGToolStripMenuItem";
            this.exportPNGToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportPNGToolStripMenuItem.Text = "Export atlas";
            this.exportPNGToolStripMenuItem.Click += new System.EventHandler(this.exportPNGToolStripMenuItem_Click);
            // 
            // exportBitmapToolStripMenuItem
            // 
            this.exportBitmapToolStripMenuItem.Name = "exportBitmapToolStripMenuItem";
            this.exportBitmapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportBitmapToolStripMenuItem.Text = "Import atlas";
            this.exportBitmapToolStripMenuItem.Click += new System.EventHandler(this.exportBitmapToolStripMenuItem_Click);
            // 
            // exportCharsetToolStripMenuItem
            // 
            this.exportCharsetToolStripMenuItem.Name = "exportCharsetToolStripMenuItem";
            this.exportCharsetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportCharsetToolStripMenuItem.Text = "Export charset";
            this.exportCharsetToolStripMenuItem.Click += new System.EventHandler(this.exportCharsetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByCharToolStripMenuItem,
            this.getCharsetToolStripMenuItem,
            this.getCharsetSpacesToolStripMenuItem,
            this.setButtonsCharsetToolStripMenuItem,
            this.unswizzleVadruToolStripMenuItem,
            this.addLowercaseToolStripMenuItem,
            this.rGBBGRToolStripMenuItem,
            this.keepButtonsOnlyToolStripMenuItem,
            this.add1pxAlphaToolStripMenuItem,
            this.rearrangeGlyphsToolStripMenuItem,
            this.bleed1pxToolStripMenuItem,
            this.scaleX2ToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem,
            this.pasteFromClipboardToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.optionsToolStripMenuItem.Text = "Tools";
            // 
            // sortByCharToolStripMenuItem
            // 
            this.sortByCharToolStripMenuItem.Name = "sortByCharToolStripMenuItem";
            this.sortByCharToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.sortByCharToolStripMenuItem.Text = "Sort by char";
            this.sortByCharToolStripMenuItem.Click += new System.EventHandler(this.sortByCharToolStripMenuItem_Click);
            // 
            // getCharsetToolStripMenuItem
            // 
            this.getCharsetToolStripMenuItem.Name = "getCharsetToolStripMenuItem";
            this.getCharsetToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.getCharsetToolStripMenuItem.Text = "Get charset";
            this.getCharsetToolStripMenuItem.Click += new System.EventHandler(this.getCharsetToolStripMenuItem_Click);
            // 
            // getCharsetSpacesToolStripMenuItem
            // 
            this.getCharsetSpacesToolStripMenuItem.Name = "getCharsetSpacesToolStripMenuItem";
            this.getCharsetSpacesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.getCharsetSpacesToolStripMenuItem.Text = "Get charset + spaces";
            this.getCharsetSpacesToolStripMenuItem.Click += new System.EventHandler(this.getCharsetSpacesToolStripMenuItem_Click);
            // 
            // setButtonsCharsetToolStripMenuItem
            // 
            this.setButtonsCharsetToolStripMenuItem.Name = "setButtonsCharsetToolStripMenuItem";
            this.setButtonsCharsetToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.setButtonsCharsetToolStripMenuItem.Text = "Set buttons charset";
            this.setButtonsCharsetToolStripMenuItem.Click += new System.EventHandler(this.setButtonsCharsetToolStripMenuItem_Click);
            // 
            // unswizzleVadruToolStripMenuItem
            // 
            this.unswizzleVadruToolStripMenuItem.Name = "unswizzleVadruToolStripMenuItem";
            this.unswizzleVadruToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.unswizzleVadruToolStripMenuItem.Text = "Unswizzle vadru";
            this.unswizzleVadruToolStripMenuItem.Click += new System.EventHandler(this.unswizzleVadruToolStripMenuItem_Click);
            // 
            // addLowercaseToolStripMenuItem
            // 
            this.addLowercaseToolStripMenuItem.Name = "addLowercaseToolStripMenuItem";
            this.addLowercaseToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.addLowercaseToolStripMenuItem.Text = "Add lowercase";
            this.addLowercaseToolStripMenuItem.Click += new System.EventHandler(this.addLowercaseToolStripMenuItem_Click);
            // 
            // rGBBGRToolStripMenuItem
            // 
            this.rGBBGRToolStripMenuItem.Name = "rGBBGRToolStripMenuItem";
            this.rGBBGRToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.rGBBGRToolStripMenuItem.Text = "RGB <-> BGR";
            this.rGBBGRToolStripMenuItem.Click += new System.EventHandler(this.rGBBGRToolStripMenuItem_Click);
            // 
            // keepButtonsOnlyToolStripMenuItem
            // 
            this.keepButtonsOnlyToolStripMenuItem.Name = "keepButtonsOnlyToolStripMenuItem";
            this.keepButtonsOnlyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.keepButtonsOnlyToolStripMenuItem.Text = "Keep buttons only";
            this.keepButtonsOnlyToolStripMenuItem.Click += new System.EventHandler(this.keepButtonsOnlyToolStripMenuItem_Click);
            // 
            // add1pxAlphaToolStripMenuItem
            // 
            this.add1pxAlphaToolStripMenuItem.Name = "add1pxAlphaToolStripMenuItem";
            this.add1pxAlphaToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.add1pxAlphaToolStripMenuItem.Text = "Add 1px alpha";
            this.add1pxAlphaToolStripMenuItem.Click += new System.EventHandler(this.add1pxAlphaToolStripMenuItem_Click);
            // 
            // rearrangeGlyphsToolStripMenuItem
            // 
            this.rearrangeGlyphsToolStripMenuItem.Name = "rearrangeGlyphsToolStripMenuItem";
            this.rearrangeGlyphsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.rearrangeGlyphsToolStripMenuItem.Text = "Rearrange glyphs";
            this.rearrangeGlyphsToolStripMenuItem.Click += new System.EventHandler(this.rearrangeGlyphsToolStripMenuItem_Click);
            // 
            // bleed1pxToolStripMenuItem
            // 
            this.bleed1pxToolStripMenuItem.Name = "bleed1pxToolStripMenuItem";
            this.bleed1pxToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.bleed1pxToolStripMenuItem.Text = "Bleed 1px";
            this.bleed1pxToolStripMenuItem.Click += new System.EventHandler(this.bleed1pxToolStripMenuItem_Click);
            // 
            // scaleX2ToolStripMenuItem
            // 
            this.scaleX2ToolStripMenuItem.Name = "scaleX2ToolStripMenuItem";
            this.scaleX2ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.scaleX2ToolStripMenuItem.Text = "Scale x2";
            this.scaleX2ToolStripMenuItem.Click += new System.EventHandler(this.scaleX2ToolStripMenuItem_Click);
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
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // legacyThpsDiscordToolStripMenuItem
            // 
            this.legacyThpsDiscordToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.legacyThpsDiscordToolStripMenuItem.Name = "legacyThpsDiscordToolStripMenuItem";
            this.legacyThpsDiscordToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.legacyThpsDiscordToolStripMenuItem.Text = "LegacyThps Discord";
            this.legacyThpsDiscordToolStripMenuItem.Click += new System.EventHandler(this.legacyThpsDiscordToolStripMenuItem_Click);
            // 
            // tHPSFontsHuntToolStripMenuItem
            // 
            this.tHPSFontsHuntToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tHPSFontsHuntToolStripMenuItem.Name = "tHPSFontsHuntToolStripMenuItem";
            this.tHPSFontsHuntToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.tHPSFontsHuntToolStripMenuItem.Text = "THPS Fonts hunt";
            this.tHPSFontsHuntToolStripMenuItem.Click += new System.EventHandler(this.tHPSFontsHuntToolStripMenuItem_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(420, 366);
            this.trackBar1.Minimum = -5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(431, 45);
            this.trackBar1.TabIndex = 7;
            this.trackBar1.Value = 5;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // fontBox
            // 
            this.fontBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fontBox.Location = new System.Drawing.Point(12, 56);
            this.fontBox.Name = "fontBox";
            this.fontBox.Size = new System.Drawing.Size(166, 347);
            this.fontBox.TabIndex = 8;
            // 
            // infoBox
            // 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.infoBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.infoBox.FullRowSelect = true;
            this.infoBox.HideSelection = false;
            this.infoBox.Location = new System.Drawing.Point(184, 20);
            this.infoBox.MultiSelect = false;
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(222, 410);
            this.infoBox.TabIndex = 9;
            this.infoBox.UseCompatibleStateImageBehavior = false;
            this.infoBox.View = System.Windows.Forms.View.Details;
            this.infoBox.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Text";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Hex";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Pos";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Size";
            // 
            // versionbox
            // 
            this.versionbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.versionbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionbox.FormattingEnabled = true;
            this.versionbox.Items.AddRange(new object[] {
            "FNT0 (missing)",
            "FNT1",
            "FNT2",
            "FNT3 (missing)"});
            this.versionbox.Location = new System.Drawing.Point(12, 409);
            this.versionbox.Name = "versionbox";
            this.versionbox.Size = new System.Drawing.Size(166, 21);
            this.versionbox.TabIndex = 10;
            this.versionbox.SelectedIndexChanged += new System.EventHandler(this.versionbox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Font info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // newFromTrueTypeToolStripMenuItem
            // 
            this.newFromTrueTypeToolStripMenuItem.Name = "newFromTrueTypeToolStripMenuItem";
            this.newFromTrueTypeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newFromTrueTypeToolStripMenuItem.Text = "New from TrueType";
            this.newFromTrueTypeToolStripMenuItem.Click += new System.EventHandler(this.newFromTrueTypeToolStripMenuItem_Click);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // pasteFromClipboardToolStripMenuItem
            // 
            this.pasteFromClipboardToolStripMenuItem.Name = "pasteFromClipboardToolStripMenuItem";
            this.pasteFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.pasteFromClipboardToolStripMenuItem.Text = "Paste from clipboard";
            this.pasteFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.pasteFromClipboardToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 442);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.versionbox);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.fontBox);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Form1";
            this.Text = "ThpsFontEd";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFNT1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPNGToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.ToolStripMenuItem importerTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getCharsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getCharsetSpacesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCharsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setButtonsCharsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unswizzleVadruToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid fontBox;
        private System.Windows.Forms.ListView infoBox;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ComboBox versionbox;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem sortByCharToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem addLowercaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBBGRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepButtonsOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add1pxAlphaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legacyThpsDiscordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tHPSFontsHuntToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBitmapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rearrangeGlyphsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bleed1pxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scaleX2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFromTrueTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteFromClipboardToolStripMenuItem;
    }
}

