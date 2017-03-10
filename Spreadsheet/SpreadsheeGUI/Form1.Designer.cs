namespace SpreadsheetGUI
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
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_New = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openfileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lblCellContents = new System.Windows.Forms.Label();
            this.txtCellContents = new System.Windows.Forms.TextBox();
            this.txtCellName = new System.Windows.Forms.TextBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.spreadsheetPanel1 = new SSGui.SpreadsheetPanel();
            this.menuBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_File,
            this.menu_Help});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(1354, 24);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuBar";
            // 
            // menu_File
            // 
            this.menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_New,
            this.menuItem_Save,
            this.menuItem_Open,
            this.menuItem_Close});
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new System.Drawing.Size(37, 20);
            this.menu_File.Text = "File";
            // 
            // menuItem_New
            // 
            this.menuItem_New.Name = "menuItem_New";
            this.menuItem_New.Size = new System.Drawing.Size(103, 22);
            this.menuItem_New.Text = "New";
            this.menuItem_New.Click += new System.EventHandler(this.menuItem_New_Click);
            // 
            // menuItem_Save
            // 
            this.menuItem_Save.Name = "menuItem_Save";
            this.menuItem_Save.Size = new System.Drawing.Size(103, 22);
            this.menuItem_Save.Text = "Save";
            this.menuItem_Save.Click += new System.EventHandler(this.menuItem_Save_Click);
            // 
            // menuItem_Open
            // 
            this.menuItem_Open.Name = "menuItem_Open";
            this.menuItem_Open.Size = new System.Drawing.Size(103, 22);
            this.menuItem_Open.Text = "Open";
            this.menuItem_Open.Click += new System.EventHandler(this.menuItem_Open_Click);
            // 
            // menuItem_Close
            // 
            this.menuItem_Close.Name = "menuItem_Close";
            this.menuItem_Close.Size = new System.Drawing.Size(103, 22);
            this.menuItem_Close.Text = "Close";
            this.menuItem_Close.Click += new System.EventHandler(this.menuItem_Close_Click);
            // 
            // menu_Help
            // 
            this.menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menu_Help.Name = "menu_Help";
            this.menu_Help.Size = new System.Drawing.Size(44, 20);
            this.menu_Help.Text = "Help";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // openfileDialog
            // 
            this.openfileDialog.DefaultExt = "ss";
            this.openfileDialog.Filter = "Spreadsheet File (*.ss)|*.ss|All files (*.*)|*.*";
            this.openfileDialog.Tag = "";
            this.openfileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.fileDialog1_FileOk);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "ss";
            this.saveFileDialog.Filter = "Spreadsheet Files (*.ss)|*.ss|All files (*.*)|*.*";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // lblCellContents
            // 
            this.lblCellContents.AutoSize = true;
            this.lblCellContents.Location = new System.Drawing.Point(45, 6);
            this.lblCellContents.Name = "lblCellContents";
            this.lblCellContents.Size = new System.Drawing.Size(72, 13);
            this.lblCellContents.TabIndex = 1;
            this.lblCellContents.Text = "Cell Contents:";
            // 
            // txtCellContents
            // 
            this.txtCellContents.Location = new System.Drawing.Point(123, 3);
            this.txtCellContents.Name = "txtCellContents";
            this.txtCellContents.Size = new System.Drawing.Size(365, 20);
            this.txtCellContents.TabIndex = 1;
            this.txtCellContents.TextChanged += new System.EventHandler(this.txtCellContents_TextChanged);
            // 
            // txtCellName
            // 
            this.txtCellName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCellName.Location = new System.Drawing.Point(3, 6);
            this.txtCellName.Name = "txtCellName";
            this.txtCellName.ReadOnly = true;
            this.txtCellName.Size = new System.Drawing.Size(26, 13);
            this.txtCellName.TabIndex = 3;
            this.txtCellName.TabStop = false;
            this.txtCellName.Text = "A1";
            this.txtCellName.TextChanged += new System.EventHandler(this.txtCellName_TextChanged);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(522, 3);
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.Size = new System.Drawing.Size(151, 20);
            this.txtValue.TabIndex = 4;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(494, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "=";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtValue);
            this.panel1.Controls.Add(this.txtCellName);
            this.panel1.Controls.Add(this.txtCellContents);
            this.panel1.Controls.Add(this.lblCellContents);
            this.panel1.Location = new System.Drawing.Point(96, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(910, 24);
            this.panel1.TabIndex = 3;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.AutoScroll = true;
            this.spreadsheetPanel1.AutoSize = true;
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.cellContent = null;
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 24);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1354, 689);
            this.spreadsheetPanel1.TabIndex = 2;
            this.spreadsheetPanel1.SelectionChanged += new SSGui.SelectionChangedHandler(this.spreadsheetPanel1_SelectionChanged);
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1354, 733);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.Text = "Spreadsheet";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem menu_File;
        private System.Windows.Forms.ToolStripMenuItem menuItem_New;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Save;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Close;
        private System.Windows.Forms.OpenFileDialog openfileDialog;
        private System.Windows.Forms.ToolStripMenuItem menu_Help;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label lblCellContents;
        private System.Windows.Forms.TextBox txtCellContents;
        private System.Windows.Forms.TextBox txtCellName;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private SSGui.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}

