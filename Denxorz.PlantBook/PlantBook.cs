using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json.Serialization;

namespace Denxorz.PlantBook;

public class PlantBook
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

    private async Task RefreshTokenIfNeededAsync()
    {
        if (token is null || token.IsExpired)
        {
            token = await GetTokenAsync();
        }
    }

    private async Task<Token> GetTokenAsync()
    {
        return await url
            .AppendPathSegment("token/")
            .PostUrlEncodedAsync(new
            {
                grant_type = "client_credentials",
                client_id = credentials.ClientId,
                client_secret = credentials.Secret,
            })
            .ReceiveJson<Token>();
    }

    public async Task<SearchResult> SearchAsync(string searchFor, int? offset = null, int? limit = null)
    {
        await RefreshTokenIfNeededAsync();

        return await url
            .AppendPathSegments("plant", "search")
            .SetQueryParams(new { limit, offset, alias = searchFor })
            .WithOAuthBearerToken(token!.AccessToken)
            .GetAsync()
            .ReceiveJson<SearchResult>();
    }

    public async Task<Plant> GetDetailsAsync(string pid)
    {
        await RefreshTokenIfNeededAsync();

        return await url
            .AppendPathSegments("plant", "detail", pid, "/")
            .WithOAuthBearerToken(token!.AccessToken)
            .GetAsync()
            .ReceiveJson<Plant>();
    }
}
