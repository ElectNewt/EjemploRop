namespace ROP
{
    /// <summary>
    /// Represents a unit type, which represents the absence of a specific value.
    /// </summary>
    public sealed class Unit
    {
        /// <summary>
        /// The single instance of the Unit type.
        /// </summary>
        public static readonly Unit Value = new Unit();
        private Unit() { }
    }
}
