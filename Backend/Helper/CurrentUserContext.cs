public class CurrentUserContext
{
    private readonly IHttpContextAccessor httpContextAccessor;
    public CurrentUserContext(IHttpContextAccessor _httpContextAccessor)
    {
        this.httpContextAccessor = _httpContextAccessor;
    }
    public int UserId => Convert.ToInt32(httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value);
    public int BusinessId => Convert.ToInt32(httpContextAccessor.HttpContext?.User.FindFirst("BusinessId")?.Value);
    public bool HasBusiness => BusinessId > 0;

}