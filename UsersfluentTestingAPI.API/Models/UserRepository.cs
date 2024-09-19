namespace UsersfluentTestingAPI.API.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;
        public UserRepository()
        {
            // Simulating a data source with some dummy data
            _users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
            };
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            //Simulate the async operation using Task.Delay
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            return _users.FirstOrDefault(u => u.Id == userId);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            //Simulate the async operation using Task.Delay
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            return _users;
        }
        public async Task AddUserAsync(User user)
        {
            //Simulate the async operation using Task.Delay
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            _users.Add(user);
        }
        public async Task UpdateUserAsync(User user)
        {
            //Simulate the async operation using Task.Delay
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            var existingUser = await GetUserByIdAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
            }
        }
        public async Task DeleteUserAsync(int userId)
        {
            //Simulate the async operation using Task.Delay
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _users.Remove(user);
            }
        }
    }
}
