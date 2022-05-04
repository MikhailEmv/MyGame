using _2DWar.Model;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using WMPLib;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace _2DWar
{
    public partial class Form1 : Form
    {
        PictureBox[] cloud;
        Random rnd;

        PictureBox[] bullets;

        PictureBox[] enemies;
        int sizeEnemy;
        int enemySpeed;

        int Score;
        int Level;

        WindowsMediaPlayer Shoot;
        WindowsMediaPlayer GameSong;
        WindowsMediaPlayer Rip;

        PlayerModel player;
        BulletModel bullet;
        CloudsModel clouds;
        EnemyModel enemy;

        public Form1()
        {
            player = new PlayerModel(position: new Vector(0, 0));
            bullet = new BulletModel(position: new Vector(0, 0));
            clouds = new CloudsModel(position: new Vector(0, 0), limitRightSide: 1280);
            enemy = new EnemyModel(position: new Vector(0, 0));
            InitializeComponent();
        }

        private void UpdatePlayerPosition()
        {
            Vector playerPosition = player.Position;
            mainPlayer.Top = (int)playerPosition.Y;
            mainPlayer.Left = (int)playerPosition.X;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cloud = new PictureBox[20];
            rnd = new Random();

            bullets = new PictureBox[1];

            enemies = new PictureBox[4];
            sizeEnemy = rnd.Next(60, 90);

            Image easyEnemies = Image.FromFile("C:\\Users\\Michael\\Desktop\\Для игры\\apap.gif");

            Score = 0;
            Level = 1;

            for (var i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(sizeEnemy, sizeEnemy);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BackColor = Color.Transparent;
                enemies[i].Image = easyEnemies;
                enemies[i].Location = new Point(
                    (i + 1) * rnd.Next(90, 160) + 1080,
                    rnd.Next(450, 600));

                this.Controls.Add(enemies[i]);
            }

            Shoot = new WindowsMediaPlayer();
            Shoot.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\shoot.mp3";
            Shoot.settings.volume = 5;

            Rip = new WindowsMediaPlayer();
            Rip.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\rip.mp3";
            Rip.settings.volume = 25;

            GameSong = new WindowsMediaPlayer();
            GameSong.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\GameSong.mp3";
            GameSong.settings.setMode("loop", true);
            GameSong.settings.volume = 15;

            for (var i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new PictureBox();
                bullets[i].BorderStyle = BorderStyle.None;
                bullets[i].Size = new Size(20, 5);
                bullets[i].BackColor = Color.White;

                this.Controls.Add(bullets[i]);
            }

            for (var i = 0; i < cloud.Length; i++)
            {
                cloud[i] = new PictureBox();
                cloud[i].BorderStyle = BorderStyle.None;
                cloud[i].Location = new Point(rnd.Next(-1000, 1280), rnd.Next(140, 320));
                
                if (i % 2 > 0)
                {
                    cloud[i].Size = new Size(rnd.Next(100, 255), rnd.Next(30, 70));
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 255, 200, 200);
                }

                else
                {
                    cloud[i].Size = new Size(150, 25);
                    cloud[i].BackColor = Color.FromArgb(rnd.Next(50, 125), 255, 205, 205);
                }

                this.Controls.Add(cloud[i]);
            }

            GameSong.controls.play();
            UpdatePlayerPosition();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (var i = 0; i < cloud.Length; i++)
                clouds.Move(Directions.Right);
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left > 10)
            {
                player.Move(Directions.Left);
                UpdatePlayerPosition();
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left < 950)
            {
                player.Move(Directions.Right);
                UpdatePlayerPosition();
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top > 480)
            {
                player.Move(Directions.Up);
                UpdatePlayerPosition();
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top < 700)
            {
                player.Move(Directions.Down);
                UpdatePlayerPosition();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_run;
            if (e.KeyCode == Keys.Left) LeftMoveTimer.Start();
            if (e.KeyCode == Keys.Right) RightMoveTimer.Start();
            if (e.KeyCode == Keys.Up) UpMoveTimer.Start();
            if (e.KeyCode == Keys.Down) DownMoveTimer.Start();
            if (e.KeyCode == Keys.Space)
            {
                Shoot.controls.play();

                for (int i = 0; i < bullets.Length; i++)
                {
                    Intersect();
                    if (bullets[i].Left > 1280)
                    {
                        bullets[i].Location = new Point(
                            mainPlayer.Location.X + 100 + 50 * i,
                            mainPlayer.Location.Y + 50);
                    }
                }
            }
        }

        private void mainPlayer_Click(object sender, EventArgs e)
        {
            // made smth new
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_idble;
            LeftMoveTimer.Stop();
            RightMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();
        }

        private void MoveBulletsTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < bullets.Length; i++)
                bullet.Move(Directions.Right);
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemySpeed);
        }

        private void MoveEnemies(PictureBox[] ememies, int speed)
        {
            for (var i = 0; i < enemies.Length; i++)
            {
                enemies[i].Left -= speed + 
                    (int)(Math.Sin(enemies[i].Left * Math.PI / 180) +
                          Math.Cos(enemies[i].Left * Math.PI / 180));

                Intersect();

                if (enemies[i].Left < this.Left && enemies[i].Top > 600)
                {
                    int sizeEnemy = rnd.Next(60, 70);
                    enemies[i].Size = new Size(sizeEnemy, sizeEnemy);
                    enemies[i].Location = new Point((i + 1) * rnd.Next(150, 250) + 1080, rnd.Next(500, 700));
                }
            }
        }

        private void Intersect()
        {
            for (var i = 0; i < enemies.Length; i++)
            {
                if (bullets[0].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    Rip.controls.play();

                    Score += 1;
                    labelScore.Text = (Score < 10) ? "0" + Score.ToString() : Score.ToString();

                    if (Score % 10 == 0)
                    {
                        Level += 1;
                        labelLevel.Text = (Level < 10) ? "0" + Level.ToString() : Level.ToString();

                        if (enemySpeed <= 3) enemySpeed += 2;
                        if (Level == 4) GameOver("Epic Power");
                    }

                    enemies[i].Location = new Point((i + 1) * rnd.Next(150, 250) + 1280, rnd.Next(420, 600));
                    bullets[0].Location = new Point(2000, mainPlayer.Location.Y + 50);
                }

                if (mainPlayer.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    mainPlayer.Visible = false;
                    GameOver("Game Over"); 
                }
            }
        }

        private void GameOver(string str)
        {
            label1.Text = str;
            label1.Location = new Point(584, 380);
            label1.Visible = true;

            GameSong.controls.stop();
            MoveEnemiesTimer.Stop();
        }
    }
}