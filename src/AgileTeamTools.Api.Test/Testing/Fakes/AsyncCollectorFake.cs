using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgileTeamTools.Api.Test.Testing.Fakes
{
    internal class AsyncCollectorFake<T> : IAsyncCollector<T>
    {
        public bool IsFlushed = false;
        public List<T> Data = new List<T>();

        public Task AddAsync(T item, CancellationToken cancellationToken = default)
        {
            Data.Add(item);
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            IsFlushed = true;
            return Task.CompletedTask;
        }
    }
}
