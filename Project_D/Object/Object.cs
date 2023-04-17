using System;

namespace Project_D
{
    /// <summary>
    /// 이 클래스는 게임 맵 상의 오브젝트를 나타내기 위한 Object 클래스입니다.
    /// 각 Object는 위치(Pos)와 타일(Tile) 정보를 가지며, 위치와 타일 정보를 설정하고 가져오는 메소드를 제공합니다.
    /// 타일 정보에 따라 콘솔에 출력될 문자를 GetTileToChar() 메소드에서 설정하며, 각 타일 정보에 맞는 색상도 설정합니다.
    /// 즉, 이 클래스는 게임에서 오브젝트의 위치와 타일 정보를 관리하고, 해당 오브젝트가 콘솔에 어떻게 출력될지 결정하는 역할을 합니다.
    /// </summary>
    class Object
    {
        private Pos pos;
        private TILE tile;

        public Object(Pos pos, TILE tile)
        {
            this.pos = pos;
            this.tile = tile;
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

        public void SetTile(TILE _tile)
        {
            tile = _tile;
        }

        /// <summary>
        /// 이 함수는 Object의 타일 종류(TILE)를 받아와 해당하는 콘솔 문자(char)를 반환하는 역할을 합니다. 
        /// switch문을 통해 각각의 TILE에 맞게 Console.ForegroundColor을 설정하고, 
        /// 그에 맞는 콘솔 문자를 변수 cTile에 할당한 후 반환합니다.
        /// </summary>
        /// <returns></returns>
        public char GetTileToChar()
        {
            char cTile = ' ';
            switch (tile)
            {
                case TILE.NONE:
                    cTile = '　';
                    break;
                case TILE.WALL:
                    Console.ForegroundColor = ConsoleColor.White;
                    cTile = '■';
                    break;
                case TILE.BOX:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    cTile = '▩';
                    break;
                case TILE.GOAL:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    cTile = '□';
                    break;
                case TILE.BOXGOAL:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    cTile = '▣';
                    break;
                case TILE.PLAYER:
                    Console.ForegroundColor = ConsoleColor.Red;
                    cTile = '★';
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Black;
                    cTile = '■';
                    break;
            }

            return cTile;
        }

        public Pos GetPos()
        {
            return pos;
        }

        public TILE GetTile()
        {
            return tile;
        }
    }
}
