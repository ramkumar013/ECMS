using ExtendedMongoMembership.Services;

namespace ECMS.WebV2
{
    public class DefaultUserProfileService : UserProfileServiceBase<ECMSMember>
    {
        public DefaultUserProfileService(string connectionString)
            : base(connectionString)
        {

        }
    }
}