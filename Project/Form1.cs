using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Media;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
namespace Project
{
    public partial class Form1 : Form
    { //
        readonly Random r = new();
        bool coinFall = false;
        bool showCardP1;
        bool showCardP2;
        bool showCardB1;
        bool showCardB2;
        int state;
        int minRaise;
        bool bluff;
        public int lx = 0;
        public int ly = 0;
        bool dx = true;
        bool dy = true;
        int eBLX = 0;
        int eBLY = 0;
        bool eBDX = true;
        bool eBDY = true;
        public Button[] buttons;
        readonly string[] names;
        readonly Image[] ifs;
        readonly Face[] fs;  // defining some variables for later
        public Form1()
        {
            InitializeComponent(); //
            buttons = [clubAce, club2, club3, club4, club5, club6, club7, club8, club9, club10, clubJack, clubQueen, clubKing, diamondAce, diamond2, diamond3, diamond4, diamond5, diamond6, diamond7, diamond8, diamond9, diamond10, diamondJack, diamondQueen, diamondKing, heartAce, heart2, heart3, heart4, heart5, heart6, heart7, heart8, heart9, heart10, heartJack, heartQueen, heartKing, spadeAce, spade2, spade3, spade4, spade5, spade6, spade7, spade8, spade9, spade10, spadeJack, spadeQueen, spadeKing];
            showCardP1 = false;
            showCardP2 = false;
            showCardB1 = false;
            showCardB2 = false;
            this.MaximizeBox = false;
            this.ControlBox = false;
            cashMoney.Visible = false;
            state = 0;
            minRaise = 0;
            bluff = false;
            if (!File.Exists("scores.txt"))
            {
                FileStream f = File.Create("scores.txt");
                f.Close();
            }
            names = File.ReadAllLines("scores.txt");
            ifs = [Properties.Resources.Gene, Properties.Resources.Mel, Properties.Resources.Mary, Properties.Resources.Poop, Properties.Resources.Smiler, Properties.Resources.Jailbreak, Properties.Resources.Hi5, Properties.Resources.Jack];
            fs = new Face[names.Length]; // giving values to some of the previously defined variables
            if (names.Length > 0)
            { //
                for (int i = 0; i < names.Length; i++)
                {
                    int ran = r.Next(0, ifs.Length);
                    fs[i] = new Face(names[i].Split(' ')[1] + ", " + names[i].Split()[0], ifs[ran]);
                }
            } // gives each of the names of anyone who is on the leaderBoard a random image
            for (int i = 0; i < buttons.Length; i++)
            {//
                var tempBut = buttons[i];
                Button but = tempBut;
                but.Enabled = false;
            } // makes every button from the buttons array  unusable for now
            System.Windows.Forms.Timer time = new() { Interval = 10 };//
            time.Tick += Move;
            time.Start(); // creates starts a timer to move the window every 10 ms
            Game(0, 0); // calls the start round function to start the round, with values 0 0 to indicate that this is the beginning of the round
        }
        public void IsEnough(int comp)
        { //
            bool empty = true;
            int count = 0;
            for (int i = 0; i < buttons.Length; i++) { if (buttons[i] != buttonBad) { count++; } }
            if (count >= comp) { empty = false; }
            if (empty == true) { Win(); } // checks to see if there are enough cards left in the deck to play a round, if not you win
        }
        static public bool Deduct(int aToDeduct, Label playerScore)
        {//
            if (int.Parse(playerScore.Text) >= aToDeduct)
            {
                playerScore.Text = (int.Parse(playerScore.Text) - aToDeduct).ToString();
                return true;
            }
            else { return false; } // removes points from your total after you buy something or raise
        }
        public void Game(int go, int stage)
        {
            if (go == 0)
            { //
                if (fs.Length > 0)
                {//
                    Face face = fs[r.Next(0, fs.Length)];
                    this.face.Image = face.GetFace();
                    nameOfBot.Text = face.GetName();
                }// gives the bot a name and face from the face class, faces are not random if you roll the same name twice, if no face found then Barry, 63 stays as the face
                if (nOfHelp.Value < nOfHelp.Maximum) { nOfHelp.Value++; }
                Assembly assembly;
                SoundPlayer sp;
                assembly = Assembly.GetExecutingAssembly();
                sp = new SoundPlayer(assembly.GetManifestResourceStream("keepGambling.MambleGusic.wav"));
                sp.Play();
                stage = 0;
                botHand.BackgroundImage = null; botHand2.BackgroundImage = null;
                botHand.Image = Properties.Resources.Back_png;
                botHand2.Image = Properties.Resources.Back_png;
                hand.BackgroundImage = Properties.Resources.Back_png;
                hand2.BackgroundImage = Properties.Resources.Back_png;
                theFlop.BackgroundImage = Properties.Resources.Back_png;
                theTurn.BackgroundImage = Properties.Resources.Back_png;
                theRiver.BackgroundImage = Properties.Resources.Back_png;
                showCardP1 = false;
                showCardP2 = false;
                showCardB1 = false;
                showCardB2 = false;
                state = 0;
                minRaise = 0;
                if (blindProgress.Value < blindProgress.Maximum) { blindProgress.Value += 5; }
                blindSize.Text = blindProgress.Value.ToString();
                lowRaise.Visible = false; midRaise.Visible = false; highRaise.Visible = false;
                if (r.Next(0, 2) == 0) { bluff = true; }
                else { bluff = false; }
                if (bigBlind.BackColor == Color.Red)
                {
                    RaisingStuff(1);
                    try
                    {
                        if (bluff == true)
                        {
                            if (r.Next(0, 2) == 0) { minRaise = blindProgress.Value; pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString(); }
                            else { minRaise = blindProgress.Value + 5; pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString(); }
                        }
                        else { minRaise = blindProgress.Value + 2; pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString(); }
                    }
                    catch { pot.Text = minRaise.ToString(); }
                    bigBlind.BackColor = Color.LawnGreen;
                }
                else
                {
                    minRaise = ((blindProgress.Value - (blindProgress.Value % 2)) / 2) + 5; pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString();
                    RaisingStuff(0);
                    bigBlind.BackColor = Color.Red;
                }
            }// changes required variables for a new round and has both the player and the bot raise by an amount
            if (go == 1 && stage == 1)
            { //
                cashMoney.Visible = true;
                BotDraw();
                Shop(); // opens the shop to allow the player to buy cards and also checks if there are enough.
            }
            if (go == 1 && stage == 2)
            {
                IsEnough(1);
                for (int i = 0; i < 1; i++)
                {//
                    int ran = r.Next(0, buttons.Length);
                    if (buttons[ran] != buttonBad)
                    {
                        theFlop.Tag = buttons[ran].Name;
                        theFlop.BackgroundImage = buttons[ran].Image;
                        showCardB1 = true;
                        buttons[ran] = buttonBad;// sets some values to others for later
                    }
                    else { i--; } // if the button it choses is buttonBad it repeats again
                }
                RaisingStuff(1); // has the player raise after they can see the card
            }
            if (go == 1 && stage == 3)
            {
                IsEnough(1);
                for (int i = 0; i < 1; i++)
                {//
                    int ran = r.Next(0, buttons.Length);
                    if (buttons[ran] != buttonBad)
                    {
                        theTurn.Tag = buttons[ran].Name;
                        theTurn.BackgroundImage = buttons[ran].Image;
                        showCardB1 = true;
                        buttons[ran] = buttonBad;// sets some values to others for later
                    }
                    else { i--; } // if the button it choses is buttonBad it repeats again
                }
                RaisingStuff(1);// has the player raise after they can see the card
            }
            if (go == 1 && stage == 4)
            {
                IsEnough(1);
                for (int i = 0; i < 1; i++)
                {//
                    int ran = r.Next(0, buttons.Length);
                    if (buttons[ran] != buttonBad)
                    {
                        theRiver.Tag = buttons[ran].Name;
                        theRiver.BackgroundImage = buttons[ran].Image;
                        showCardB1 = true;
                        buttons[ran] = buttonBad;// sets some values to others for later
                    }
                    else { i--; } // if the button it choses is buttonBad it repeats again
                }
                RaisingStuff(1);// has the player raise after they can see the card
            }
            if (go == 2) { RaisingStuff(1); }
            if (stage == 5)
            {//
                if (Value(hand, hand2, theFlop, theTurn, theRiver) > Value(botHand, botHand2, theFlop, theTurn, theRiver))
                {
                    playerScore.Text = (int.Parse(playerScore.Text) + (int.Parse(pot.Text))).ToString();
                }
                pot.Text = "0";
                IsEnough(7);
                Game(0, 0); // judges the value of both the player cards and the bot cards to see who wins, gives teh player the pot if they win
            }
        }
        public void Lose()
        { //
            loser.Size = new Size(1396, 756);
            loser.BringToFront();
            loser.Visible = true;
            Refresh();
            System.Threading.Thread.Sleep(2000);
            this.Close(); // tells the player that they lost for 2 seconds before exiting the program
        }
        public void Win()
        {//
            Form3 y = new(playerScore.Text);
            var childForms = Application.OpenForms.OfType<Form2>().ToList();
            if (childForms.Count == 1) { childForms.FirstOrDefault()!.Close(); }
            y.MaximizeBox = false;
            y.Show();
            this.Hide(); // opens the leaderBoard form for the player to enter their name and be placed on the leaderBoard and closes all instances of form2
        }
        static double Value(PictureBox h, PictureBox h2, PictureBox f, PictureBox t, PictureBox r)
        {
            double value = 0;
            int[] symbols = new int[5];
            string[] suits = new string[5];
            PictureBox[] cardImages = [h, h2, f, t, r];
            for (int i = 0; i < cardImages.Length; i++)
            {
                double adding = 0;
                var a = cardImages[i].Tag.ToString();
                string[] split = a.Split("2");
                if (split.Length != 1) { adding = 2; symbols[i] = 2; }
                split = a.Split("3");
                if (split.Length != 1) { adding = 3; symbols[i] = 3; }
                split = a.Split("4");
                if (split.Length != 1) { adding = 4; symbols[i] = 4; }
                split = a.Split("5");
                if (split.Length != 1) { adding = 5; symbols[i] = 5; }
                split = a.Split("6");
                if (split.Length != 1) { adding = 6; symbols[i] = 6; }
                split = a.Split("7");
                if (split.Length != 1) { adding = 7; symbols[i] = 7; }
                split = a.Split("8");
                if (split.Length != 1) { adding = 8; symbols[i] = 8; }
                split = a.Split("9");
                if (split.Length != 1) { adding = 9; symbols[i] = 9; }
                split = a.Split("10");
                if (split.Length != 1) { adding = 10; symbols[i] = 10; }
                split = a.Split("Jack");
                if (split.Length != 1) { adding = 11; symbols[i] = 11; }
                split = a.Split("Queen");
                if (split.Length != 1) { adding = 12; symbols[i] = 12; }
                split = a.Split("King");
                if (split.Length != 1) { adding = 13; symbols[i] = 13; }
                split = a.Split("Ace");
                if (split.Length != 1) { adding = 14; symbols[i] = 14; } // adds to the adding variable the value of the card they have, adds the value to symbols for later //
                split = a.Split("club");
                if (split.Length != 1) { adding *= 1; suits[i] = "club"; }
                split = a.Split("diamond");
                if (split.Length != 1) { adding *= 1.025; suits[i] = "diamond"; }
                split = a.Split("heart");
                if (split.Length != 1) { adding *= 1.05; suits[i] = "heart"; }
                split = a.Split("spade");
                if (split.Length != 1) { adding *= 1.075; suits[i] = "spade"; } //multiplies the adding variable by the value the suit of the card they have, adds the value to suits for later
                value += adding;// adds the adding variable to the total score
            }
            for (int i = 0; i < symbols.Length; i++)
            { //
                for (int j = 0; j < symbols.Length - 1; j++)
                {
                    if (symbols[j] < symbols[j + 1])
                    {
                        (symbols[j], symbols[j + 1]) = (symbols[j + 1], symbols[j]);
                    }
                }
            } // sorts the symbols array into ascending order //
            int pairs = 0;
            bool tOAK = false;
            bool straight = true;
            bool flush = true;
            bool fOAK = true; // defines variables for later
            for (int i = 1; i < 4; i++)
            {
                if (symbols[i - 1] == symbols[i] && symbols[i] != symbols[i + 1]) { pairs++; }
                else if (symbols[i - 1] == symbols[i] && symbols[i] == symbols[i + 1]) { tOAK = true; } // tests for pair, 2 pairs, or three of a king
            }
            for (int i = 0; i < 4; i++) { if (symbols[i + 1] != symbols[i] + 1) { straight = false; } } // tests for a straight
            if (symbols[0] == 2 && symbols[1] == 3 && symbols[2] == 4 && symbols[3] == 5 && symbols[4] == 14) { straight = true; } // tests for a straight where the ace is worth 1
            for (int i = 0; i < 5; i++) { if (suits[i] != suits[i]) { flush = false; } } // tests for a flush
            int nW = 0; // defines number of wrong as 0 for the next step// 
            for (int i = 0; i < 4; i++)
            {
                if (symbols[i] != symbols[i + 1]) { nW++; }
                if (nW > 1) { fOAK = false; } // tests to see if there is four of a kind
            }
            if (tOAK == true && pairs == 1) { value *= 100; }
            else if (tOAK == true && pairs == 2) { value *= 400; }
            else if (pairs != 0) { value *= 25 * pairs; }
            if (straight == true) { value *= 200; }
            if (flush == true) { value *= 300; }
            if (fOAK == true) { value *= 500; }
            return value; // increases the players score if these tests return true
        }
        public void RaisingStuff(int u)
        { // 
            lowRaise.Text = blindProgress.Value.ToString();
            midRaise.Text = (blindProgress.Value + 2).ToString();
            highRaise.Text = (blindProgress.Value + 5).ToString();
            if (u == 0)
            {
                if (int.Parse(lowRaise.Text) < minRaise) { lowRaise.Text = minRaise.ToString(); }
                if (int.Parse(midRaise.Text) < minRaise * 2) { midRaise.Text = (2 * minRaise).ToString(); }
                if (int.Parse(highRaise.Text) < minRaise * 5) { highRaise.Text = (5 * minRaise).ToString(); }
            }
            else
            {
                if (int.Parse(lowRaise.Text) < minRaise) { lowRaise.Text = minRaise.ToString(); }
                if (int.Parse(midRaise.Text) < minRaise + 2) { midRaise.Text = (2 + minRaise).ToString(); }
                if (int.Parse(highRaise.Text) < minRaise + 5) { highRaise.Text = (5 + minRaise).ToString(); }
            }
            lowRaise.Visible = true;
            midRaise.Visible = true;
            highRaise.Visible = true;
            // gives the player the options to raise
            if (int.Parse(lowRaise.Text) > int.Parse(playerScore.Text)) { Lose(); } // the player loses if they cant afford to raise
        }
        private void PlayerRaise(object sender, EventArgs e)
        {
            Button button = (Button)sender; //
            if (int.Parse(button.Text) <= int.Parse(playerScore.Text))
            {
                state++;
                minRaise = int.Parse(button.Text);
                lowRaise.Visible = false;
                midRaise.Visible = false;
                highRaise.Visible = false;
                Deduct(int.Parse(button.Text), playerScore); coinFall = true; coin.Enabled = true;
                pot.Text = (int.Parse(pot.Text) + int.Parse(button.Text)).ToString();
                try
                {
                    if (bluff == true)
                    {
                        if (r.Next(0, 2) == 0) { pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString(); }
                        else { minRaise += 5; pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString(); }
                    }
                    else { minRaise += 2; pot.Text = (int.Parse(pot.Text) + (minRaise)).ToString(); }
                }
                catch { pot.Text = minRaise.ToString(); } // has the bot raise after the player does
                Game(1, state);//deducts the money on the button they pressed, hides all of these buttons increases game state and updates the players score, then continues the game
            }
        }
        private void BotDraw()
        {//
            IsEnough(2);
            for (int i = 0; i < 2; i++)
            {
                int ran = r.Next(0, buttons.Length);
                if (buttons[ran] != buttonBad)
                {
                    if (showCardB1 == false)
                    {
                        botHand.Tag = buttons[ran].Name;
                        botHand.BackgroundImage = buttons[ran].Image;
                        showCardB1 = true;
                        buttons[ran] = buttonBad;
                    }
                    else if (showCardB2 == false)
                    {
                        botHand2.Tag = buttons[ran].Name;
                        botHand2.BackgroundImage = buttons[ran].Image;
                        showCardB2 = true;
                        buttons[ran] = buttonBad;
                    }
                }
                else { i--; }
            }// the bot chooses and removes 2 cards from the deck
        }
        private void PlayerDraw(object sender, EventArgs e)
        {//
            if (lowRaise.Visible == false)
            {
                Button button = (Button)sender;
                if (showCardP1 == false)
                {
                    hand.Tag = button.Name;
                    hand.BackgroundImage = button.Image;
                    showCardP1 = true;
                    button.Enabled = false;
                    for (int i = 0; i < buttons.Length; i++) { if (buttons[i] == button) { buttons[i] = buttonBad; } }
                }
                else if (showCardP2 == false)
                {
                    hand2.Tag = button.Name;
                    hand2.BackgroundImage = button.Image;
                    showCardP2 = true;
                    button.Enabled = false;
                    for (int i = 0; i < buttons.Length; i++) { if (buttons[i] == button) { buttons[i] = buttonBad; } }
                    Game(2, 0);
                }
            }// the player clicks a button, the button has an image, which is taken and used as the image on the card
        }// after the player chooses 2 cards it calls start round with 2 0 to indicate a specific section of it
        private int Shop()
        { //
            int Cost = 0;
            for (int i = 0; i < buttons.Length; i++) { if (buttons[i].Enabled == true) { buttons[i] = buttonBad; } }
            IsEnough(2);
            Form2 uhh = new(buttons, buttonBad, playerScore, blindProgress);
            var childForms = Application.OpenForms.OfType<Form2>().ToList();
            if (childForms.Count == 1) { childForms.FirstOrDefault()!.Close(); }
            uhh.MaximizeBox = false;
            uhh.Show();
            return Cost; // opens the shop which allows the player to buy cards to use later in the game
        }// the shop is on a second form so that it opens in a new window
        private void Help(object sender, EventArgs e)
        {//
            if (nOfHelp.Value > 0)
            {
                if (botHand.Image != null) { botHand.Image = null; }
                else if (botHand2.Image != null) { botHand2.Image = null; }
                nOfHelp.Value--;
            }
        }// shows the player 1 of the bots cards, if both are visible, does nothing
        private void CashMoney_Click(object sender, EventArgs e) { Win(); } // if the player clicks cash in they are taken to the win screen
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
            if (((lx == 0 && ly == 0) || (lx == screenWidth - this.Width && ly == 0) || (lx == 0 && ly == screenHeight - this.Height) || (lx == screenWidth - this.Width && ly == screenHeight - this.Height)) && int.Parse(playerScore.Text) > 0)
            {
                Deduct(1, playerScore); coinFall = true; coin.Enabled = true;
            }
            if (eBLX <= 0) { eBDX = false; }//
            if (eBLX >= this.Width - 64) { eBDX = true; }
            if (eBDX == false) { eBLX += 4; }
            if (eBDX == true) { eBLX -= 4; }
            if (eBLY <= 0) { eBDY = false; }
            if (eBLY >= this.Height - 64) { eBDY = true; }
            if (eBDY == false) { eBLY += 8; }
            if (eBDY == true) { eBLY -= 8; }
            exitButton.Location = new Point(eBLX, eBLY); // same as before but for the exit button //
            if (coinFall == true) { coin.Visible = true; coin.Location = new Point(coin.Location.X, coin.Location.Y + 8); }
            if (coin.Location.Y > screenHeight) { coin.Visible = false; coin.Location = new Point(r.Next(0, this.Width), -231); coinFall = false; }
            exitButton.Location = new Point(eBLX, eBLY); // makes the fally sky coin fall
        }
        private void ExitButton_Click(object sender, EventArgs e) { this.Close(); } // exit button closes the program when clicked
        private void Coin_Click(object sender, EventArgs e) { Deduct(-1, playerScore); coin.Enabled = false; } // you get 1 coin for clicking the fally sky coin but you only click it once per time it shows up
    }
    class Face(string n, Image f)
    {//
        readonly private string name = n;
        readonly private Image faceImage = f;
        public string GetName() { return name; }
        public Image GetFace() { return faceImage; }
    }// face class has a name and a face such that when the same name is chosen twice, the image stays the same
}