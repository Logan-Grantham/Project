using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Project
{
    public partial class Form2 : System.Windows.Forms.Form
    {//
        readonly Random r = new();
        public int lx;
        public int ly;
        bool dx = false;
        bool dy = true;
        bool button1image = false;
        bool button2image = false;
        bool button3image = false;
        bool button4image = false;
        bool button5image = false;
        readonly ProgressBar blindProgress;
        public Button[] buttons;
        public Button buttonBad;
        public Button[] buttons2temp;
        public Label playerScore;// defines variables for later
        public Form2(Button[] button, Button butBad, Label pScore, ProgressBar blindP)
        {
            InitializeComponent();//
            blindProgress = blindP;
            buttons2temp = button;
            buttons = button;
            buttonBad = butBad;
            playerScore = pScore;// defines variables which come from form 1, to be used later //
            System.Windows.Forms.Timer time = new()
            { Interval = 10 };
            time.Tick += Move;
            time.Start();// starts the timer to move the window and calls the move procedure every 10ms
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lx = Screen.PrimaryScreen.Bounds.Width - this.Width;
            ly = Screen.PrimaryScreen.Bounds.Height - this.Height - 48;//
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;// sets the buttons to be invisible, will be visible once assigned a card
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = buttons2temp[i];
            } // backs up the buttons array so that it can be reverted
            for (int i = 0; i < buttons.Length; i++)
            {//
                bool empty = true;
                for (int j = 0; j < buttons.Length; j++)
                {
                    if (buttons[j] != buttonBad)
                    {
                        empty = false;
                    }
                } // checks that there is a card to add to the shop //
                int ran = r.Next(0, buttons.Length);
                if (buttons[ran] != buttonBad)
                {
                    if (button1image == false)
                    {
                        Cost1.Text = (int.Parse(buttons[ran].Tag.ToString()) - 2 + blindProgress.Value * 0.4).ToString();
                        button1.Text = buttons[ran].Name;
                        button1.Visible = true;
                        button1image = true;
                        button1.BackgroundImage = buttons[ran].Image;
                    }
                    else if (button2image == false)
                    {
                        Cost2.Text = (int.Parse(buttons[ran].Tag.ToString()) - 2 + (blindProgress.Value) * 0.4).ToString();
                        button2.Text = buttons[ran].Name;
                        if (button2.Text != button1.Text)
                        {
                            button2.Visible = true;
                            button2image = true;
                            button2.BackgroundImage = buttons[ran].Image;
                        }
                    }
                    else if (button3image == false)
                    {
                        Cost3.Text = (int.Parse(buttons[ran].Tag.ToString()) - 2 + (blindProgress.Value) * 0.4).ToString();
                        button3.Text = buttons[ran].Name;
                        if (button3.Text != button1.Text && button3.Text != button2.Text)
                        {
                            button3.Visible = true;
                            button3image = true;
                            button3.BackgroundImage = buttons[ran].Image;
                        }
                    }
                    else if (button4image == false)
                    {
                        Cost4.Text = (int.Parse(buttons[ran].Tag.ToString()) - 2 + (blindProgress.Value) * 0.4).ToString();
                        button4.Text = buttons[ran].Name;
                        if (button4.Text != button1.Text && button4.Text != button2.Text && button4.Text != button3.Text)
                        {
                            button4.Visible = true;
                            button4image = true;
                            button4.BackgroundImage = buttons[ran].Image;
                        }
                    }
                    else if (button5image == false)
                    {
                        Cost5.Text = (int.Parse(buttons[ran].Tag.ToString()) - 2 + (blindProgress.Value) * 0.4).ToString();
                        button5.Text = buttons[ran].Name;
                        if (button5.Text != button1.Text && button5.Text != button2.Text && button5.Text != button3.Text && button5.Text != button4.Text)
                        {
                            button5.Visible = true;
                            button5image = true;
                            button5.BackgroundImage = buttons[ran].Image;
                        }
                    }
                    Refresh();
                    // shows a card in the shop if there is one, gives it a price of its tag -2 + the big blind * 0.4 so that it starts at the default price and increases by 2 each round
                }// the text is set to the name of the button it takes so that the program can distinguish which card it is later on
                else if (empty == false) { i--; } // decrements if the program is not empty but a card was not chosen so that there is always the maximum amount of cards that it can hold
            }
            buttons = buttons2temp; // reverts buttons back to its original state
        }

        public void Button_Click(object sender, EventArgs e)
        {//
            bool con = false;
            Button button = (Button)sender;
            if (button == button1) { con = Form1.Deduct(int.Parse(Cost1.Text), playerScore); }
            else if (button == button2) { con = Form1.Deduct(int.Parse(Cost2.Text), playerScore); }
            else if (button == button3) { con = Form1.Deduct(int.Parse(Cost3.Text), playerScore); }
            else if (button == button4) { con = Form1.Deduct(int.Parse(Cost4.Text), playerScore); }
            else if (button == button5) { con = Form1.Deduct(int.Parse(Cost5.Text), playerScore); }
            if (con == true)
            {
                for (int i = 0; i < buttons2temp.Length; i++)
                {
                    if (buttons2temp[i].Name == button.Text)
                    {
                        buttons2temp[i].Enabled = true;
                        break;
                    }
                }
                button.Visible = false;
                Refresh();
            }// finds which button was pressed and deducts the cost from the players money
        }
        private void Move(object sender, EventArgs e)
        {//
            this.BringToFront();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            if (lx <= 0) { dx = false; }
            if (lx >= screenWidth - this.Width) { dx = true; }
            if (dx == false) { lx += 2; }
            if (dx == true) { lx -= 2; }
            if (ly <= 0) { dy = false; }
            if (ly >= screenHeight - this.Height) { dy = true; }
            if (dy == false) { ly += 4; }
            if (dy == true) { ly -= 4; }
            this.Location = new Point(lx, ly);// increments lx and ly if they should go up (their corresponding d variable is false), or down if they should go down, then it sets the position of the window to these 
        }
    }
}
