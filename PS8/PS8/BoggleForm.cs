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
        //If there is a game in progress.

        private bool isActiveGame;
        public bool isActive
        {
            set
            {
                isActiveGame = value;
            }
            get { return isActiveGame; }
        }
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
        public void ViewPlayer1Word(List<string> player1List)
        {

            wordPanel.Visible = true;
            foreach (var item in player1List)
            {
                wordPanel.Text = wordPanel.Text + "\r" + item.ToString().ToUpper();
                
            }

        }

        public void ViewPlayer2Word(List<string> player2List )
        {
            wordPanel2.Visible = true;
            foreach (var item in player2List)
            {
                wordPanel2.Text = wordPanel2.Text + "\r" + item.ToString().ToUpper();
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

        public void EnableJoin(bool state, bool playAgain)
        {
            if (playAgain == true)
            {
                joinButton.Text = "Play Again";
                joinButton.Enabled = true;
            }      
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
            SetStatusLabel(false, true);
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
            if (state && active)
            {
                statusLabel.Visible = state;
                statusLabel.Text = "Active Game";
                statusLabel.ForeColor = Color.Green;
            }
            else if (state && !active)
            {
                statusLabel.Visible = state;
                statusLabel.ForeColor = Color.DarkRed;
                statusLabel.Text = "Waiting for Player 2 to join...";  
            }
            else if (!state && !active)
            {
                statusLabel.Visible = true;
                statusLabel.ForeColor = Color.Black;
                statusLabel.Text = "Game Over";
            }
            else if (!state && active)
                statusLabel.Visible = state;
        }
        private void joinButton_Click(object sender, EventArgs e)
        {
            cancelbutton1.Enabled = true;
            cancelbutton1.Text = "Cancel";
            Clear();
            joinButton.Enabled = false;
            if (JoinGame != null)
            {
                int n = 0;
                bool result = Int32.TryParse(timeBox.Text, out n);
                if (result & n >= 5 & n <= 120)
                {
                    JoinGame(n);
                    JoinEnabled(false);
                }
                else
                {
                    MessageBox.Show("Enter a number between 5-120 (seconds).");
                    JoinEnabled(true);
                }
                    
            }
        }

        private void timeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click_1(object sender, EventArgs e)
        {
            registerButton.Enabled = true;
            cancelButton.Enabled = false;
            timeBox.Enabled = false;
            joinButton.Enabled = false;
            if (CancelPressed != null)
                CancelPressed(1);
            if (CancelPressed != null)
                //CancelPressed(2);
            SetStatusLabel(false, true);
            joinButton.Text = "Join Game";
        }

        private void cancelbutton1_Click(object sender, EventArgs e)
        {
            if (CancelPressed != null)
                CancelPressed(2);
            registerButton_Click_1(sender, e);
            SetStatusLabel(false, true);
            wordBox.Enabled = false;
            wordButton.Enabled = false;
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
            wordBox.KeyUp += WordBox_KeyUp;
        }

        private void WordBox_KeyUp(object sender, KeyEventArgs e)
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
