using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_tac_toe_game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Переменные
        Graphics gr;
        Bitmap bmap;
        int[,] Matrix = new int[3, 3];

        bool flagfigure = false;
        bool flagmove = true; // false - ходит робот, true - человек
        struct Alternative
        {
            public int appraisal;
            public int[,] map;
        }
        List<Alternative> Alternativs = new List<Alternative>();
        bool flag_clipping = false;
        #endregion

        #region Методы класса

        // прорисовка доски
        private Bitmap Field(PictureBox pictbox)
        {
            bmap = new Bitmap(pictbox.Width, pictbox.Height);
            gr = Graphics.FromImage(bmap);
            gr.DrawLine(new Pen(Color.Black), 0, 100, 300, 100);
            gr.DrawLine(new Pen(Color.Black), 0, 200, 300, 200);
            gr.DrawLine(new Pen(Color.Black), 100, 0, 100, 300);
            gr.DrawLine(new Pen(Color.Black), 200, 0, 200, 300);

            return bmap;
        }

        // прорисовка хода и его отображение в массиве
        private Bitmap PaintShape(Graphics gr, Bitmap bmappar, int x, int y, bool typeshape = true)
        {
            gr = Graphics.FromImage(bmappar);
            int xp = ConvertToCoordinate(x), yp = ConvertToCoordinate(y);
            if (!typeshape) // крестик = true; нолик = false
            {
                //map[ConvertToIndex(y), ConvertToIndex(x)] = 1;
                gr.DrawEllipse(new Pen(Color.Black), new Rectangle(xp, yp, 50, 50));
                return bmappar;
            }
            else
            {
                //map[ConvertToIndex(y), ConvertToIndex(x)] = 2;
                gr.DrawLine(new Pen(Color.Black), xp, yp, xp + 50, yp + 50);
                gr.DrawLine(new Pen(Color.Black), xp, yp + 50, xp + 50, yp);
                return bmappar;
            }
        }

        // прорисовка позиции
        private Bitmap PaintPosition(Graphics gr, Bitmap bmappar, int[,] map)
        {
            bmappar = Field(pictureBox1);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (map[i, j] == 2)
                        bmappar = PaintShape(gr, bmappar, j * 100, i * 100, true);
                    if (map[i, j] == 1)
                        bmappar = PaintShape(gr, bmappar, j * 100, i * 100, false);
                }
            return bmappar;
        }

        private int ConvertToCoordinate(int n, int sizefild = 100)
        {
            return (n / sizefild) * sizefild + (sizefild - 50) / 2;
        }

        private int ConvertToIndex(int n)
        {
            return n / 100;
        }

        // определение выигранной позиции
        private bool GameEnd(int[,] map, bool ShapeType)
        {
            if (ShapeType) // искать за крестик
            {
                for (int i = 0; i < 3; i++)
                {
                    if (map[i, 0] == 2 && map[i, 1] == 2 && map[i, 2] == 2)
                        return true;
                    if (map[0, i] == 2 && map[1, i] == 2 && map[2, i] == 2)
                        return true;
                }
                if ((map[0, 0] == 2 && map[1, 1] == 2 && map[2, 2] == 2) ||
                    (map[0, 2] == 2 && map[1, 1] == 2 && map[2, 0] == 2)) // результат по диагонали
                    return true;

                return false; //крестики не выиграли
            }
            else           // искать за нолик
            {
                for (int i = 0; i < 3; i++)
                {
                    if (map[i, 0] == 1 && map[i, 1] == 1 && map[i, 2] == 1)
                        return true;
                    if (map[0, i] == 1 && map[1, i] == 1 && map[2, i] == 1)
                        return true;
                }
                if ((map[0, 0] == 1 && map[1, 1] == 1 && map[2, 2] == 1) ||
                    (map[0, 2] == 1 && map[1, 1] == 1 && map[2, 0] == 1)) // результат по диагонали
                    return true;

                return false; //нолики не выиграли
            }
        }

        // оценка конеченой позиции: виграно 1, ничья 0, поражение -1
        private int ValuePositioin(int[,] map, bool ShapeType)
        {
            if (ShapeType)
            {
                if (GameEnd(map, ShapeType))
                    return 1;
            }
            else
            {
                if (GameEnd(map, ShapeType))
                    return 1;
            }

            return 0;
        }

        // метод оценки возможного хода
        long numeric = 0; // подсчет общего числа вызовов <Appraisal>
        private int Appraisal(bool ShapeType)
        {
            numeric++; //тест

            int max = -1;

            // определяем тип фигуры противника
            bool oppShapeType = !ShapeType;

            bool findmove = false; // свободных клеток нет
            // проверяем текущую позицию на проигрыш
            if (GameEnd(Matrix, oppShapeType) == true)
                return -1;

            // генерируем и оцениваем
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (Matrix[i, j] == 0)
                    {
                        if (ShapeType)
                            Matrix[i, j] = 2;
                        else Matrix[i, j] = 1;

                        findmove = true;
                        // вот она - рекурсия, оцениваем текущую позицию
                        int tmp = -(Appraisal(oppShapeType));
                        if (tmp > max) // выбираем лучшую оценку варианта (ее и будем возвращать)
                        {
                            max = tmp;

                        }

                        // востанавливаем позицию
                        Matrix[i, j] = 0;
                        if (flag_clipping == true && max == 1)
                        {
                            max = 1;
                            break;
                        }
                    }
                }
            if (findmove == false)
                return 0;

            return max;
        }

        // случайный ход из начальной позиции
        private int RandomMove(int endindex)
        {
            return new Random().Next(0, endindex);
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Field(pictureBox1);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Matrix[ConvertToIndex(e.Y), ConvertToIndex(e.X)] > 0 || GameEnd(Matrix, flagfigure) == true
                || GameEnd(Matrix, !flagfigure) == true)
                return;
            if (flagmove == false) // false это ход компьютера
                return;

            if (flagfigure) // true - крестики, false - нолики
                Matrix[ConvertToIndex(e.Y), ConvertToIndex(e.X)] = 2;
            else
                Matrix[ConvertToIndex(e.Y), ConvertToIndex(e.X)] = 1;

            pictureBox1.Image = PaintShape(gr, bmap, e.X, e.Y, flagfigure);
            if (GameEnd(Matrix, true) == true)
            {
                MessageBox.Show("Крестики выиграли");
                return;
            }
            if (GameEnd(Matrix, false) == true)
            {
                MessageBox.Show("Нолики выиграли");
                return;
            }
            #region Проверка на ничью
            bool Equality = true;
            foreach (int n in Matrix)
            {
                if (n != 0)
                    Equality = false;
            }
            if (Equality)
            {
                MessageBox.Show("Ничья!");
                return;
            }

            #endregion
            flagmove = !flagmove; // смена очереди хода
            flagfigure = !flagfigure; // смена типа фигуры чья очередь хода
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Text = "";
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Text += Matrix[i, j].ToString() + " -- ";
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bool MatrixClear = true;
            foreach (int n in Matrix)
            {
                if (n != 0)
                    MatrixClear = false;
            }
            // если доска чистая, то разрешаем компьютеру первой сделать ход
            bool flagRandom = false;
            if (MatrixClear == true)
            {
                flagmove = false;
                flagRandom = true;
            }

            listBox1.Items.Clear();
            numeric = 0; // тестовая переменнаяs


            if (flagmove == true)
                return;
            Alternative alternative;
            Alternativs.Clear();

            #region Нополнение списка вохможных ходов c их оценкой
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (Matrix[i, j] == 0) // если поле пустое ставим значение 1 или 2
                    {
                        if (flagfigure)
                            Matrix[i, j] = 2;
                        else Matrix[i, j] = 1;

                        alternative.appraisal = -Appraisal(!flagfigure);
                        alternative.map = (int[,])Matrix.Clone();
                        Alternativs.Add(alternative);

                        Matrix[i, j] = 0; // восстановление позиции
                    }
                }
            if (Alternativs.Count == 0)
            {
                MessageBox.Show("Ничья!");
                return;
            }
            #endregion

            #region Вывод списка возможных ходов на Листбокс
            foreach (Alternative a in Alternativs)
            {
                listBox1.Items.Add((listBox1.Items.Count + 1).ToString() + ".  " + "Оценка: " + a.appraisal.ToString());
            }

            Text = "";
            Text = "Дерево перебора: " + numeric;
            #endregion

            #region Выбор хода
            //выибираем лучший ход из списка вариантов
            int max = -1, id = 0;
            for (int i = 0; i < Alternativs.Count; i++)
            {
                if (Alternativs[i].appraisal > max)
                {
                    max = Alternativs[i].appraisal;
                    id = i;
                }
            }
            #endregion

            #region Ход
            //делаем ход из варианта с индексом id
            if (flagRandom == false)
            {
                Matrix = (int[,])Alternativs[id].map.Clone();
                pictureBox1.Image = PaintPosition(gr, bmap, Matrix);
            }
            else
            {
                Matrix = (int[,])Alternativs[RandomMove(Alternativs.Count)].map.Clone();
                pictureBox1.Image = PaintPosition(gr, bmap, Matrix);
            }


            flagmove = !flagmove; // передаем очередь хода
            flagfigure = !flagfigure; // меняем тип фигуры

            if (GameEnd(Matrix, true) == true)
            {
                MessageBox.Show("Крестики выиграли");
                return;
            }
            if (GameEnd(Matrix, false) == true)
            {
                MessageBox.Show("Нолики выиграли");
                return;
            }
            #region Проверка на ничью
            bool Equality = true;
            foreach (int n in Matrix)
            {
                if (n == 0)
                    Equality = false;
            }
            if (Equality)
            {
                MessageBox.Show("Ничья!");
                return;
            }

            #endregion
            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            flagmove = false;
            flagfigure = true;

            // Оцениваем подготовленную тестовую позицию:

            numeric = 0; // тестовая переменная

            #region Оценка
            if (flagmove == true)
                return;
            Alternative alternative;
            Alternativs.Clear();

            #region Нополнение списка вохможных ходов c их оценкой
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (Matrix[i, j] == 0) // если поле пустое ставим значение 1 или 2
                    {
                        if (flagfigure)
                            Matrix[i, j] = 2;
                        else Matrix[i, j] = 1;

                        alternative.appraisal = -Appraisal(!flagfigure);
                        alternative.map = (int[,])Matrix.Clone();
                        Alternativs.Add(alternative);

                        Matrix[i, j] = 0; // восстановление позиции
                    }
                }
            #endregion
            #region Вывод списка возможных ходов на Листбокс
            foreach (Alternative a in Alternativs)
            {
                listBox1.Items.Add((listBox1.Items.Count + 1).ToString() + ".  " + "Оценка: " + a.appraisal.ToString());
            }

            Text = "";
            Text = "Число запусков Appraisal: " + numeric;
            #endregion

            #endregion

            #region Выбор хода
            ////выибираем лучший ход из списка вариантов
            //int max = -1, id = 0;
            //for (int i = 0; i < Alternativs.Count; i++)
            //{
            //    if (Alternativs[i].appraisal > max)
            //    {
            //        max = Alternativs[i].appraisal;
            //        id = i;
            //    }
            //}
            #endregion
            #region Ход
            ////делаем лучший ход из списка вариантов
            //Matrix = (int[,])Alternativs[id].map.Clone();
            //pictureBox1.Image = PaintPosition(gr, bmap, Matrix);


            //flagmove = !flagmove;
            //flagfigure = !flagfigure;
            //listBox1.Items.Add(numeric);

            //if (GameEnd(Matrix, true) == true)
            //{
            //    MessageBox.Show("Крестики выиграли");
            //    return;
            //}
            //if (GameEnd(Matrix, false) == true)
            //{
            //    MessageBox.Show("Нолики выиграли");
            //    return;
            //}
            #endregion

            flagmove = !flagmove;
            flagfigure = !flagfigure;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numeric = 0;
            listBox1.Items.Clear();

            Matrix = new int[3, 3];
            pictureBox1.Image = Field(pictureBox1);

            flagfigure = false;
            flagmove = true;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            flagfigure = !flagfigure;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            flag_clipping = checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
