namespace ThpsQScriptEd
{
    partial class SettingsForm
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
            this.importBox = new System.Windows.Forms.GroupBox();
            this.fixIncorrectChecksumsCb = new System.Windows.Forms.CheckBox();
            this.useDegreesCb = new System.Windows.Forms.CheckBox();
            this.roundAnglesCb = new System.Windows.Forms.CheckBox();
            this.useCapsCb = new System.Windows.Forms.CheckBox();
            this.preferSymbolicCb = new System.Windows.Forms.CheckBox();
            this.useTabCb = new System.Windows.Forms.CheckBox();
            this.applyCosmeticsCb = new System.Windows.Forms.CheckBox();
            this.alwaysLoadSourceCb = new System.Windows.Forms.CheckBox();
            this.exportBox = new System.Windows.Forms.GroupBox();
            this.removeTrailNewlinesCb = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.forceTHUG2Cb = new System.Windows.Forms.CheckBox();
            this.useSymFileCb = new System.Windows.Forms.CheckBox();
            this.useOldLineCb = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.extEditor = new System.Windows.Forms.TextBox();
            this.extEditLabel = new System.Windows.Forms.Label();
            this.browseExtEditorButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.enableBackupsCb = new System.Windows.Forms.CheckBox();
            this.minQBlevelCb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scriptsPathBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.browseScriptsButton = new System.Windows.Forms.Button();
            this.importBox.SuspendLayout();
            this.exportBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // importBox
            // 
            this.importBox.Controls.Add(this.fixIncorrectChecksumsCb);
            this.importBox.Controls.Add(this.useDegreesCb);
            this.importBox.Controls.Add(this.roundAnglesCb);
            this.importBox.Controls.Add(this.useCapsCb);
            this.importBox.Controls.Add(this.preferSymbolicCb);
            this.importBox.Controls.Add(this.useTabCb);
            this.importBox.Controls.Add(this.applyCosmeticsCb);
            this.importBox.Location = new System.Drawing.Point(14, 14);
            this.importBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.importBox.Name = "importBox";
            this.importBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.importBox.Size = new System.Drawing.Size(233, 239);
            this.importBox.TabIndex = 0;
            this.importBox.TabStop = false;
            this.importBox.Text = "Import";
            // 
            // fixIncorrectChecksumsCb
            // 
            this.fixIncorrectChecksumsCb.AutoSize = true;
            this.fixIncorrectChecksumsCb.Location = new System.Drawing.Point(15, 177);
            this.fixIncorrectChecksumsCb.Name = "fixIncorrectChecksumsCb";
            this.fixIncorrectChecksumsCb.Size = new System.Drawing.Size(153, 19);
            this.fixIncorrectChecksumsCb.TabIndex = 9;
            this.fixIncorrectChecksumsCb.Text = "Fix incorrect checksums";
            this.fixIncorrectChecksumsCb.UseVisualStyleBackColor = true;
            this.fixIncorrectChecksumsCb.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged_1);
            // 
            // useDegreesCb
            // 
            this.useDegreesCb.AutoSize = true;
            this.useDegreesCb.Location = new System.Drawing.Point(15, 127);
            this.useDegreesCb.Name = "useDegreesCb";
            this.useDegreesCb.Size = new System.Drawing.Size(198, 19);
            this.useDegreesCb.TabIndex = 8;
            this.useDegreesCb.Text = "Show Angle values in degrees (°)";
            this.useDegreesCb.UseVisualStyleBackColor = true;
            this.useDegreesCb.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // roundAnglesCb
            // 
            this.roundAnglesCb.AutoSize = true;
            this.roundAnglesCb.Location = new System.Drawing.Point(15, 152);
            this.roundAnglesCb.Name = "roundAnglesCb";
            this.roundAnglesCb.Size = new System.Drawing.Size(207, 19);
            this.roundAnglesCb.TabIndex = 6;
            this.roundAnglesCb.Text = "Round angles (may lose precision)";
            this.roundAnglesCb.UseVisualStyleBackColor = true;
            this.roundAnglesCb.CheckedChanged += new System.EventHandler(this.roundAnglesCb_CheckedChanged);
            // 
            // useCapsCb
            // 
            this.useCapsCb.AutoSize = true;
            this.useCapsCb.Location = new System.Drawing.Point(15, 102);
            this.useCapsCb.Name = "useCapsCb";
            this.useCapsCb.Size = new System.Drawing.Size(134, 19);
            this.useCapsCb.TabIndex = 7;
            this.useCapsCb.Text = "Uppercase keywords";
            this.useCapsCb.UseVisualStyleBackColor = true;
            this.useCapsCb.CheckedChanged += new System.EventHandler(this.useCaps_CheckedChanged);
            // 
            // preferSymbolicCb
            // 
            this.preferSymbolicCb.AutoSize = true;
            this.preferSymbolicCb.Enabled = false;
            this.preferSymbolicCb.Location = new System.Drawing.Point(15, 77);
            this.preferSymbolicCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.preferSymbolicCb.Name = "preferSymbolicCb";
            this.preferSymbolicCb.Size = new System.Drawing.Size(161, 19);
            this.preferSymbolicCb.TabIndex = 7;
            this.preferSymbolicCb.Text = "Prefer symbolic operators";
            this.preferSymbolicCb.UseVisualStyleBackColor = true;
            this.preferSymbolicCb.CheckedChanged += new System.EventHandler(this.preferSymbolicCb_CheckedChanged);
            // 
            // useTabCb
            // 
            this.useTabCb.AutoSize = true;
            this.useTabCb.Location = new System.Drawing.Point(15, 52);
            this.useTabCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.useTabCb.Name = "useTabCb";
            this.useTabCb.Size = new System.Drawing.Size(137, 19);
            this.useTabCb.TabIndex = 0;
            this.useTabCb.Text = "Prefer tab for nesting";
            this.useTabCb.UseVisualStyleBackColor = true;
            this.useTabCb.CheckedChanged += new System.EventHandler(this.useTabCb_CheckedChanged);
            // 
            // applyCosmeticsCb
            // 
            this.applyCosmeticsCb.AutoSize = true;
            this.applyCosmeticsCb.Location = new System.Drawing.Point(15, 27);
            this.applyCosmeticsCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.applyCosmeticsCb.Name = "applyCosmeticsCb";
            this.applyCosmeticsCb.Size = new System.Drawing.Size(135, 19);
            this.applyCosmeticsCb.TabIndex = 0;
            this.applyCosmeticsCb.Text = "Apply cosmetic fixes";
            this.applyCosmeticsCb.UseVisualStyleBackColor = true;
            this.applyCosmeticsCb.CheckedChanged += new System.EventHandler(this.equalsWantsSpaceCb_CheckedChanged);
            // 
            // alwaysLoadSourceCb
            // 
            this.alwaysLoadSourceCb.AutoSize = true;
            this.alwaysLoadSourceCb.Location = new System.Drawing.Point(15, 47);
            this.alwaysLoadSourceCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.alwaysLoadSourceCb.Name = "alwaysLoadSourceCb";
            this.alwaysLoadSourceCb.Size = new System.Drawing.Size(198, 19);
            this.alwaysLoadSourceCb.TabIndex = 1;
            this.alwaysLoadSourceCb.Text = "Always load Q source if available";
            this.alwaysLoadSourceCb.UseVisualStyleBackColor = true;
            this.alwaysLoadSourceCb.CheckedChanged += new System.EventHandler(this.alwaysLoadSourceCb_CheckedChanged);
            // 
            // exportBox
            // 
            this.exportBox.Controls.Add(this.removeTrailNewlinesCb);
            this.exportBox.Controls.Add(this.checkBox1);
            this.exportBox.Controls.Add(this.forceTHUG2Cb);
            this.exportBox.Controls.Add(this.useSymFileCb);
            this.exportBox.Controls.Add(this.useOldLineCb);
            this.exportBox.Location = new System.Drawing.Point(255, 14);
            this.exportBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.exportBox.Name = "exportBox";
            this.exportBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.exportBox.Size = new System.Drawing.Size(233, 155);
            this.exportBox.TabIndex = 1;
            this.exportBox.TabStop = false;
            this.exportBox.Text = "Export";
            // 
            // removeTrailNewlinesCb
            // 
            this.removeTrailNewlinesCb.AutoSize = true;
            this.removeTrailNewlinesCb.Location = new System.Drawing.Point(14, 77);
            this.removeTrailNewlinesCb.Name = "removeTrailNewlinesCb";
            this.removeTrailNewlinesCb.Size = new System.Drawing.Size(162, 19);
            this.removeTrailNewlinesCb.TabIndex = 6;
            this.removeTrailNewlinesCb.Text = "Remove trailing line feeds";
            this.removeTrailNewlinesCb.UseVisualStyleBackColor = true;
            this.removeTrailNewlinesCb.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(14, 127);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 19);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Force THUG2+ IF clause";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // forceTHUG2Cb
            // 
            this.forceTHUG2Cb.AutoSize = true;
            this.forceTHUG2Cb.Enabled = false;
            this.forceTHUG2Cb.Location = new System.Drawing.Point(14, 102);
            this.forceTHUG2Cb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.forceTHUG2Cb.Name = "forceTHUG2Cb";
            this.forceTHUG2Cb.Size = new System.Drawing.Size(147, 19);
            this.forceTHUG2Cb.TabIndex = 4;
            this.forceTHUG2Cb.Text = "Force THUG+ randoms";
            this.forceTHUG2Cb.UseVisualStyleBackColor = true;
            // 
            // useSymFileCb
            // 
            this.useSymFileCb.AutoSize = true;
            this.useSymFileCb.Location = new System.Drawing.Point(14, 52);
            this.useSymFileCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.useSymFileCb.Name = "useSymFileCb";
            this.useSymFileCb.Size = new System.Drawing.Size(185, 19);
            this.useSymFileCb.TabIndex = 3;
            this.useSymFileCb.Text = "Save symbols in a separate file";
            this.useSymFileCb.UseVisualStyleBackColor = true;
            this.useSymFileCb.CheckedChanged += new System.EventHandler(this.useSymFileCb_CheckedChanged);
            // 
            // useOldLineCb
            // 
            this.useOldLineCb.AutoSize = true;
            this.useOldLineCb.Location = new System.Drawing.Point(14, 27);
            this.useOldLineCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.useOldLineCb.Name = "useOldLineCb";
            this.useOldLineCb.Size = new System.Drawing.Size(164, 19);
            this.useOldLineCb.TabIndex = 2;
            this.useOldLineCb.Text = "Use short new line symbol";
            this.useOldLineCb.UseVisualStyleBackColor = true;
            this.useOldLineCb.CheckedChanged += new System.EventHandler(this.useOldLineCb_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(390, 321);
            this.okButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(98, 24);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // extEditor
            // 
            this.extEditor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.extEditor.Location = new System.Drawing.Point(100, 292);
            this.extEditor.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.extEditor.Name = "extEditor";
            this.extEditor.Size = new System.Drawing.Size(286, 23);
            this.extEditor.TabIndex = 3;
            this.extEditor.TextChanged += new System.EventHandler(this.extEditor_TextChanged);
            // 
            // extEditLabel
            // 
            this.extEditLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.extEditLabel.AutoSize = true;
            this.extEditLabel.Location = new System.Drawing.Point(11, 296);
            this.extEditLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.extEditLabel.Name = "extEditLabel";
            this.extEditLabel.Size = new System.Drawing.Size(86, 15);
            this.extEditLabel.TabIndex = 4;
            this.extEditLabel.Text = "External editor:";
            // 
            // browseExtEditorButton
            // 
            this.browseExtEditorButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.browseExtEditorButton.Location = new System.Drawing.Point(390, 292);
            this.browseExtEditorButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.browseExtEditorButton.Name = "browseExtEditorButton";
            this.browseExtEditorButton.Size = new System.Drawing.Size(98, 23);
            this.browseExtEditorButton.TabIndex = 5;
            this.browseExtEditorButton.Text = "Browse...";
            this.browseExtEditorButton.UseVisualStyleBackColor = true;
            this.browseExtEditorButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enableBackupsCb);
            this.groupBox1.Controls.Add(this.alwaysLoadSourceCb);
            this.groupBox1.Location = new System.Drawing.Point(255, 175);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 78);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Behaviour";
            // 
            // enableBackupsCb
            // 
            this.enableBackupsCb.AutoSize = true;
            this.enableBackupsCb.Location = new System.Drawing.Point(15, 22);
            this.enableBackupsCb.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.enableBackupsCb.Name = "enableBackupsCb";
            this.enableBackupsCb.Size = new System.Drawing.Size(120, 19);
            this.enableBackupsCb.TabIndex = 7;
            this.enableBackupsCb.Text = "Backup Q sources";
            this.enableBackupsCb.UseVisualStyleBackColor = true;
            // 
            // minQBlevelCb
            // 
            this.minQBlevelCb.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.minQBlevelCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.minQBlevelCb.FormattingEnabled = true;
            this.minQBlevelCb.Items.AddRange(new object[] {
            "THPS3",
            "THPS4",
            "THUG1",
            "THUG2"});
            this.minQBlevelCb.Location = new System.Drawing.Point(132, 322);
            this.minQBlevelCb.Name = "minQBlevelCb";
            this.minQBlevelCb.Size = new System.Drawing.Size(94, 23);
            this.minQBlevelCb.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 326);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Minimum QB mode:";
            // 
            // scriptsPathBox
            // 
            this.scriptsPathBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.scriptsPathBox.Location = new System.Drawing.Point(100, 263);
            this.scriptsPathBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.scriptsPathBox.Name = "scriptsPathBox";
            this.scriptsPathBox.Size = new System.Drawing.Size(286, 23);
            this.scriptsPathBox.TabIndex = 10;
            this.scriptsPathBox.TextChanged += new System.EventHandler(this.scriptsPathBox_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 266);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Scripts folder:";
            // 
            // browseScriptsButton
            // 
            this.browseScriptsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.browseScriptsButton.Location = new System.Drawing.Point(390, 262);
            this.browseScriptsButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.browseScriptsButton.Name = "browseScriptsButton";
            this.browseScriptsButton.Size = new System.Drawing.Size(98, 23);
            this.browseScriptsButton.TabIndex = 12;
            this.browseScriptsButton.Text = "Browse...";
            this.browseScriptsButton.UseVisualStyleBackColor = true;
            this.browseScriptsButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 357);
            this.Controls.Add(this.browseScriptsButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.scriptsPathBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.minQBlevelCb);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.browseExtEditorButton);
            this.Controls.Add(this.extEditLabel);
            this.Controls.Add(this.extEditor);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.exportBox);
            this.Controls.Add(this.importBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QScripted settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.importBox.ResumeLayout(false);
            this.importBox.PerformLayout();
            this.exportBox.ResumeLayout(false);
            this.exportBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox importBox;
        private System.Windows.Forms.CheckBox alwaysLoadSourceCb;
        private System.Windows.Forms.CheckBox useTabCb;
        private System.Windows.Forms.CheckBox applyCosmeticsCb;
        private System.Windows.Forms.GroupBox exportBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox forceTHUG2Cb;
        private System.Windows.Forms.CheckBox useSymFileCb;
        private System.Windows.Forms.CheckBox useOldLineCb;
        private System.Windows.Forms.TextBox extEditor;
        private System.Windows.Forms.Label extEditLabel;
        private System.Windows.Forms.Button browseExtEditorButton;
        private System.Windows.Forms.CheckBox preferSymbolicCb;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox useCapsCb;
        private System.Windows.Forms.CheckBox roundAnglesCb;
        private System.Windows.Forms.CheckBox removeTrailNewlinesCb;
        private System.Windows.Forms.CheckBox useDegreesCb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox enableBackupsCb;
        private System.Windows.Forms.ComboBox minQBlevelCb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox fixIncorrectChecksumsCb;
        private System.Windows.Forms.TextBox scriptsPathBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button browseScriptsButton;
    }
}