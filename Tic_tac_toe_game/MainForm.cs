using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tic_tac_toe_game.Model;

namespace Tic_tac_toe_game
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Поля

        private Graphics _graphics;

        private Bitmap _bmap;

        /// <summary>
        /// Массив содержащий позицию.
        /// </summary>
        private int[,] _matrix = new int[3, 3];

        /// <summary>
        /// true - крестики, false - нолики
        /// </summary>
        private bool _flagfigure = false;

        /// <summary>
        /// false - ходит робот, true - человек
        /// </summary>
        private bool _flagmove = true;

        private List<Alternative> _alternativs = new List<Alternative>();
        
        private bool _flagClipping = false;

        /// <summary>
        /// Подсчет общего числа вызовов <Appraisal>.
        /// </summary>
        private long _numeric = 0;
        #endregion

        #region Методы

        /// <summary>
        /// Прорисовка доски.
        /// </summary>
        /// <param name="pictbox"></param>
        /// <returns></returns>
        private Bitmap Field(PictureBox pictbox)
        {
            _bmap = new Bitmap(pictbox.Width, pictbox.Height);
            _graphics = Graphics.FromImage(_bmap);
            _graphics.DrawLine(new Pen(Color.Black), 0, 100, 300, 100);
            _graphics.DrawLine(new Pen(Color.Black), 0, 200, 300, 200);
            _graphics.DrawLine(new Pen(Color.Black), 100, 0, 100, 300);
            _graphics.DrawLine(new Pen(Color.Black), 200, 0, 200, 300);

            return _bmap;
        }

        /// <summary>
        /// Прорисовка хода и его отображение в массиве.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="bmappar"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="typeshape">крестик = true; нолик = false</param>
        /// <returns></returns>
        private Bitmap PaintShape(Graphics gr, Bitmap bmappar, int x, int y, bool typeshape = true)
        {
            gr = Graphics.FromImage(bmappar);
            int xp = ConvertToCoordinate(x), yp = ConvertToCoordinate(y);
            if (!typeshape)
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

        /// <summary>
        /// Прорисовка позиции.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="bmappar"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private Bitmap PaintPosition(Graphics gr, Bitmap bmappar, int[,] map)
        {
            bmappar = Field(pictureBoxGameBoard);
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

        /// <summary>
        /// Определение выигранной позиции.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="ShapeType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Оценка конеченой позиции: виграно 1, ничья 0, поражение -1.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="ShapeType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод оценки возможного хода.
        /// </summary>
        /// <param name="ShapeType"></param>
        /// <returns></returns>
        private int Appraisal(bool ShapeType)
        {
            _numeric++; //тест

            int max = -1;

            // определяем тип фигуры противника
            bool oppShapeType = !ShapeType;

            bool findmove = false; // свободных клеток нет
            // проверяем текущую позицию на проигрыш
            if (GameEnd(_matrix, oppShapeType) == true)
                return -1;

            // генерируем и оцениваем
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (_matrix[i, j] == 0)
                    {
                        if (ShapeType)
                            _matrix[i, j] = 2;
                        else _matrix[i, j] = 1;

                        findmove = true;
                        // вот она - рекурсия, оцениваем текущую позицию
                        int tmp = -(Appraisal(oppShapeType));
                        if (tmp > max) // выбираем лучшую оценку варианта (ее и будем возвращать)
                        {
                            max = tmp;

                        }

                        // востанавливаем позицию
                        _matrix[i, j] = 0;
                        if (_flagClipping == true && max == 1)
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

        /// <summary>
        /// Случайный ход из начальной позиции.
        /// </summary>
        /// <param name="endindex"></param>
        /// <returns></returns>
        private int RandomMove(int endindex)
        {
            return new Random().Next(0, endindex);
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            pictureBoxGameBoard.Image = Field(pictureBoxGameBoard);
        }

        private void pictureBoxGameBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (_matrix[ConvertToIndex(e.Y), ConvertToIndex(e.X)] > 0 || 
                GameEnd(_matrix, _flagfigure) == true || 
                GameEnd(_matrix, !_flagfigure) == true)
                return;

            if (_flagmove == false)
                return;

            if (_flagfigure)
                _matrix[ConvertToIndex(e.Y), ConvertToIndex(e.X)] = 2;
            else
                _matrix[ConvertToIndex(e.Y), ConvertToIndex(e.X)] = 1;

            pictureBoxGameBoard.Image = PaintShape(_graphics, _bmap, e.X, e.Y, _flagfigure);
            if (GameEnd(_matrix, true) == true)
            {
                MessageBox.Show("Крестики выиграли");
                return;
            }
            if (GameEnd(_matrix, false) == true)
            {
                MessageBox.Show("Нолики выиграли");
                return;
            }

            #region Проверка на ничью
            bool Equality = true;
            foreach (int n in _matrix)
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
            _flagmove = !_flagmove; // смена очереди хода
            _flagfigure = !_flagfigure; // смена типа фигуры чья очередь хода
        }

        private void buttonArray_Click(object sender, EventArgs e)
        {
            Text = "";
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Text += _matrix[i, j].ToString() + " -- ";
        }

        private void pictureBoxGameBoard_MouseUp(object sender, MouseEventArgs e)
        {
            bool matrixClear = true;
            foreach (int n in _matrix)
            {
                if (n != 0)
                    matrixClear = false;
            }

            // если доска чистая, то разрешаем компьютеру первой сделать ход
            bool flagRandom = false;
            if (matrixClear == true)
            {
                _flagmove = false;
                flagRandom = true;
            }

            listBoxListOfRatings.Items.Clear();
            _numeric = 0; // тестовая переменнаяs

            if (_flagmove == true)
                return;

            Alternative alternative;
            _alternativs.Clear();

            #region Нополнение списка вохможных ходов c их оценкой
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_matrix[i, j] == 0) // если поле пустое ставим значение 1 или 2
                    {
                        if (_flagfigure)
                            _matrix[i, j] = 2;
                        else _matrix[i, j] = 1;

                        alternative.appraisal = -Appraisal(!_flagfigure);
                        alternative.map = (int[,])_matrix.Clone();
                        _alternativs.Add(alternative);

                        _matrix[i, j] = 0; // восстановление позиции
                    }
                }
            }

            if (_alternativs.Count == 0)
            {
                MessageBox.Show("Ничья!");
                return;
            }
            #endregion

            #region Вывод списка возможных ходов на Листбокс
            foreach (Alternative a in _alternativs)
            {
                listBoxListOfRatings.Items.Add((listBoxListOfRatings.Items.Count + 1).ToString() + ".  " + "Оценка: " + a.appraisal.ToString());
            }

            Text = "";
            Text = "Дерево перебора: " + _numeric;
            #endregion

            #region Выбор хода
            //выибираем лучший ход из списка вариантов
            int max = -1, id = 0;
            for (int i = 0; i < _alternativs.Count; i++)
            {
                if (_alternativs[i].appraisal > max)
                {
                    max = _alternativs[i].appraisal;
                    id = i;
                }
            }
            #endregion

            #region Ход
            //делаем ход из варианта с индексом id
            if (flagRandom == false)
            {
                _matrix = (int[,])_alternativs[id].map.Clone();
                pictureBoxGameBoard.Image = PaintPosition(_graphics, _bmap, _matrix);
            }
            else
            {
                _matrix = (int[,])_alternativs[RandomMove(_alternativs.Count)].map.Clone();
                pictureBoxGameBoard.Image = PaintPosition(_graphics, _bmap, _matrix);
            }

            _flagmove = !_flagmove; // передаем очередь хода
            _flagfigure = !_flagfigure; // меняем тип фигуры

            if (GameEnd(_matrix, true) == true)
            {
                MessageBox.Show("Крестики выиграли");
                return;
            }
            if (GameEnd(_matrix, false) == true)
            {
                MessageBox.Show("Нолики выиграли");
                return;
            }

            #region Проверка на ничью
            bool Equality = true;
            foreach (int n in _matrix)
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

        private void btnTestPosition_Click(object sender, EventArgs e)
        {
            listBoxListOfRatings.Items.Clear();
            _flagmove = false;
            _flagfigure = true;

            // Оцениваем подготовленную тестовую позицию:

            _numeric = 0; // тестовая переменная

            #region Оценка
            if (_flagmove == true)
                return;
            Alternative alternative;
            _alternativs.Clear();

            #region Нополнение списка вохможных ходов c их оценкой
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (_matrix[i, j] == 0) // если поле пустое ставим значение 1 или 2
                    {
                        if (_flagfigure)
                            _matrix[i, j] = 2;
                        else _matrix[i, j] = 1;

                        alternative.appraisal = -Appraisal(!_flagfigure);
                        alternative.map = (int[,])_matrix.Clone();
                        _alternativs.Add(alternative);

                        _matrix[i, j] = 0; // восстановление позиции
                    }
                }
            #endregion
            #region Вывод списка возможных ходов на Листбокс
            foreach (Alternative a in _alternativs)
            {
                listBoxListOfRatings.Items.Add((listBoxListOfRatings.Items.Count + 1).ToString() + ".  " + "Оценка: " + a.appraisal.ToString());
            }

            Text = "";
            Text = "Число запусков Appraisal: " + _numeric;
            #endregion

            #endregion

            _flagmove = !_flagmove;
            _flagfigure = !_flagfigure;
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            _numeric = 0;
            listBoxListOfRatings.Items.Clear();

            _matrix = new int[3, 3];
            pictureBoxGameBoard.Image = Field(pictureBoxGameBoard);

            _flagfigure = false;
            _flagmove = true;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            _flagfigure = !_flagfigure;
        }

        private void checkBoxCuttingOffBranches_CheckStateChanged(object sender, EventArgs e)
        {
            _flagClipping = checkBoxCuttingOffBranches.Checked;
        }

    }
}
