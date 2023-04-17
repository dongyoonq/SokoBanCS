using System;
using System.Diagnostics;
using System.Threading;

namespace Project_D
{
    // GameMode 클래스는 게임의 전반적인 동작을 담당하는 클래스입니다.
    // 게임 루프(GameLoop)를 실행하며, 게임 입력, 갱신, 출력, 종료 여부를 확인합니다.
    // 또한, 게임 상태를 나타내는 여러 변수들을 정의하고 초기화합니다.
    // singleton 패턴으로 구현되어 있어서, GetInstance() 메소드를 통해 인스턴스를 가져와 사용합니다.
    class GameMode
    {
        static bool flag = false;               // flag 변수는 되돌리기 버튼이 눌렸는지 나타내는 변수입니다.
        static bool IsPrevReset = false;        // IsPrevReset 변수는 이전 상태로 되돌리는 기능이 실행되었는지를 나타내는 변수입니다.

        static Map prevMap = new Map();         // prevMap은 이전 상태의 맵 정보를 저장하는 변수입니다.
        static Map originMap = new Map();       // originMap은 초기 맵 정보를 저장하는 변수입니다.

        static KEY input;                       // input 변수는 사용자의 입력값을 가지는 KEY 열거형 변수입니다.
        static Pos prevPos;                     // prevPos 변수는 이전에 플레이어가 위치한 좌표를 저장하는 변수입니다.

        static int PrevGoalCount = 0;           // PrevGoalCount는 이전에 달성한 골의 개수를 저장하는 변수입니다.
        static int GoalCount = 0;               // GoalCount는 현재 달성한 골의 개수를 저장하는 변수입니다.
        static int PurposeCount = 0;            // PurposeCount는 맵에 존재하는 골지점의 개수를 저장하는 변수입니다.
        static int MoveCount = 0;               // MoveCount는 플레이어가 이동한 횟수를 저장하는 변수입니다.
        static int Level = 0;                   // Level은 현재 진행중인 레벨을 저장하는 변수입니다.

        static ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.Magenta };

        private static GameMode instance;
        public static GameMode GetInstance()    // GetInstance() 메소드는 instance 변수가 null일 경우, 새로운 인스턴스를 생성하고 반환합니다.
        {
            if (instance == null)
                instance = new GameMode();
            return instance;
        }

        private GameMode()
        {

        }

        // GameLoop() 메소드는 게임 루프를 실행하며, 각 단계마다 Input(), Update(), Render(), End() 메소드를 실행합니다
        public bool GameLoop()                  
        
        {
            // 2. 게임 입력 (Input)
            if (!Input())
                return false;

            // 3. 게임 갱신 (Update)
            Update(input, Player.GetInstance());

            // 4. 게임 출력 (Render)
            Render(Player.GetInstance());

            // 5. 게임이 종료되는지 확인
            if (End())
            {
                return false;
            }

            return true;
        }

        // 이 메소드는 게임의 초기화를 담당하는 메소드입니다.
        // Console 창의 초기화를 진행하고, 게임 장면을 초기화합니다.
        // 현재 레벨의 맵을 불러와서 이전 맵과 원본 맵을 설정하며, 목표 개수를 설정합니다.
        // 플레이어를 맵 상의 시작 위치에 놓습니다.
        // Console 창의 크기를 조정하고, 렌더링을 진행합니다.
        // 모든 초기화가 성공적으로 완료되면 true를 반환합니다.
        // 게임이 실행될때 가장 먼저 생성 되어야할 것들이 생성이 되어야 true형으로 반환되어 게임이 실행된다.
        // 초기화 될것이 하나라도 생성이 안되면 false로 반환되어 게임이 실행되지 않는다.
        public bool Init()
        {
            // 콘솔 초기화
            Console.Clear();
            Console.Title = "Project-D GameMode";
            Console.CursorVisible = false;

            // 장면 초기화
            if (!Scene.GetInstance().Init())
                return false;

            Scene.GetInstance().SetScene(Level);

            prevMap.ResetMap(Scene.GetInstance().GetCurrMap());
            originMap.ResetMap(Scene.GetInstance().GetCurrMap());
            originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
            prevMap.CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
            PurposeCount = Scene.GetInstance().GetPurposeCount();

            // 플레이어 초기화
            if (Player.GetInstance() == null)
                return false;
            Scene.GetInstance().GetCurrMap().SetPlayerStartPoint();

            Console.SetWindowSize(50, Scene.GetInstance().GetCurrMap().oMap.GetLength(0) + 9);

            // 그리기 초기화
            if (!Render(Player.GetInstance()))
                return false;

            return true;
        }

