namespace Cowsay.Abstractions
{
    public interface IBubbleBlower
    {
        string GetBubble(string phrase, int maxCols, bool thoughtBubble = false);
    }
}
