class SnakeGame
{
    class GraphNode
    {
        public uint x, y;
        public GraphNode(uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private uint width = 20, height = 10;
    private LinkedList<GraphNode> snake;
    private HashSet<(uint, uint)> snakeBody;
    private GraphNode food;
    private String direction = "Right";
    private String nextDirection = "Right";
    private Random random = new Random();
    private bool gameOver = false;

    public SnakeGame()
    {
        InitializeSnake();
        PrepFood();
    }

    static void Main()
    {
        SnakeGame game = new SnakeGame();

        game.PrintTable();

        Console.WriteLine("Press Enter to start the game.");
        Console.ReadLine();

        Thread Controller = new Thread(() =>
        {
            while(!game.gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    game.InputReader(key);
                }
            }
        });

        Controller.Start();

        game.PrintTable();

        while (!game.gameOver)
        {
            game.Update();
            game.PrintTable();
            Thread.Sleep(200);
        }

        game.PrintTable();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Game Over!");
        Console.ResetColor();
    }

    private void Update()
    {
        Move();
    }

    private void PrintTable()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(new string('#', (int)width + 3));
        for (uint y = 0; y <= height; y++)
        {
            Console.Write("#");
            Console.ResetColor();
            for (uint x = 0; x <= width; x++)
            {
                if (snake.First.Value.x == x && snake.First.Value.y == y)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("O");
                }
                else if (snakeBody.Contains((x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("X");
                }
                else if (food.x == x && food.y == y)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("*");
                }
                else
                    Console.Write(".");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("#");
        }
        Console.WriteLine(new string('#', (int)width + 3));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Score: " + (snake.Count - 1) * 10);
        Console.ResetColor();
    }

    private void InitializeSnake()
    {
        snake = new LinkedList<GraphNode>();
        snakeBody = new HashSet<(uint, uint)>();
        snake.AddFirst(new GraphNode(width / 2, height / 2));
        snakeBody.Add((width / 2, height / 2));
    }

    private void PrepFood()
    {
        do
        {
            food = new GraphNode((uint)random.Next(0, (int)width), (uint)random.Next(0, (int)height));
        } while (snakeBody.Contains((food.x, food.y)));
    }

    private void InputReader(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (direction != "Down" && nextDirection != "Down")
                    nextDirection = "Up";
                break;
            case ConsoleKey.DownArrow:
                if (direction != "Up" && nextDirection != "Up")
                    nextDirection = "Down";
                break;
            case ConsoleKey.LeftArrow:
                if (direction != "Right" && nextDirection != "Right")
                    nextDirection = "Left";
                break;
            case ConsoleKey.RightArrow:
                if (direction != "Left" && nextDirection != "Left")
                    nextDirection = "Right";
                break;
            case ConsoleKey.W:
                if (direction != "Down" && nextDirection != "Down")
                    nextDirection = "Up";
                break;
            case ConsoleKey.S:
                if (direction != "Up" && nextDirection != "Up")
                    nextDirection = "Down";
                break;
            case ConsoleKey.A:
                if (direction != "Right" && nextDirection != "Right")
                    nextDirection = "Left";
                break;
            case ConsoleKey.D:
                if (direction != "Left" && nextDirection != "Left")
                    nextDirection = "Right";
                break;
        }
        Thread.Sleep(200);
    }

    private void Move()
    {
        direction = nextDirection;
        GraphNode newHead = snake.First.Value;
        if (newHead == null)
            return;

        uint newX = newHead.x, newY = newHead.y;

        switch (direction)
        {
            case "Right":
                newX++;
                break;

            case "Left":
                newX--;
                break;

            case "Up":
                newY--;
                break;

            case "Down":
                newY++;
                break;
        }

        if(newX >= width || newY >= height)
        {
            gameOver = true;
            return;
        }

        if (snakeBody.Contains((newX, newY)))
        {
            gameOver = true;
            return;
        }

        GraphNode newerHead = new GraphNode(newX, newY);
        snake.AddFirst(newerHead);
        snakeBody.Add((newHead.x, newHead.y));
        FoodHandler(newerHead);
    }

    private void FoodHandler(GraphNode newHead)
    {
        if (newHead.x == food.x && newHead.y == food.y)
            PrepFood();
        else
        {
            snakeBody.Remove((snake.Last.Value.x, snake.Last.Value.y));
            snake.RemoveLast();
        }
    }
}