using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Checkout.PaymentsGateway.Api.UnitTests.Cache
{
    public class DistributedCacheMock : IDistributedCache
    {
        public bool SetStringAsyncWasCalled;

        public async Task SetStringAsync(string key, string value,
            DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            SetStringAsyncWasCalled = true;
            await Task.CompletedTask;
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = new CancellationToken())
        {
            SetStringAsyncWasCalled = true;
            await Task.CompletedTask;
        }

        #region NotImplementedMock

        public byte[] Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            throw new NotImplementedException();
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}