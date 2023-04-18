using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameDemo
{
    class Player
    {
        public int x, y;    //플레이어 위치
        public int w, h;  //플레이어 너비 높이
        public int speed;   //플레이어 스피드

        public Player()
        {
            //x,y 좌표
            x = 300;
            y = 300;
            //플레이어 너비 높이
            w = 18;
            h = 18;
            //플레이어의 스피드
            speed = 4;
        }


    }
}
