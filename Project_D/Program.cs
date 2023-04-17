using System;

namespace Project_D
{
    /// <summary>
    /// 플레이어가 움직이는 방향을 나타낸 것이다.
    /// 위방향, 아래방향, 왼쪽방향, 오른쪽방향, 그리고 그외를
    /// UP,DOWN,LEFT,RIGHT,NONE을 열거형으로 보기 편하게 정리했다.
    /// 실제로는 UPDATE에서 UP,DOWN,LEFT,RIGHT 키를 누르면 작동한다.
    /// </summary>
    public enum KEY
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UNDO,
        F1,
        NONE
    }

    /// <summary>
    /// 타일의 종류를 나타낸 것이다.
    /// 빈공간, 벽, 박스, 골지점, 골과 박스가 만났을 경우를
    /// NONE, WALL, BOX, GOAL, BOXGOAL, Player.. 을 열거형으로 보기 편하게 정리했다.
    /// 실제로는 UPDATE에서 NONE, WALL, BOX, GOAL, BOXGOAL를 검사하고, RENDER에서 그리게 된다.
    /// </summary>
    public enum TILE
    {
        NONE,
        WALL,
        BOX,
        GOAL,
        BOXGOAL,
        PLAYER,
        DEFAULT
    }

    /// <summary>
    /// 플레이어의 좌표 x,y 를 한묶음으로 정리하기 위하여 int형 정수 x, y를 가지는 구조체 변수이다.
    /// </summary>
    public struct Pos
    {
        public int x;
        public int y;

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    /// <summary>
    /// 이 코드는 게임의 메인 프로그램을 나타냅니다. Main 메소드는 콘솔에서 메뉴를 표시하고, 사용자가 선택한 메뉴에 따라 게임을 실행합니다.
    /// 메인 루프(while (true))는 사용자가 종료 메뉴(3)를 선택할 때까지 반복됩니다.
    /// 각 메뉴 항목에는 콘솔에 표시될 메시지와 해당 메뉴 항목을 실행하기 위한 코드가 포함되어 있습니다.
    /// Editor 모드(1) 또는 Game Play 모드(2)가 선택될 때마다 해당 모드의 Init() 메소드가 호출되어 게임이 초기화됩니다.
    /// Init() 메소드가 성공적으로 실행된 후에는 해당 모드의 GameLoop() 메소드가 반복적으로 호출되어 게임이 실행됩니다.
    /// 사용자가 Editor 모드 또는 Game Play 모드를 종료하면 해당 모드의 Release() 메소드가 호출되어 게임에서 사용한 자원들이 해제됩니다.
    /// 마지막으로 사용자가 종료 메뉴를 선택하면 Environment.Exit() 메소드를 호출하여 프로그램이 종료됩니다.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                {
                    Console.CursorVisible = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Title = "Project-D";
                    Console.SetWindowSize(60, 18);
                    Console.WriteLine("[ Main Menu ]\n");
                    Console.WriteLine("1. Editor Mode");
                    Console.WriteLine("2. Game Play");
                    Console.WriteLine("3. Exit");

                    Console.SetCursorPosition(0, 5);
                }

                ConsoleKeyInfo menu = Console.ReadKey();

                switch (menu.Key)
                {
                    // 1번키 : MapEditorMode
                    case ConsoleKey.D1:
                        if (!Game.GetInstance().EditorInit())
                        { 
                            Console.WriteLine("멥 에디터를 불러오는데 실패했습니다.");
                            break;
                        }
                        break;
                    // 2번키 : GamePlayMode
                    case ConsoleKey.D2:
                        if (!Game.GetInstance().GameInit())
                        {
                            Console.WriteLine("게임 초기화에 실패하였습니다.");
                            break;
                        }
                        break;
                    // 3번키 : Program Exit
                    case ConsoleKey.D3:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
