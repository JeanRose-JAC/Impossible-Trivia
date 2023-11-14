using System;
using System.Timers;
using static System.Console;

namespace Project
{
    class Program
    {
        //==================
        // GLOBAL VARIBALES
        //==================

        // Playable space limits
        const int COL_MIN = 1;
        const int COL_MAX = 71;
        const int ROW_MIN = 6;
        const int ROW_MAX = 26;

        //Initialize player coordinates
        static double col = COL_MAX / 2;
        static double row = ROW_MAX / 2;

        // Varaibles for scoring
        static bool endGame = false;
        static int level;
        static int score;

        //Classes for making the letters move
        static Timer letterTimer = new Timer();
        static Random ran = new Random();

        //Variables for the multiple choice
        //Initialize them with random coordinates
        static int A_col = ran.Next(COL_MIN, COL_MAX);
        static int A_row = ran.Next(ROW_MIN, ROW_MAX);
        static int B_col = ran.Next(COL_MIN, COL_MAX);
        static int B_row = ran.Next(ROW_MIN, ROW_MAX);
        static int C_col = ran.Next(COL_MIN, COL_MAX);
        static int C_row = ran.Next(ROW_MIN, ROW_MAX);

        static void Main(string[] args)
        {
            //===============================================
            // - SETTING UP THE CONSOLE WINDOW
            //
            // - Specifies the size and color of the console
            //===============================================

            int width = 150;
            int height = 40;
            SetWindowSize(width, height);
            BackgroundColor = ConsoleColor.DarkGray;
            ForegroundColor = ConsoleColor.Black;

            //================================================================
            // - MENU
            //
            // - Lets user choose a menu by inputting the corresponding number
            // - 1: Starts the game
            // - 2: Goes to the instruction screen
            // - 3: Quits the menu
            //================================================================


            string gameName = @"
 ________         __                                 __              __   __                             
|  |  |  |.-----.|  |.----.-----.--------.-----.    |  |_.-----.    |  |_|  |--.-----.                   
|  |  |  ||  -__||  ||  __|  _  |        |  -__|    |   _|  _  |    |   _|     |  -__|                   
|________||_____||__||____|_____|__|__|__|_____|    |____|_____|    |____|__|__|_____|                   
                                                                                                         
 _______                                    __ __     __             _______        __         __        
|_     _|.--------.-----.-----.-----.-----.|__|  |--.|  |.-----.    |_     _|.----.|__|.--.--.|__|.---.-.
 _|   |_ |        |  _  |  _  |__ --|__ --||  |  _  ||  ||  -__|      |   |  |   _||  ||  |  ||  ||  _  |
|_______||__|__|__|   __|_____|_____|_____||__|_____||__||_____|      |___|  |__|  |__| \___/ |__||___._|
                  |__|                                                                                   ";


            int choice;
            int FIRST_CHOICE = 1;
            int LAST_CHOICE = 3;

            do
            {
                Clear();
                Console.WriteLine(gameName);
                WriteLine("\n1. Start the game");
                WriteLine("2. Instructions");
                WriteLine("3. Quit");
                Write("\nEnter your option: ");

                do
                {
                    choice = integerValidation();
                    //If user inputs a number that is out of range : ERROR
                    if (choice < FIRST_CHOICE || choice > LAST_CHOICE)
                    {
                        WriteLine("Invalid input. Please try again.");
                    }
                }
                //Loops until user puts a correct input
                while (choice < FIRST_CHOICE || choice > LAST_CHOICE);

                switch (choice)
                {
                    case 1:
                        Clear();
                        getGame();
                        goBackToMenu();
                        break;
                    case 2:
                        Clear();
                        getInstructions();
                        goBackToMenu();
                        break;
                    case 3:
                        WriteLine("You just quit the game.");
                        break;
                }

            } 
            //Menu loops until player chooses 3 to quit
            while (choice != LAST_CHOICE);

        }

        #region.validateInput

        static int integerValidation()
        {
            //==========================================================
            // - INTEGER VALIDATION
            //
            // - A function that checks whether the input is an integer.
            // - If input is wrong, an error message is displayed.
            // - It loops until a correct integer is typed.
            //==========================================================

            string answer;
            bool success;
            int correctInt;

            do
            {
                answer = ReadLine();
                success = int.TryParse(answer, out correctInt);
                if (!success || correctInt < 0)
                {
                    WriteLine("Invalid input. Please try again.");
                }
            } while (!success || correctInt < 0);

            return correctInt;
        }

        #endregion.validateInput

        #region.getGame

