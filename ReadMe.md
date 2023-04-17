# [ 소코반 게임 프로젝트 ]

## <strong>부연설명</strong>
절차지향이었지만
욕심내서 객체지향으로 설계함
하지만 설계가 시작부터 엉켜서 추상화랑, 다형성, 상속
개념을 사용하지 못했음

### <strong>작업일수</strong>
대충 초반 1일 
설계랑 플레이시 버그땜에 3일, 에디터 모드 1일
5일 가량 걸린듯

<hr>

## <strong>프로젝트 및 클래스 설명</strong>
최상위 Main(Program)에 바로 밑에
GameLoop가 동작하는 Game 클래스와 Scene 클래스가 Manager분류로
설계하였고(싱글턴 패턴으로 관리자를 만들어 객체 하나만 사용하도록함)

Scene 클래스는 Map 객체를 가지는 관리자로
현재 씬(맵)의 정보를 가지고 있고 파일을 읽어 맵을 가져오고
에디터 모드로 파일을 쓰기로 맵을 저장하는 메서드가 있음

GameMode와 EditorMode도 PlayMode 분류로 이녀석들도 싱글턴으로 설계함
하나만 사용하는 객체이기 때문

그밑에 Object 분류로 Map, Object, Player가 있음 Map은 Object 객체를 가지고있음
Object는 타일정보와, 그 타일의 좌표정보를 가짐

이 게임은 싱글게임이라 Player가 하나만 필요할거라 예상해 이것도 싱글톤으로 설계

## <strong>플레이 방식</strong>
플레이시 1번을 누르면 Editor모드가 나옴 Goal지점과 Box의 개수가 같고,
플레이어가 단 하나만 있을때 맵이 저장될 수 있도록 설계함 이러한 만들어진 맵은
Map 폴더에 txt 파일로 저장된다.

2번을 누르면 Game모드가 나온다 실제 게임을 플레이 할 수 있다.
w,a,s,d와 화살표키로 이동 가능하며 박스를 골에 다 넣으면 클리어하는 게임이다.
특스키로 T를 누르면 뒤로가기, R키를 누르면 리셋, F1은 골을 다 넣었다고 가정하는
치트키, F2는 다음맵, F3는 이전맵으로 돌아가는 키다.

3번을 누르면 프로그램이 종료된다.