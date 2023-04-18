using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;
using System.IO;

namespace gameDemo
{
    public partial class Form1 : Form
    {
        const int BULLET_NUM = 35;  //최대 총알 갯수
        const int STAR_NUM = 1;     //최대 별 갯수
        const int MAP_H = 650;      //클라이언트 높이
        const int MAP_W = 650;      //클라이언트 너비

        struct BULLET
        {
            public bool exist;    //총알 존재유무
            public int x, y;      //총알 생성위치
            public int speed;     //총알 스피드
            public int direction; //총알 진행방향
        }
        BULLET[] bullet = new BULLET[BULLET_NUM]; //배열로 총알 갯수 제한

        struct STAR
        {
            public bool exist;   //별 존재유무 
            public int x, y;     //별 위치
        }
        STAR[] star = new STAR[STAR_NUM];  //배열로 별 갯수 제한

        //총알 높이너비
        const int bW = 5;
        const int bH = 5;
        //별 높이너비
        const int sW = 25;
        const int sH = 25;


        //점수를 위해 선언
        float starScore = 35.5f;       //별의 점수
        double tempScore = 0;          //증가하는 점수는 소수점
        long score = 0;                //증가한 점수를 정수로 표현
        static long record_score = 0;  //데이터상 가장 높은점수
        int listnum = 0;

        //랜덤함수를 위해 선언
        Random random = new Random();


        //플레이어 생성
        Player player = new Player();

        //리스트 클래스 생성
        List<long> list = new List<long>();

        //효과음(별 먹었을때, 죽었을때) 생성
        SoundPlayer sndStar;
        SoundPlayer sndDead;

        //게임 전체 영역에 대한 이미지를 위해 비트맵 객체
        Bitmap hspace, hplayer, hbullet, hstar;
        Bitmap hArea = new Bitmap(MAP_W, MAP_H);




        /*
* 출처 : https://ko.wikipedia.org/wiki/%EC%9C%88%EB%8F%84%EC%9A%B0_%EB%9D%BC%EC%9D%B4%EB%B8%8C%EB%9F%AC%EB%A6%AC_%ED%8C%8C%EC%9D%BC
* 
* -- USER32.DLL-- 
* USER32.DLL은 윈도우 USER 구성 요소를 구현한다. 
* 윈도우 구성 요소는 창이나 메뉴 같은 윈도우 사용자 인터페이스의 표준 요소들을 생성하고 다룬다. 
* 그러므로 프로그램들에게 그래픽 사용자 인터페이스(GUI)를 구현할 수 있게 해준다. 
* 프로그램들은 창 생성이나 관리, 그리고 창 메시지 받기 등을 수행하기 위해 
* 윈도우 USER에서 함수들을 호출한다.
* GDI에 관한 많은 USER32.DLL 함수들은 GDI32.DLL에 의해 내보내진 것들이다. 
* 어떤 종류의 프로그램들은 또한 GDI 함수들을 직접적으로 
* 호출하여 낮은 수준의 드로잉을 수행하기도 한다.
* 
*/
        //키 이벤트를 처리하기 위해 필요함
        [DllImport("User32.dll")]

        //키보드로부터 입력한 키값을 얻어오는 윈도우 기반 메소드
        private static extern short GetKeyState(int nVirtKey);



        /*
 * 출처 : https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/interop/how-to-use-platform-invoke-to-play-a-wave-file
 * --winmm.dll--
 * 웨이브 파일을 선택하면 winmm.dll 라이브러리의 PlaySound() 메서드를 사용하여 재생
 */

        //사운드를 처리하기 위해 필요
        [DllImport("winmm.dll")]

        //사운드 음원 재생 및 정지와 같은 기능을 수행하기 위한 윈도우 기반 메소드
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);





        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Size = new Size(MAP_H, MAP_W);
            //쓸 리소스(이미지)를 로드
            hspace = Properties.Resources.space;
            hplayer = Properties.Resources.player;
            hbullet = Properties.Resources.bullet;
            hstar = Properties.Resources.star;

