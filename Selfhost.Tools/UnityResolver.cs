namespace Selfhost.Tools
{
    using System.Web.Http.Dependencies;
    using Unity;

    public sealed class UnityResolver : UnityScope, IDependencyResolver
    {
        public UnityResolver(IUnityContainer container) : base(container)
        {
        }

        IDependencyScope IDependencyResolver.BeginScope()
        {
            return CreateChildScope();
        }
    }
}
