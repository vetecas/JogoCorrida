using JogoCorrida;
using System.Media;



namespace JogoCorridaWinFormsApp
{
    public partial class FormJogoCorrida : Form
    {
        Jogo jogo;
        DateTime tempoUltimaMovimentaca = DateTime.Now;
        List<PictureBox> pictureBoxes = [];
        int pontuacao = 0;
        SoundPlayer? sp;

        public FormJogoCorrida(String Nivel)
        {
            InitializeComponent();
            this.FormClosing += FormJogoCorrida_FormClosing;
            SomJogoCorrida();

            // Faz parar de piscar
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            jogo = new Jogo
            {
                Faixa1Inicio = 129,
                Faixa1Fim = 198,
                Faixa2Inicio = 258,
                Faixa2Fim = 325
            };

            jogo.YMax = 696;
            jogo.IniciarJogo();
            jogo.Carro.PosicaoX = jogo.PosicionaObjeto(1);
            //jogo.Velociade = 50;

            if (Nivel == "Facil")
            {
                jogo.Velociade = 60;

            }
            else if (Nivel == "Medio")
            {
                jogo.Velociade = 40;
            }
            else
            {
                jogo.Velociade = 20;
            }

            foreach (var ob in jogo.Obstaculos) // CRIANDO OBSTACULOS
            {
                var picOb = new PictureBox();
                picOb.BackColor = Color.Transparent;
                picOb.BackgroundImage = Properties.Resources.Carro_Obstaculo_Laranja;
                picOb.BackgroundImageLayout = ImageLayout.Stretch;
                picOb.Size = new Size(PicCarro.Width, PicCarro.Height);
                pictureBoxes.Add(picOb); // ADICIONA A IMAGEM NA LISTA INTERNA
                this.Controls.Add(picOb); // ADICIONA A IMAGEM NO FORMULARIO
            }
            TimerJogo.Enabled = true;
        }

        private void FormJogoCorrida_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {

                jogo.Carro.PosicaoX = jogo.PosicionaObjeto(1);
            }

            if (e.KeyCode == Keys.Right)
            {

                jogo.Carro.PosicaoX = jogo.PosicionaObjeto(2);
            }
        }

        private void TimerJogo_Tick(object sender, EventArgs e)
        {
            PicCarro.Location = new Point(jogo.Carro.PosicaoX, jogo.Carro.PosicaoY);
            var i = 0;

            foreach (var ob in jogo.Obstaculos)
            {
                if (i < pictureBoxes.Count)
                {
                    pictureBoxes[i].Visible = true; // PROBLEMA NO CARRO DO LADO ESQUERDO TRAVADO
                    pictureBoxes[i].Location = new Point(ob.PosicaoX, ob.PosicaoY);
                }
                else
                {
                    pictureBoxes[i].Visible = false;
                }

                i++;
            }

            if ((DateTime.Now - tempoUltimaMovimentaca).TotalMilliseconds >= jogo.Velociade)
            {
                tempoUltimaMovimentaca = DateTime.Now;
                jogo.MovimentaObstaculos();
                pontuacao += 1;
                lblPontuacao.Text = $"Pontos: {pontuacao}";
            }

            if (jogo.ChecarColisao())
            {
                GamerOver();
                Application.Exit();
            }

        }

        private void GamerOver()
        {
            TimerJogo.Enabled = false;
            TocarSomBatida();
            MessageBox.Show($"Fim de Jogo! Pontuação final: {pontuacao}", "Game Over");
            Close();
        }

        private void SomJogoCorrida()
        {
            sp = new SoundPlayer();
            sp.SoundLocation = Path.Combine(Application.StartupPath, "Resources", "somMotorCarro.wav");
            sp.PlayLooping();

        }

        private void TocarSomBatida()
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = Path.Combine(Application.StartupPath, "Resources", "somBatidaCarro.wav");
            sp.Play();
            Thread.Sleep(1000);
        }

        private void FormJogoCorrida_FormClosing(object? sender, FormClosingEventArgs e) // encerra o jogo quando fechar a janela FormJogo
        {
            // Para o Timer imediatamente para parar colisões e som
            if (TimerJogo != null)
            {
                TimerJogo.Stop();
                TimerJogo.Enabled = false;
            }
            sp?.Stop();
        }

    }

}


