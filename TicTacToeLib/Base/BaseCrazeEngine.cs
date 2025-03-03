using CRAZELib.Enums;

namespace CRAZELib.Base
{
    /// <summary>
    /// Базовый класс движка игры крестики-нолики.
    /// </summary>
    public abstract class BaseCrazeEngine
    {
        /// <summary>
        /// Создание экземпляра движка.
        /// </summary>
        /// <param name="czn">
        /// FEN Tic-tac-toe. Нотация позиции на основе шахматного FEN.
        /// Пример fenttt: "cz1/3/3 z 3", где в подстроке "cz1/" "c" - крестик, "z" - нолик, "1" - число пустых полей, "/" - разделитель. 
        /// "3/3" - по три пустых полей на горизонталях.
        /// "z" - предстоит ход ноликов.
        /// "3" - порядковый номер предстоящего хода.
        /// </param>
        /// <param name="force">
        /// Уровень силы игры движка.
        /// </param>
        public BaseCrazeEngine(string czn, CrazeForce force) {}

        /// <summary>
        /// Ход движка. Принимает строку позции в формате fenttt.
        /// </summary>
        /// <param name="czn">Строка в формате fenttt.</param>
        /// <returns>Позиция после хода и результат игры.</returns>
        public abstract (string position, ResultGame resultGame) Move(string czn);
    }
}
