using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class UserDatabaseRepository : IUserRepository
{
    private readonly StakeholdersContext _dbContext;

    public UserDatabaseRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(string username)
    {
        return _dbContext.Users.Any(user => user.Username == username);
    }

    public User? GetActiveByName(string username)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Username == username && user.IsActive);
    }
    public User Create(User user)
    {
        /*if (user.Role == UserRole.Tourist)
        {
            var tourist = new Tourist(user.Username, user.Password, user.Role, user.IsActive, user.IsVerified);
            _dbContext.Tourists.Add(tourist); 
            _dbContext.SaveChanges(); 
        }*/

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        //_dbContext.SocialProfiles.Add(new SocialProfile(user.Id, ""));
        //_dbContext.SaveChanges();
        return user;
    }

    public long GetPersonId(long userId)
    {
        var person = _dbContext.People.FirstOrDefault(i => i.UserId == userId);
        if (person == null) throw new KeyNotFoundException("Not found.");
        return person.Id;
    }

    public List<User> GetAll()
    {
        return _dbContext.Users.ToList();
    }

    public User Update(User user)
    {
        try
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        return user;
    }

    public User GetUserById(long id)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Id == id);
        if (user == null) throw new KeyNotFoundException("Not found.");

        return user;
    }

    public User GetUserByUsername(string username)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Username == username);
        if (user == null) throw new KeyNotFoundException("Not found.");

        return user;
    }

    public User UpdatePassword(long userId, string password)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Id == userId);
        if (user == null) throw new KeyNotFoundException("Not found user with ID: " + userId);
        user.SetNewPassword(password);
        _dbContext.SaveChanges();
        return user;
    }
}