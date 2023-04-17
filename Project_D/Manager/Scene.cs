using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Project_D
{
    /*  Scene 클래스는 Map 객체를 관리하며, 싱글톤 패턴으로 구현되어 있습니다.
        sceneList는 Scene 클래스의 인스턴스들이 공유하는 Map 객체 리스트를 나타내고, currMap은 현재 Scene에서 사용하는 Map 객체를 나타냅니다.
        GetInstance() 메서드는 Scene 클래스의 싱글톤 인스턴스를 가져오는 메서드이며, ReleaseInstance() 메서드는 싱글톤 인스턴스를 해제하는 메서드입니다.
        생성자는 private으로 선언되어 있으므로 외부에서 인스턴스를 생성할 수 없습니다. Scene 클래스의 인스턴스는 GetInstance() 메서드를 통해 얻을 수 있습니다.
     */
    class Scene
    {
        private static List<Map> sceneList;         // Scene 클래스의 인스턴스들이 공유하는 Map 객체 리스트
        private static Map currMap = new Map();     // 현재 Scene에서 사용하는 Map 객체
            
        private static Scene instance;              // Scene 클래스의 싱글톤 인스턴스
        public static Scene GetInstance()           // Scene 클래스의 싱글톤 인스턴스를 가져오는 메서드
        {
            if (instance == null)                   // 싱글톤 인스턴스가 아직 생성되지 않았을 경우
                instance = new Scene();             // 인스턴스를 생성한다.
            return instance;                        // 인스턴스를 반환한다.
        }

        public static void ReleaseInstance()        // 싱글톤 인스턴스를 해제하는 메서드
        {
            instance = null;                        // 인스턴스를 null로 설정하여 해제한다.
        }

        private Scene()
        {


        }

        // 해당 클래스(Scene)의 초기화를 위한 함수입니다.
        // sceneList를 초기화하고, 파일의 개수를 가져와 반복문을 통해 각 파일로부터 맵 정보를 로드합니다.
        // 맵 정보가 정상적으로 생성되지 않으면 false를 반환합니다.
        // 현재 맵(currMap)을 sceneList의 첫 번째 맵으로 설정하고 true를 반환합니다.
        public bool Init()
        {
            sceneList = new List<Map>();
            int fileCount = LoadFileCount();

            for(int i = 0; i < fileCount - 1; i++)
            {
                if (!CreateScene(StringToChar(LoadFileToStringMap(i + 1))))
                    return false;
            }

            currMap = sceneList[0];
            return true;
        }

        // EditorInit 함수는 에디터 모드에서 사용될 때 호출되는 함수입니다.
        // 새로운 List<Map>을 생성하고, LoadFileEditMap 함수를 사용하여 현재 맵에 대한 문자열을 가져와
        // CreateScene 함수를 호출하여 Map 인스턴스를 생성합니다. 이후, 생성된 Map 인스턴스를 currMap 변수에 할당하고 true 값을 반환합니다.
        // 만약 CreateScene 함수가 false 값을 반환하면, false 값을 반환합니다.
        public bool EditorInit()
        {
            sceneList = new List<Map>();

            if (!CreateScene(StringToChar(LoadFileEditMap())))
                return false;
            currMap = sceneList[0];
            return true;
        }

        // Map 객체를 인자로 받아 sceneList에 추가한다.
        public void AddScene(Map map)
        {
            sceneList.Add(map);
        }

        // 현재 맵의 인덱스를 기준으로 다음 맵으로 변경하는 함수들입니다.
        // sceneList에서 현재 맵의 인덱스를 찾은 뒤, 다음/이전 인덱스를 찾아 currMap에 할당합니다.
        public void ChangNextScene()
        {
            currMap = sceneList[sceneList.IndexOf(currMap) + 1];
        }

        // 현재 맵의 인덱스를 기준으로 이전 맵으로 변경하는 함수들입니다.
        // sceneList에서 현재 맵의 인덱스를 찾은 뒤, 다음/이전 인덱스를 찾아 currMap에 할당합니다.
        public void ChangPrevScene()
        {
            currMap = sceneList[sceneList.IndexOf(currMap) - 1];
        }

        /// <summary>
        /// 이 함수는 맵 데이터를 받아와서 새로운 Map 객체를 생성하고, 
        /// SetMap 함수를 사용하여 해당 맵 데이터를 Map 객체에 할당합니다. 
        /// 그 후, 생성된 Map 객체를 sceneList에 추가합니다. 
        /// 함수 실행이 정상적으로 완료되면 true를 반환하고, 그렇지 않으면 false를 반환합니다.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool CreateScene(char[,] map)
        {
            Map scene = new Map();
            if (!scene.SetMap(map))
                return false;
            sceneList.Add(scene);
            return true;
        }

        /// <summary>
        /// 이 함수는 현재 맵의 정보를 콘솔에 출력해주는 함수입니다.
        /// 이중 반복문을 통해 현재 맵의 모든 타일 정보를 가져와 각 타일의 상태에 따라 다른 문자를 출력합니다.
        /// NONE인 경우엔 '　', WALL인 경우엔 '■', BOX인 경우엔 '●', GOAL인 경우엔 '☆', BOXGOAL인 경우엔 '★' 문자를 출력합니다. 
        /// 출력한 후에는 개행 문자를 이용해 다음 줄로 넘어갑니다.
        /// </summary>
        public void SceneShow()
        {
            for (int y = 0; y < currMap.oMap.GetLength(0); y++)
            {
                for (int x = 0; x < currMap.oMap.GetLength(1); x++)
                {
                    Console.Write(currMap.oMap[y, x].GetTileToChar());
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 이 함수는 SceneShow()를 오버로딩한 함수로 매개 변수로 받은 맵의 정보를 콘솔에 출력해주는 함수입니다.
        /// 이중 반복문을 통해 현재 맵의 모든 타일 정보를 가져와 각 타일의 상태에 따라 다른 문자를 출력합니다.
        /// NONE인 경우엔 '　', WALL인 경우엔 '■', BOX인 경우엔 '●', GOAL인 경우엔 '☆', BOXGOAL인 경우엔 '★' 문자를 출력합니다. 
        /// 출력한 후에는 개행 문자를 이용해 다음 줄로 넘어갑니다.
        /// </summary>
        /// <param name="map"></param>
        public void SceneShow(Map map)
        {
            for (int y = 0; y < map.oMap.GetLength(0); y++)
            {
                for (int x = 0; x < map.oMap.GetLength(1); x++)
                {
                    Console.Write(map.oMap[y, x].GetTileToChar());
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 이 함수는 현재 장면(맵)을 특정 장면(Level)로 이동시켜주는 함수이다.
        /// </summary>
        /// <param name="level"></param>
        public void SetScene(int level)
        {
            currMap = sceneList[level];
        }

        /// <summary>
        /// 현재 Scene 객체에 저장된 Map 객체의 목표(goal) 타일 개수를 반환하는 함수입니다.
        /// Map 클래스의 GetGoalCount() 함수를 호출하여 목표 타일 개수를 가져온 후, 이 값을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public int GetPurposeCount()
        {
            return currMap.GetGoalCount();
        }

        /// <summary>
        /// 현재 장면(맵)의 정보를 Map 객체로 반환해주는 함수이다.
        /// </summary>
        /// <returns></returns>
        public Map GetCurrMap()
        {
            return currMap;
        }

        /// <summary>
        /// 장면(맵)들을 List로 가진 sceneList를 반환해주는 함수이다.
        /// </summary>
        /// <returns></returns>
        public List<Map> GetListMap()
        {
            return sceneList;
        }

        /// <summary>
        /// 장면(맵)들을 List로 가진 sceneList에서 특정 맵(Level)을 반환해주는 함수이다.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Map GetListMap(int level)
        {
            return sceneList[level];
        }

        /// <summary>
        /// 해당 함수는 Scene 클래스에 할당된 sceneList와 currMap을 null로 초기화하여 메모리 누수를 방지하는 역할을 합니다. 
        /// Scene 인스턴스가 더 이상 필요하지 않을 때 호출됩니다.
        /// </summary>
        public void SceneRelease()
        {
            sceneList = null;
            currMap = null;
        }

        /// <summary>
        /// 이 함수는 맵 파일의 개수를 반환합니다.
        /// DirectoryInfo 클래스를 사용하여 맵 파일이 저장된 디렉토리를 지정합니다.
        /// GetFiles() 메서드를 사용하여 해당 디렉토리에서 "*.txt" 확장자를 가진 파일을 검색합니다.
        /// 검색된 파일의 개수를 반환합니다.
        /// </summary>
        /// <returns></returns>
        private int LoadFileCount()
        {
            DirectoryInfo directory = new DirectoryInfo(@"..\..\..\Manager\Map");
            FileInfo[] file = directory.GetFiles("*.txt");
            return file.Length;
        }

        /// <summary>
        /// 해당 함수는 인자로 전달된 번호(num)에 해당하는 파일(Map[num].txt)을 읽어와서 파일 내용을 문자열(string) 형태로 반환하는 함수입니다.
        /// 파일 경로는 고정된 경로에 Map 폴더 안에 있는 파일들을 찾아오도록 되어있으며, 
        /// StreamReader 클래스를 이용해 파일을 읽어오고, 읽어온 내용을 문자열에 저장합니다.마지막으로 읽기가 끝난 파일을 닫아줍니다.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string LoadFileToStringMap(int num)
        {
            StreamReader file = new StreamReader(@"..\..\..\Manager\Map\Map" + num + ".txt");

            string strMap = "";
            strMap = file.ReadToEnd();

            file.Close();

            return strMap;
        }

        /// <summary>
        /// 이 함수는 "EditMap.txt" 파일을 읽어서 문자열로 반환하는 함수이다. 
        /// 이 파일은 게임 맵 에디터에서 생성한 맵 데이터를 저장하고 있는 파일로, 이 함수를 통해 이전에 생성된 맵 데이터를 불러올 수 있다.
        /// </summary>
        /// <returns></returns>
        private string LoadFileEditMap()
        {
            StreamReader file = new StreamReader(@"..\..\..\Manager\Map\EditMap.txt");

            string strMap = "";
            strMap = file.ReadToEnd();

            file.Close();

            return strMap;
        }

        // 문자열 형태의 맵 데이터를 이차원 char 배열 형태로 변환하는 함수
        // 맵 데이터는 개행 문자('\n') 기준으로 분리되어 있으며, 각 줄의 문자열은 이차원 배열의 각 행에 대응한다.
        // 이 함수는 문자열로부터 각 줄을 분리하여 이차원 char 배열 형태로 변환하고 반환한다.
        private char[,] StringToChar(string str)
        {
            string[] readLineStr = str.Split("\r\n");
            char[,] newMap = new char[readLineStr.Length, readLineStr[0].Length];
            for (int y = 0; y < readLineStr.Length; y++)
            {
                for(int x = 0; x < readLineStr[y].Length; x++)
                {
                    newMap[y, x] = readLineStr[y][x];
                }
            }

            return newMap;
        }

        /// <summary>
        /// CharToString 함수는 현재 Map 객체의 oMap 멤버 변수를 문자열로 변환하여 파일로 저장하는 기능을 수행한다.
        /// 변환된 문자열은 줄 바꿈 문자 '\r\n'을 이용하여 각 줄로 구분되며, Map 폴더 내에 Map { 새로 추가되는 파일 번호 }.txt 파일로 저장된다.
        /// @param map 현재 편집 중인 Map 객체, 실제 동작은 EditorMode 에서 현재 맵을 저장 시켜주는 역할을 한다.
        /// </summary>
        /// <param name="map"></param>
        public void CharToString(Map map)
        {
            string str = "";
            int fileCount = LoadFileCount();

            for (int y = 0; y < map.oMap.GetLength(0); y++)
            {
                for (int x = 0; x < map.oMap.GetLength(1); x++)
                {
                    str += map.oMap[y, x].GetTileToChar();
                }
                str += "\r\n";
            }

            string path = @"..\..\..\Manager\Map\Map" + fileCount + ".txt";

            File.WriteAllText(path, str);
        }
    }
}
