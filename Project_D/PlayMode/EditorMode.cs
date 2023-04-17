using System;

namespace Project_D
{
    /*
        * 이 코드는 에디터 모드에서의 게임 루프를 구현한 클래스인 EditorMode입니다. 
        * CheckCreateMap, SuccesCreateMap, originMap, obj, input 등 여러 변수와 싱글톤 패턴을 이용해 생성된 객체 instance가 있습니다.

        GameLoop 메소드는 게임 루프를 구현합니다.

        게임 초기화 과정은 생략되어 있습니다.
        1. Input 메소드를 호출해 사용자의 입력값을 처리합니다.
        2. Update 메소드를 호출해 사용자 입력에 따라 객체들의 상태를 업데이트합니다.
        3. Render 메소드를 호출해 객체들의 현재 상태를 화면에 출력합니다.
        4. End 메소드를 호출해 게임 종료 조건을 확인합니다. 종료 조건이 만족되면 false를 반환하고 게임 루프를 종료합니다. 
        그렇지 않으면 true를 반환하고 게임 루프를 계속 진행합니다.
    */
    class EditorMode
    {
        static bool CheckCreateMap;             // 맵 생성이 가능한지의 여부를 확인하는 변수
        static bool SuccesCreateMap;            // 맵 생성이 성공했는지 확인하는 변수

        static Map originMap = new Map();       // 에디터 모드에서 사용되는 맵 객체
        static Object obj;                      // 에디터 모드에서 사용되는 Object 객체

        static KEY input;                       // 사용자의 입력값을 가지는 변수 KEY 선언

        private static EditorMode instance;     // 싱글톤 객체를 생성하기 위한 인스턴스 변수 선언
        public static EditorMode GetInstance()  // 싱글톤 패턴을 이용해 EditorMode 객체 생성
        {
            if (instance == null)
                instance = new EditorMode();
            return instance;
        }

        private EditorMode()
        {

        }

        public bool GameLoop()                  // 에디터 모드에서의 게임 루프
        {
            // 2. 게임 입력 (Input)
            if (!Input())
                return false;

            // 3. 게임 갱신 (Update)
            Update(input, Player.GetInstance());

            // 4. 게임 출력 (Render)
            // 객체들의 현재 상태를 화면에 출력
            Render(Player.GetInstance(), obj);

            // 5. 게임이 종료되는지 확인
            if (End())
            {
                // 게임 루프 종료
                return false;
            }

            // 게임 루프 계속 진행
            return true;
        }

        // 이 메소드는 맵 에디터의 초기화를 담당하는 메서드이다.
        public bool Init()
        {
            // 콘솔 초기화
            Console.Clear();
            Console.Title = "Project-D EditorMode";
            Console.CursorVisible = false;
            Console.SetWindowSize(62, 27);

            // 장면 초기화
            // Scene 싱글톤 인스턴스의 EditorInit() 메소드 실행 결과가 false이면
            if (!Scene.GetInstance().EditorInit())
                return false;   // 초기화 실패

            // Scene 싱글톤 인스턴스의 SetScene() 메소드를 이용해 장면 설정
            Scene.GetInstance().SetScene(0);

            // originMap 객체를 초기화하고, 현재 맵 정보를 복사합니다.
            originMap.ResetMap(Scene.GetInstance().GetCurrMap()); 
            originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));

            // 플레이어 초기화
            // Player 싱글톤 인스턴스를 초기화합니다. Player 싱글톤 인스턴스가 null이면 초기화에 실패하며, 
            // SetPos() 메소드를 이용해 플레이어의 위치를 초기화합니다.
            if (Player.GetInstance() == null)
                return false;

            Player.GetInstance().SetPos(1, 1);

            // 그리기 초기화
            // Player와 obj를 전달하며, Render() 메소드를 호출하여 그리기를 초기화합니다. 초기화에 실패하면 false를 반환합니다.
            if (!Render(Player.GetInstance(), obj))
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

