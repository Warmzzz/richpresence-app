using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordRpcDemo;

namespace RichPresence
{
    public partial class Form1 : Form
    {
        public NotifyIcon notifyIcon;
        public BackgroundWorker bw;
        public DiscordRpc.EventHandlers handlers;
        public DiscordRpc.RichPresence presence;
        public string cbChecked;
        public SaveFileDialog SFD;
        string pathSave;
        public OpenFileDialog OFD;
        string pathOpen;
        int endNum;
        int startNum;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bw = new BackgroundWorker();
            bw.DoWork += (obs, ea) => RpcDiscordChange();
            bw.RunWorkerAsync();

            this.Hide();

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("discorde.ico");
            notifyIcon.Text = "RichPresence-App";

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Ouvrir", Image.FromFile("discorde.ico"), IconMenuOpen);
            notifyIcon.ContextMenuStrip.Items.Add("Quitter", Image.FromFile("discorde.ico"), IconMenuQuit);

            notifyIcon.Visible = true;
        }

        private void IconMenuQuit(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void IconMenuOpen(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tbDetails_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void RpcDiscordChange()
        {
            try
            {
                this.handlers = default(DiscordRpc.EventHandlers);
                if (String.IsNullOrWhiteSpace(tbClientID.Text))
                {
                    DiscordRpc.Initialize("897850326286221363", ref this.handlers, true, null);
                }
                else
                {
                    DiscordRpc.Initialize(tbClientID.Text, ref this.handlers, true, null);
                }

                if (!String.IsNullOrWhiteSpace(tbDetails.Text))
                {
                    this.presence.details = tbDetails.Text;
                }

                if (!String.IsNullOrWhiteSpace(tbState.Text))
                {
                    this.presence.state = tbState.Text;
                }

                if (!String.IsNullOrWhiteSpace(tbStartTimestamp.Text))
                {
                    try{ startNum = int.Parse(tbStartTimestamp.Text); }catch{}
                    this.presence.startTimestamp = startNum;
                }

                if (!String.IsNullOrWhiteSpace(tbEndTimestamp.Text))
                {
                    try { endNum = int.Parse(tbEndTimestamp.Text); }catch{}
                    this.presence.endTimestamp = endNum;
                }

                if (checkBox1.Checked)
                {
                    if (String.IsNullOrWhiteSpace(tbImage.Text))
                    {
                        this.presence.largeImageKey = tbImage.Text;
                        this.presence.largeImageText = tbImage.Text;
                    }
                    else
                    {
                        this.presence.largeImageKey = "discorde";
                        this.presence.largeImageText = "RichPresence-App";
                    }
                }

                DiscordRpc.UpdatePresence(ref this.presence);
                MessageBox.Show(text:"Votre Rpc Discord a été changé avec succès !",caption:"Tout va bien", buttons: MessageBoxButtons.OK);
            }
            catch (Exception e)
            {
                MessageBox.Show(text:$"Une erreur s'est produite lors du changement de votre Rpc Discord !\n\n{e}", caption: "Une erreur s'est produite", buttons:MessageBoxButtons.OK, icon:MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                if (checkBox1.Checked)
                {
                    cbChecked = "True";
                }
                else
                {
                    cbChecked = "False";
                }

                string textSave =
                    $"{tbDetails.Text}|{tbState.Text}|{tbStartTimestamp.Text}|{tbEndTimestamp.Text}|{tbClientID.Text}|{tbImage.Text}|{cbChecked}";
                SFD = new SaveFileDialog();
                SFD.FileName = "RichPresenceApp.txt";
                SFD.Filter = "Fichier texte (*.txt)|*.txt";
                DialogResult dr = SFD.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    pathSave = SFD.FileName;
                }

                File.WriteAllText(pathSave, textSave);
            }
            catch
            {
                MessageBox.Show(text: "Une erreur s'est produite lors de la sauvegarde du fichier !", caption: "Une erreur s'est produite", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
                this.Close();
            }
            catch
            {
                return;
            }
        }

        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] textOpen;
                OFD = new OpenFileDialog();
                OFD.Filter = "Fichier texte (*.txt)|*.txt";
                DialogResult dialogResult = OFD.ShowDialog();
                if(dialogResult == DialogResult.OK){pathOpen = OFD.FileName;}
                string textO = File.ReadAllText(pathOpen);
                textOpen = textO.Split('|');
                tbDetails.Text = textOpen[0];
                tbState.Text = textOpen[1];
                tbStartTimestamp.Text = textOpen[2];
                tbEndTimestamp.Text = textOpen[3];
                tbClientID.Text = textOpen[4];
                tbImage.Text = textOpen[5];
                if (textOpen[6] == "True")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
            catch
            {
                MessageBox.Show(text: "Une erreur s'est produite lors de l'ouverture de fichier de sauvegarde !", caption: "Une erreur s'est produite", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }
        }
    }
}