            //효과음 로드
            sndStar = new SoundPlayer(Properties.Resources.starSound);
            sndDead = new SoundPlayer(Properties.Resources.bomb);

            StartGame();
        }

        private void StartGame()
        {
            for (int i = 0; i < BULLET_NUM; i++) //총알 존재 다 false로 초기화
                bullet[i].exist = false;
            for (int i = 0; i < STAR_NUM; i++)   //별 존재 다 false로 초기화
                star[i].exist = false;

            //배경음악 재생
            mciSendString("open \"" + "../../../resource/background.mp3" + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);

            //플레이어 위치 및 스코어 초기화
            player.x = 300;
            player.y = 300;
            score = 0;
            tempScore = 0;


            //데이터가 있다면 최고스코어 불러오기
            try
            {
                using (StreamReader sr = new StreamReader("../../../score.txt"))
                {
                    record_score = long.Parse(sr.ReadLine()); //최고점수 읽기
                }
            }
            catch(Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("../../../score.txt"))
                {
                    sw.Write("0"); //만약 경로에 score.txt 없다면 만들고 0을 넣음
                }

                using (StreamWriter sw = new StreamWriter("../../../error.txt"))
                {
                    sw.Write(ex.ToString()); //에러로그를 주어진 경로에만든다.
                }
            }


            //타이머 시작
            timer1.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:      //R키 감지
                    StartGame();  //다시 시작
                    break;
            }
        }

        //그래픽 객체를 통해 화면에 비트맵 객체를 출력
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (hArea != null)
            {
                e.Graphics.DrawImage(hArea, 0, 0); //DrawImage()메소드는 이미지를 출력
                // 여기서는 전체적인 너비의 이미지 영역을 그려줌
                //DrawImage(그릴이미지, 시작좌표x, 시작좌표y)
            }
        }

        //메뉴스트립트
        private void toolStripMenuItem1_1_Click(object sender, EventArgs e)
        {
            Application.Exit();  //프로그램종료
        }

        private void toolStripMenuI2_Click(object sender, EventArgs e) //설명서
        {
            MessageBox.Show("Move with arrow keys\n Evadae all projectiles and get the highest score\n Bonus Points for getting the STAR! \n Press 'R' to restart");
        }



        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 배경색으로 지우지 않음
            // 가상메소드로서 오버라이딩한 형태
            // 이미지를 원래 반복적으로 다시 그려주는데 그럴때 깜빡임 현상이 일어남
            // 원래의 이 메서드는 화면 지우는 기능을 함
            // 이 메서드를 오버라이딩 하여 아무런 기능도 하지 않도록 깜빡임 현상 제거
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //충돌 계산을 위한 사각형들
            Rectangle playert, bullett, start, irt;

            int i;


            Graphics g = Graphics.FromImage(hArea);  //그래픽 객체 얻어오기
            g.DrawImage(hspace, 0, 0);               //스페이스 배경 그리기
            g.DrawImage(hplayer, player.x - player.w / 2, player.y - player.h / 2); // 내 캐릭 그리기


            //왼쪽 방향키 누른 상태에서 내 캐릭 왼쪽으로 지정한만큼 움직이기
            //이동단위는 음수(왼쪽)
            if (GetKeyState((int)Keys.Left) < 0)
            {
                player.x = player.x - player.speed;
                player.x = Math.Max(player.w, player.x); //가장 왼쪽까지 가면 더이상 움직이면 안됨
            }
            //오른쪽 방향키 누른 상태에서 내 캐릭 오른쪽으로 지정한만큼 움직이기
            //이동단위는 양수(오른쪽)
            if (GetKeyState((int)Keys.Right) < 0)
            {
                player.x = player.x + player.speed;
                player.x = Math.Min(ClientSize.Width - player.w / 2, player.x); //가장 오른쪽까지 가면 더이상 움직이면 안됨
            }
            //아래쪽 방향키 누른 상태에서 내 캐릭 아래쪽으로 지정한만큼 움직이기
            //이동단위는 양수(아래쪽)
            if (GetKeyState((int)Keys.Down) < 0)
            {
                player.y = player.y + player.speed;
                player.y = Math.Min(ClientSize.Height - player.h / 2, player.y); //가장 아래까지 가면 더이상 움직이면 안됨
            }
            //위쪽 방향키 누른 상태에서 내 캐릭 위쪽으로 지정한만큼 움직이기
            //이동단위는 음수(위쪽)
            if (GetKeyState((int)Keys.Up) < 0)
            {
                player.y = player.y - player.speed;
                player.y = Math.Max(player.h, player.y); //가장 위까지 가면 더이상 움직이면 안됨
            }



            //총알 생성
            if(random.Next(10) == 1) //0~9 사이 난수 생성 중 0이 된 경우 조건문 진입
            {
                for (i = 0; i < BULLET_NUM && bullet[i].exist == true; i++) {; }
                // 아래 i 값을 설정해주기 위한 과정
                // 최대 총알 값보다 적으면서 현재 존재하는 총알의 개수를 확인 한 다음
                // 그 다음에 오는 i 값은 exist가 false이므로 
                // 이 for문이 정지되고 i 값이 설정 됨   
                

                if (i != BULLET_NUM)  //만약 총알이 최대개수가 아니라면
                {
                    if(random.Next(2) == 1) // 두번중 한번만 위에서 생성
                    {
                        //총알 생성 좌표와 이동방향
                        bullet[i].x = random.Next(ClientSize.Width - bH / 2); //x 좌표는 전체 x 랜덤
                        bullet[i].y = 0;                                      //y좌표는 맵끝이기에 고정
                        bullet[i].direction = 1;                              //방향
                    }
                    
                    bullet[i].speed = random.Next(1, 4) + 1;  //속도 랜덤
                    bullet[i].exist = true;                   //그 총알은 존재하는것으로 변경
                }
            }

            //총알 이동방향
            for(i = 0; i < BULLET_NUM; i++)
            {
                if (bullet[i].exist == false)  //총알이 존재하지않으면 건너뜀
                    continue;

                if (random.Next(2) == 1)      //50%확률로 오른쪽으로 
                {
                    bullet[i].x += bullet[i].speed * 1;
                    bullet[i].y += bullet[i].speed;
                }
                else                          //50%확률로 왼쪽으로 왔다갔다
                {
                    bullet[i].x += bullet[i].speed * -1;
                    bullet[i].y += bullet[i].speed;
                }

                if (bullet[i].x < 0 || bullet[i].x > ClientSize.Width || //만약 총알이 맵밖으로 나가면 존재안하게
                    bullet[i].y < 0 || bullet[i].y > ClientSize.Height)
                {
                    bullet[i].exist = false;  //총알 사라짐
                }
                else //그게 아니라면
                    g.DrawImage(hbullet, bullet[i].x - bW / 2, bullet[i].y); //총알 계속 그려주기
            }

            //별 생성
            if (random.Next(1) == 0)
            {
                for (i = 0; i < STAR_NUM && star[i].exist == true; i++) {; }
                // 아래 i 값을 설정해주기 위한 과정
                // 최대 별 값보다 적으면서 현재 존재하는 별의 개수를 확인 한 다음
                // 그 다음에 오는 i 값은 exist가 false이므로 
                // 이 for문이 정지되고 i 값이 설정 됨  

                if (i != STAR_NUM) // 만약 별이 없으면(1개만존재할수 있으므로)
                {
                    //위쪽 랜덤한 좌표에 별을 생성
                    star[i].x = random.Next(sW / 2, ClientSize.Width - sW / 2); //x좌표는 전체 너비의 랜덤
                    star[i].y = random.Next(sH, 100);                    //난이도를 위해 y좌표는 위쪽 랜덤
                    star[i].exist = true;
                }
            }

            for(i = 0; i < STAR_NUM; i++)
            {
               g.DrawImage(hstar, star[i].x - sW / 2, star[i].y); //별 계속 그려주기
            }


            //충돌체크

            //플레이어 사각형 설정
            playert = new Rectangle(player.x - player.w / 2, player.y, player.w, player.h);

            for(i = 0; i < BULLET_NUM; i++) //총알 최대 개수만큼
            {
                if (bullet[i].exist == false) continue; // 총알 없다면 건너뜀

                //총알 사각형 찾아서 설정
                bullett = new Rectangle(bullet[i].x - bW / 2, bullet[i].y, bW, bH);

                irt = Rectangle.Intersect(playert, bullett); // 플레이어랑 겹치는 부분 찾고

                if(irt.IsEmpty == false) //겹치는 부분이 비어있지 않다면
                {
                    g.Clear(Color.White); //배경없애고
                    sndDead.Play();       //터지는 소리
                    GameStop();           //게임 멈추기
                    timer1.Stop();        //타이머 스탑시키기
                }
            }

            for (i = 0; i < STAR_NUM; i++) // 별 최대 개수만큼
            {
                //별 사각형 찾아서 설정
                start = new Rectangle(star[i].x - sW / 2, star[i].y, sW, sH);

                irt = Rectangle.Intersect(playert, start); // 플레이어랑 겹치는 부분 찾기

                if (irt.IsEmpty == false) // 겹치는 부분이 비어있지 않다면
                {
                    sndStar.Play();          //별먹는소리
                    star[i].exist = false;   //먹은 별은 존재하지않는다
                    tempScore += starScore;  //점수에 별점수 추가
                }
            }

            //시간이 지남에 따라 점수 증가
            tempScore += 0.1;
            score = (long)tempScore; //깔끔하게 정수형태로 표현

            if (score > record_score)  //점수가 최고점수보다 높아지면 
                record_score = score;  //최고점수 갱신

            //점수판
            //화면에 출력을 위한 부분, 폰트 설정
            Font _font = new System.Drawing.Font(new FontFamily("바탕체"), 14, FontStyle.Bold);

            string result;             //점수를 넣어줄 스트링클래스 
            result = score.ToString(); //점수를 형변환

            //문자 그리기(무엇을 쓸지, 폰트 설정, 색상 설정, 위치 설정)
            g.DrawString("Record : " + result, _font, Brushes.White, new PointF(10, 20));
            g.DrawString("New Record : " + record_score.ToString(), _font, Brushes.White, new PointF(300, 20));





            Invalidate(); //화면 전체를 업데이트
        }


        private void GameStop()
        {
            mciSendString("stop MediaFile", null, 0, IntPtr.Zero); //배경음 없애고 (종료되니까)

            //리스트에 스코어 저장
            list.Add(score);

            Graphics g = Graphics.FromImage(hArea); // 그래픽 객체 얻어오기
            g.DrawImage(hspace, 0, 0); // 바다 이미지 그리기

            //화면에 출력을 위한 부분, 폰트 설정
            Font _font = new System.Drawing.Font(new FontFamily("바탕체"), 50, FontStyle.Bold);
            //문자 그리기(무엇을 쓸지, 폰트 설정, 색상 설정, 위치 설정)
            g.DrawString("GameOver", _font, Brushes.White, new PointF(200, 300));
            g.DrawString("R to Restart", _font, Brushes.White, new PointF(80, 400));


            //스코어 파일 출력
            using (StreamWriter sw = new StreamWriter("../../../score.txt")) //이 좌표에
            {
                sw.Write(record_score.ToString()); // 최고점수를 score.txt에 저장
            }

            using (StreamWriter sw = new StreamWriter("../../../scoreLog.txt")) //이 좌표에
            {
                sw.Write(list[listnum++]); // 최근점수를 scoreLog.txt에 저장
            }




        }

    }
}
