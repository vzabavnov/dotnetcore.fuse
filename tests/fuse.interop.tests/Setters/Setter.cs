using Fuse.Interop.FuseOpt;

namespace fuse.interop.tests
{
    public abstract class Setter
    {
        protected Setter(string template, string args)
        {
            Template = template;
            Args = args;
        }

        public string Args { get; }

        public string Template { get; }

        public abstract bool Success { get; }


        public bool Result { get; protected set; }

        public abstract void Init(FuseOptions options);

        public static implicit operator object[](Setter setter)
        {
            return new object[] {setter};
        }
    }
}