using System;

namespace StansAssets.Git
{
    static class Gits
    {
        const string k_ProcessNameGit = "git";

        public static Git Get(string path)
        {
            return new Git(command => new Terminal(k_ProcessNameGit, path, command).Call().output);
        }

        public static Git GetFromCurrentDirectory()
        {
            return Get(Environment.CurrentDirectory);
        }
    }
}
