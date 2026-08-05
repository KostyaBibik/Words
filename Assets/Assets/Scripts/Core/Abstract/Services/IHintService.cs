namespace Core.Services
{
    public interface IHintService
    {
        /// <summary>Highlights the opening cluster of a word that is still unsolved.</summary>
        /// <returns>False when there is nothing left to hint at.</returns>
        bool TryShowHint();
    }
}
