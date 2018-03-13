namespace Selfhost.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Http.Dependencies;
    using Unity;
    using Unity.Exceptions;

    public class UnityScope : IDependencyScope
    {
        private readonly IUnityContainer _container;
        private bool _disposed;

        internal UnityScope(IUnityContainer container)
        {
            _disposed = false;
            _container = container;
        }

        ~UnityScope()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        object IDependencyScope.GetService(Type serviceType)
        {
            object resolved = null;

            try
            {
                resolved = _container.Resolve(serviceType);
                Trace.WriteLine($"RESOLVED {serviceType.Name}={resolved}");
            }
            catch (ResolutionFailedException)
            {
                Trace.WriteLine($"FAILED TO RESOLVE {serviceType.Name}");
            }

            return resolved;
        }

        IEnumerable<object> IDependencyScope.GetServices(Type serviceType)
        {
            IEnumerable<object> resolved = null;

            try
            {
                resolved = _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                resolved = new List<object>();
            }

            return resolved;
        }

        protected IDependencyScope CreateChildScope()
        {
            return new UnityScope(_container.CreateChildContainer());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                _disposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
