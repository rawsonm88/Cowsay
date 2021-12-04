namespace Cowsay.Abstractions
{
    public interface IBubbleBlower
    {
        /// <summary>
        /// Generates the speech/thought bubble.
        /// </summary>
        /// <param name="phrase">The phrase to include inside the bubble.</param>
        /// <param name="maxCols">The maximum columns of text before wrapping.</param>
        /// <param name="isThought">True, if this should be a thought bubble.</param>
        /// <returns>The ASCII speech/thought bubble.</returns>
        string GetBubble(string phrase, int maxCols, bool isThought = false);
    }
}
