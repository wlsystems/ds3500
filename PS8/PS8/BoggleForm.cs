//Tracy King  u0040235

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
        //New BoggleForm
        public BoggleForm()
        {
            InitializeComponent();
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
        /// Backing variable for UserRegistered property
        /// </summary>
        private bool _userRegistered = false;

        //sets and returns game status
        private bool isActiveGame;
        public bool isActive
        {
            set
            {
                isActiveGame = value;
            }
            get { return isActiveGame; }
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
            cancelbutton1.Text = "Leave";  //used to leave a game after game is started
        }


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
        /// Adds words to Player 2 Lisit, takes in a list of strings, iterates through each word and converts to uppercase as needed.
        /// This occurs after game is completed. 
        /// </summary>
        public void ViewPlayer1Word(List<string> player1List)
        {

            wordPanel.Visible = true;
            foreach (var item in player1List)
            {
                wordPanel.Text = wordPanel.Text + "\r" + item.ToString().ToUpper(); //uppercases each word and adds a return before adding new text

            }

        }

        /// <summary>
        ///Adds words to Player 2 List, takes in a list of strings, iterates through each word and converts to uppercase as needed.
        /// This occurs after game is completed. 
        /// </summary>
        /// <param name="player2List"></param>
        public void ViewPlayer2Word(List<string> player2List)
        {
            wordPanel2.Visible = true;
            foreach (var item in player2List)
            {
                wordPanel2.Text = wordPanel2.Text + "\r" + item.ToString().ToUpper(); //uppercases each word and adds a return before adding new text
            }


        }

        /// <summary>
        /// Enables time box.
        /// </summary>
        public void TimeEnabled(bool state)
        {
            timeBox.Enabled = state;
        }

        /// <summary>
        /// If a current game has ended either by quitting or time expiring, allows the user to enter another game. 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="playAgain"></param>
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

        /// <summary>
        /// Updates the label for player 2's name when the game is started.
        /// </summary>  
        public void Player2Update(dynamic nickname)
        {
            label_player2score.Text = nickname + "'s score";
        }

        /// <summary>
        /// Updates the label for player 1's name when the game is started. 
        /// </summary>
        /// <param name="nickname"></param>
        public void Player1Update(dynamic nickname)
        {
            label_player1score.Text = nickname + "'s score";
        }

        /// <summary>
        /// Updates the countdown time for the time remaining in the game. 
        /// </summary>
        /// <param name="timeLeft"></param>
        public void UpdateTimer(dynamic timeLeft)
        {
            textBox_Timer.Text = timeLeft + "";
        }

        /// <summary>
        /// If text is entered into the word box, enables the submit button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordBox_TextChanged(object sender, EventArgs e)
        {
            wordButton.Enabled = UserRegistered && wordBox.Text.Trim().Length > 0;
        }

        /// <summary>
        ///  Help menu for the TAs 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a simple Boogle Client. A user can enter their name and their chosen server " +
                "and press register.   They can cancel the registering process by clicking the " +
                "cancel button underneath the register button.  After you have been successfully registered " +
                "you can enter a desired game time between 5-120 seconds and click join game." +
                "You can use the cancel button next to join game should you no longer want to play " +
                " Once you have joined you will join an existing game or wait for " +
                "the next available opponent. The status will appear on the bottom left.  As soon as an opponent " +
                "is available the game will start.  You can enter words in the box below the gameboard and submit " +
                "them by clicking on submit word. You will be able to watch the timer and the current score. " +
                "If you choose to exit early, you can click the Leave button.  After the game is over, all of the words  " +
                "played by both opponents will appear to the right of the board.  Happy Spelling!");
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

        /// <summary>
        /// Clears the game board of letters from previous game. 
        /// </summary>
        private void ClearBoard()
        {
            label1.Text = ""; label2.Text = ""; label3.Text = ""; label4.Text = ""; label5.Text = ""; label6.Text = ""; label7.Text = ""; label8.Text = ""; label9.Text = ""; label10.Text = ""; label11.Text = ""; label12.Text = ""; label13.Text = ""; label14.Text = ""; label15.Text = ""; label16.Text = "";
        }

        /// <summary>
        ///  The name box has been changed, checks to see if the server box has been entered and enables if appropriate. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            if (nameBox.Text.Length > 0 && serverBox.Text.Length > 0)
                registerButton.Enabled = true;
            if (nameBox.Text.Length <= 0 | serverBox.Text.Length <= 0)
                registerButton.Enabled = false;
        }

        /// <summary>
        /// Loads form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// The join button has been clicked, attempts to parse the time, and checks to see if the time submitted is within
        /// the allowed 5-120 seconds, has the user resubmit if it is not.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// The cancel button was pressed during registeration, cancels and allows the user to attempt again if desird. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click_1(object sender, EventArgs e)
        {
            registerButton.Enabled = true;
            cancelButton.Enabled = false;
            timeBox.Enabled = false;
            joinButton.Enabled = false;
            if (CancelPressed != null)
                CancelPressed(1);
            if (CancelPressed != null)
                SetStatusLabel(false, true);
            joinButton.Text = "Join Game";
            if (CancelPressed != null)
                CancelPressed(2);
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

        /// <summary>
        /// This takes in the string of 16 letters, parses it into characters and then checks for the letter Q to handle it 
        /// as a "Qu."  Sets the 16 texboxes for the game board. 
        /// </summary>
        /// <param name="s"></param>
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

        /// <summary>
        /// Updates the score for player one when called. 
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore1(int score)
        {
            textBox_player1Score.Text = score.ToString();
        }

        /// <summary>
        /// Updates the score for player 2 when called. 
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore2(int score)
        {
            textBox_player2Score.Text = score.ToString();
        }

        /// <summary>
        /// Enables the "submit word" button when called, depending on if the word box contains text. 
        /// </summary>
        /// <param name="wordState"></param>
        public void submitEnableControls(bool wordState)
        {
            wordButton.Enabled = wordState;
        }

        /// <summary>
        ///  Enables the word butto when test is entered into word box. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void wordBox_TextChanged_1(object sender, EventArgs e)
        {
            wordButton.Enabled = true;
            wordBox.KeyUp += WordBox_KeyUp;
        }

        /// <summary>
        /// Handles the user using an enter key to submit.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// The submit word button has been clicked, first Submit Pressed action.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wordButton_Click(object sender, EventArgs e)
        {
            SubmitPressed(wordBox.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableControls(false);
        }

        private void wordPanel_TextChanged(object sender, EventArgs e)
        {

        }

        private void gameBoard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void gameBoard_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
