using System;
using System.Collections.Generic;

namespace Flappy_bird
{
    class Program
    {
        //member variables
        public static char[,] gameCell = new char[10,20];
        public static bool resetTF = true;
        public static int score;
        public static int highscore = 0;
        public static int birdYpos = 4;
        public static char bird = 'O';
        static void Main(string[] args)
        {
            if(resetTF == true)
            {
                ResetGameCell();
                resetTF = false;
            }

            //used classes
            Grid FlappyGrid = new Grid(gameCell);
            Pipe FlappyPipe = new Pipe(gameCell);
            Bird FlappyBird = new Bird(bird,gameCell,birdYpos);

            //gameloop
            while(true)
            {
                Console.Clear();
                FlappyGrid.moveGrid();
                if(FlappyBird.checkDeath()==true) break;
                (gameCell,score) = FlappyPipe.SetPipe();
                FlappyBird.DestroyTrace();
                FlappyBird.move();
                if(score > highscore) highscore = score;
                Console.SetCursorPosition(22,0);
                System.Console.Write("Your Score is: {0}",score);
                Console.SetCursorPosition(22,1);
                System.Console.Write("Your HighScore is: {0}",highscore);
                FlappyGrid.RenderMap();
                System.Threading.Thread.Sleep(180);
            }
        }

        //Funktion for creating blank grid
        static char[,] ResetGameCell()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    gameCell[i,j] = ' ';
                }
            }
            return gameCell;
        }
    }

    //Class for game grid
    class Grid
    {
        //Membervariables and constructor
        public char[,] gameCell { get; set; }
        public Grid(char[,] gameCell)
        {
            this.gameCell = gameCell;
        }

        //Funktion for moving grid
        public char[,] moveGrid()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    if(x == 19)
                    {
                        gameCell[y,x] = ' ';
                    }
                    else
                    {
                        gameCell[y,x] = gameCell[y,x+1];
                    }
                    
                }   
            }
            return gameCell;
        }

        //Funktion for writing gameMap
        public void RenderMap()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.SetCursorPosition(j,i);
                    Console.Write(gameCell[i,j]);
                }
            }
        }
    }

    //Class for bird
    class Bird
    {
        //Membervariables and constructor
        public char bird { get; set; }
        public char[,] gameCell { get; set; }
        public int birdYpos { get; set; }
        public Bird(char bird, char[,] gameCell, int birdYpos)
        {
            this.bird = bird;
            this.gameCell = gameCell;
            this.birdYpos = birdYpos;
        }

        //Funktion for moving bird
        public char[,] move()
        {
            birdYpos++;
            if(jump()==true)
            {
                 birdYpos = birdYpos - 2;
            }

            gameCell[birdYpos,10] = bird;
            return gameCell;
        }

        //Funktion for destroying trace left behinde
        public char[,] DestroyTrace()
        {
            gameCell[birdYpos,9] = ' ';
            return gameCell;
        }

        public bool checkDeath()
        {
            if(gameCell[birdYpos,10] == 'X')
            {
                return true;
            }
            else return false;
        }

        //Funktion for making bird fly up
        public bool jump()
        {
            if(Console.KeyAvailable)
            {
                if(Console.ReadKey().KeyChar == ' ')
                {
                    return true;
                }
            }
            return false;
        }

    }

    //Class for pipe
    class Pipe
    {
        //Membervariables and constructor
        Random rnd = new Random();
        public int score = 0;
        public int pipeHole = 0;
        public char[,] gameCell { get; set; }
        public int counter = 0;

        public int counter2 = 0;
        bool firstSpawn = true;
        public Pipe(char[,] gameCell)
        {
            this.gameCell = gameCell;
        }

        //Sets pipe to grid
        public (char[,],int) SetPipe()
        {
            if(counter == 8)
            {
                GeneratePipe();
                for (int i = 0; i < 10; i++)
                {
                    gameCell[i,19] = 'X';
                }
                for (int i = 0; i < rnd.Next(2,6); i++)
                {
                    gameCell[pipeHole,19] = ' ';
                    pipeHole++;
                    if(pipeHole == 9) break;
                }
                if(firstSpawn == true) firstSpawn = false;
                else score++;
                
                counter = 0;
            }
            counter++;
            return (gameCell,score);
        }

        //Generates pipe lenght
        public int GeneratePipe()
        {   
            pipeHole = rnd.Next(0,8); 
            return pipeHole;
        }
    }
}    