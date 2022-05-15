using _2DWar.Model;
using System;
using System.Collections;
using System.Collections.Generic;
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
        Random rnd;

        CloudModel[] cloudsModel;
        PictureBox[] clouds;

        Dictionary<string, BulletModel> bulletsModel;
        Dictionary<string, PictureBox> bullets;

        Dictionary<string, EnemyModel> enemiesModel;
        Dictionary<string, PictureBox> enemies;

        int enemySpeed;

        int Score;
        int Level;

        WindowsMediaPlayer Shoot;
        WindowsMediaPlayer GameSong;
        WindowsMediaPlayer Rip;

        Image easyEnemies = Image.FromFile("C:\\Users\\Michael\\Desktop\\Для игры\\apap.gif");

        PlayerModel player;

        public Form1()
        {
            player = new PlayerModel(position: new Vector(0, 600));
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rnd = new Random();

            clouds = new PictureBox[20];
            cloudsModel = new CloudModel[20];

            bullets = new Dictionary<string, PictureBox>();
            bulletsModel = new Dictionary<string, BulletModel>();

            enemies = new Dictionary<string, PictureBox>();
            enemiesModel = new Dictionary<string, EnemyModel>();

            Score = 0;
            Level = 1;

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

            for (var i = 0; i < 4; i++) SpawnEnemy(easyEnemies);

            SpawnClouds();

            GameSong.controls.play();

            UpdatePlayerPosition();
        }

        private void UpdatePlayerPosition()
        {
            Vector playerPosition = player.Position;
            mainPlayer.Left = (int)playerPosition.X;
            mainPlayer.Top = (int)playerPosition.Y;
        }

        private void UpdateCloud(int cloudIndex)
        {
            var cloudModel = cloudsModel[cloudIndex];
            clouds[cloudIndex].Left = (int)cloudModel.Position.X;
            clouds[cloudIndex].Top = (int)cloudModel.Position.Y;
            clouds[cloudIndex].Size = new Size(cloudModel.Width, cloudModel.Height);
        }

        private void UpdateBullet(string id)
        {
            var bulletModel = bulletsModel[id];
            bullets[id].Left = (int)bulletModel.Position.X;
            bullets[id].Top = (int)bulletModel.Position.Y;
        }

        private void UpdateEnemyPosition(string id)
        {
            var enemyModel = enemiesModel[id];
            enemies[id].Left = (int)enemyModel.Position.X;
            enemies[id].Top = (int)enemyModel.Position.Y;
            enemies[id].Size = new Size(enemyModel.Width, enemyModel.Height);
        }

        private void RemoveEnemy(string id)
        {
            this.Controls.Remove(enemies[id]);
            enemies.Remove(id);
            enemiesModel.Remove(id);
        }

        private void RemoveBullet(string id)
        {
            this.Controls.Remove(bullets[id]);
            bullets.Remove(id);
            bulletsModel.Remove(id);
        }

        private void SpawnEnemy(Image easyEnemies)
        {
            var enemySize = rnd.Next(60, 90);
            var id = Guid.NewGuid().ToString();
            var enemyPosition = new Vector(rnd.Next(90, 160) + 1080, rnd.Next(450, 600));
            var enemyModel = new EnemyModel(id: id, position: enemyPosition, height: enemySize, width: enemySize);
            var enemy = new PictureBox();
            enemy.SizeMode = PictureBoxSizeMode.Zoom;
            enemy.BackColor = Color.Transparent;
            enemy.Image = easyEnemies;

            enemiesModel[id] = enemyModel;
            enemies[id] = enemy;

            this.Controls.Add(enemy);
        }
       
        private void SpawnBullet()
        {
            var id = Guid.NewGuid().ToString();
            var bulletPosition = new Vector(mainPlayer.Location.X + 100 + 50,
                                            mainPlayer.Location.Y + 50);
            var bulletModel = new BulletModel(id: id, position: bulletPosition, limitRightSide: 1280);
            var bullet = new PictureBox();
            bullet.BorderStyle = BorderStyle.None;
            bullet.Size = new Size(20, 5);
            bullet.BackColor = Color.White;

            bulletsModel[id] = bulletModel;
            bullets[id] = bullet;

            this.Controls.Add(bullet);
        }

        private void MoveBulletsTimer_Tick(object sender, EventArgs e)
        {
            var removeIds = new List<string>();
            foreach (KeyValuePair<string, BulletModel> entry in bulletsModel)
            {
                if (entry.Value.IsActive)
                {
                    entry.Value.Move(Directions.Right);
                    UpdateBullet(entry.Key);
                }
                else removeIds.Add(entry.Key);
            }

            for (var i = 0; i < removeIds.Count; i++) RemoveBullet(removeIds[i]);

        }

        private void SpawnClouds()
        {
            for (var i = 0; i < clouds.Length; i++)
            {
                int height;
                int width;
                Color color;

                if (i % 2 > 0)
                {
                    height = rnd.Next(30, 70);
                    width = rnd.Next(100, 255);
                    color = Color.FromArgb(rnd.Next(50, 125), 255, 200, 200);
                }

                else
                {
                    height = 25;
                    width = 150;
                    color = Color.FromArgb(rnd.Next(50, 125), 255, 205, 205);
                }

                var cloudPosition = new Vector(rnd.Next(-1000, 1280), rnd.Next(250, 360));
                cloudsModel[i] = new CloudModel(position: cloudPosition, limitRightSide: 1280, 
                    height: height, width: width);
                clouds[i] = new PictureBox();
                clouds[i].BorderStyle = BorderStyle.None;
                clouds[i].BackColor = color;
                UpdateCloud(i);

                this.Controls.Add(clouds[i]);
            }
        }

        /// <summary>
        /// Заставляет облака двигаться
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (var i = 0; i < clouds.Length; i++)
            {
                cloudsModel[i].Move(Directions.Right);
                UpdateCloud(i);
            }
        }

        /// <summary>
        /// Движение игрока влево
        /// </summary>
        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left > 10)
            {
                player.Move(Directions.Left);
                UpdatePlayerPosition();
            }
        }

        /// <summary>
        /// Движение игрока вправо
        /// </summary>
        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left < 950)
            {
                player.Move(Directions.Right);
                UpdatePlayerPosition();
            }
        }

        /// <summary>
        /// Движение игрока вверх
        /// </summary>
        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top > 480)
            {
                player.Move(Directions.Up);
                UpdatePlayerPosition();
            }
        }

        /// <summary>
        /// Движение игрока вниз
        /// </summary>
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top < 700)
            {
                player.Move(Directions.Down);
                UpdatePlayerPosition();
            }
        }

        /// <summary>
        /// Клавиша - движение + реализация выстрела
        /// </summary>
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
                SpawnBullet();
            }
        }

        /// <summary>
        /// Если ничего не нажимаем
        /// </summary>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.cowboy_idble;
            LeftMoveTimer.Stop();
            RightMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            var removeIds = new List<string>(); 
            foreach (KeyValuePair<string, EnemyModel> entry in enemiesModel)
            {
                if (entry.Value.IsAlive)
                {
                    entry.Value.Move(Directions.Left);
                    UpdateEnemyPosition(entry.Key);
                }
                else removeIds.Add(entry.Key);
            }

            for (var i = 0; i < removeIds.Count; i++)
            {
                RemoveEnemy(removeIds[i]);
                SpawnEnemy(easyEnemies);
            }

            Intersect();
        }

        private void Intersect()
        {
            foreach (KeyValuePair<string, PictureBox> enemyEntry in enemies)
            {
                var enemyBounds = enemyEntry.Value.Bounds;  
                foreach (KeyValuePair<string, PictureBox> bulletEntry in bullets)
                {
                    if (bulletEntry.Value.Bounds.IntersectsWith(enemyBounds))
                    {
                        Rip.controls.play();
                        CalculateAndDrawScores();

                        bulletsModel[bulletEntry.Key].Disactivation();
                        enemiesModel[enemyEntry.Key].Mortification();
                    }
                }

                if (mainPlayer.Bounds.IntersectsWith(enemyBounds))
                {
                    mainPlayer.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void CalculateAndDrawScores()
        {
            Score += 1;
            labelScore.Text = (Score < 10) ? "0" + Score.ToString() : Score.ToString();

            if (Score % 10 == 0)
            {
                Level += 1;
                labelLevel.Text = (Level < 10) ? "0" + Level.ToString() : Level.ToString();

                if (enemySpeed <= 3) enemySpeed += 2;
                if (Level == 4) GameOver("Epic Power");
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

        private void mainPlayer_Click(object sender, EventArgs e)
        {
            // there's nothing here
        }
    }
}