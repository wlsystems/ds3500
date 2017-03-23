namespace PS8
{
    partial class BoggleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoggleForm));
            this.gameBoard = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.wordButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverBox = new System.Windows.Forms.TextBox();
            this.registerButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.cancelbutton1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuItem_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordBox = new System.Windows.Forms.TextBox();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.joinButton = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label_player1score = new System.Windows.Forms.Label();
            this.textBox_player1Score = new System.Windows.Forms.TextBox();
            this.label_player2score = new System.Windows.Forms.Label();
            this.textBox_player2Score = new System.Windows.Forms.TextBox();
            this.label_Timer = new System.Windows.Forms.Label();
            this.textBox_Timer = new System.Windows.Forms.TextBox();
            this.wordPanel = new System.Windows.Forms.RichTextBox();
            this.wordPanel2 = new System.Windows.Forms.RichTextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.gameBoard.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameBoard
            // 
            this.gameBoard.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gameBoard.ColumnCount = 5;
            this.gameBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gameBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gameBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gameBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.gameBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.gameBoard.Controls.Add(this.label2, 1, 0);
            this.gameBoard.Controls.Add(this.label1, 0, 0);
            this.gameBoard.Controls.Add(this.label3, 3, 0);
            this.gameBoard.Controls.Add(this.label4, 4, 0);
            this.gameBoard.Controls.Add(this.label5, 0, 1);
            this.gameBoard.Controls.Add(this.label6, 1, 1);
            this.gameBoard.Controls.Add(this.label7, 3, 1);
            this.gameBoard.Controls.Add(this.label8, 4, 1);
            this.gameBoard.Controls.Add(this.label9, 0, 3);
            this.gameBoard.Controls.Add(this.label10, 1, 3);
            this.gameBoard.Controls.Add(this.label11, 3, 3);
            this.gameBoard.Controls.Add(this.label12, 4, 3);
            this.gameBoard.Controls.Add(this.label13, 0, 4);
            this.gameBoard.Controls.Add(this.label14, 1, 4);
            this.gameBoard.Controls.Add(this.label15, 3, 4);
            this.gameBoard.Controls.Add(this.label16, 4, 4);
            this.gameBoard.Location = new System.Drawing.Point(298, 19);
            this.gameBoard.Name = "gameBoard";
            this.gameBoard.RowCount = 5;
            this.gameBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.66887F));
            this.gameBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.33113F));
            this.gameBoard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gameBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.gameBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.gameBoard.Size = new System.Drawing.Size(300, 300);
            this.gameBoard.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(74, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 74);
            this.label2.TabIndex = 1;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 74);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(148, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 74);
            this.label3.TabIndex = 2;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(220, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 74);
            this.label4.TabIndex = 3;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 74);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 75);
            this.label5.TabIndex = 3;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(74, 74);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 75);
            this.label6.TabIndex = 3;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(148, 74);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 75);
            this.label7.TabIndex = 3;
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(220, 74);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 75);
            this.label8.TabIndex = 3;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(0, 149);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 74);
            this.label9.TabIndex = 3;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(74, 149);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 74);
            this.label10.TabIndex = 3;
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(148, 149);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 74);
            this.label11.TabIndex = 3;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(220, 149);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 74);
            this.label12.TabIndex = 3;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(0, 223);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 77);
            this.label13.TabIndex = 3;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(74, 223);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 77);
            this.label14.TabIndex = 3;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(148, 223);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 77);
            this.label15.TabIndex = 3;
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Trebuchet MS", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(220, 223);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 77);
            this.label16.TabIndex = 3;
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wordButton
            // 
            this.wordButton.Enabled = false;
            this.wordButton.Location = new System.Drawing.Point(401, 374);
            this.wordButton.Name = "wordButton";
            this.wordButton.Size = new System.Drawing.Size(100, 23);
            this.wordButton.TabIndex = 1;
            this.wordButton.Text = "Submit Word";
            this.wordButton.UseVisualStyleBackColor = true;
            this.wordButton.Click += new System.EventHandler(this.wordButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(11, 42);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(63, 39);
            this.nameBox.Margin = new System.Windows.Forms.Padding(2);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(187, 20);
            this.nameBox.TabIndex = 4;
            this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(11, 82);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(79, 13);
            this.serverLabel.TabIndex = 5;
            this.serverLabel.Text = "Server Address";
            // 
            // serverBox
            // 
            this.serverBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverBox.Location = new System.Drawing.Point(94, 75);
            this.serverBox.Margin = new System.Windows.Forms.Padding(2);
            this.serverBox.Name = "serverBox";
            this.serverBox.Size = new System.Drawing.Size(198, 20);
            this.serverBox.TabIndex = 6;
            this.serverBox.TextChanged += new System.EventHandler(this.serverBox_TextChanged);
            // 
            // registerButton
            // 
            this.registerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.registerButton.Enabled = false;
            this.registerButton.Location = new System.Drawing.Point(94, 119);
            this.registerButton.Margin = new System.Windows.Forms.Padding(2);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(151, 41);
            this.registerButton.TabIndex = 7;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click_1);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(138, 177);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 26);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click_1);
            // 
            // cancelbutton1
            // 
            this.cancelbutton1.Enabled = false;
            this.cancelbutton1.Location = new System.Drawing.Point(217, 257);
            this.cancelbutton1.Name = "cancelbutton1";
            this.cancelbutton1.Size = new System.Drawing.Size(75, 23);
            this.cancelbutton1.TabIndex = 14;
            this.cancelbutton1.Tag = "doneButton";
            this.cancelbutton1.Text = "Cancel";
            this.cancelbutton1.UseVisualStyleBackColor = true;
            this.cancelbutton1.Click += new System.EventHandler(this.cancelbutton1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Help});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1009, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItem_Help
            // 
            this.menuItem_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem});
            this.menuItem_Help.Name = "menuItem_Help";
            this.menuItem_Help.Size = new System.Drawing.Size(44, 20);
            this.menuItem_Help.Text = "Help";
            this.menuItem_Help.Click += new System.EventHandler(this.menuItem_Help_Click);
            // 
            // typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem
            // 
            this.typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem.Name = resources.GetString(@"typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem.Name");
            this.typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem.Text = "Help";
            // 
            // wordBox
            // 
            this.wordBox.Enabled = false;
            this.wordBox.Location = new System.Drawing.Point(401, 336);
            this.wordBox.Name = "wordBox";
            this.wordBox.Size = new System.Drawing.Size(100, 20);
            this.wordBox.TabIndex = 17;
            this.wordBox.TextChanged += new System.EventHandler(this.wordBox_TextChanged_1);
            // 
            // timeBox
            // 
            this.timeBox.Enabled = false;
            this.timeBox.Location = new System.Drawing.Point(31, 259);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(100, 20);
            this.timeBox.TabIndex = 18;
            // 
            // joinButton
            // 
            this.joinButton.Enabled = false;
            this.joinButton.Location = new System.Drawing.Point(137, 257);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(75, 23);
            this.joinButton.TabIndex = 19;
            this.joinButton.Text = "Join Game";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 229);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(256, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Enter a desired playing time between 5-120 seconds.";
            // 
            // label_player1score
            // 
            this.label_player1score.AutoSize = true;
            this.label_player1score.Location = new System.Drawing.Point(623, 39);
            this.label_player1score.Name = "label_player1score";
            this.label_player1score.Size = new System.Drawing.Size(79, 13);
            this.label_player1score.TabIndex = 21;
            this.label_player1score.Text = "Player 1 Score:";
            // 
            // textBox_player1Score
            // 
            this.textBox_player1Score.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_player1Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_player1Score.Location = new System.Drawing.Point(756, 37);
            this.textBox_player1Score.Name = "textBox_player1Score";
            this.textBox_player1Score.ReadOnly = true;
            this.textBox_player1Score.Size = new System.Drawing.Size(34, 19);
            this.textBox_player1Score.TabIndex = 22;
            this.textBox_player1Score.Text = "0";
            // 
            // label_player2score
            // 
            this.label_player2score.AutoSize = true;
            this.label_player2score.Location = new System.Drawing.Point(830, 39);
            this.label_player2score.Name = "label_player2score";
            this.label_player2score.Size = new System.Drawing.Size(79, 13);
            this.label_player2score.TabIndex = 23;
            this.label_player2score.Text = "Player 2 Score:";
            // 
            // textBox_player2Score
            // 
            this.textBox_player2Score.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_player2Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_player2Score.Location = new System.Drawing.Point(951, 37);
            this.textBox_player2Score.Name = "textBox_player2Score";
            this.textBox_player2Score.ReadOnly = true;
            this.textBox_player2Score.Size = new System.Drawing.Size(34, 19);
            this.textBox_player2Score.TabIndex = 24;
            this.textBox_player2Score.Text = "0";
            // 
            // label_Timer
            // 
            this.label_Timer.AutoSize = true;
            this.label_Timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Timer.Location = new System.Drawing.Point(696, 417);
            this.label_Timer.Name = "label_Timer";
            this.label_Timer.Size = new System.Drawing.Size(82, 25);
            this.label_Timer.TabIndex = 25;
            this.label_Timer.Text = "TIMER";
            // 
            // textBox_Timer
            // 
            this.textBox_Timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Timer.Location = new System.Drawing.Point(794, 405);
            this.textBox_Timer.Name = "textBox_Timer";
            this.textBox_Timer.ReadOnly = true;
            this.textBox_Timer.Size = new System.Drawing.Size(100, 44);
            this.textBox_Timer.TabIndex = 26;
            // 
            // wordPanel
            // 
            this.wordPanel.BackColor = System.Drawing.SystemColors.Control;
            this.wordPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.wordPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wordPanel.Location = new System.Drawing.Point(624, 60);
            this.wordPanel.Name = "wordPanel";
            this.wordPanel.Size = new System.Drawing.Size(155, 328);
            this.wordPanel.TabIndex = 28;
            this.wordPanel.Text = "";
            this.wordPanel.Visible = false;
            // 
            // wordPanel2
            // 
            this.wordPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.wordPanel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.wordPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wordPanel2.Location = new System.Drawing.Point(823, 60);
            this.wordPanel2.Name = "wordPanel2";
            this.wordPanel2.Size = new System.Drawing.Size(155, 328);
            this.wordPanel2.TabIndex = 31;
            this.wordPanel2.Text = "";
            this.wordPanel2.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.statusLabel.Location = new System.Drawing.Point(9, 358);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(333, 30);
            this.statusLabel.TabIndex = 32;
            this.statusLabel.Text = "Waiting for Player 2 to join...";
            this.statusLabel.Visible = false;
            // 
            // BoggleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 461);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.wordPanel2);
            this.Controls.Add(this.wordPanel);
            this.Controls.Add(this.textBox_Timer);
            this.Controls.Add(this.label_Timer);
            this.Controls.Add(this.textBox_player2Score);
            this.Controls.Add(this.label_player2score);
            this.Controls.Add(this.textBox_player1Score);
            this.Controls.Add(this.label_player1score);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.wordBox);
            this.Controls.Add(this.gameBoard);
            this.Controls.Add(this.cancelbutton1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.serverBox);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.wordButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BoggleForm";
            this.Text = "BOGGLE";
            this.Load += new System.EventHandler(this.BoggleForm_Load);
            this.gameBoard.ResumeLayout(false);
            this.gameBoard.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel gameBoard;
        private System.Windows.Forms.Button wordButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverBox;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button cancelbutton1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Help;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox wordBox;
        private System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label_player1score;
        private System.Windows.Forms.TextBox textBox_player1Score;
        private System.Windows.Forms.Label label_player2score;
        private System.Windows.Forms.TextBox textBox_player2Score;
        private System.Windows.Forms.Label label_Timer;
        private System.Windows.Forms.TextBox textBox_Timer;
        private System.Windows.Forms.RichTextBox wordPanel;
        private System.Windows.Forms.RichTextBox wordPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem typeYourNameToAndServerAndClickToRegister2NextClickJoinGameToEnterAGameIfNoOtherPlayersAreInQueueYouWillAutomaticallyBeForcedToJoinAGameWhenAnotherPlayerArrives3IfYouClickleaveOnceTheGameStartsYouWillLeaveTheCurrentGame4TheTimeEnteredByTheFirstPlayerThatJoinedWillBeUsedToolStripMenuItem;
        private System.Windows.Forms.Label statusLabel;
    }
}

