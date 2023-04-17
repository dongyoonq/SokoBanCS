namespace Project_D
{
    /* 이 클래스는 Singleton 디자인 패턴을 활용한 Game 클래스입니다. 
     * 게임을 실행하기 위해 먼저 Game 클래스의 인스턴스를 가져와야 하며, 
     * 이는 GetInstance() 메소드를 통해 구현됩니다. GetInstance() 메소드는 인스턴스가 없으면 인스턴스를 생성하고, 
     * 그렇지 않으면 이미 생성된 인스턴스를 반환합니다.
     * 
     * Game 클래스에는 GameInit()과 EditorInit() 두 개의 메소드가 있습니다. 
     * GameInit()은 게임 실행을 위한 메소드로, GameMode를 초기화한 후 게임 루프를 돌며 게임을 실행합니다. 
     * EditorInit()은 맵 에디터 실행을 위한 메소드로, EditorMode를 초기화한 후 에디터 루프를 돌며 맵을 수정할 수 있습니다.
     * 
     * 두 메소드 모두 GameMode 또는 EditorMode의 Init() 메소드를 호출하여 초기화하고, 
     * GameMode 또는 EditorMode의 GameLoop() 메소드를 호출하여 게임 루프 또는 에디터 루프를 돌리며 게임 또는 에디터를 실행합니다. 
     * 게임 또는 에디터 루프가 종료되면 GameMode 또는 EditorMode의 Release() 메소드를 호출하여 메모리를 해제합니다.
     */
    class Game
    {
        private static Game instance;
        public static Game GetInstance()
        {
            if (instance == null)
                instance = new Game();
            return instance;
        }

        private Game()
        {

        }

        /// <summary>
        /// 게임 모드의 게임루프 진행
        /// </summary>
        /// <returns></returns>
        public bool GameInit()
        {
            if(GameMode.GetInstance().Init())
            {
                while (GameMode.GetInstance().GameLoop()) ;
            }
            else
            {
                return false;
            }
            GameMode.GetInstance().Release();
            return true;
        }

        /// <summary>
        /// 에디터 모드의 게임루프 진행
        /// </summary>
        /// <returns></returns>
        public bool EditorInit()
        {
            if (EditorMode.GetInstance().Init())
            {
                while (EditorMode.GetInstance().GameLoop()) ;
            }
            else
            {
                return false;
            }
            EditorMode.GetInstance().Release();
            return true;
        }
    }
}
