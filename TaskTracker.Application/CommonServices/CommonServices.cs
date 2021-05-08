

namespace TaskTracker
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Linq.Extensions;

    using Authorization.Users;
    using Authorization.Users.Dto;
    using Mailers;

    public class CommonServices : ITransientDependency
    {
        private readonly IRepository<User, long> _userRepository;

        private readonly IApplicationMailer _mailer = new ApplicationMailer();

        public CommonServices(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserListDto> GetUser(GetUserInput input)
        {
            var toReturn = await _userRepository.GetAll()
                .WhereIf(input.UserId != null, x => x.Id == input.UserId)
                .WhereIf(!string.IsNullOrEmpty(input.EmailAddress), x => x.EmailAddress == input.EmailAddress)
                .FirstOrDefaultAsync();

            return toReturn.MapTo<UserListDto>();
        }

        public async Task<ListResultOutput<UserListDto>> GetUsers()
        {
            var toReturn = await _userRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.Surname).ToListAsync();
            return new ListResultOutput<UserListDto>(toReturn.MapTo<List<UserListDto>>());
        }

        public async Task<ListResultOutput<UserListDto>> GetUsersByRole(Enum.Role role)
        {
            var toReturn = await _userRepository.GetAll()
                .Include(x => x.Roles)
                .Where(r => r.IsActive && r.Roles.Any(p => p.RoleId == (int)role)).OrderBy(x => x.Surname).ToListAsync();
            return new ListResultOutput<UserListDto>(toReturn.MapTo<List<UserListDto>>());
        }

        public async Task<ListResultOutput<UserBasicListDto>> GetBasicUsersByRole(Enum.Role role)
        {
            var toReturn = await _userRepository.GetAll()
                .Where(r => r.IsActive && r.Roles.Any(p => p.RoleId == (int)role)).OrderBy(x => x.Surname).ToListAsync();
            return new ListResultOutput<UserBasicListDto>(toReturn.MapTo<List<UserBasicListDto>>());
        }

        public async Task ContactMe(ContactMeInput input)
        {
            var sender = await _userRepository.GetAsync(input.SenderId);
            var postOwner = await _userRepository.GetAsync(input.PostOwnerId);
            var recipient = postOwner.EmailAddress;
            var output = new NotifyPostOwnerOutput { Sender = sender.Name + " " + sender.Surname, SenderEmail = sender.EmailAddress, Subject = input.Subject, Message = input.Message };
            await _mailer.PostOwnerNotified(output, recipient).SendAsync();
        }
    }
}