        static void getGame()
        {
            //============================================================
            // - GET GAME
            //
            // - Calls all of the function needed to get the game started
            //============================================================


            CursorVisible = false;
            const int numLevels = 3;
            //Initializes score to 0 every new game
            score = 0;

            //Questions with the multiple choice
            string[] question = new string[numLevels]
            {
             "What is the capital of Burkina Faso? \nA.) Ogougdauauo \nB.) Ouagadougou \nC.) Ouguuoodaga \n",
             "What is Plankton's first name in Spongebob Squarepants? \nA.) Sheldon \nB.) Harold \nC.) Dennis \n",
             "What is type of sprinkler system used in data centers? \nA.) Chemical \nB.) Dry \nC.) Wet \n"
            };

            //Game only ends when player has gone through all levels
            for (level = 0; level < numLevels; level++)
            {
                endGame = false;
                WriteLine(question[level]);
                printPlayableSpace();
                setUpLetterTimer();
                setUpControls();
            }
            WriteLine("Final score: " + score);
        }

        #region.printArea
        static void printPlayableSpace()
        {
            //----------------------------------------------------------------------
            // + Playable Space
            //
            // + Displays the rectangular space where the player chases the letters
            // + Player cannot go beyond the perimeter
            //----------------------------------------------------------------------


            string[] grid = new string[22]
            {
                "#----------------------------------------------------------------------#",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "|                                                                      |",
                "#----------------------------------------------------------------------#",
            };

            foreach (string gridPart in grid)
            {
                WriteLine(gridPart);
            }
        }
        #endregion.printArea

        #region.makePlayerMove

        //------------------------------------------------------------------------------------------------------------
        // + Player
        //
        // + For each movement of the player icon:
        // + UpArrow: the icon stays in the same col but moves 1 row up (subtracting 1 from current row value)
        // + DownArrow: Same col but moves one row down (adding 1 to the current row)
        // + RightArrow: Icon moves to the right col (substracting 1 from current col) but stays in the same row
        // + LeftArrow: Moves to the left col (adding 1 to current col) but same row
        //
        // + Each key stroke is being sent to makePlayerMove() in order to erase the old position and draw the new one
        //-------------------------------------------------------------------------------------------------------------

        static void setUpControls()
        {
            //From notes of day 37 by Sandy

            ConsoleKeyInfo key;

            while (!endGame)
            {
                key = ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        makePlayerMove(col, row, col, row - 1);
                        row = row - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        makePlayerMove(col, row, col, row + 1);
                        row = row + 1;
                        break;
                    case ConsoleKey.RightArrow:
                        makePlayerMove(col, row, col + 1, row);
                        col = col + 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        makePlayerMove(col, row, col - 1, row);
                        col = col - 1;
                        break;
                }
            }
        }

        static void makePlayerMove(double oldcol, double oldrow, double col, double row)
        {
            //if statements make sure that player does not go beyond the boundaries
            if (oldcol < COL_MIN) oldcol = COL_MIN + 1;
            if (oldcol > COL_MAX - 1) oldcol = COL_MAX - 1;
            if (oldrow < ROW_MIN) oldrow = ROW_MIN + 1;
            if (oldrow > ROW_MAX - 1) oldrow = ROW_MAX - 1;
            //Erases old position
            SetCursorPosition((int)oldcol, (int)oldrow);
            Write(" ");

            if (col < COL_MIN) col = COL_MIN + 1;
            if (col > COL_MAX - 1) col = COL_MAX - 1;
            if (row < ROW_MIN) row = ROW_MIN + 1;
            if (row > ROW_MAX - 1) row = ROW_MAX - 1;
            //Draws new position
            SetCursorPosition((int)col, (int)row);
            Write("$");

            //Everytime that the player moves,
            //this function verifies if they have landed on a letter
            checkingTheAnswers();
        }
        #endregion.makePlayerMove

        #region.makeLettersMove

        //-------------------------------------------------------
        // + Letters
        //
        // + Every 3 sec, the letters change coordinates randomly,
        // + making it harder for players to catch their answer
        //--------------------------------------------------------

        static void setUpLetterTimer()
        {
            letterTimer.Interval = 3000; //milliseconds
            letterTimer.Elapsed += makeLettersMove;
            letterTimer.AutoReset = true;
            letterTimer.Enabled = true;
        }

        static void makeLettersMove(Object s, ElapsedEventArgs e)
        {
            //erases the old position
            SetCursorPosition(A_col, A_row);
            Write(" ");
            A_col = ran.Next(ROW_MIN, ROW_MAX);
            A_row = ran.Next(ROW_MIN, ROW_MAX);
            //draws the new position
            SetCursorPosition(A_col, A_row);
            Write("A"); 

            SetCursorPosition(B_col, B_row);
            Write(" ");
            B_col = ran.Next(ROW_MIN, ROW_MAX);
            B_row = ran.Next(ROW_MIN, ROW_MAX);
            SetCursorPosition(B_col, B_row);
            Write("B");

            SetCursorPosition(C_col, C_row);
            Write(" ");
            C_col = ran.Next(ROW_MIN, ROW_MAX);
            C_row = ran.Next(ROW_MIN, ROW_MAX);
            SetCursorPosition(C_col, C_row);
            Write("C");

        }
        #endregion.makeLettersMove

