﻿using System;
using System.Windows.Forms;

namespace ThreePM
{
    public partial class DeveloperForm : BaseForm
    {
        public DeveloperForm()
        {
            InitializeComponent();
        }

        protected override void InitPlayer()
        {
            this.Player.SongOpened += new EventHandler<ThreePM.MusicPlayer.SongEventArgs>(Player_SongOpened);
        }

        private void Player_SongOpened(object sender, ThreePM.MusicPlayer.SongEventArgs e)
        {
            if (albumPanel1.DataSource != null)
            {
                albumPanel1.SelectedItem = e.Song;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "A")
            {
                albumPanel1.BringToFront();
                albumPanel1.DataSource = this.Library.GetAlbumsAsEntries();
            }
            else
            {
                try
                {
                    dataGridView1.DataSource = this.Library.GetDataSet(textBox1.Text).Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Oops");
                }
            }
        }
    }
}
