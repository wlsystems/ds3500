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


        private void registration_TextChanged(object sender, EventArgs e)
        {
            registerButton.Enabled = nameBox.Text.Trim().Length > 0 && serverBox.Text.Trim().Length > 0;
        }


        private void registerButton_Click(object sender, EventArgs e)
        {
            if (RegisterPressed != null)
            {
                RegisterPressed(nameBox.Text.Trim(), serverBox.Text.Trim());
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

    }
}
