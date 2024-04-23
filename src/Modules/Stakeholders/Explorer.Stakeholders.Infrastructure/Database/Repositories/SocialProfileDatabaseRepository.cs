using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class SocialProfileDatabaseRepository : ISocialProfileRepository
    {

        private readonly StakeholdersContext _dbContext;

        public SocialProfileDatabaseRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SocialProfile Get(long userId)
        {
            var profile = _dbContext.SocialProfiles.Include(sp => sp.Followed)
                                                            .FirstOrDefault(sc => sc.UserId == userId);

            if (profile == null) throw new KeyNotFoundException("Social profile not found.");
            profile.SetFollowers(GetFollowers(profile));
            //profile.SetFollowable(GetFollowable(profile));

            return profile;
        }

        public SocialProfile Update(SocialProfile profile)
        {
            try
            {
                _dbContext.SocialProfiles.Update(profile);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseValues = entry.GetDatabaseValues();
                var clientValues = entry.Entity;

                entry.OriginalValues.SetValues(databaseValues);

                _dbContext.SaveChanges();
            }
            profile.SetFollowers(GetFollowers(profile));
            //profile.SetFollowable(GetFollowable(profile));

            return profile;
        }

        private List<User> GetFollowers(SocialProfile profile)
        {
            var followersSocialProfiles = _dbContext.SocialProfiles
                .Where(sp => sp.Followed.Any(followedUser => followedUser.Id == profile.UserId))
                .ToList();

            var followerUserIds = followersSocialProfiles.Select(sp => sp.UserId).ToList();

            var followers = _dbContext.Users
                .Where(user => followerUserIds.Contains(user.Id))
                .ToList();

            return followers;
        }

        private List<User> GetFollowable(SocialProfile profile)
        {
            return _dbContext.Users
                .Where(u => !profile.Followed.Contains(u) && u.Id != profile.UserId)
                .ToList();
        }
    }
}
