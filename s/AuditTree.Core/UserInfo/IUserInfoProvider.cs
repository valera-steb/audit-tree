namespace AuditTree.Core.UserInfo
{
    public interface IUserInfoProvider
    {
        string GetUserId();
        string GetUserInfo();
    }
}
