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
            wordButton.Enabled = state && UserRegistered && wordBox.Text.Length > 0;
            timeButton.Enabled = UserRegistered;

            foreach (Control control in wordPanel.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = state && UserRegistered;
                }
            }
            cancelButton.Enabled = !state;
            
        }



        /// <summary>
        /// Adds a row to the task display.
        /// </summary>
        public void AddWord(string wordplayed, int score, bool belongsToUser)
        {
            int row = wordPanel.Controls.Count / 3;
            wordPanel.Controls.Add(new Label() { Text = wordplayed });


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
            wordPanel.Controls.Clear();
        }

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
        /// Fired when one of the filter has changed, at the end of the
        /// game the client will show both players' list, during the game it will show only the local player.
        /// </summary>
        public event Action<bool> FilterChanged;


        /// <summary>
        /// Fires when a player wants to end a current
        /// session. 
        /// </summary>
        public event Action DonePressed;

        /// <summary>
        /// Fires when an ongoing action must be canceled.
        /// </summary>
        public event Action CancelPressed;

        /// <summary>
        /// Enables time box.
        /// </summary>
        public void JoinGame(bool state)
        {
            timeBox.Enabled = state;

        }


        private void TimeBox_TextChanged(object sender, EventArgs e)
        {
            timeButton.Enabled = UserRegistered &  timeBox.Text.Trim().Length > 0;
        }

        private void TimeButton_Click(object sender, EventArgs e)
        {
            if (JoinGame != null)
            {
                int time = int.Parse(timeBox.Text.Trim());
                if ((time > 5) && (time < 120))
                {
                    JoinGame(time);
                }
                else
                    MessageBox.Show("Your time is not within the allowed limits of 5-120 seconds.");
                  
            }
        }


        private void WordBox_TextChanged(object sender, EventArgs e)
        {
            wordButton.Enabled = UserRegistered && wordBox.Text.Trim().Length > 0;
        }

        private void WordButton_Click(object sender, EventArgs e)
        {
            if (SubmitPressed != null)
            {
                SubmitPressed(wordBox.Text.Trim());
            }
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

        //The user wants to cancel registration.  
        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (CancelPressed != null)
            {
                CancelPressed();
            }
        }

        private void menuItem_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is how to play Boogle.");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
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
            SetServerURL(nameBox.Text, serverBox.Text);
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            if (nameBox.Text.Length > 0 && serverBox.Text.Length > 0)
                registerButton.Enabled = true;
            if (nameBox.Text.Length <= 0 | serverBox.Text.Length <= 0)
                registerButton.Enabled = false;
        }
    }
}
