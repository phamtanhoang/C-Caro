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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            ToolTipImg();
        }

        private void ToolTipImg()
        {
            string txt = "Chọn ảnh từ máy tính của bạn";
            ToolTip tip = new ToolTip();
            tip.SetToolTip(btFile1, txt);
            tip.SetToolTip(btFile2, txt);
        }

        //Không cho nhấn Alt+F4
        private bool AltF4 = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.F4))
                return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AltF4 == true)
                e.Cancel = true;
        }

        //Thông tin 2 người 
        public static Image img1, img2;
        public static string name1,name2;
        private void btFile1_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                if (bt.Name == "btFile1")
                {
                    try
                    {
                        tbPic1.Text = open.FileName;
                        img1 = Image.FromFile(open.FileName);
                    }catch(System.OutOfMemoryException)
                    {
                        MessageBox.Show("File ảnh của người chơi 1 không hợp lệ!!!");
                        tbPic1.Text = "";
                    }
                        
                }
                if (bt.Name == "btFile2")
                {
                    try
                    {
                        tbPic2.Text = open.FileName;
                        img2 = Image.FromFile(open.FileName);
                    }
                    catch (System.OutOfMemoryException)
                    {
                        MessageBox.Show("File ảnh của người chơi 1 không hợp lệ!!!");
                        tbPic2.Text = "";
                    }
                    
                }
            }
        }

        //Click Music
        private void btMusic_Click(object sender, EventArgs e)
        {
            if (PlayGame.bMusic)
            {
                btMusic.Text = "Âm thanh: Tắt";
                PlayGame.bMusic = false;
                PlayGame.MusicPlay.controls.stop();
            }
            else
            {
                btMusic.Text = "Âm thanh: Bật";
                PlayGame.bMusic = true;
                PlayGame.MusicPlay.controls.play();
            }
        }

        
        //Click Start
        Random rd = new Random();
        private void btStart_Click(object sender, EventArgs e)
        {
            if (tbName1.Text != "" && tbName2.Text != "")
            {
                AltF4 = false;
                name1 = tbName1.Text;
                name2 = tbName2.Text;
                int a = rd.Next(1, 6);
                int b = a;
                while (b == a)
                    b = rd.Next(1, 6);
                if (tbPic1.Text == "")
                    img1 = Image.FromFile(Application.StartupPath + @"\Player\" + a.ToString() + ".png");
                if (tbPic2.Text == "")
                    img2 = Image.FromFile(Application.StartupPath + @"\Player\" + b.ToString() + ".png");
                this.Close();
            }
            else
                MessageBox.Show("Vui lòng nhập tên hai người chơi", "Thông Báo");

        }
        //Click Exit
        private void button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn thoát trò chơi không???", "Exit Game", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                AltF4 = false;
                Application.Exit();
            }
                
        }

        
    }
}
