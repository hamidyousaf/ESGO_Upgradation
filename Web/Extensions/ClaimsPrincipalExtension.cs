namespace Web.Extensions;

public static class ClaimsPrincipalExtension
{
    public static int GetEmployeeId(this ClaimsPrincipal user)
    {
        return Convert.ToInt32(user.FindFirst(x => x.Type == "EmployeeId").Value);
    }
    public static int GetEmployerId(this ClaimsPrincipal user)
    {
        return Convert.ToInt32(user.FindFirst(x => x.Type == "EmployerId").Value);
    }
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
