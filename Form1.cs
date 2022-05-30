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

        PlayerModel playerModel;

        Dictionary<string, GunSackModel> gunSacksModel;
        Dictionary<string, PictureBox> gunSacks;

        private int Score;
        private int Level;

        WindowsMediaPlayer Shoot;
        WindowsMediaPlayer GameSong;
        WindowsMediaPlayer Rip;
        WindowsMediaPlayer HeavyBreathing;
        WindowsMediaPlayer DeathSound;
        WindowsMediaPlayer WinSound;

        Image firstEnemy = Image.FromFile("C:\\Users\\Michael\\Desktop\\Для игры\\apap.gif");
        Image secondEnemy = Image.FromFile("C:\\Users\\Michael\\Desktop\\Для игры\\4HVD1.gif");
        Image thirdEnemy = Image.FromFile("C:\\Users\\Michael\\Desktop\\Для игры\\BRyx.gif");
        Image gunSackImage = Image.FromFile("C:\\Users\\Michael\\Desktop\\Для игры\\5IPp.gif");

        public Form1()
        {
            playerModel = new PlayerModel(position: new Vector(10, 570), false);
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

            gunSacks = new Dictionary<string, PictureBox>();
            gunSacksModel = new Dictionary<string, GunSackModel>();

            Score = 0;
            Level = 1;

            Shoot = new WindowsMediaPlayer();
            Shoot.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\shoot.mp3";
            Shoot.settings.volume = 4;
            Shoot.controls.stop();

            Rip = new WindowsMediaPlayer();
            Rip.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\rip.mp3";
            Rip.settings.volume = 18;
            Rip.controls.stop();

            GameSong = new WindowsMediaPlayer();
            GameSong.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\GameSong.mp3";
            GameSong.settings.setMode("loop", true);
            GameSong.settings.volume = 15;

            HeavyBreathing = new WindowsMediaPlayer();
            HeavyBreathing.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\hb.mp3";
            HeavyBreathing.settings.volume = 25;
            HeavyBreathing.controls.stop();

            DeathSound = new WindowsMediaPlayer();
            DeathSound.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\deathsound.mp3";
            DeathSound.settings.volume = 15;
            DeathSound.controls.stop();

            WinSound = new WindowsMediaPlayer();
            WinSound.URL = "C:\\Users\\Michael\\Desktop\\Для игры\\winsound.mp3";
            WinSound.settings.volume = 8;
            WinSound.controls.stop();

            for (var i = 0; i < 3; i++) SpawnEnemy(firstEnemy, secondEnemy, thirdEnemy);

            SpawnClouds();

            SpawnGunSack(gunSackImage);

            GameSong.controls.play();

            UpdatePlayerPosition(false);
        }

        private void UpdateGunSackPosition(string id)
        {
            var gunSackModel = gunSacksModel[id];
            gunSacks[id].Left = (int)gunSackModel.Position.X;
            gunSacks[id].Top = (int)gunSackModel.Position.Y;
            gunSacks[id].Size = new Size(200, 200);
        }

        private void UpdatePlayerPosition(bool jumpFlag)
        {
            Vector playerPosition = playerModel.Position;
            mainPlayer.Left = (int)playerPosition.X;
            mainPlayer.Top = (int)playerPosition.Y;
            playerModel.jumpFlag = jumpFlag;
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

        private void RemoveGunSack(string id)
        {
            this.Controls.Remove(gunSacks[id]);
            gunSacks.Remove(id);
            gunSacksModel.Remove(id);
        }

        private void MakeDifficultySetting(Image firstEnemy, Image secondEnemy,
            Image thirdEnemy, EnemyModel enemyModel, PictureBox enemy)
        {
            if (Level > 0 && Level < 3)
                enemy.Image = firstEnemy;
            else
            {
                HeavyBreathing.controls.play();

                if (Level >= 3 && Level < 5)
                {
                    enemy.Image = secondEnemy;
                    enemyModel.enemySpeed++;
                    // playerModel.playerSpeed--;
                }

                if (Level >= 5 && Level < 7)
                {
                    enemy.Image = thirdEnemy;
                    enemyModel.enemySpeed += 2;
                    // playerModel.playerSpeed--;
                }
            }
        }

        private void SpawnGunSack(Image gunSackImage)
        {
            var id = Guid.NewGuid().ToString();
            var gunSackPosition = new Vector(700, 50);
            var gunSackModel = new GunSackModel(id: id, position: gunSackPosition);
            var gunSack = new PictureBox();
            gunSack.SizeMode = PictureBoxSizeMode.Zoom;
            gunSack.BackColor = Color.Transparent;
            gunSack.Image = gunSackImage;

            gunSacksModel[id] = gunSackModel;
            gunSacks[id] = gunSack;

            this.Controls.Add(gunSack);
        }

        private void SpawnEnemy(Image firstEnemy, Image secondEnemy, Image thirdEnemy)
        {
            var enemySize = rnd.Next(60, 90);
            var id = Guid.NewGuid().ToString();
            var enemyPosition = new Vector(rnd.Next(30, 300) + 1080, rnd.Next(250, 650));
            var enemyModel = new EnemyModel(id: id, position: enemyPosition,
                height: enemySize, width: enemySize);
            var enemy = new PictureBox();
            enemy.SizeMode = PictureBoxSizeMode.Zoom;
            enemy.BackColor = Color.Transparent;

            MakeDifficultySetting(firstEnemy, secondEnemy, thirdEnemy, enemyModel, enemy);

            enemiesModel[id] = enemyModel;
            enemies[id] = enemy;

            this.Controls.Add(enemy);
        }

        private void SpawnBullet(bool flag)
        {
            var id = Guid.NewGuid().ToString();
            var bulletPosition = new Vector(mainPlayer.Location.X + 200,
                                            mainPlayer.Location.Y + 150);
            var bulletModel = new BulletModel(id: id, position: bulletPosition, 
                limitRightSide: 1280, flag: flag);
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
                    color = Color.FromArgb(rnd.Next(70, 125), 255, 150, 255);
                }

                else
                {
                    height = 25;
                    width = 150;
                    color = Color.FromArgb(rnd.Next(50, 125), 255, 205, 205);
                }

                var cloudPosition = new Vector(rnd.Next(-1000, 1280), rnd.Next(150, 250));
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
                playerModel.Move(Directions.Left);
                UpdatePlayerPosition(false);
            }
        }

        /// <summary>
        /// Движение игрока вправо
        /// </summary>
        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Left < 950)
            {
                playerModel.Move(Directions.Right);
                UpdatePlayerPosition(false);
            }
        }

        /// <summary>
        /// Движение игрока вверх
        /// </summary>
        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top > 400)
            {
                playerModel.Move(Directions.Up);
                UpdatePlayerPosition(false);
            }
        }

        /// <summary>
        /// Движение игрока вниз
        /// </summary>
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (mainPlayer.Top < 570)
            {
                playerModel.Move(Directions.Down);
                UpdatePlayerPosition(false);
            }
        }

        /// <summary>
        /// Клавиша - движение + реализация выстрела
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.walk;
            if (e.KeyCode == Keys.Left) LeftMoveTimer.Start();
            if (e.KeyCode == Keys.Right) RightMoveTimer.Start();
            if (e.KeyCode == Keys.Up) UpMoveTimer.Start();
            if (e.KeyCode == Keys.Down) DownMoveTimer.Start();
            if (e.KeyCode == Keys.Space)
            {
                mainPlayer.Image = Properties.Resources.shoot;
                Shoot.controls.play();
                SpawnBullet(false);
            }
            if (e.KeyCode == Keys.F)
            {
                mainPlayer.Image = Properties.Resources.shoot;
                Shoot.controls.play();
                SpawnBullet(true);
            }
            if (e.KeyCode == Keys.G)
            {
                mainPlayer.Image = Properties.Resources.Jump;
                UpdatePlayerPosition(true);
            }
        }

        /// <summary>
        /// Если ничего не нажимаем
        /// </summary>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            mainPlayer.Image = Properties.Resources.Idle;
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
                SpawnEnemy(firstEnemy, secondEnemy, thirdEnemy);
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
                    DeathSound.controls.play();
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

                if (Level >= 4) HeavyBreathing.controls.play();

                if (Level == 7) 
                {
                    HeavyBreathing.controls.stop();
                    WinSound.controls.play();
                    GameOver("You Won");
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

        private void MoveGunSackTimer_Tick(object sender, EventArgs e)
        {
            var removeIds = new List<string>();
            foreach (KeyValuePair<string, GunSackModel> entry in gunSacksModel)
            {
                if (entry.Value.IsAlreadyCought)
                {
                    entry.Value.Move(Directions.Down);
                    UpdateGunSackPosition(entry.Key);
                }
                else removeIds.Add(entry.Key);
            }

            for (var i = 0; i < removeIds.Count; i++)
            {
                RemoveGunSack(removeIds[i]);
                SpawnGunSack(gunSackImage);
            }

            Intersect();
        }

        private void mainPlayer_Click(object sender, EventArgs e)
        {
            // there's nothing here
        }
    }
}