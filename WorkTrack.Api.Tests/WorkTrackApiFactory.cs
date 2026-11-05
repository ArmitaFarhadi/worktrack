using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkTrack.Domain.Entities;

using WorkTrack.Domain.Contracts;
using WorkTrack.Api.Tests.Fakes;

namespace WorkTrack.Api.Tests;

public class WorkTrackApiFactory
    : WebApplicationFactory<Program>
{
    private readonly IReadOnlyCollection<WorkItem> _items;
    private readonly TimeProvider _clock;
    public WorkTrackApiFactory(
        IReadOnlyCollection<WorkItem>? items = null,
        TimeProvider? clock = null)
    {
        _items = items ?? Array.Empty<WorkItem>();
        _clock = clock ?? TimeProvider.System;
    }
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IWorkItemStore>();

            services.AddScoped<IWorkItemStore>(_ =>
             new FakeWorkItemStore
             {
                 Items = _items
             });
             services.RemoveAll<TimeProvider>();

             services.AddSingleton<TimeProvider>(_clock);
     });
    }
}