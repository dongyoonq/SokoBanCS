namespace Project_D
{

    /// <summary>
    /// Player 객체를 생성할 수 있게 만든다.
    /// 하지만 싱글플레이 게임이므로 Player을 유일한 객체가 되어야하므로 싱글턴 패턴으로 static으로 선언했다.
    /// 다른 플레이어를 생성시킬수 없다.
    /// Player는 현재위치의 좌표를 가지고 있는 Pos를 멤버로 가지게 되고
    /// 플레이어 좌표를 설정하는 SetPos와 가져올수있는 GetPos함수를 만들었다.
    /// </summary>
    class Player
    {
        private Pos pos;

        private static Player instance;
        public static Player GetInstance()
        {
            if (instance == null)
            {
                instance = new Player();
            }

            return instance;
        }

        public static void ReleaseInstance()
        {
            instance = null;
        }

        private Player()
        {

        }

        public void SetPos(Pos _pos)
        {
            pos = _pos;
        }

        public void SetPos(int x, int y)
        {
            pos.x = x;
            pos.y = y;
        }

        public Pos GetPos()
        {
            return pos;
        }
    }
}
