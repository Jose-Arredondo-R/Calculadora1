using System;
using System.Drawing;
using System.Windows.Forms;

public class FormCalculadora : Form
{
    private TextBox pantalla;
    private double acumulado = 0;
    private string operador = null;
    private bool nuevoNumero = true;

    public FormCalculadora()
    {
        Text = "Calculadora";
        ClientSize = new Size(332, 392);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        pantalla = new TextBox
        {
            Left = 6,
            Top = 10,
            Width = 318,
            Font = new Font("Segoe UI", 28),
            TextAlign = HorizontalAlignment.Right,
            ReadOnly = true,
            Text = "0"
        };
        Controls.Add(pantalla);

        AgregarBoton("C", 0, 0, 2);
        AgregarBoton("⌫", 0, 2, 1);
        AgregarBoton("÷", 0, 3, 1);

        AgregarBoton("7", 1, 0, 1);
        AgregarBoton("8", 1, 1, 1);
        AgregarBoton("9", 1, 2, 1);
        AgregarBoton("×", 1, 3, 1);

        AgregarBoton("4", 2, 0, 1);
        AgregarBoton("5", 2, 1, 1);
        AgregarBoton("6", 2, 2, 1);
        AgregarBoton("-", 2, 3, 1);

        AgregarBoton("1", 3, 0, 1);
        AgregarBoton("2", 3, 1, 1);
        AgregarBoton("3", 3, 2, 1);
        AgregarBoton("+", 3, 3, 1);

        AgregarBoton("0", 4, 0, 2);
        AgregarBoton(".", 4, 2, 1);
        AgregarBoton("=", 4, 3, 1);
    }

    private void AgregarBoton(string texto, int fila, int columna, int span)
    {
        Button boton = new Button
        {
            Text = texto,
            Left = 6 + columna * 80,
            Top = 80 + fila * 60,
            Width = span * 80 - 2,
            Height = 58,
            Font = new Font("Segoe UI", 14)
        };
        boton.Click += (s, e) => PresionarBoton(texto);
        Controls.Add(boton);
    }

    private void PresionarBoton(string texto)
    {
        if (char.IsDigit(texto[0]))
            EscribirDigito(texto);
        else if (texto == ".")
            EscribirPunto();
        else if (texto == "C")
            Limpiar();
        else if (texto == "⌫")
            Borrar();
        else if (texto == "=")
            Igual();
        else
            ElegirOperador(texto);
    }

    private void EscribirDigito(string digito)
    {
        if (nuevoNumero || pantalla.Text == "0")
            pantalla.Text = digito;
        else
            pantalla.Text += digito;
        nuevoNumero = false;
    }

    private void EscribirPunto()
    {
        if (nuevoNumero)
        {
            pantalla.Text = "0.";
            nuevoNumero = false;
        }
        else if (!pantalla.Text.Contains("."))
        {
            pantalla.Text += ".";
        }
    }

    private void Borrar()
    {
        if (nuevoNumero) return;
        string t = pantalla.Text;
        t = t.Length > 1 ? t.Substring(0, t.Length - 1) : "0";
        if (t == "-") t = "0";
        pantalla.Text = t;
    }

    private void Limpiar()
    {
        acumulado = 0;
        operador = null;
        nuevoNumero = true;
        pantalla.Text = "0";
    }

    private void ElegirOperador(string nuevoOperador)
    {
        if (operador != null && !nuevoNumero)
        {
            if (!Calcular()) return;
        }
        else
        {
            acumulado = double.Parse(pantalla.Text);
        }
        operador = nuevoOperador;
        nuevoNumero = true;
    }

    private void Igual()
    {
        if (operador == null) return;
        if (Calcular())
        {
            operador = null;
            nuevoNumero = true;
        }
    }

    private bool Calcular()
    {
        double b = double.Parse(pantalla.Text);

        switch (operador)
        {
            case "+": acumulado += b; break;
            case "-": acumulado -= b; break;
            case "×": acumulado *= b; break;
            case "÷":
                if (b == 0)
                {
                    Limpiar();
                    pantalla.Text = "No se puede dividir entre 0";
                    return false;
                }
                acumulado /= b;
                break;
        }

        pantalla.Text = acumulado.ToString();
        return true;
    }
}

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new FormCalculadora());
    }
}