            // 사용자의 키입력에 따라 플레이어의 이동방향, 타일 생성 등의 명령을 처리
            switch (info.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    input = KEY.UP;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    input = KEY.DOWN;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    input = KEY.LEFT;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    input = KEY.RIGHT;
                    break;
                case ConsoleKey.D1: // 1~5번 버튼으로 블록을 설정할 수 있다.
                    CreateObject(TILE.WALL, Player.GetInstance());
                    input = KEY.NONE;
                    break;
                case ConsoleKey.D2:
                    CreateObject(TILE.BOX, Player.GetInstance());
                    input = KEY.NONE;
                    break;
                case ConsoleKey.D3:
                    CreateObject(TILE.GOAL, Player.GetInstance());
                    input = KEY.NONE;
                    break;
                case ConsoleKey.D4:
                    CreateObject(TILE.PLAYER, Player.GetInstance());
                    input = KEY.NONE;
                    break;
                case ConsoleKey.D5:
                    CreateObject(TILE.NONE, Player.GetInstance());
                    input = KEY.NONE;
                    break;
                case ConsoleKey.P: // 현재 맵저장 버튼
                    input = KEY.NONE;
                    if (!Scene.GetInstance().GetCurrMap().CheckEditorMap()) // 현재 맵을 저장할 수 있는지 체크하는 함수 호출
                    {
                        CheckCreateMap = true;
                        break;
                    }
                    // 현재 맵이 정상적으로 저장된 경우
                    Scene.GetInstance().CharToString(Scene.GetInstance().GetCurrMap());
                    SuccesCreateMap = true;
                    break;
                case ConsoleKey.R: // 현재 맵리셋 버튼
                    // 맵을 초기화
                    Scene.GetInstance().GetCurrMap().ReleaseInstance();

                    Scene.GetInstance().GetCurrMap().ResetMap(originMap);
                    Scene.GetInstance().GetCurrMap().CopyValueObject(originMap, originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));

                    input = KEY.NONE;
                    Console.Clear();
                    originMap.CopyValueObject(Scene.GetInstance().GetCurrMap(), originMap.oMap.GetLength(0), originMap.oMap.GetLength(1));

                    // 플레이어 리셋 위치 설정
                    Player.GetInstance().SetPos(1, 1);
                    break;
                case ConsoleKey.Escape: // 에디터 모드 종료
                    return false;
                default:
                    input = KEY.NONE;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 이 코드는 게임 업데이트 부분으로, 사용자의 입력에 따라 게임 오브젝트의 위치를 갱신합니다.
        /// 입력된 키와 플레이어 객체를 인자로 받는 함수입니다.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="player"></param>
        private static void Update(KEY input, Player player)
        {
            Pos prevPos = Player.GetInstance().GetPos();
            int posX = player.GetPos().x;
            int posY = player.GetPos().y;

            switch (input)
            {
                case KEY.UP:
                    player.SetPos(posX, --posY);
                    break;
                case KEY.DOWN:
                    player.SetPos(posX, ++posY);
                    break;
                case KEY.LEFT:
                    player.SetPos(--posX, posY);
                    break;
                case KEY.RIGHT:
                    player.SetPos(++posX, posY);
                    break;
                default:
                    break;
            }

            //  갱신된 위치와 이전 위치를 비교하여, 플레이어가 맵 밖으로 벗어나지 않았는지 확인합니다.
            CheckOutOfBound(player, Scene.GetInstance().GetCurrMap(), prevPos);
        }

