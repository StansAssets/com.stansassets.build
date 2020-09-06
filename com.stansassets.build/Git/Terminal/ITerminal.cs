namespace StansAssets.Git
{
    interface ITerminal
    {
        (string output, string error) Call(params string[] commands);
    }
}