        /// <summary>
        /// 키 입력에 따라 KEY 정보를 갱신해준다.
        /// ConsoleKeyInfo 구조체 변수 info에 키정보를 입력받아 그 키가 무슨 키에 따라 
        /// DIRECTION에 키값을 저장해준다.
        /// </summary>
        private static bool Input()
        {
            ConsoleKeyInfo info = Console.ReadKey();
            switch (info.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    prevPos = Player.GetInstance().GetPos();
                    input = KEY.UP;
                    MoveCount++;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    prevPos = Player.GetInstance().GetPos();
                    input = KEY.DOWN;
                    MoveCount++;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    prevPos = Player.GetInstance().GetPos();
                    input = KEY.LEFT;
                    MoveCount++;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    prevPos = Player.GetInstance().GetPos();
                    input = KEY.RIGHT;
                    MoveCount++;
                    break;
                case ConsoleKey.R:                                          // R 키를 눌렀을 때, 게임을 재시작한다.
                    Reset();                                                // 게임을 초기화한다.
                    // 현재 맵 정보를 원본 맵 정보에 복사한다
                    originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1)); 
                    // 목적지 개수를 현재 맵의 목적지 개수로 초기화한다.
                    PurposeCount = Scene.GetInstance().GetPurposeCount(); 
                    // 이전 맵 정보를 현재 맵 정보로 초기화한다.
                    prevMap.ResetMap(Scene.GetInstance().GetCurrMap()); 
                    // 이전 맵 정보를 원본 맵 정보로 복사한다.
                    prevMap.CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1)); 
                    Scene.GetInstance().GetCurrMap().SetPlayerStartPoint(); // 플레이어의 시작 위치를 설정한다.
                    break;
                case ConsoleKey.T:                                          // T 키를 눌렀을 때, 이전 상태로 되돌린다.
                    input = KEY.UNDO;
                    break;
                case ConsoleKey.D1:                                         // D1~D4 키를 눌렀을 때, 1~4 번 맵을 이동한다.
                    MoveLevel(0);
                    break;
                case ConsoleKey.D2:
                    MoveLevel(1);
                    break;
                case ConsoleKey.D3:
                    MoveLevel(2);
                    break;
                case ConsoleKey.D4:
                    MoveLevel(3);
                    break;
                case ConsoleKey.F1:                                         // 현재 맵의 목적지 개수와 현재 골 개수를 같게 만들어 다음 맵으로 넘어가게 한다.
                    GoalCount = PurposeCount;
                    input = KEY.F1;
                    break;
                case ConsoleKey.F2:                                         // 이전 맵으로 되돌린다.
                    if (Level == Scene.GetInstance().GetListMap().Count - 1)
                        return false;
                    Reset();                                                // 현재맵을 기존맵으로 돌려주고
                    NextLevelInit();                                        // 다음맵으로 넘어간다
                    Level++;
                    break;
                case ConsoleKey.F3:                                         // 다음 맵으로 넘어간다.
                    if (Level == 0)
                        return false;
                    Reset();
                    PrevLevelInit();
                    Level--;
                    break;
                case ConsoleKey.Escape:                                     // ESC버튼을 누르면 Main Menu로 돌아간다.
                    Reset();
                    Level = 0;
                    return false;
                default:
                    input = KEY.NONE;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 매개변수로 KEY, PLAYER, TILE을 받아 현재 상황을 실시간으로 업데이트로 바꿔준다.
        /// 플레이어 좌표를 Input에 따라 다르게 갱신(처리)해주고, TILE의 정보에 따라 다르게 갱신(처리)해준다.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="player"></param>
        /// <param name="map"></param>
        private static void Update(KEY input, Player player)
        {
            int posX = player.GetPos().x;
            int posY = player.GetPos().y;

            // 입력값에 따라 위,아래,왼쪽,오른쪽에 player 좌표를 바꿔준다.
            switch (input)
            {
                case KEY.UP:
                    prevMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), prevMap.oMap.GetLength(0), prevMap.oMap.GetLength(1));
                    PrevGoalCount = GoalCount;
                    player.SetPos(posX, --posY);
                    IsPrevReset = false;
                    break;
                case KEY.DOWN:
                    prevMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), prevMap.oMap.GetLength(0), prevMap.oMap.GetLength(1));
                    PrevGoalCount = GoalCount;
                    player.SetPos(posX, ++posY);
                    IsPrevReset = false;
                    break;
                case KEY.LEFT:
                    prevMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), prevMap.oMap.GetLength(0), prevMap.oMap.GetLength(1));
                    PrevGoalCount = GoalCount;
                    player.SetPos(--posX, posY);
                    IsPrevReset = false;
                    break;
                case KEY.RIGHT:
                    prevMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), prevMap.oMap.GetLength(0), prevMap.oMap.GetLength(1));
                    PrevGoalCount = GoalCount;
                    player.SetPos(++posX, posY);
                    IsPrevReset = false;
                    break;
                case KEY.UNDO:
                    flag = true;
                    Player.GetInstance().SetPos(prevPos);
                    return;
                case KEY.F1:
                    return;
                default:
                    break;
            }

            // 이동한 좌표를 가지고 체크를 해주는 함수를 호출한다.
            // 이동한 좌표가 갈수 있는 곳인지, 박스가 있는지에 따라 다르게 처리를 해준다.
            CheckTileMap(player, Scene.GetInstance().GetCurrMap());

        }

        // Render 함수는 게임 화면을 그리는 역할을 수행한다.
        // 매개변수로 Player 객체를 받아와서 플레이어의 위치에 플레이어를 그려준다.
        // 이 때 전체 화면을 지우고 다시 그리는 작업을 수행한다.
        // 또한 현재 맵의 크기에 맞게 콘솔 창의 크기를 조정한다.
        private static bool Render(Player player)
        {
            // 그리기 수행시 전체를 지우고 다시 그리는 작업을 수행한다.
            Console.Clear();
            Console.SetWindowSize(50, Scene.GetInstance().GetCurrMap().oMap.GetLength(0) + 9);

            // Player가 없거나 Map이 없을 경우 false를 반환하여 게임이 비정상적으로 종료된다.
            if (player == null)
                return false;

            // flag 변수가 false인 경우는 되돌리기 버튼이 눌리지 않은 상황이므로
            // 현재 맵을 그대로 출력한다.
            // flag 변수가 true인 경우는 되돌리기 버튼이 눌린 상황이므로
            // 이전 상황으로 되돌린 후 출력한다.
            if (!flag)
            {
                Scene.GetInstance().SceneShow();
            }
            else
            {
                // 되돌리기 버튼이 눌린 경우 flag 변수를 false로 초기화한다.
                flag = false;

                // 이전 상황에서의 목표 개수를 현재 상황에서도 그대로 유지시킨다.
                GoalCount = PrevGoalCount;
                if (!IsPrevReset)
                {
                    // 이전 상황으로 되돌린다.
                    Scene.GetInstance().SceneShow(prevMap);
                    // 이전 상황의 맵을 현재 맵으로 깊은 복사한다.
                    Scene.GetInstance().GetCurrMap().CopyValueObject(prevMap, prevMap.oMap.GetLength(0), prevMap.oMap.GetLength(1));
                    // 플레이어의 시작 위치를 설정한다.
                    Scene.GetInstance().GetCurrMap().SetPlayerStartPoint();
                }
                else
                {
                    // 이전 상황에서 리셋 버튼이 눌린 경우
                    // 이전 맵과 현재 맵을 초기화한다. 원래 맵으로 초기화하고 값을 복사한다. 기타 변수를 초기화한다.
                    Reset();

                    // 맵을 복사하고 재설정한다.
                    originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
                    PurposeCount = Scene.GetInstance().GetPurposeCount();
                    prevMap.ResetMap(Scene.GetInstance().GetCurrMap());
                    prevMap.CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
                    Scene.GetInstance().GetCurrMap().SetPlayerStartPoint();
                    Scene.GetInstance().SceneShow(Scene.GetInstance().GetCurrMap());
                }
                IsPrevReset = false;
            }

            // 플레이어의 위치에 플레이어를 그려준다.
            Console.SetCursorPosition(player.GetPos().x * 2, player.GetPos().y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("★");

            DefaultPrint();

            Debug.WriteLine("{0}", MoveCount);
            return true;
        }

        /// <summary>
        /// 게임 종료 조건이 달성되면 End에 True가 들어와 LOOP를 빠져나가게 된다.
        /// </summary>
        /// <returns>TRUE/FALSE</returns>
        private static bool End()
        {
            if (GameClear())
                return true;

            return false;
        }

        /// <summary>
        /// LOOP를 빠져 나왔을때 후처리 (맵을 전부 날려주고 빈 맵으로 설정, 전역변수를 초기값으로 설정)
        /// </summary>
        public void Release()
        {
            Player.ReleaseInstance();
            Scene.GetInstance().GetCurrMap().ReleaseInstance();
            Scene.GetInstance().SceneRelease();
            originMap.ReleaseInstance();
            prevMap.ReleaseInstance();
            input = KEY.NONE;
            GoalCount = 0; PurposeCount = 0; MoveCount = 0; Level = 0;
            Console.Clear();
        }

        /// <summary>
        /// 다음 맵으로 이동하는 함수이다. 내부 동작은 Reset()과 유사하다.
        /// </summary>
        private static void NextLevelInit()
        {
            Scene.GetInstance().ChangNextScene();
            PurposeCount = Scene.GetInstance().GetPurposeCount();
            originMap.ResetMap(Scene.GetInstance().GetCurrMap());
            originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
            prevMap.ResetMap(Scene.GetInstance().GetCurrMap());
            //prevMap.CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
            Scene.GetInstance().GetCurrMap().SetPlayerStartPoint();
            Render(Player.GetInstance());
            IsPrevReset = true;
        }

        /// <summary>
        /// 이전 맵으로 이동하는 함수이다. 내부 동작은 Reset()과 유사하다.
        /// </summary>
        private static void PrevLevelInit()
        {
            Scene.GetInstance().ChangPrevScene();
            PurposeCount = Scene.GetInstance().GetPurposeCount();
            originMap.ResetMap(Scene.GetInstance().GetCurrMap());
            originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
            prevMap.ResetMap(Scene.GetInstance().GetCurrMap());
            //prevMap.CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));
            Scene.GetInstance().GetCurrMap().SetPlayerStartPoint();
            Render(Player.GetInstance());
            IsPrevReset = true;
        }

        /// <summary>
        /// 위에서 부터 설명하면, 현재 맵과 바로 전상황을 저장한 맵을 다 날려주고
        /// 현재맵을 기존 맵의 빈 맵으로 초기화 시키고(초기 내용은 빈타일과 0,0 좌표의 Object를 가지는 맵) 
        /// 현재맵에 대해 다시 기존 맵(originMap)의 내용을 불러와 깊은 복사를 시켜 현재맵에 저장시킨다.
        /// 이외 나머지 값들도 초기화 시켜준다.
        /// 여기서 기존 맵이란 플레이어가 박스를 움직이거나 플레이어를 움직이거나 골을 넣거나 했던 것들을 하기 전 초기상태의 맵이다.
        /// </summary>
        private static void Reset()
        {
            Scene.GetInstance().GetCurrMap().ReleaseInstance();
            prevMap.ReleaseInstance();

            Scene.GetInstance().GetCurrMap().ResetMap(originMap);
            Scene.GetInstance().GetCurrMap().CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));

            input = KEY.NONE;
            GoalCount = 0; PurposeCount = 0; MoveCount = 0; IsPrevReset = true;
            Console.Clear();
        }

        /// <summary>
        /// 위에서 부터 설명하면, 현재 맵과 바로 전상황을 저장한 맵을 다 날려주고
        /// 현재맵을 기존 맵의 빈 맵으로 초기화 시키고(초기 내용은 빈타일과 0,0 좌표의 Object를 가지는 맵) 
        /// 현재맵에 대해 다시 기존 맵(originMap)의 내용을 불러와 깊은 복사를 시켜 현재맵에 저장시킨다.
        /// 그리고 현재 장면(맵)을 매개변수로 들어온 Level로 설정해주고
        /// 기존 맵(originMap)과 전상황을 저장한 맵(prevMap)을 초기의 빈 맵으로 초기화 시키고
        /// 기존 맵(originMap)의 내용은 지금 현재 맵으로 설정해준다.
        /// 마지막으로 바뀐 맵에서의 플레이어 위치를 찾아 좌표를 설정하고, 목표 골 개수도 찾아서 설정해준다.
        /// 이외 나머지 값들도 초기화 시켜준다.
        /// 여기서 기존 맵이란 플레이어가 박스를 움직이거나 플레이어를 움직이거나 골을 넣거나 했던 것들을 하기 전 초기상태의 맵이다.
        /// </summary>
        /// <param name="Lev"></param>
        private static void MoveLevel(int Lev)
        {
            Scene.GetInstance().GetCurrMap().ReleaseInstance(); // 현재맵을 날려준다.
            prevMap.ReleaseInstance();

            Scene.GetInstance().GetCurrMap().ResetMap(originMap); // 현재맵을 빈 맵으로 초기화 시킨다.
            Scene.GetInstance().GetCurrMap().CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1)); // 바뀐맵을 다시 originMap에 저장

            Scene.GetInstance().SetScene(Lev); // 현재 장면을 Lev의 장면으로 설정
            originMap.ResetMap(Scene.GetInstance().GetCurrMap());
            prevMap.ResetMap(Scene.GetInstance().GetCurrMap());
            originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1)); // 기존맵을 현재 맵으로 저장

            PurposeCount = Scene.GetInstance().GetPurposeCount();
            Scene.GetInstance().GetCurrMap().SetPlayerStartPoint();
            input = KEY.NONE;
            GoalCount = 0; MoveCount = 0; Level = Lev; IsPrevReset = true;
            Console.Clear();
        }

        /// <summary>
        /// 플레이어가 움직였을때 조건에 따라 체크하고 동작시키는 함수이다.
        /// 매개변수로 플레이어가 이전에 있던 위치를 prevPos에 가져온다.
        /// </summary>
        /// <param name="prevPos"></param>
        private static void CheckTileMap(Player player, Map map)
        {
            // 플레이어를 움직였을때 그 좌표가 벽이면 원래 플레이어 좌표로 돌아간다.
            if (map.GetObject(player.GetPos().y, player.GetPos().x).GetTile() == TILE.WALL)
            {
                player.SetPos(prevPos);
                MoveCount--;
            }
            // 플레이어를 움직였을때 그 좌표가 박스이면 조건에 따라 처리해준다.
            else if (map.GetObject(player.GetPos().y, player.GetPos().x).GetTile() == TILE.BOX)
            {
                Pos BoxPos = player.GetPos(); // 현재 박스가 있는 좌표를 BoxPos에 저장해둔다.
                // 입력에 따라 BOX에 대한 다른 조건을 처리한다. 우선 좌표 먼저 움직여 준다.
                switch (input)
                {
                    case KEY.UP:
                        BoxPos.y--;
                        break;
                    case KEY.DOWN:
                        BoxPos.y++;
                        break;
                    case KEY.LEFT:
                        BoxPos.x--;
                        break;
                    case KEY.RIGHT:
                        BoxPos.x++;
                        break;
                }

                // 위에 조건을 처리한 후에
                // BOX가 움직인 좌표에 아무것도 없을때 박스를 그 좌표로 움직여주고,
                // 원래 박스가 있던 좌표는 빈공간으로 만들어 주고 플레이어를 움직여준다.
                if (map.GetObject(BoxPos.y, BoxPos.x).GetTile() == TILE.NONE)
                {
                    map.GetObject(BoxPos.y, BoxPos.x).SetTile(TILE.BOX);
                    map.GetObject(player.GetPos().y, player.GetPos().x).SetTile(TILE.NONE);
                }
                // BOX가 움직인 좌표가 골 지점일때 박스를 그 좌표로 움직여주고, 그 좌표는 BOXGOAL이 된다.
                // 원래 박스가 있던 좌표는 빈공간으로 만들어 주고 플레이어를 움직여준다.
                // 골이 되었으므로 GoalCount를 하나 증가 시켜준다.
                else if (map.GetObject(BoxPos.y, BoxPos.x).GetTile() == TILE.GOAL)
                {
                    GoalCount++;
                    map.GetObject(BoxPos.y, BoxPos.x).SetTile(TILE.BOXGOAL);
                    map.GetObject(player.GetPos().y, player.GetPos().x).SetTile(TILE.NONE);
                }
                // BOX가 움직인 좌표가 벽이거나, 박스일때 아무동작도 안하게 한다.
                // 또한 플레이어는 원래 플레이어 좌표로 돌아간다.
                else
                {
                    player.SetPos(prevPos);
                    MoveCount--;
                }
            }
            // 플레이어를 움직였을때 그 좌표가 박스골이면 조건에 따라 처리해준다.
            else if (map.GetObject(player.GetPos().y, player.GetPos().x).GetTile() == TILE.BOXGOAL)
            {
                Pos GoalPos = player.GetPos();// 현재 골이 되어 있는 좌표를 GoalBox 저장해둔다.
                // 입력에 따라 BOX에 다른 조건을 처리한다. 우선 좌표 먼저 움직여 준다.
                switch (input)
                {
                    case KEY.UP:
                        GoalPos.y--;
                        break;
                    case KEY.DOWN:
                        GoalPos.y++;
                        break;
                    case KEY.LEFT:
                        GoalPos.x--;
                        break;
                    case KEY.RIGHT:
                        GoalPos.x++;
                        break;
                }

                // 위에 조건을 처리한 후에
                // GOALBOX가 움직인 좌표에 아무것도 없을때 박스를 그 좌표로 움직여주고,
                // 원래 박스가 있던 좌표는 골로 만들어 주고 플레이어를 움직여준다.
                // 골이 되었던것이 빠져나왔으므로 GoalCount를 하나 감소 시켜준다.
                if (map.GetObject(GoalPos.y, GoalPos.x).GetTile() == TILE.NONE)
                {
                    map.GetObject(GoalPos.y, GoalPos.x).SetTile(TILE.BOX);
                    map.GetObject(player.GetPos().y, player.GetPos().x).SetTile(TILE.GOAL);
                    GoalCount--;
                }
                // GOALBOX가 움직인 좌표가 골 지점일때 박스를 그 좌표로 움직여주고, 그 좌표는 BOXGOAL이 된다.
                // 원래 박스가 있던 좌표는 골으로 만들어 주고 플레이어를 움직여준다.
                else if (map.GetObject(GoalPos.y, GoalPos.x).GetTile() == TILE.GOAL)
                {
                    map.GetObject(GoalPos.y, GoalPos.x).SetTile(TILE.BOXGOAL);
                    map.GetObject(player.GetPos().y, player.GetPos().x).SetTile(TILE.GOAL);
                }
                // GOALBOX가 움직인 좌표가 벽이거나, 박스, 골박스일때 아무동작도 안하게 한다.
                // 또한 플레이어는 원래 플레이어 좌표로 돌아간다.
                else
                {
                    player.SetPos(prevPos);
                    MoveCount--;
                }
            }
        }

        /// <summary>
        /// 게임 클리어 조건(레벨/최종 레벨)을 검사하고 이 클래스의 End()에서 호출한다.
        /// </summary>
        /// <returns></returns>
        private static bool GameClear()
        {
            if (GoalCount == PurposeCount)
            {
                Level++;
                Random random = new Random();
                if (Level == Scene.GetInstance().GetListMap().Count)
                {
                    for(int i = 0; i < 45; i++)
                    {
                        Console.Clear();
                        Console.ForegroundColor = colors[random.Next(colors.Length)];
                        Console.WriteLine("★ 축하합니다! 모든 게임을 클리어 하였습니다!! ★");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("[ {0} ] 레벨의 플레이어의 총 움직임 수 : [ {1} ]\n잠시후 초기화면으로 이동합니다...", Level, MoveCount);
                        Thread.Sleep(100);
                    }
                    Level = 0;
                    return true;
                }

                for (int i = 0; i < 15; i++)
                {
                    Console.Clear();
                    Console.ForegroundColor = colors[random.Next(colors.Length)];
                    Console.WriteLine("★ 축하합니다! ★");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("[ {0} ] 레벨을 클리어 하였습니다.\n플레이어의 총 움직임 수 : [ {1} ]\n잠시후 다음 레벨을 진행합니다...", Level, MoveCount);
                    Thread.Sleep(200);
                }

                Reset(); // 현재맵을 기존맵으로 돌려주고
                NextLevelInit(); // 다음맵으로 넘어간다
            }
            return false;
        }

        /// <summary>
        /// GameMode에서 기본으로 콘솔에 표시되는 문구들
        /// </summary>
        private static void DefaultPrint()
        {
            Console.SetCursorPosition(0, Scene.GetInstance().GetCurrMap().oMap.GetLength(0) + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("★ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Player ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("■ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Wall ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("▩ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Box ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("□ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Goal ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("▣ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Goal_In ");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("R : Restart, ");
            Console.Write("T : Undo, ");
            Console.WriteLine("ESC : Main Menu");
            Console.WriteLine();
            Console.WriteLine("GoalCount : {0}", GoalCount);
            Console.WriteLine("Level : {0}", Level + 1);
        }
    }
}
