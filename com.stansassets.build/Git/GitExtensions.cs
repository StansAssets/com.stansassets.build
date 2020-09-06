namespace StansAssets.Git
{
    static class GitExtensions
    {
        public static string Add(this Git git, string filter) => git.Add.Files(filter);
        public static string Push(this Git git) => git.Push.Default();
        public static string Commit(this Git git, string message) => git.Commit.WithMessage(message);
    }
}
