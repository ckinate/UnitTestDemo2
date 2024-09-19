namespace UsersfluentTestingAPI.API.Models
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            //Any Business Logic
            return await _userRepository.GetUserByIdAsync(userId);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            //Any Business Logic
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task AddUserAsync(User user)
        {
            //Any Business Logic
            await _userRepository.AddUserAsync(user);
        }
        public async Task UpdateUserAsync(User user)
        {
            //Any Business Logic
            await _userRepository.UpdateUserAsync(user);
        }
        public async Task DeleteUserAsync(int userId)
        {
            //Any Business Logic
            await _userRepository.DeleteUserAsync(userId);
        }
    }
}
