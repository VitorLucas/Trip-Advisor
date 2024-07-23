namespace  Trip.Advisor.Be.Api.ApiKeyAuth
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string userApiKey);
    }
}
