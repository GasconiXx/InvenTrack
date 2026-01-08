using System.Net.Http.Headers;
using Inventrack.App.Services.Interfaces;

namespace Inventrack.App.Services.Http;

public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly ISessionService _session;

    public AuthHeaderHandler(ISessionService session) => _session = session;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _session.Token;
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}

