﻿using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using FastColoredTextBoxNS;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace ThpsQScriptEd
{
    partial class MainForm
    {

        //autogenerated

        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpHasValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpHashInQBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearGlobalCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validateSymbolCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.parseNodeArrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bruteTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grepScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grepEnfgapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.hideScriptsListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortOfManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legacyThpsGithubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legacyThpsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInNotepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openScriptsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripFiller = new System.Windows.Forms.ToolStripStatusLabel();
            this.QBlevelbox = new System.Windows.Forms.ToolStripStatusLabel();
            this.checksumHelper = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scriptList = new System.Windows.Forms.ListBox();
            this.codeBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.codeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.editorToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.openInNotepadToolStripMenuItem,
            this.openScriptsFolderToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(778, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.ToolTipText = "Only saves source as a Q file";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.ShortcutKeyDisplayString = "F5";
            this.compileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.compileToolStripMenuItem.Text = "Compile";
            this.compileToolStripMenuItem.ToolTipText = "Compiles QB file and saves source as a Q file";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(183, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpHasValuesToolStripMenuItem,
            this.dumpHashInQBToolStripMenuItem,
            this.clearGlobalCacheToolStripMenuItem,
            this.validateSymbolCacheToolStripMenuItem,
            this.toolStripSeparator1,
            this.parseNodeArrayToolStripMenuItem,
            this.bruteTestToolStripMenuItem,
            this.grepToolStripMenuItem,
            this.toolStripMenuItem2,
            this.settingsToolStripMenuItem,
            this.rollbackToolStripMenuItem,
            this.searchTextToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // dumpHasValuesToolStripMenuItem
            // 
            this.dumpHasValuesToolStripMenuItem.Name = "dumpHasValuesToolStripMenuItem";
            this.dumpHasValuesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.dumpHasValuesToolStripMenuItem.Text = "Dump hashes in text";
            this.dumpHasValuesToolStripMenuItem.Click += new System.EventHandler(this.dumpHasValuesToolStripMenuItem_Click);
            // 
            // dumpHashInQBToolStripMenuItem
            // 
            this.dumpHashInQBToolStripMenuItem.Name = "dumpHashInQBToolStripMenuItem";
            this.dumpHashInQBToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.dumpHashInQBToolStripMenuItem.Text = "Dump hashes in QB";
            this.dumpHashInQBToolStripMenuItem.Click += new System.EventHandler(this.dumpHashInQBToolStripMenuItem_Click);
            // 
            // clearGlobalCacheToolStripMenuItem
            // 
            this.clearGlobalCacheToolStripMenuItem.Name = "clearGlobalCacheToolStripMenuItem";
            this.clearGlobalCacheToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.clearGlobalCacheToolStripMenuItem.Text = "Clear global cache";
            this.clearGlobalCacheToolStripMenuItem.Click += new System.EventHandler(this.clearGlobalCacheToolStripMenuItem_Click);
            // 
            // validateSymbolCacheToolStripMenuItem
            // 
            this.validateSymbolCacheToolStripMenuItem.Name = "validateSymbolCacheToolStripMenuItem";
            this.validateSymbolCacheToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.validateSymbolCacheToolStripMenuItem.Text = "Validate symbol cache";
            this.validateSymbolCacheToolStripMenuItem.Click += new System.EventHandler(this.validateSymbolCacheToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(188, 6);
            // 
            // parseNodeArrayToolStripMenuItem
            // 
            this.parseNodeArrayToolStripMenuItem.Enabled = false;
            this.parseNodeArrayToolStripMenuItem.Name = "parseNodeArrayToolStripMenuItem";
            this.parseNodeArrayToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.parseNodeArrayToolStripMenuItem.Text = "Parse nodeArray";
            // 
            // bruteTestToolStripMenuItem
            // 
            this.bruteTestToolStripMenuItem.Enabled = false;
            this.bruteTestToolStripMenuItem.Name = "bruteTestToolStripMenuItem";
            this.bruteTestToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.bruteTestToolStripMenuItem.Text = "Brute test";
            // 
            // grepToolStripMenuItem
            // 
            this.grepToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grepScriptToolStripMenuItem,
            this.grepEnfgapToolStripMenuItem});
            this.grepToolStripMenuItem.Name = "grepToolStripMenuItem";
            this.grepToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.grepToolStripMenuItem.Text = "Grep";
            // 
            // grepScriptToolStripMenuItem
            // 
            this.grepScriptToolStripMenuItem.Name = "grepScriptToolStripMenuItem";
            this.grepScriptToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.grepScriptToolStripMenuItem.Text = "Scripts";
            this.grepScriptToolStripMenuItem.Click += new System.EventHandler(this.grepScriptToolStripMenuItem_Click);
            // 
            // grepEnfgapToolStripMenuItem
            // 
            this.grepEnfgapToolStripMenuItem.Name = "grepEnfgapToolStripMenuItem";
            this.grepEnfgapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.grepEnfgapToolStripMenuItem.Text = "Gaps";
            this.grepEnfgapToolStripMenuItem.Click += new System.EventHandler(this.grepEnfgapToolStripMenuItem_Click_1);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(188, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click_1);
            // 
            // rollbackToolStripMenuItem
            // 
            this.rollbackToolStripMenuItem.Enabled = false;
            this.rollbackToolStripMenuItem.Name = "rollbackToolStripMenuItem";
            this.rollbackToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.rollbackToolStripMenuItem.Text = "Rollback";
            // 
            // searchTextToolStripMenuItem
            // 
            this.searchTextToolStripMenuItem.Name = "searchTextToolStripMenuItem";
            this.searchTextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchTextToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.searchTextToolStripMenuItem.Text = "Search text...";
            this.searchTextToolStripMenuItem.Click += new System.EventHandler(this.searchTextToolStripMenuItem_Click);
            // 
            // editorToolStripMenuItem
            // 
            this.editorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordWrapToolStripMenuItem,
            this.fontToolStripMenuItem,
            this.toolStripComboBox1,
            this.hideScriptsListToolStripMenuItem});
            this.editorToolStripMenuItem.Name = "editorToolStripMenuItem";
            this.editorToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.editorToolStripMenuItem.Text = "Editor";
            // 
            // wordWrapToolStripMenuItem
            // 
            this.wordWrapToolStripMenuItem.CheckOnClick = true;
            this.wordWrapToolStripMenuItem.Name = "wordWrapToolStripMenuItem";
            this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.wordWrapToolStripMenuItem.Text = "Word wrap";
            this.wordWrapToolStripMenuItem.ToolTipText = resources.GetString("wordWrapToolStripMenuItem.ToolTipText");
            this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem_Click_1);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.fontToolStripMenuItem.Text = "Font...";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.AutoSize = false;
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.DropDownWidth = 121;
            this.toolStripComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // hideScriptsListToolStripMenuItem
            // 
            this.hideScriptsListToolStripMenuItem.CheckOnClick = true;
            this.hideScriptsListToolStripMenuItem.Name = "hideScriptsListToolStripMenuItem";
            this.hideScriptsListToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.hideScriptsListToolStripMenuItem.Text = "Hide scripts list";
            this.hideScriptsListToolStripMenuItem.Click += new System.EventHandler(this.hideScriptsListToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortOfManualToolStripMenuItem,
            this.legacyThpsGithubToolStripMenuItem,
            this.legacyThpsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // sortOfManualToolStripMenuItem
            // 
            this.sortOfManualToolStripMenuItem.Name = "sortOfManualToolStripMenuItem";
            this.sortOfManualToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.sortOfManualToolStripMenuItem.Text = "Documentation";
            this.sortOfManualToolStripMenuItem.Click += new System.EventHandler(this.sortOfManualToolStripMenuItem_Click);
            // 
            // legacyThpsGithubToolStripMenuItem
            // 
            this.legacyThpsGithubToolStripMenuItem.Name = "legacyThpsGithubToolStripMenuItem";
            this.legacyThpsGithubToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.legacyThpsGithubToolStripMenuItem.Text = "LegacyThps Github";
            this.legacyThpsGithubToolStripMenuItem.Click += new System.EventHandler(this.legacyThpsGithubToolStripMenuItem_Click);
            // 
            // legacyThpsToolStripMenuItem
            // 
            this.legacyThpsToolStripMenuItem.Name = "legacyThpsToolStripMenuItem";
            this.legacyThpsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.legacyThpsToolStripMenuItem.Text = "LegacyThps Discord";
            this.legacyThpsToolStripMenuItem.Click += new System.EventHandler(this.legacyThpsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeyDisplayString = "F1";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openInNotepadToolStripMenuItem
            // 
            this.openInNotepadToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.openInNotepadToolStripMenuItem.Name = "openInNotepadToolStripMenuItem";
            this.openInNotepadToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.openInNotepadToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.openInNotepadToolStripMenuItem.Text = "Send to external editor";
            this.openInNotepadToolStripMenuItem.Click += new System.EventHandler(this.openInNotepadToolStripMenuItem_Click);
            // 
            // openScriptsFolderToolStripMenuItem
            // 
            this.openScriptsFolderToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.openScriptsFolderToolStripMenuItem.Name = "openScriptsFolderToolStripMenuItem";
            this.openScriptsFolderToolStripMenuItem.Size = new System.Drawing.Size(119, 20);
            this.openScriptsFolderToolStripMenuItem.Text = "Open scripts folder";
            this.openScriptsFolderToolStripMenuItem.Click += new System.EventHandler(this.openScriptsFolderToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoText,
            this.statusStripFiller,
            this.QBlevelbox,
            this.checksumHelper});
            this.statusStrip1.Location = new System.Drawing.Point(0, 478);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(778, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoText
            // 
            this.infoText.DoubleClickEnabled = true;
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(0, 17);
            this.infoText.DoubleClick += new System.EventHandler(this.infoText_DoubleClick);
            // 
            // statusStripFiller
            // 
            this.statusStripFiller.Name = "statusStripFiller";
            this.statusStripFiller.Size = new System.Drawing.Size(643, 17);
            this.statusStripFiller.Spring = true;
            // 
            // QBlevelbox
            // 
            this.QBlevelbox.Name = "QBlevelbox";
            this.QBlevelbox.Size = new System.Drawing.Size(0, 17);
            // 
            // checksumHelper
            // 
            this.checksumHelper.Name = "checksumHelper";
            this.checksumHelper.Size = new System.Drawing.Size(118, 17);
            this.checksumHelper.Text = "toolStripStatusLabel1";
            this.checksumHelper.Click += new System.EventHandler(this.checksumHelper_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scriptList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.codeBox);
            this.splitContainer1.Size = new System.Drawing.Size(778, 454);
            this.splitContainer1.SplitterDistance = 190;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 99;
            this.splitContainer1.TabStop = false;
            // 
            // scriptList
            // 
            this.scriptList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scriptList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptList.FormattingEnabled = true;
            this.scriptList.ItemHeight = 15;
            this.scriptList.Location = new System.Drawing.Point(0, 0);
            this.scriptList.Name = "scriptList";
            this.scriptList.Size = new System.Drawing.Size(190, 454);
            this.scriptList.TabIndex = 99;
            this.scriptList.TabStop = false;
            this.scriptList.UseTabStops = false;
            this.scriptList.DoubleClick += new System.EventHandler(this.scriptList_DoubleClick);
            // 
            // codeBox
            // 
            this.codeBox.AllowDrop = false;
            this.codeBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.codeBox.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(" +
    "?<range>:)\\s*(?<range>[^;]+);";
            this.codeBox.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.codeBox.BackBrush = null;
            this.codeBox.CharHeight = 15;
            this.codeBox.CharWidth = 7;
            this.codeBox.CurrentLineColor = System.Drawing.Color.Gray;
            this.codeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.codeBox.DefaultMarkerSize = 8;
            this.codeBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.codeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.codeBox.IsReplaceMode = false;
            this.codeBox.LeftBracket = '{';
            this.codeBox.LeftBracket2 = '[';
            this.codeBox.Location = new System.Drawing.Point(0, 0);
            this.codeBox.Name = "codeBox";
            this.codeBox.Paddings = new System.Windows.Forms.Padding(0);
            this.codeBox.RightBracket = '}';
            this.codeBox.RightBracket2 = ']';
            this.codeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.codeBox.ServiceColors = null;
            this.codeBox.Size = new System.Drawing.Size(583, 454);
            this.codeBox.TabIndex = 100;
            this.codeBox.TabLength = 2;
            this.codeBox.Zoom = 100;
            this.codeBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.codeBox_MouseUp_1);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 500);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ThpsQScriptEd";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.codeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoText;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInNotepadToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dumpHasValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        public static System.Windows.Forms.ProgressBar progressBar1;
        private ToolStripMenuItem dumpHashInQBToolStripMenuItem;
        private ToolStripMenuItem sortOfManualToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private SplitContainer splitContainer1;
        private ListBox scriptList;
        private ToolStripMenuItem hideScriptsListToolStripMenuItem;
        private ToolStripMenuItem clearGlobalCacheToolStripMenuItem;
        private ToolStripMenuItem rollbackToolStripMenuItem;
        private ToolStripStatusLabel statusStripFiller;
        private ToolStripStatusLabel QBlevelbox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem parseNodeArrayToolStripMenuItem;
        private ToolStripMenuItem bruteTestToolStripMenuItem;
        private ToolStripMenuItem grepToolStripMenuItem;
        private ToolStripMenuItem grepScriptToolStripMenuItem;
        private ToolStripMenuItem grepEnfgapToolStripMenuItem;
        private FastColoredTextBoxNS.FastColoredTextBox codeBox;
        private ToolStripMenuItem searchTextToolStripMenuItem;
        private ToolStripMenuItem validateSymbolCacheToolStripMenuItem;
        private ToolStripMenuItem openScriptsFolderToolStripMenuItem;
        private ToolStripStatusLabel checksumHelper;
        private ToolStripMenuItem legacyThpsToolStripMenuItem;
        private ToolStripMenuItem legacyThpsGithubToolStripMenuItem;
    }
}

