using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Goodreads.Clients;
using Xunit;

namespace Goodreads.Tests.Clients
{
    public class UsersClientTests
    {
        private readonly IUsersClient UsersClient;
        private readonly int UserId;

        public UsersClientTests()
        {
            UsersClient = Helper.GetAuthClient().Users;
            UserId = Helper.GetUserId();
        }

        public class TheGetByUserIdMethod : UsersClientTests
        {
            [Fact]
            public async Task ReturnsAUser()
            {
                var user = await UsersClient.GetByUserId(UserId);

                Assert.NotNull(user);
                Assert.Equal(user.Id, UserId);
            }

            [Fact]
            public async Task ReturnsNullIfNotFound()
            {
                var user = await UsersClient.GetByUserId(int.MaxValue);

                Assert.Null(user);
            }
        }

        public class TheGetByUsernameMethod : UsersClientTests
        {
            [Fact]
            public async Task ReturnsAUser()
            {
                var username = "adamkrogh";
                var user = await UsersClient.GetByUsername(username);

                Assert.NotNull(user);
                Assert.Equal(user.Username, username);
            }

            [Fact]
            public async Task ReturnsNullIfNotFound()
            {
                var username = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var user = await UsersClient.GetByUsername(username);

                Assert.Null(user);
            }
        }

        public class TheGetListOfFriendsMethod : UsersClientTests
        {
            [Fact]
            public async Task ReturnsFriends()
            {
                var friends = await UsersClient.GetListOfFriends(UserId);

                Assert.NotNull(friends);
                Assert.NotEmpty(friends.List);
                Assert.True(friends.Pagination.TotalItems > 0);
            }

            [Fact]
            public async Task ReturnsNullIfNotFound()
            {
                var friends = await UsersClient.GetListOfFriends(userId: -1);

                Assert.Null(friends);
            }
        }

        public class TheGetAllListsMethods : UsersClientTests
        {
            [Fact]
            public void ReturnsAllFriends()
            {
                var friends = UsersClient.GetListOfAllFriends(UserId);

                Assert.NotNull(friends);
                Assert.True(friends.Count > 0);
            }

            [Fact]
            public void ReturnsAllFollowers()
            {
                var followers = UsersClient.GetListOfAllFollowers(UserId);

                Assert.NotNull(followers);
                Assert.True(followers.Count > 0);
            }

            [Fact]
            public void ReturnsAllFollowing()
            {
                var following = UsersClient.GetListOfAllFollowing(UserId);

                Assert.NotNull(following);
                Assert.True(following.Count > 0);
            }
        }

        public class TheGetListOfFollowersMethod : UsersClientTests
        {
            [Fact]
            public async Task ReturnsFollowers()
            {
                var followers = await UsersClient.GetListOfFollowers(UserId);

                Assert.NotNull(followers);
                Assert.NotEmpty(followers.List);
                Assert.True(followers.Pagination.TotalItems > 0);
            }

            [Fact]
            public async Task ReturnsNullIfNotFound()
            {
                var followers = await UsersClient.GetListOfFollowers(userId: -1);

                Assert.Null(followers);
            }
        }

        public class TheGetListOfFollowingMethod : UsersClientTests
        {
            [Fact]
            public async Task ReturnsFollowing()
            {
                var following = await UsersClient.GetListOfFollowing(UserId);

                Assert.NotNull(following);
                Assert.NotEmpty(following.List);
                Assert.True(following.Pagination.TotalItems > 0);
            }

            [Fact]
            public async Task ReturnsNullIfNotFound()
            {
                var following = await UsersClient.GetListOfFollowing(userId: -1);

                Assert.Null(following);
            }
        }


        public class TheGetAuthenticatedUserIdMethod : UsersClientTests
        {
            [Fact]
            public async Task ReturnsUserIdWhenAuthenticated()
            {
                var id = await UsersClient.GetAuthenticatedUserId();

                Assert.NotNull(id);
                Assert.Equal(id.Value, Helper.GetUserId());
            }

            [Fact]
            public async Task ReturnsNullWhenNotAuthenticated()
            {
                var client = Helper.GetClient();
                var id = await client.Users.GetAuthenticatedUserId();

                Assert.Null(id);
            }
        }
    }
}
