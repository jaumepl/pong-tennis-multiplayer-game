using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using personal.Hubs;

namespace tennis1
{
    public class TennisGame
    {
        public int LeftWins { get; set; }
        public int RighttWins { get; set; }
        public int Alt_Y { get; set; }

        public int Ample_X { get; set; }

        public float ballX { get; set; }
        public float ballY { get; set; }
        public float ballSpeedX { get; set; }
        public float ballSpeedY { get; set; }
        public float palaDreta { get; set; }
        public float palaEsquerra { get; set; }
        public bool inicialized { get; set; }
        public string namePlayer1 { get; set; }
        public string namePlayer2 { get; set; }

        //initial values

        public int TennisId { get; set; }

        private int _Initial_Ample_X;
        private int _Initial_Alt_Y;
        private float _Initial_ballX;
        private float _Initial_ballY;
        private float _Initial_ballSpeedX;
        private float _Initial_ballSpeedY;

        public TennisGame(int Id, int tAmple_X, int tAlt_Y, float tballX, float tballY,
                        float tballSpeedX, float tballSpeedY, bool tinicialized)
        {
            TennisId=Id;
            Ample_X = tAmple_X;
            Alt_Y = tAlt_Y;
            ballX = tballX;
            ballY = tballY;
            ballSpeedX = tballSpeedX;
            ballSpeedY = tballSpeedY;
            inicialized = tinicialized;

            _Initial_Ample_X = tAmple_X;
            _Initial_Alt_Y = tAlt_Y;
            _Initial_ballX = tballX;
            _Initial_ballY = tballY;
            _Initial_ballSpeedX = tballSpeedX;
            _Initial_ballSpeedY = tballSpeedY;
            inicialized = tinicialized;
        }

        public void restart(int dir)
        {
            Ample_X = _Initial_Ample_X;
            Alt_Y = _Initial_Alt_Y;
            ballX = _Initial_ballX;
            ballY = _Initial_ballY;
            ballSpeedX = (dir)*_Initial_ballSpeedX;
            ballSpeedY = _Initial_ballSpeedY;
            palaDreta = Alt_Y / 2 - 50;
            palaEsquerra = Alt_Y / 2  - 50;
        }

