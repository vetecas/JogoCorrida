using JogoCorrida;
class Progran
{
    static void Main()
    {
        Jogo jogo = new Jogo {
        
            Faixa1Inicio = 1,
            Faixa1Fim = 11,
            Faixa2Inicio = 13,
            Faixa2Fim = 22
        };

        jogo.YMax = 11;
        jogo.IniciarJogo();
        jogo.Velociade = 800;
        var tempoUltimaMovimentacao = DateTime.Now;

        jogo.Carro.PosicaoX = jogo.PosicionaObjeto(1);
        jogo.Carro.PosicaoY = 10;
            
        for (; ; ) {

            DesenharElemento(jogo.Carro.PosicaoY, jogo.Carro.PosicaoX, '8');

            foreach (var ob in jogo.Obstaculos)
            {
                if (ob.PosicaoY >= 0)
                {
                    DesenharElemento(ob.PosicaoY, ob.PosicaoX, '0');
                }
            }
            
            if((DateTime.Now - tempoUltimaMovimentacao).Microseconds >= jogo.Velociade){
                tempoUltimaMovimentacao = DateTime.Now;
                jogo.MovimentaObstaculos();
            }

            if (Console.KeyAvailable)
            {
                var tecla = Console.ReadKey();

                if(tecla.Key == ConsoleKey.LeftArrow)
                {
                    jogo.Carro.PosicaoX = jogo.PosicionaObjeto(1);

                } else if (tecla.Key == ConsoleKey.RightArrow)
                {
                    jogo.Carro.PosicaoX = jogo.PosicionaObjeto(2);
                }

                TocarSom();
            }

            if (jogo.ChecarColisao())
            {
                GamerOver();
                TocarSom();
                break;
            }

            Thread.Sleep(200);
        }
    }

    public static void DesenharElemento(int linha, int coluna, char simbolo)
    {
        var xOriginal = Console.CursorLeft;
        var yOriginal = Console.CursorTop;
        Console.SetCursorPosition(coluna, linha);
        Console.Write(simbolo.ToString());
        Console.SetCursorPosition(xOriginal, yOriginal);

    }

    public static void GamerOver()
    {
        Console.Clear();
        Console.WriteLine("GAMER OVER");

    }

    static void TocarSom()
    {
        Console.Beep();
    }

}