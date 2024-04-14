namespace Laby3
{
    public partial class Form1 : Form
    {
        int size;
        int liczba_watkow;
        int rpt;
        volatile Macierz wynik_t;
        volatile Macierz wynik_p;
        Macierz mac1;
        Macierz mac2;
        Thread[] watki;
        public Form1()
        {
            InitializeComponent();
            size = 100;
            liczba_watkow = 4;
            rpt = (size + liczba_watkow - 1) / liczba_watkow;
            wynik_t = new Macierz(size);
            wynik_p = new Macierz(size);
            mac1 = new Macierz(size);
            mac2 = new Macierz(size);
            mac1.init(); mac2.init();
            textBox3.Text = size.ToString();
            watki = new Thread[liczba_watkow];
        }
        void calculate(object threadIndexObj)
        {
            int thread_id = (int)threadIndexObj;
            int start_row = thread_id * rpt;
            int end_row = (thread_id == liczba_watkow - 1) ? size : start_row + rpt;
            for (int i = start_row; i < end_row; i++)
            {
                if (start_row >= size)
                {
                    break;
                }
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        wynik_t.data[i, j] += mac1.data[i, k] * mac2.data[k, j];
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wynik_t.zeruj();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < liczba_watkow; i++)
            {
                watki[i] = new Thread(calculate);
                watki[i].Start(i);
            }
            foreach (Thread thread in watki)
            {
                thread.Join();
            }
            watch.Stop();
            textBox1.Text = watch.Elapsed.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wynik_p.zeruj();

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = liczba_watkow;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Parallel.For(0, size, options, i =>
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        wynik_p.data[i, j] += mac1.data[i, k] * mac2.data[k, j];
                    }
                }
            });
            watch.Stop(); textBox2.Text = watch.Elapsed.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mac1.init();
            mac2.init();
        }
    }
}
