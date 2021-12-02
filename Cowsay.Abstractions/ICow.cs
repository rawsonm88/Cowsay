namespace Cowsay.Abstractions
{
    public interface ICow
    {
        string Format { get; }

        string Say(string phrase, string cowEyes = "oo", string cowTongue = "  ", int maxCols = 40, bool isThought = false);
    }
}
