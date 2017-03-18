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
        /// Adds a row to the task display.
        /// </summary>
        public void AddWord(string wordplayed)
        {
            label_WordsPlayed.Visible = true;
            wordPanel.Visible = true;
            wordPanel.Text = wordPanel.Text.ToString() + "\r" + wordplayed.ToUpper();
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
            label_WordsPlayed.Visible = false;
            label_wordPlayed2.Visible = false;
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
        /// Fired when user must be registered.
        /// Parameters are name and requested client address. 
        /// </summary>
        public event Action<string, string> RegisterPressed;

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

        internal void ViewOpponentsWords(List<string> oppWords)
        {
            label_wordPlayed2.Visible = true;
            wordPanel2.Visible = true;
            foreach (var item in oppWords)
            {
                wordPanel2.Text = wordPanel2.Text + "\r" + item.ToString().ToUpper();
            }

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
                int time = int.Parse(timeBox.Text.Trim());
                if ((time >= 5) && (time <= 120))
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
            if (s.Equals("q"))
                s = "qu";
            for (int i = 0; i < 16; i++)
            {
                switch (i)
                {
                    case 0:
                        label1.Text = s.ElementAt(i).ToString();
                        break;
                    case 1:
                        label2.Text = s.ElementAt(i).ToString();
                        break;
                    case 2:
                        label3.Text = s.ElementAt(i).ToString();
                        break;
                    case 3:
                        label4.Text = s.ElementAt(i).ToString();
                        break;
                    case 4:
                        label5.Text = s.ElementAt(i).ToString();
                        break;
                    case 5:
                        label6.Text = s.ElementAt(i).ToString();
                        break;
                    case 6:
                        label7.Text = s.ElementAt(i).ToString();
                        break;
                    case 7:
                        label8.Text = s.ElementAt(i).ToString();
                        break;
                    case 8:
                        label9.Text = s.ElementAt(i).ToString();
                        break;
                    case 9:
                        label10.Text = s.ElementAt(i).ToString();
                        break;
                    case 10:
                        label11.Text = s.ElementAt(i).ToString();
                        break;
                    case 11:
                        label12.Text = s.ElementAt(i).ToString();
                        break;
                    case 12:
                        label13.Text = s.ElementAt(i).ToString();
                        break;
                    case 13:
                        label14.Text = s.ElementAt(i).ToString();
                        break;
                    case 14:
                        label15.Text = s.ElementAt(i).ToString();
                        break;
                    case 15:
                        label16.Text = s.ElementAt(i).ToString();
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
        }

        private void wordButton_Click(object sender, EventArgs e)
        {
            SubmitPressed(wordBox.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableControls(false);
        }

        private void wordPanel2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