        /// <summary>
        /// 이 메소드는 맵에디터에서 그리기 작업을 수행합니다.
        /// 먼저, 콘솔 창을 초기화하고, 플레이어와 맵을 출력합니다.그 후, 플레이어의 현재 위치에 빨간색 별표(★)를 출력합니다.
        /// 마지막으로, 만약 맵 저장에 실패했을 경우 적절한 에러 메시지를 출력하고, 맵 저장에 성공했을 경우 성공 메시지를 출력합니다.
        /// 그리고 EditorDefaultPrint() 메소드를 호출하여 기본적인 맵 에디터 메뉴를 출력합니다.
        /// 마지막으로, true를 반환하여 작업이 성공적으로 완료되었음을 알립니다.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool Render(Player player, Object obj)
        {
            Console.Clear();

            if (player == null)
                return false;

            Scene.GetInstance().SceneShow();

            Console.SetCursorPosition(player.GetPos().x * 2, player.GetPos().y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("★");

            if(CheckCreateMap)
            {
                Console.SetCursorPosition(0, 24);
                Console.WriteLine("* 플레이어(★)가 없거나 여러개 설정 했습니다. 또한");
                Console.WriteLine("* 골과 박스는 최소 하나이상, 골과 박스 개수는 같아야합니다.");
                CheckCreateMap = false;
            }
            
            if(SuccesCreateMap)
            {
                Console.SetCursorPosition(0, 24);
                Console.WriteLine("* 맵 저장에 성공하셨습니다 !!");
                SuccesCreateMap = false; 
            }

            EditorDefaultPrint();

            return true;
        }

        // 맵 에디터 모드의 경우 사용자가 ESC 버튼을 누르기 전까지 GameLoop를 무한히 돈다.
        // ESC 버튼 누를시 false가 반환하여 게임 루프를 탈출한다.
        private static bool End()
        {
            return false;
        }

        /// <summary>
        /// 이 메서드는 맵 에디터가 종료될 때 호출되어 필요한 자원들을 해제하는 역할을 합니다.
        /// 맵 에디터에서 사용된 자원들을 해제하여, 이후 프로그램이 실행될 때 이전 상태와 충돌하지 않도록 하는 역할을 합니다.
        /// </summary>
        public void Release()
        {
            Player.ReleaseInstance();                               // Player 객체의 인스턴스를 해제합니다.
            Scene.GetInstance().GetCurrMap().ReleaseInstance();     // 현재 맵 객체의 인스턴스를 해제합니다.
            Scene.GetInstance().SceneRelease();                     // Scene 객체의 자원을 해제합니다.
            originMap.ReleaseInstance();                            // 원래 맵 객체의 인스턴스를 해제합니다.    
            obj = null;                                             // Object 객체를 null로 초기화합니다.
            input = KEY.NONE;                                       // 입력값을 초기화합니다.
            CheckCreateMap = false; SuccesCreateMap = false;        // 맵 생성과 관련된 변수들을 초기화합니다.
            Console.Clear();                                        // 콘솔 창을 초기화합니다.
        }

        // 이 함수는 맵 에디터에서 새로운 오브젝트를 생성하는 데 사용됩니다.
        // tile 매개변수에는 생성할 오브젝트의 타일 종류를 지정하고, player 매개변수에는 타일 생성할 위치를 지정하는 플레이어 객체를 전달합니다.
        // 새로운 Object 인스턴스를 생성하고, 해당 위치와 타일 종류를 설정한 후, 현재 Scene의 현재 Map에 해당 Object를 추가합니다.
        // 이후 obj 객체를 null로 초기화하여 메모리 누수를 방지합니다.
        private static void CreateObject(TILE tile, Player player)
        {
            obj = new Object(new Pos(0, 0), TILE.DEFAULT);
            obj.SetTile(tile);
            obj.SetPos(player.GetPos().x, player.GetPos().y);

            Scene.GetInstance().GetCurrMap().SetObject(Scene.GetInstance().GetCurrMap(), obj);

            obj = null;
        }

        /// <summary>
        /// 이 메소드는 플레이어의 위치가 맵 경계를 넘어가는지 확인하는 역할을 합니다. 
        /// 만약 플레이어가 경계를 넘어가면 이전 위치로 돌아가게 됩니다. 이를 통해 플레이어가 유효한 위치에서만 이동할 수 있도록 보장합니다. 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="map"></param>
        /// <param name="prevPos"></param>
        private static void CheckOutOfBound(Player player, Map map, Pos prevPos)
        {
            if (player.GetPos().y < 1 || player.GetPos().y >= 19 || player.GetPos().x >= 19 || player.GetPos().x < 1)
            {
                player.SetPos(prevPos);
            }
        }

        /// <summary>
        /// Editor 모드에서 기본으로 콘솔에 표시되는 문구들
        /// </summary>
        private static void EditorDefaultPrint()
        {
            Console.SetCursorPosition(0, 20);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("1. ■ ");
            Console.Write("Wall    ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("2. ▩ ");
            Console.Write("Box    ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("3. □ ");
            Console.WriteLine("Goal  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("4. ★ ");
            Console.Write("Player  ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("5. ");
            Console.Write("None/Erase");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("R : Restart   P : Save   ESC : Main Menu");
        }
    }
}
