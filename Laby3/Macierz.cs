using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laby3
{   internal class Macierz
    {
        public int [,]data {  get; set; }
        private int rows;
        private int columns;

        public Macierz(int size)
        {
            this.data = new int[size, size];
            rows = size;
            columns = size;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.data[i, j] = 0;
                }
            }
        }
        public void init()
        {
            Random random = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.data[i, j] = random.Next(1, 10);
                }
            }
        }
        public override string ToString()
        {
            string wynik="";
            for (int i = 0;i < rows; i++)
            {
                for(int j = 0;j < columns; j++)
                {
                    wynik = wynik + " " + data[i, j];
                    if (j == columns - 1)
                    {
                        wynik = wynik + Environment.NewLine;
                    }
                }
            }
            return wynik;
        }
        public Macierz mnozenie(Macierz other)
        {
            Macierz wynik = new Macierz(this.rows);
            for (int i=0; i<this.rows; i++)
            {
                for (int j=0; j < this.columns; j++)
                {
                    for (int k =0; k<this.rows; k++)
                    {
                        wynik.data[i,j] += this.data[i,k] * other.data[k,j];
                    }
                }
            }
            return wynik;
        }
        public void zeruj()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.data[i, j] = 0;
                }
            }
        }
    }
}
