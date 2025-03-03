namespace CRAZELib.Enums
{
    /// <summary>
    /// Результат после хода.
    /// </summary>
    public enum ResultGame
    {
        /// <summary>
        /// Нет результата - игра продолжается.
        /// </summary>
        None = 0,

        /// <summary>
        /// Крестики выиграли.
        /// </summary>
        Cross = 1,

        /// <summary>
        /// Нолики выиграли.
        /// </summary>
        Zero = 2,

        /// <summary>
        /// Ничья.
        /// </summary>
        Draw = 3
    }
}
