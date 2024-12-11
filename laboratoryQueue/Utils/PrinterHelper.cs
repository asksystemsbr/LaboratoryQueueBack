using System;
using System.Drawing;
using System.Drawing.Printing;

public static class PrinterHelper
{
    private static Font printFont;
    private static string texto = string.Empty;
    private static string[] linhas = null;
    private static int posicaoLinha = 0;

    public static void ImprimirSenha(string numeroSenha, string tipoServico, DateTime horario)
    {
        try
        {
            int tamanhoFonte = 10; // Tamanho da fonte padrão
            string cabecalho = "SISTEMA DE SENHAS\n";
            string rodape = "Obrigado por aguardar!";

            texto = $"{cabecalho}\n";
            texto += $"Senha: {numeroSenha}\n";
            texto += $"Serviço: {tipoServico}\n";
            texto += $"Emitido: {horario:dd/MM/yyyy HH:mm:ss}\n";
            texto += $"{rodape}";

            linhas = texto.Split('\n');
            for (int i = 0; i < linhas.Length; i++)
            {
                linhas[i] += " \n ";
            }

            posicaoLinha = 0;

            printFont = new Font("Courier New", tamanhoFonte); // Fonte monoespaçada para alinhamento
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            pd.Print();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na impressão: {ex.Message}");
        }
    }

    private static void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
        float linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
        float yPos = 10; // Margem superior
        float leftMargin = 5; // Margem esquerda
        int count = 0;

        try
        {
            while (count < linesPerPage && (posicaoLinha < linhas.Length))
            {
                string line = linhas[posicaoLinha];
                yPos += printFont.GetHeight(ev.Graphics);
                ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
                posicaoLinha++;
            }

            ev.HasMorePages = posicaoLinha < linhas.Length;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao renderizar página: {ex.Message}");
        }
    }
}
