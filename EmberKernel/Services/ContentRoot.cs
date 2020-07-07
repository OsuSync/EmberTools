namespace EmberKernel.Services
{
    internal class ContentRoot : IContentRoot
    {
        public string ContentDirectory { get; private set; }
        public ContentRoot(string root)
        {
            this.ContentDirectory = root;
        }
    }
}
