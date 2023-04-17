namespace Project_D
{
    // Object 인스턴스의 2차원 배열을 조작하는 다양한 메서드가 있는 Map 클래스이다. 
    class Map
    {
        public Object[,] oMap;  // 지도를 나타내는 Object 객체의 2차원 배열입니다.

        /// <summary>
        /// 이 함수는 입력으로 받은 'CurrMap' 맵을 초기화하는 함수입니다.
        /// 맵은 'Object' 클래스의 2차원 배열인 'oMap'으로 구성되어 있습니다.함수는 
        /// 그 다음, 이중 for문을 통해 'oMap' 배열의 각 원소를 순회하면서 'Object' 클래스의 객체를 생성하여 해당 위치에 할당합니다.
        /// 따라서 이 함수를 호출하면 'CurrMap'과 동일한 크기의 빈 맵이 생성되며, 각 위치에는 초기값으로 Pos(0,0)과 TILE.DEFAULT 값을 가진 'Object' 객체가 할당됩니다.
        /// <param name="CurrMap"></param>
        public void ResetMap(Map CurrMap)
        {
            oMap = new Object[CurrMap.oMap.GetLength(0), CurrMap.oMap.GetLength(1)];
            for (int y = 0; y < CurrMap.oMap.GetLength(0); y++)
            {
                for (int x = 0; x < CurrMap.oMap.GetLength(1); x++)
                {
                    oMap[y, x] = new Object(new Pos(0, 0), TILE.DEFAULT);
                }
            }
        }

        /*  이 함수는 2차원 char 배열 'map'을 입력으로 받아서, 이를 기반으로 'oMap' 배열을 초기화하는 함수입니다.
            먼저 'oMap' 배열을 입력으로 받은 'map'의 크기와 동일한 크기로 새로 생성합니다. 
            그리고 foreach문을 사용하여 'map' 배열에 빈공간이나, 정해놓은 문자 이외 다른 문자가 들어갔을 경우 false를 리턴하도록 처리합니다.
            그 다음, 이중 for문을 통해 'oMap' 배열의 각 원소를 순회하면서 'Object' 클래스의 객체를 생성하여 해당 위치에 할당합니다. 여기서는 Pos(0,0)과 TILE.DEFAULT로 초기화됩니다.
            그리고 다시 이중 for문을 사용하여 'map' 배열을 순회하면서, 각 위치에 해당하는 'oMap' 배열의 
            'Object' 객체에 대해, 입력으로 받은 'map' 배열의 문자에 따라 'Tile' 속성을 설정합니다. 
            이 때, '▩', '□', '▣', '■', '★', '　' 문자는 각각 BOX, GOAL, BOXGOAL, WALL, PLAYER, NONE으로 매핑됩니다.
            따라서 이 함수를 호출하면, 입력으로 받은 'map' 배열에 대응하는 'oMap' 배열이 생성되며, 
            'map' 배열의 문자에 따라 'Tile' 속성이 설정된 'Object' 객체가 할당됩니다. 
            만약 'map' 배열에 빈공간이나, 정해놓은 문자 이외 다른 문자가 들어갔을 경우에는 false를 반환합니다.
         */
        public bool SetMap(char[,] map)
        {
            oMap = new Object[map.GetLength(0), map.GetLength(1)];
            // 맵에 빈공간이나, 정해놓은 문자 이외 다른 문자가 들어갔을 경우 false를 리턴
            foreach (char c in map)
                if (string.IsNullOrEmpty(c.ToString()) && c != '■' && c != '　' && c != '▩' && c != '□' && c != '▣')
                    return false;

            for (int y = 0; y < oMap.GetLength(0); y++)
            {
                for (int x = 0; x < oMap.GetLength(1); x++)
                {
                    oMap[y, x] = new Object(new Pos(0, 0), TILE.DEFAULT);
                }
            }

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    oMap[y, x].SetPos(y, x);
                    switch (map[y, x])
                    {
                        case '　':
                            oMap[y, x].SetTile(TILE.NONE);
                            break;
                        case '■':
                            oMap[y, x].SetTile(TILE.WALL);
                            break;
                        case '▩':
                            oMap[y, x].SetTile(TILE.BOX);
                            break;
                        case '□':
                            oMap[y, x].SetTile(TILE.GOAL);
                            break;
                        case '▣':
                            oMap[y, x].SetTile(TILE.BOXGOAL);
                            break;
                        case '★':
                            oMap[y, x].SetTile(TILE.PLAYER);
                            break;
                        default:
                            break;
                    }
                }
            }

            //Array.Copy(backBuffer, frontBuffer, MAX * MAX);

            return true;
        }

        /*  이 메서드는 지정된 Map 객체 내에서 객체의 위치와 타일을 설정합니다.
            @param map 객체의 위치와 타일이 설정될 Map 객체입니다.
            @param obj Map 객체 내에서 위치와 타일이 설정될 Object입니다.
            이 메서드는 주어진 Object의 위치를 검색하여 해당 위치를 Map 객체의 oMap 배열에서 설정합니다.
            그리고나서 같은 위치의 oMap 배열에 대한 타일을 주어진 Object의 타일로 설정합니다.
        */
        public void SetObject(Map map, Object obj)
        {
            map.oMap[obj.GetPos().y, obj.GetPos().x].SetPos(obj.GetPos());
            map.oMap[obj.GetPos().y, obj.GetPos().x].SetTile(obj.GetTile());
        }

        /*  이 메서드는 현재 Map 객체에서 플레이어의 시작 위치를 설정합니다.
            @return 플레이어 시작 위치가 성공적으로 설정되었는지 여부입니다.
            이 메서드는 반복문을 사용하여 Map 객체의 모든 위치에서 플레이어 타일을 찾습니다.
            플레이어 타일을 찾으면 해당 위치를 Player 객체의 시작 위치로 설정하고,
            Map 객체에서 해당 위치의 타일을 NONE으로 변경한 후 true를 반환합니다.
            만약 플레이어 타일을 찾지 못하면 false를 반환합니다.
        */
        public bool SetPlayerStartPoint()
        {
            for (int y = 0; y < oMap.GetLength(0); y++)
            {
                for (int x = 0; x < oMap.GetLength(1); x++)
                {
                    if (oMap[y, x].GetTile() == TILE.PLAYER)
                    {
                        Player.GetInstance().SetPos(x, y);
                        oMap[y, x].SetTile(TILE.NONE);
                        return true;
                    }
                }
            }
            return false;
        }

        /*  이 메서드는 지정된 Map 객체에서 모든 객체의 위치와 타일 정보를 복사합니다. (깊은 복사)
            @param map 정보를 복사할 Map 객체입니다.
            @param Width 복사할 맵의 너비입니다.
            @param Height 복사할 맵의 높이입니다.
            이 메서드는 반복문을 사용하여 모든 Map 객체의 위치와 타일 정보를 검색하고,
            이 정보를 현재 객체의 oMap 배열에 복사합니다.
        */
        public void CopyValueObject(Map map, int Width, int Height)
        {
            for(int y = 0; y < Width; y++)
            {
                for (int x = 0; x < Height; x++)
                {
                    oMap[y, x].SetPos(map.oMap[y, x].GetPos());
                    oMap[y, x].SetTile(map.oMap[y, x].GetTile());
                }
            }
        }

        /*  이 메서드는 현재 Map 객체에서 목적지 타일의 수를 반환합니다.
            @return Map 객체에서 목적지 타일의 수입니다.
            이 메서드는 반복문을 사용하여 Map 객체의 모든 위치에서 목적지 타일의 수를 계산합니다.
            계산된 결과는 PurposeCount 변수에 저장되며, 마지막에 반환됩니다.
        */
        public int GetGoalCount()
        {
            int PurposeCount = 0;
            for (int y = 0; y < oMap.GetLength(0); y++)
            {
                for (int x = 0; x < oMap.GetLength(1); x++)
                {
                    if (oMap[y, x].GetTile() == TILE.GOAL)
                        PurposeCount++;
                }
            }

            return PurposeCount;
        }

        /*  이 메서드는 현재 Map 객체가 EditorMode에서 저장될 수 있는지 확인합니다.
            @return Map 객체가 유효하면 true, 그렇지 않으면 false를 반환합니다.
            이 메서드는 반복문을 사용하여 Map 객체의 모든 위치에서 플레이어, 상자, 목적지 타일의 개수를 카운트합니다.
            각각의 개수를 변수에 저장한 후, 다음 조건을 만족하는지 확인합니다.
            플레이어가 1명이고, 상자와 목적지 타일의 개수가 같고, 이들의 개수가 0보다 큰 경우
            만약 이 조건을 만족하면 true를 반환하고, 그렇지 않으면 false를 반환합니다.
        */
        public bool CheckEditorMap()
        {
            int PlayerCnt = 0; int BoxCnt = 0; int GoalCnt = 0;
            for (int y = 0; y < oMap.GetLength(0); y++)
            {
                for (int x = 0; x < oMap.GetLength(1); x++)
                {
                    switch(oMap[y, x].GetTile())
                    {
                        case TILE.PLAYER:
                            PlayerCnt++;
                            break;
                        case TILE.BOX:
                            BoxCnt++;
                            break;
                        case TILE.GOAL:
                            GoalCnt++;
                            break;
                        default:
                            break;
                    }
                }
            }
            return (PlayerCnt == 1 && BoxCnt == GoalCnt && GoalCnt > 0 && BoxCnt > 0) ? true : false;
        }

        public Object GetObject(int y, int x)
        {
            return oMap[y, x];
        }

        public Object[,] GetMap()
        {
            return oMap;
        }

        /*  이 메서드는 Map 객체가 소멸될 때, oMap 배열의 모든 요소를 null로 초기화하여 메모리를 해제합니다.
            이 메서드는 반복문을 사용하여 oMap 배열의 모든 요소를 null로 설정합니다.
        */
        public void ReleaseInstance()
        {
            for (int y = 0; y < oMap.GetLength(0); y++)
            {
                for (int x = 0; x < oMap.GetLength(1); x++)
                {
                    oMap[y, x] = null;
                }
            }
        }
    }
}