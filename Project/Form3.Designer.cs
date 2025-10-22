using System.Text.RegularExpressions;
using System.IO;
using System.Numerics;
using System.Diagnostics;
namespace Project
{
    public partial class Form3 : Form
    {
        readonly string pMoney;
        public int lx = 0;
        public int ly;
        bool dx = true;
        bool dy = false;
        public Form3(string pM)
        {
            pMoney = pM;
            InitializeComponent();
            Reload(); // calls the reload function which is defined later //
            System.Windows.Forms.Timer time = new()
            { Interval = 10 };
            time.Tick += Move;
            time.Start();//creates and starts a timer to call the move procedure every 10ms
        }
        void Reload()
        {
            ly = Screen.PrimaryScreen.Bounds.Height - this.Height; // assigns ly the value of the height of the screen minus the height of the form 3 window
            textBox.Text = null; // removes all text from the textbox
            leaderBoard.Items.Clear(); // empties the leaderBoard
            string[] str = File.ReadAllLines("scores.txt");//reads from the scores text document to be used to keep a leaderBoard//
            bool changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 0; i < str.Length - 1; i++)
                {
                    if (int.Parse(str[i].Split(' ')[0]) < int.Parse(str[i + 1].Split(' ')[0]))
                    {
                        changed = true;
                        (str[i + 1], str[i]) = (str[i], str[i + 1]);
                    }
                }
            }// sorts the str array into ascending order based on how large the score of the person was //
            for (int i = 0; i < str.Length; i++)
            {
                string money = "";
                for (int j = 0; j < str[i].Length; j++)
                {
                    if (str[i][j] == ' ') { break; }
                    money += str[i][j];
                }
                string player = "";
                for (int j = str[i].Length - 1; j > 0; j--)
                {
                    if (str[i][j] == ' ') { break; }
                    player = str[i][j] + player;
                }
                leaderBoard.Items.Add((i + 1) + ". " + money + " money: " + player);
            }
        } // fills the leaderBoard list box with each player's scores and names
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); // ensures that the user cannot enter special characters, but can ues control keys such as delete
        }
        private void Update(object sender, EventArgs e)
        {//
            if (textBox.Text != "")
            {
                StreamWriter sw = new("scores.txt", true);
                sw.WriteLine(pMoney + " " + textBox.Text);
                sw.Close();
                Environment.Exit(0);
            }
        }// when the update button is pressed, it opens the text file and writes the users score, followed by a space and their name, then closes the program
        private void Move(object sender, EventArgs e)
        {//
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height; // 
            if (lx <= 0) { dx = false; }
            if (lx >= screenWidth - this.Width) { dx = true; }
            if (dx == false) { lx += 1; }
            if (dx == true) { lx -= 1; }
            if (ly <= 0) { dy = false; }
            if (ly >= screenHeight - this.Height) { dy = true; }
            if (dy == false) { ly += 2; }
            if (dy == true) { ly -= 2; }
            this.Location = new Point(lx, ly);// increments lx and ly if they should go up (their corresponding d variable is false), or down if they should go down, then it sets the position of the window to these 
        }
    }
}
