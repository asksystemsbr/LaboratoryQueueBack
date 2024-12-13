using System;
using System.Drawing;
using System.Drawing.Printing;

namespace PasswordPrinter.Utils
{
    public static class PrinterHelper
    {
        public static void ImprimirSenha(string numeroSenha, string tipoServico, DateTime horario)
        {
            try
            {
                var printFont = new Font("Courier New", 10);
                var texto = $"Senha: {numeroSenha}\nServiço: {tipoServico}\nEmitido: {horario:dd/MM/yyyy HH:mm:ss}\nObrigado por aguardar!";
                var pd = new PrintDocument();
                pd.PrintPage += (sender, ev) =>
                {
                    ev.Graphics.DrawString(texto, printFont, Brushes.Black, 10, 10);
                };
                pd.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao imprimir: {ex.Message}");
            }
        }
    }
}