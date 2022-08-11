using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BaiTapLon
{
    public partial class PlayGame : Form
    {
        public static WMPLib.WindowsMediaPlayer MusicPlay = new WMPLib.WindowsMediaPlayer();

        public PlayGame()
        {
            Music();
            Login f = new Login();
            f.ShowDialog();
            InitializeComponent();
        }

        //Âm thanh
        public static bool bMusic = true;
        private void Music()
        {
            MusicPlay.URL = Application.StartupPath.ToString() + "\\Music.mp3";
            MusicPlay.controls.play();
        }

        //Load form
        private void PlayGame_Load(object sender, EventArgs e)
        {
            HienThiNguoiChoi();
            LoadGame();
        }

        //Label đếm ngược trò chơi
        Label lbStart = new Label()
        {
            BackColor = Color.BurlyWood,
            AutoSize = false,
            ForeColor = Color.DarkRed,
            Font = new Font("Palatino Linotype", 30, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(75, 175),
            Size = new Size(300, 100),
        };

        //Panel luật chơi
        Panel pnlLuatChoi = new Panel();
        string law = "- Người chơi bên trái sẽ là người đi trước, đánh vào vị trí bất kỳ trên bàn cờ.\n " +
            "- Khi tới lượt: người chơi phải đánh vào một ô trên bàn cờ. Đánh đủ 5 ô trở lên theo chiều dọc, ngang hoặc theo đường chéo mà không bị đối phương chặn 2 đầu thì sẽ là người thắng cuộc.\n" +
            "- Mỗi người chơi có 30s để suy nghĩ và đánh. Sau 30s đó người chơi không đánh vào ô sẽ bị xử thua.\n" +
            "\n *Chúc anh em xã hội chơi vui vẻ*";

        // Load Game
        private void LoadGame()
        {
            lbLaw.Text = law;
            pnlLuatChoi = pnlLaw;
            pnlLuatChoi.Visible = false;
            pnlChess.Controls.Clear();
            pnlChess.Controls.Add(pnlLuatChoi);
            pnlChess.Controls.Add(lbStart);
            VeBanCo();
            count = 5;
            TimeLoadGame.Enabled = true;
            Nguoi2();
            pic4.Visible = true;
            setUpProgressBar();
            bTrue = true;
        }

        public List<List<Button>> MaTran { get; set; }

        //Vẽ bàn cờ
        public const int size = 16;
        void VeBanCo()
        {
            MaTran = new List<List<Button>>();
            
            Button bt = new Button()
            { Width = 0, Location = new Point(0, 0) };

            for (int i = 0; i < size; i++)
            {
                MaTran.Add(new List<Button>());
                for (int j = 0; j < size; j++)
                {
                    Button btnew = new Button()
                    {
                        Width = 30,
                        Height = 30,
                        Location = new Point(bt.Location.X + bt.Width, bt.Location.Y),
                        Tag = i.ToString(),
                        Font = new Font("Poor Richard", 16, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.WhiteSmoke,
                    };
                    btnew.Click += btn_Click;
                    pnlChess.Controls.Add(btnew);
                    MaTran[i].Add(btnew);
                    bt = btnew;
                }
                bt.Location = new Point(0, bt.Location.Y + 30);
                bt.Width = 0;
            }
            
            //Set up 
            lbStart.Visible = true;
            lbStart.Text = "";
        }

        private Point NhanDiem(Button bt)
        {

            //Tọa độ
            int doc = Convert.ToInt32(bt.Tag);
            int ngang = MaTran[doc].IndexOf(bt);

            Point point = new Point(ngang, doc);

            return point;
        }

        //Kiểm tra xem game đã kết thúc hay chưa
        private bool KTKetThuc(Button bt)
        {
            return KTKetThucNgang(bt) || KTKetThucDoc(bt) || KTKetThucCheoTrenXuong(bt) || KTKetThucCheoDuoiLen(bt);
        }


        //Thắng ngang
        private bool KTKetThucNgang(Button bt)
        {
            Point point = NhanDiem(bt);
            //Đếm bên trái
            int coutLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (MaTran[point.Y][i].Text == bt.Text)
                    coutLeft++;
               else
                    break;
                

            }
            //Đếm bên phải
            int coutRight = 0;
            for (int i = point.X + 1; i < size; i++)
            {
                if (MaTran[point.Y][i].Text == bt.Text)
                    coutRight++;
                else 
                    break;
                
            }
            //Kiểm tra có bị chặn 2 đầu hay không và tô màu
            if (coutLeft + coutRight >= 5)
            {
                try
                {
                    if (MaTran[point.Y][point.X - coutLeft].Text != "" && MaTran[point.Y][point.X + coutRight + 1].Text != "")
                        return false;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                }
                int temp = coutLeft - 1;
                for (int i = 0; i < coutLeft + coutRight; i++)
                    MaTran[point.Y][point.X - temp--].BackColor = Color.PaleGoldenrod;
                return true;

            }  
            return false;
        }
        //Thắng dọc
        private bool KTKetThucDoc(Button bt)
        {
            Point point = NhanDiem(bt);
            //Đếm bên trên
            int coutTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (MaTran[i][point.X].Text == bt.Text)
                    coutTop++;
                else
                    break;

            }
            //Đếm bên dưới
            int coutBottom = 0;
            for (int i = point.Y + 1; i < size; i++)
            {
                if (MaTran[i][point.X].Text == bt.Text)
                    coutBottom++;
                else
                    break;
            }
            //Kiểm tra có bị chặn 2 đầu hay không và tô màu
            if (coutTop + coutBottom >= 5)
            {
                try
                {
                    if (MaTran[point.Y - coutTop][point.X].Text != "" && MaTran[point.Y + coutBottom + 1][point.X].Text != "")
                        return false;
                }     
                catch (System.ArgumentOutOfRangeException)
                {
                }
                int temp = coutTop - 1;
                for (int i = 0; i < coutTop + coutBottom; i++)
                    MaTran[point.Y - temp--][point.X].BackColor = Color.PaleGoldenrod;
                return true;

            }
            return false;
        }
        //Thắng chéo
        private bool KTKetThucCheoTrenXuong(Button bt)
        {
            Point point = NhanDiem(bt);
            //Đếm bên chéo trên
            int coutTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.Y - i < 0 || point.X - i < 0)
                    break;
                if (MaTran[point.Y - i][point.X - i].Text == bt.Text)
                    coutTop++;
                else
                    break;
            }
            //Đếm bên chéo dưới
            int coutBottom = 0;
            for (int i = 1; i <= size - point.X; i++)
            {
                if (point.Y + i >= size || point.X + i >= size)
                    break;
                if (MaTran[point.Y + i][point.X + i].Text == bt.Text)
                    coutBottom++;
                else
                    break;
            }
            //Kiểm tra có bị chặn 2 đầu hay không và tô màu
            if (coutTop + coutBottom >= 5)
            {
                try
                {
                    if (MaTran[point.Y - coutTop][point.X - coutTop].Text != "" && MaTran[point.Y + coutBottom + 1][point.X + coutBottom + 1].Text != "")
                        return false;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                }
                int temp = coutTop - 1;
                for (int i = 0; i < coutTop + coutBottom; i++)
                {
                    MaTran[point.Y - temp][point.X - temp].BackColor = Color.PaleGoldenrod;
                    temp--;
                }
                return true;

            }
            return false;
        }
        private bool KTKetThucCheoDuoiLen(Button bt)
        {
            Point point = NhanDiem(bt);
            //Đếm bên chéo trên
            int coutTop = 0;
            for (int i = 0; i <= size; i++)
            {
                if (point.X + i > size || point.Y - i < 0)
                    break;
                if (MaTran[point.Y - i][point.X + i].Text == bt.Text)
                    coutTop++;
                else
                    break;
            }
            //Đếm bên chéo dưới
            int coutBottom = 0;
            for (int i = 1; i <= size; i++)
            {
                if (point.Y + i >= size || point.X - i < 0)
                    break;
                if (MaTran[point.Y + i][point.X - i].Text == bt.Text)
                    coutBottom++;
                else
                    break;
            }
            if (coutTop + coutBottom >= 5)
            {
                try
                {
                    if (MaTran[point.Y - coutTop][point.X + coutTop].Text != "" && MaTran[point.Y - coutBottom - 1][point.X + coutBottom + 1].Text != "")
                        return false;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                }
                int temp1 = coutTop - 1;
                for (int i = 0; i < coutTop + coutBottom; i++)
                {
                    MaTran[point.Y - temp1][point.X + temp1].BackColor = Color.PaleGoldenrod;
                    temp1--;
                }
                return true;

            }
            return false;
            
        }

        //Kết thúc trò chơi
        private void KetThuc()
        {
            undoToolStripMenuItem.Enabled = false;
            string people;
            if (NguoiDangChoi == true) 
            {
                people = p2.HoTen;
                p2.Diem++;
            }
            else
            {
                people = p1.HoTen;
                p1.Diem++;
            }
            timePlayer.Enabled = false;
            timer1.Enabled = false;
            game = false;
            MessageBox.Show("Chúc mừng " + people + " đã giành chiến thắng");
            lbScore1.Text = "Số điểm: " + p1.Diem;
            lbScore2.Text = "Số điểm: " + p2.Diem;
            
        }

        //Đổi người chơi
        public bool NguoiDangChoi = true;
        public void Nguoi1() 
        {
            pic1.Visible = false;
            picLoad1.Visible = false;
            pic2.Visible = true;
            picLoad2.Visible = true;
            pic3.Visible = true;
            pic4.Visible = false;
            NguoiDangChoi = false;
        }
        public void Nguoi2()
        {
            pic1.Visible = true;
            picLoad1.Visible = true;
            pic2.Visible = false;
            picLoad2.Visible = false;
            pic3.Visible = false;
            pic4.Visible = true;
            NguoiDangChoi = true;
        }
        private void DoiNguoiChoi(Button btClick)
        {
            t = 300;
            if (NguoiDangChoi == true)
            {
                btClick.Text = "X";
                btClick.ForeColor = Color.Red;

                Nguoi1();
            }
            else
            {
                btClick.Text = "O";
                btClick.ForeColor = Color.Blue;

                Nguoi2();
            }
            setUpProgressBar();
        }

        //Kiểm tra game đã đc bắt đầu hay chưa
        private bool game = false;

        //Click vào bàn cờ       
        private void btn_Click(object sender, EventArgs e)
        {
            Button btClick = (Button)sender;
            PlayTimeLine.Push(NhanDiem(btClick));
            if (btClick.Text == ""&&game==true)
            {
                DoiNguoiChoi(btClick);
                if (KTKetThuc(btClick))
                    KetThuc();
            }
            
        }

        //Người chơi
        NguoiChoi p1, p2;
        private void Player()
        {
            p1 = new NguoiChoi(Login.name1,Login.img1);
            p2 = new NguoiChoi(Login.name2, Login.img2);
        }

        //Hiện thông tin 2 người chơi
        private void HienThiNguoiChoi()
        {
            Player();
            lbName1.Text = p1.HoTen;
            picPlayer1.Image = p1.HinhAnh;
            lbName2.Text = p2.HoTen;
            picPlayer2.Image = p2.HinhAnh;
            
        }

        //khai báo progressBar
        private void setUpProgressBar()
        {
            progressBar1.Step = progressBar2.Step = 100;
            progressBar1.Maximum = progressBar2.Maximum = 30000;
            progressBar1.Value = progressBar2.Value = 0;
        }

        //Đếm ngược thời gian vào game
        private int count;
        private void TimeLoadGame_Tick(object sender, EventArgs e)
        {
            int term;
            if (count > 1&& count< 5)
            {
                term = count - 1;
                lbStart.Text = term.ToString();
            }
            if (count == 1)

                lbStart.Text = "Start";
            if (count == 0)
            {
                lbStart.Visible = false;
                timePlayer.Enabled = true;
                game = true;
                undoToolStripMenuItem.Enabled = true;
            }
            count--;
        
        }

        //Thời gian mỗi lần đánh của người chơi
        private int t=300;
        private void timePlayer_Tick(object sender, EventArgs e)
        {
            if (t == 0)
            {
                KetThuc();
            }
            if (NguoiDangChoi==true)
                progressBar1.PerformStep();
            else
                progressBar2.PerformStep();
            t--;
        }

        //Xoay vòng emoji suy nghĩ
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (game)
            {
                timer1.Interval = 150;
                Image img1 = picLoad1.Image;
                Image img2 = picLoad2.Image;
                img1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                img2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                picLoad1.Image = img1;
                picLoad2.Image = img2;
            }
        }

        //Chạy banner trong luật chơi
        private void timerInfo_Tick(object sender, EventArgs e)
        {
            lbInfo.Text = lbInfo.Text.Substring(1) + lbInfo.Text.Substring(0, 1);
        }

        //Đầu hàng
        private void btQuit1_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if(game)
            {
                string people;
                if (bt.Name == "btQuit1")
                {
                    people = p2.HoTen;
                    p2.Diem++;
                }
                else
                {
                    people = p1.HoTen;
                    p1.Diem++;
                }
                timePlayer.Enabled = false;
                timer1.Enabled = false;
                game = false;
                MessageBox.Show("Chúc mừng " + people + " đã giành chiến thắng");
                lbScore1.Text = "Số điểm: " + p1.Diem;
                lbScore2.Text = "Số điểm: " + p2.Diem;
            }
        }

        //Quay trở về home
        private bool bTrue;
        private void backHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            bTrue = false;
            var result = MessageBox.Show("Bạn có muốn Back Home hay không???\n Bấm Yes để thực hiện", "Back Home", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                System.Windows.Forms.Application.Restart();
            else
                Start();
        }

        //Game mới
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            var result = MessageBox.Show("Bạn có muốn tạo game mới???", "Reset Game", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                LoadGame();
            else
                Start();
            undoToolStripMenuItem.Enabled = false;
        }

        //Dừng trò chơi
        private void Stop()
        {
            if (!game)
                TimeLoadGame.Enabled = false;
            else
            {
                timePlayer.Enabled = false;
                timer1.Enabled = false;
            }
        }
        private void Start()
        {
            if (!game)
                TimeLoadGame.Enabled = true;
            else
            {
                timePlayer.Enabled = true;
                timer1.Enabled = true;
            }
        }
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            var result = MessageBox.Show("Bấm \"Ok\" hoặc \"X\" để tiếp tục trò chơi!!!", "Pause Game");
            Start();
        }



        //Hoàn tác
        public Stack<Point> PlayTimeLine=new Stack<Point>();
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PlayTimeLine.Count <= 0)
                return;

            Point oldPoint = PlayTimeLine.Pop();
            Button btn = MaTran[oldPoint.Y][oldPoint.X];
            btn.Text = "";
            t = 300;
            if (NguoiDangChoi == true)
            {
                Nguoi1();
            }
            else
            {
                Nguoi2();
            }
            setUpProgressBar();
        }

        //Thoát trò chơi
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void PlayGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bTrue)
            {
                Stop();
                var result = MessageBox.Show("Bạn có muốn thoát trò chơi không???", "Exit Game", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    Start();
                }
            }     
        }

        //Âm thanh
        private void musicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            if (bMusic)
            {
                var result = MessageBox.Show("Bạn có muốn tắt âm thanh không???", "Music", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    bMusic = false;
                    MusicPlay.controls.stop();
                }
                
            }
            else
            {
                var result = MessageBox.Show("Bạn có muốn bật âm thanh không???", "Music", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    bMusic = true;
                    MusicPlay.controls.play();
                }              
            }
            Start();
        }


        //Giúp đỡ
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if(pnlLuatChoi.Visible == false)
            {
                pnlLuatChoi.Visible = true;
                Stop();
            }
            else
            {
                pnlLuatChoi.Visible = false;
                Start();
            }
                
        }
    }
}
