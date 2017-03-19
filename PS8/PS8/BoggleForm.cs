using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
{
    public partial class BoggleForm : Form
    {
        public BoggleForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If state == true, enables all controls that are normally enabled; disables Cancel.
        /// If state == false, disables all controls; enables Cancel.
        /// </summary>
        public void EnableControls(bool state)
        { 
            registerButton.Enabled = state;
            wordBox.Enabled = state && UserRegistered && timeBox.Text.Length > 0;
            joinButton.Enabled = false;
            cancelButton.Enabled = !state;
            cancelbutton1.Enabled = state;
            cancelbutton1.Text = "Leave";
        }



        /// <summary>
        /// Adds words to Player 1 List
        /// </summary>
        public void ViewPlayer1Word(List<string> player1List, List<int> player1Score)
        {

            wordPanel.Visible = true;
            player1ScoreList.Visible = true;
            foreach (var item in player1List)
            {
                wordPanel.Text = wordPanel.Text + "\r" + item.ToString().ToUpper();
                
            }
            foreach (var item in player1Score)
            {
                player1ScoreList.Text = player1ScoreList.Text + "\r" + item;
            }
        }

        public void ViewPlayer2Word(List<string> player2List, List<int> player2Score )
        {
            wordPanel2.Visible = true;
            player2ScoreList.Visible = true;
            foreach (var item in player2List)
            {
                wordPanel2.Text = wordPanel2.Text + "\r" + item.ToString().ToUpper();
            }
            foreach (var item in player2Score)
            {
                player2ScoreList.Text = player2ScoreList.Text + "\r" + item;
            }

        }


        /// <summary>
        /// Backing variable for UserRegistered property
        /// </summary>
        private bool _userRegistered = false;

        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        public bool UserRegistered
        {
            get { return _userRegistered; }
            set
            {
                _userRegistered = value;
                WordBox_TextChanged(null, null);
            }
        }
        /// <summary>
        /// Removes all the words played from the list. 
        /// </summary>
        public void Clear()
        {
            wordPanel.Visible = false;
            wordPanel2.Visible = false;
            wordPanel.Text = "";
            wordPanel2.Text = "";
            player1ScoreList.Visible = false;
            player2ScoreList.Visible = false;
            player1ScoreList.Text = "";
            player2ScoreList.Text = "";

        }

        /// <summary>
        /// Get the status of the game.
        /// </summary>
        public event Action GameStatus;

        /// <summary>
        /// Fired when the join game button is pushed
        /// </summary>
        public event Action<int> JoinGame;

        /// <summary>
        /// Fired when register button is pushed.
        /// </summary>
        public event Action<string, string> SetServerURL;


        /// <summary>
        /// Fired when a new word is played. 
        /// Parameter is the word played.
        /// </summary>
        public event Action<string> SubmitPressed;


        /// <summary>
        /// Fires when a player wants to end a current
        /// session. 
        /// </summary>
        public event Action DonePressed;

        /// <summary>
        /// Fires when an ongoing action must be canceled.
        /// </summary>
        public event Action<int> CancelPressed;

        /// <summary>
        /// Enables time box.
        /// </summary>
        public void TimeEnabled(bool state)
        {
            timeBox.Enabled = state;
        }

        /// <summary>
        /// Enables time box.
        /// </summary>
        public void JoinEnabled(bool state)
        {
            joinButton.Enabled = state;
        }

        /// <summary>
        /// Enables or disables the cancel-join box.
        /// </summary>
        public void CancelJoinEnabled(bool state)
        {
            cancelbutton1.Enabled = state;
        }

        internal void Player2Update(dynamic nickname)
        {
            label_player2score.Text = nickname + "'s score";
        }

        internal void Player1Update(dynamic nickname)
        {
            label_player1score.Text = nickname + "'s score";
        }

        internal void UpdateTimer(dynamic timeLeft)
        {
            textBox_Timer.Text = timeLeft + " ";
        }

        private void WordBox_TextChanged(object sender, EventArgs e)
        {
            wordButton.Enabled = UserRegistered && wordBox.Text.Trim().Length > 0;
        }


        /// <summary>
        /// Player wants to end a current game seesion. 
        /// </summary>
        private void FireDoneEvent()
        {
            if (DonePressed != null)
            {
                DonePressed();
            }
        }

        private void menuItem_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is how to play Boogle.");
        }



        /// <summary>
        /// Sets the URL of the server and fires the action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverBox_TextChanged(object sender, EventArgs e)
        {
            if (nameBox.Text.Length > 0 && serverBox.Text.Length > 0)
                registerButton.Enabled = true;
            if (nameBox.Text.Length <= 0 | serverBox.Text.Length <= 0)
                registerButton.Enabled = false;
        }


        private void registerButton_Click_1(object sender, EventArgs e)
        {
            cancelButton.Enabled = true;
            SetServerURL(nameBox.Text, serverBox.Text);
            registerButton.Enabled = false;
            joinButton.Enabled = true;
            cancelbutton1.Enabled = false;
            ClearBoard();
            CancelPressed(3);
            textBox_Timer.Text = "";
            textBox_player1Score.Text = "0";
            textBox_player2Score.Text = "0";
            SetStatusLabel(false, false);
        }

        private void ClearBoard()
        {
            label1.Text = ""; label2.Text = ""; label3.Text = ""; label4.Text = ""; label5.Text = ""; label6.Text = ""; label7.Text = ""; label8.Text = ""; label9.Text = ""; label10.Text = ""; label11.Text = ""; label12.Text = ""; label13.Text = ""; label14.Text = ""; label15.Text = ""; label16.Text = "";
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            if (nameBox.Text.Length > 0 && serverBox.Text.Length > 0)
                registerButton.Enabled = true;
            if (nameBox.Text.Length <= 0 | serverBox.Text.Length <= 0)
                registerButton.Enabled = false;
        }

        private void BoggleForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Show or hide the status label.
        /// </summary>
        /// <param name="state"></param>
        public void SetStatusLabel(bool state, bool active)
        {
            statusLabel.Visible = state;
            if (active)
            {
                statusLabel.Text = "Active Game";
                statusLabel.ForeColor = Color.Green;
            }
            else
            {
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Waiting for Player 2 to join...";  
            }
                
                
        }
        private void joinButton_Click(object sender, EventArgs e)
        {
            cancelbutton1.Enabled = true;
            Clear();
            joinButton.Enabled = false;
            if (JoinGame != null)
            {
                int time = 0;
                if (timeBox.Text != "")
                    time = int.Parse(timeBox.Text.Trim());
                if ((time >= 5) && (time <= 120) )
                {
                    JoinGame(time);
                }
                else
                    MessageBox.Show("You did not enter a valid time.");         
            }
        }

        private void timeBox_TextChanged(object sender, EventArgs e)
        {
            joinButton.Enabled = UserRegistered & timeBox.Text.Trim().Length > 0;
        }

        private void cancelButton_Click_1(object sender, EventArgs e)
        {
            registerButton.Enabled = true;
            cancelButton.Enabled = false;
            timeBox.Enabled = false;
            joinButton.Enabled = false;
            if (CancelPressed != null)
                CancelPressed(1);
        }

        private void cancelbutton1_Click(object sender, EventArgs e)
        {
            if (CancelPressed != null)
                CancelPressed(2);
            registerButton_Click_1(sender, e);
            SetStatusLabel(false, false);
        }
        public void SetLabel(string s)
        {
            string temp = "";
            for (int i = 0; i < 16; i++)
            {
                temp = s.ElementAt(i).ToString();
                if (temp.Equals("Q"))
                    temp = "Qu";
                switch (i)
                {
                    case 0:
                        label1.Text = temp;
                        break;
                    case 1:
                        label2.Text = temp;
                        break;
                    case 2:
                        label3.Text = temp;
                        break;
                    case 3:
                        label4.Text = temp;
                        break;
                    case 4:
                        label5.Text = temp;
                        break;
                    case 5:
                        label6.Text = temp;
                        break;
                    case 6:
                        label7.Text = temp;
                        break;
                    case 7:
                        label8.Text = temp;
                        break;
                    case 8:
                        label9.Text = temp;
                        break;
                    case 9:
                        label10.Text = temp;
                        break;
                    case 10:
                        label11.Text = temp;
                        break;
                    case 11:
                        label12.Text = temp;
                        break;
                    case 12:
                        label13.Text = temp;
                        break;
                    case 13:
                        label14.Text = temp;
                        break;
                    case 14:
                        label15.Text = temp;
                        break;
                    case 15:
                        label16.Text = temp;
                        break;
                }
            }

        }

        internal void UpdateScore1(int score)
        {
            textBox_player1Score.Text = score.ToString();
        }


        internal void UpdateScore2(int score)
        {
            textBox_player2Score.Text = score.ToString();
        }

        internal void submitEnableControls(bool wordState)
        {
            wordButton.Enabled = wordState;
        }

        private void wordBox_TextChanged_1(object sender, EventArgs e)
        {
            wordButton.Enabled = true;
            wordBox.KeyDown += WordBox_KeyDown;
        }

        private void WordBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool pressed = false;
            if (e.KeyCode == Keys.Enter)
            {
                if (pressed == false)
                {
                    pressed = true;
                    MessageBox.Show("EE");
                    e.Handled = true;
                    if (SubmitPressed != null)
                    {
                        SubmitPressed(wordBox.Text);
                        wordBox.Text = "";
                        return;
                    }
                }
            }
        }

        private void wordButton_Click(object sender, EventArgs e)
        {
            SubmitPressed(wordBox.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableControls(false);
        }


    }
}
