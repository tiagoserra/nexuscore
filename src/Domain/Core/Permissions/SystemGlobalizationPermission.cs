namespace Domain.Core.Permissions;

public static class SystemGlobalizationPermission
{
    public const string Module = "core";
    public const string Role = "role::core::systemglobalization";

    public static class Policy 
    {
        public const string Read = "policy::core::systemglobalization::read"; 
        public const string Write = "policy::core::systemglobalization::write"; 
        public const string Delete = "policy::core::systemglobalization::delete";
    }
}