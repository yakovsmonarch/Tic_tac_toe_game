using System;
using System.Collections.Generic;
using CRAZELib.Base;
using CRAZELib.Enums;
using CRAZELib.Model;

namespace CRAZELib.Engine.ArrTwoEngine
{
    public class YakovCrazeEngine : BaseCrazeEngine
    {
        public YakovCrazeEngine(string czn, CrazeForce force) : base(czn, force)
        {
            (_matrix, _startMove, _numberStartMove) = ParseCZN(czn);
        }

        public override (string position, ResultGame resultGame) Move(string czn)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Номер ходя в стартововой позиции.
        /// </summary>
        private readonly int _numberStartMove;

        /// <summary>
        /// Чей ход был в стартовой позиции.
        /// </summary>
        private readonly char _startMove;

        /// <summary>
        /// Клон массива для тестов.
        /// </summary>
        /// <returns></returns>
        public int[,] CloneArrPos()
        {
            return (int[,])_matrix.Clone();
        }

        /// <summary>
        /// Массив содержащий позицию.
        /// </summary>
        private readonly int[,] _matrix;

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
        /// Инициализация позиции.
        /// </summary>
        /// <param name="czn">Строка в формате fenttt.</param>
        private (int[,] position, char startMove, int numberStartMove) ParseCZN(string czn)
        {
            int[,] result = new int[3, 3];
            string[] arr = czn.Split();

            string[] posLines = arr[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            char currentMove = arr[1][0];
            int counterMove = int.TryParse(arr[2], out int outCounterMove) ?
                outCounterMove :
                throw new Exception($"Не удалось получить номер хода из строки '{arr[2]}'.");


            for (int row = 0; row < 3; row++)
            {
                char[] shapes = posLines[row].ToLower().ToCharArray();
                for (int col = 0; col < shapes.Length; col++)
                {
                    if (int.TryParse(shapes[col].ToString(), out int namberFieldEmpty))
                    {
                        for (int pos = col; pos < namberFieldEmpty; namberFieldEmpty++)
                        {
                            result[row, pos] = 0;
                        }
                        continue;
                    }

                    if (shapes[col] == 'c')
                    {
                        result[row, col] = 2;
                    }
                    else if (shapes[col] == 'z')
                    {
                        result[row, col] = 1;
                    }
                    else
                    {
                        throw new NotImplementedException($"Не найдено сотояние поля '{shapes[col]}'.");
                    }

                }
            }

            return (result, currentMove, counterMove);
        }

    }
}