        #region.verifyAnswers

        //----------------------------------------------------------------------------
        // + Checking the answer
        //
        // + If player lands on a letter, it means that they have entered their answer
        // + Player's answer gets saved, displayed and verified
        // + Their points is also counted 
        //----------------------------------------------------------------------------

        static void checkingTheAnswers()
        {
            string answer;
            ConsoleKeyInfo enter;
            enter = ReadKey(true);

            //Cheat sheet
            // Level 0 ans = B
            // Level 1 & 2 ans = A

            if (level == 0)
            {
                if (col == B_col && row == B_row)
                {
                    answer = "B";
                    rightAns(answer);
                    //The repeated lines of code below should only be executed if the player lands on a later
                    //Therefore, they cannot be outside the 'if - else if'
                    WriteLine("\nPress ENTER to go to next level.");
                    while (enter.Key != ConsoleKey.Enter)
                        enter = ReadKey(true);
                    Clear();
                    endGame = true;
                }
                else if (col == A_col && row == A_row || col == C_col && row == C_row)
                {

                    if (col == C_col) answer = "C";
                    else answer = "A";
                    wrongAns(answer);
                    WriteLine("\nPress ENTER to go to next level.");
                    while (enter.Key != ConsoleKey.Enter)
                        enter = ReadKey(true);
                    Clear();
                    endGame = true;
                }
            }
            else if (level == 1 || level == 2)
            {
                if (col == A_col && row == A_row)
                {
                    answer = "A";
                    rightAns(answer);
                    if (level == 1)
                    {
                        WriteLine("\nPress ENTER to go to next level.");
                        while (enter.Key != ConsoleKey.Enter)
                            enter = ReadKey(true);
                        Clear();
                        endGame = true;
                    }
                    else
                    {
                        WriteLine("\nPress ENTER to finish game.");
                        while (enter.Key != ConsoleKey.Enter)
                            enter = ReadKey(true);
                        Clear();
                        endGame = true;
                    }
                }
                else if (col == B_col && row == B_row || col == C_col && row == C_row)
                {
                    if (col == C_col) answer = "C";
                    else answer = "B";
                    wrongAns(answer);
                    if (level == 1)
                    {
                        WriteLine("\nPress ENTER to go to next level.");
                        while (enter.Key != ConsoleKey.Enter)
                            enter = ReadKey(true);
                        Clear();
                        endGame = true;
                    }
                    else
                    {
                        WriteLine("\nPress ENTER to finish game.");
                        while (enter.Key != ConsoleKey.Enter)
                            enter = ReadKey(true);
                        Clear();
                        endGame = true;
                    }
                }
            }
        }

        static void rightAns(string right)
        {
            //Stops the letters from moving
            letterTimer.Stop();
            //Increments score by 1 when user gets the correct answer
            score++;
            //Makes sure that the WriteLine does not print on the playable area
            SetCursorPosition(COL_MIN - 1, ROW_MAX + 2);
            WriteLine("You selected " + right + ". You got the RIGHT answer!");
        }

        static void wrongAns(string wrong)
        {
            letterTimer.Stop();
            SetCursorPosition(COL_MIN - 1, ROW_MAX + 2);
            WriteLine("You selected " + wrong + ". You got the WRONG answer!");
        }

        #endregion.verifygAnswers

        #endregion.getGame

        #region.getInstructions

        static void getInstructions()
        {
            //===========================================
            // - GET INSTRUCTIONS
            //
            // - Displays the instructions on the screen
            //===========================================


            WriteLine("INSTRUCTIONS");
            WriteLine("1.) Read the question and the multiple choice." +
                "\n2.) When you are ready to answer, press any arrow key to make the player icon ($) appear on the grid." +
                "\n3.) Using the arrow keys to move around, chase the letter of your answer.");

            WriteLine("\nTIPS");
            WriteLine("1.) There is not time limit, so take your time :)" +
                "\n2.) Actively move your icon around because the letters can generate where you are and therefore" +
                       " saving that letter as the answer even though that was not your choice. So be careful!!");
        }

        #endregion.getInstructions

        #region.goBackToMenu

        static void goBackToMenu()
        {
            //============================================================================
            // - GO BACK TO MENU
            //
            // - When a menu option is done executing, user is sent back to the MAIN MENU
            //============================================================================

            WriteLine("\nPress any key to go back to Main Menu");
            ReadKey(true);
        }

        #endregion.goBackToMenu
    }
}