        public void start()
        {
            RighttWins = 0;
            LeftWins = 0;
            palaDreta = Alt_Y / 2 - 50;
            palaEsquerra = Alt_Y / 2 - 50;
            while (!Program.SharedObj[0].inicialized)
            {
                Thread.Sleep(1000);
            }
            Program.GlobalHubContext.Clients.All
                .SendAsync("setGameDetails",
                    (0, 0, Program.SharedObj[0].namePlayer1, "Waiting for player 2"));
            while (Program.SharedObj[0].namePlayer1 == null || Program.SharedObj[0].namePlayer2 == null)
            {
                Thread.Sleep(1000);
            }
            Program.GlobalHubContext.Clients.All
                .SendAsync("setGameDetails",
                    (0, 0, Program.SharedObj[0].namePlayer1, Program.SharedObj[0].namePlayer2));
            while (true)
            {
                Thread.Sleep(10);
                //nova posició, suma delta
                float deltaX = ballSpeedX;
                float deltaY = ballSpeedY;
                ballX = ballX + deltaX;
                ballY = ballY + deltaY;
                //Topalls
                if (ballX >= Ample_X)
                {
                    ballSpeedX = -ballSpeedX;
                    ballX = Ample_X;

                }
                if (ballY >= Alt_Y)
                {
                    ballSpeedY = -ballSpeedY;
                    ballY = Alt_Y;
                }
                if (ballX <= 0)
                {
                    RighttWins += 1;
                    Program.GlobalHubContext.Clients.All
                        .SendAsync("setGameDetails",
                        (LeftWins, RighttWins, Program.SharedObj[0].namePlayer1, Program.SharedObj[0].namePlayer2));
                    restart(-1);
                }
                if (ballX >= Ample_X)
                {
                    LeftWins += 1;
                    Program.GlobalHubContext.Clients.All
                        .SendAsync("setGameDetails",
                        (LeftWins, RighttWins, Program.SharedObj[0].namePlayer1, Program.SharedObj[0].namePlayer2));
                    restart(1);
                }
                if (ballY <= 0)
                {
                    ballSpeedY = -ballSpeedY;
                    ballY = 0;
                }
                if (ballX <= 10 && ballX >= 1 && ballY >= palaEsquerra && ballY <= palaEsquerra + 100)
                {
                    ballSpeedX = -ballSpeedX;
                }
                if (ballX <= Ample_X - 1 && ballX >= Ample_X - 10 && ballY >= palaDreta && ballY <= palaDreta + 100)
                {
                    ballSpeedX = -ballSpeedX;
                }
                if (Program.GlobalHubContext != null)
                {
                    Program.GlobalHubContext.Clients.All
                        .SendAsync("setGamePositions",
                            (Program.SharedObj[0].palaEsquerra, Program.SharedObj[0].palaDreta, ballX, ballY));
                }
                //leftMouseX, leftMouseY, rigthMouseX, rightMouseY, ballPosX, ballPosY
            }
            // public float leftRacketX { get; set; }
            // public float leftRacketY { get; set; }
            // public float rightRacketX { get; set; }
            // public float rightRacketY { get; set; }
            // public bool winningScreenShowed { get; set; }
            // public bool stopped { get; set; }
            // public bool requestId { get; set; }
            // public static int computerLevel = 9; //range 0 - 10
            // public float player1Score { get; set; }
            // public float player2Score { get; set; }
            // private int WINNING_SCORE = 3;
            // private int PADDLE_HEIGHT = 100;
            // private int PADDLE_THICKNESS = 10;

            // public void loop()
            // {
            //     if (!stopped) {
            //     moveEverything();
            //     }
            // }

            // public void start() {
            //     stopped = false;
            // }

            // function stop() {
            //     if (requestId) {
            //     window.cancelAnimationFrame(requestId);
            //     }
            //     stopped = true;
            // }

            // start();

            // canvas.addEventListener('mousemove', function(e){
            //     var mousePos = calculateMousePos(e);
            //     paddle1Y = mousePos.y - (PADDLE_HEIGHT/2);
            // });

            // canvas.addEventListener('click', function(){
            //     if(!winningScreenShowed){
            //     return;
            //     }
            //     winningScreenShowed = false;
            //     playerScore = 0;
            //     computerScore = 0;
            // });

            // function getRadomNumber(min, max){
            //     return Math.random() * (max - min) + min;
            // }


            // public void moveEverything(){   
            //     if(winningScreenShowed){
            //     return;
            //     }
            //     var hitThePaddle1 = (ballY > paddle1Y && ballY < paddle1Y + PADDLE_HEIGHT);
            //     var hitThePaddle2 = (ballY > paddle2Y && ballY < paddle2Y + PADDLE_HEIGHT);
            //     computerMovement();
            //     ballX = ballX + ballSpeedX;//del servidor
            //     ballY = ballY + ballSpeedY;
            //     if(ballX > W-20){
            //     if(hitThePaddle2){
            //         ballSpeedX = -ballSpeedX;
            //     }else if(ballX > W){
            //                 playerScore++;
            //         resetBall();
            //     }
            //     }
            //     if(ballX < 20){
            //     if(hitThePaddle1){
            //         ballSpeedX = -ballSpeedX;
            //         var deltaY = ballY - (paddle1Y + (PADDLE_HEIGHT/2));
            //         ballSpeedY = deltaY * 0.2;
            //     }else if(ballX < 0){
            //         computerScore++;
            //         resetBall();
            //         stop();
            //         setTimeout(function(){
            //         start();
            //         }, 1000);
            //     }
            //     }
            //     if(ballY > H){
            //     ballSpeedY = -ballSpeedY;
            //     }
            //     if(ballY < 0){
            //     ballSpeedY = -ballSpeedY;
            //     }
            // };

            // function drawNet(){
            //     for(var i = 0; i < H; i+=40){
            //     drawRect(W/2-1, i, 2, 20, 'white');
            //     }
            // };

            // function drawEverything(){
            //     drawRect(0, 0, W, H, 'white');
            //     drawNet();    
            //     drawRect(0, paddle1Y, PADDLE_THICKNESS, PADDLE_HEIGHT, 'red');
            //     drawRect(W-PADDLE_THICKNESS, paddle2Y, PADDLE_THICKNESS, PADDLE_HEIGHT, 'grey');
            //     drawCircle(ballX, ballY, 5, 'black');
            //     if(winningScreenShowed){
            //     ctx.fillStyle = 'black';
            //     if(playerScore == WINNING_SCORE){
            //         ctx.fillText('Has guanyat!', 100, 100);
            //     }else if(computerScore >= WINNING_SCORE){
            //         ctx.fillText('Ha guanyat l´ordinador!', W-150, 100);  
            //     }
            //     ctx.fillText('click per continuar', 300, 310);
            //     return;
            //     }
            //     ctx.fillStyle = 'black';
            //     ctx.fillText(playerScore, 100, 100);
            //     ctx.fillText(computerScore, W-100, 100);
            // }

            // function drawRect(x, y, w, h, color){
            //     ctx.fillStyle = color;
            //     ctx.fillRect(x, y, w, h);
            // }

            // function drawCircle(x, y, rad, color){
            //     ctx.fillStyle = color;
            //     ctx.beginPath();
            //     ctx.arc(x, y, rad, 0, Math.PI * 2, false);
            //     ctx.fill();
            // }

            // function calculateMousePos(e){
            //     var rect = canvas.getBoundingClientRect();
            //     var root = document.documentElement;
            //     var mouseX = e.clientX - rect.left - root.scrollLeft;
            //     var mouseY = e.clientY - rect.top - root.scrollTop;
            //     return {
            //     x: mouseX,
            //     y: mouseY
            //     }
            // }

            // function resetBall(){
            //     if(playerScore == WINNING_SCORE || computerScore == WINNING_SCORE){
            //     winningScreenShowed = true;
            //     }
            //     ballX = W/2;
            //     ballY = H/2;
            //         ballSpeedY = 0;
            //     paddle2Y = 150;
            //     paddle1Y = 150;
            // }
        }
    }
}