using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json.Serialization;

namespace PlantBook;

/// <inheritdoc />
public class PlantBook : IPlantBook
{
    private const string url = "https://open.plantbook.io/api/v1";

    private readonly OAuth2ClientCredentials credentials;
    private Token? token;

    static PlantBook()
    {
        FlurlHttp.Configure(settings =>
        {
            settings.JsonSerializer = new NewtonsoftJsonSerializer(new()
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace,
            });
        });
    }

    public PlantBook(OAuth2ClientCredentials credentials)
    {
        this.credentials = credentials;
    }

    private async Task<Token> RefreshTokenIfNeededAsync(CancellationToken cancellationToken = default)
    {
        if (token is null || token.IsExpired)
        {
            token = await GetTokenAsync(cancellationToken);
        }
        return token;
    }

    private async Task<Token> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        return await url
            .AppendPathSegment("token/")
            .PostUrlEncodedAsync(new
            {
                grant_type = "client_credentials",
                client_id = credentials.ClientId,
                client_secret = credentials.Secret,
            }, 
            cancellationToken)
            .ReceiveJson<Token>();
    }

    /// <inheritdoc />
    public async Task<SearchResult> SearchAsync(string alias, int? offset = null, int? limit = null, bool? userplant = null, CancellationToken cancellationToken = default)
    {
        return await url
            .AppendPathSegments("plant", "search")
            .SetQueryParams(new { limit, offset, alias, userplant })
            .WithOAuthBearerToken((await RefreshTokenIfNeededAsync(cancellationToken)).AccessToken)
            .GetAsync(cancellationToken)
            .ReceiveJson<SearchResult>();
    }

    /// <inheritdoc />
    public async Task<Plant> GetDetailsAsync(string pid, CancellationToken cancellationToken = default)
    {
        return await url
            .AppendPathSegments("plant", "detail", pid, "/")
            .WithOAuthBearerToken((await RefreshTokenIfNeededAsync(cancellationToken)).AccessToken)
            .GetAsync(cancellationToken)
            .ReceiveJson<Plant>();
    }

    /// <inheritdoc />
    public async Task<Plant> CreateAsync(Plant plant, CancellationToken cancellationToken = default)
    {
        return await url
            .AppendPathSegments("plant", "create")
            .WithOAuthBearerToken((await RefreshTokenIfNeededAsync(cancellationToken)).AccessToken)
            .PostJsonAsync(plant, cancellationToken)
            .ReceiveJson<Plant>();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string pid, CancellationToken cancellationToken = default)
    {
        await url
            .AppendPathSegments("plant", "delete")
            .WithOAuthBearerToken((await RefreshTokenIfNeededAsync(cancellationToken)).AccessToken)
            .SendJsonAsync(HttpMethod.Delete, new { pid }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Plant> UpdateAsync(Plant plant, CancellationToken cancellationToken = default)
    {
        return await url
            .AppendPathSegments("plant", "update")
            .WithOAuthBearerToken((await RefreshTokenIfNeededAsync(cancellationToken)).AccessToken)
            .PatchJsonAsync(plant, cancellationToken)
            .ReceiveJson<Plant>();
    }
}
