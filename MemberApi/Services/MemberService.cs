using MemberApi.Interfaces;
using MemberApi.Models;

namespace MemberApi.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMembers _repository;

        public MemberService(IMembers repository)
        {
            _repository = repository;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            // Simulate async (later this will be DB call)
            return await Task.FromResult(_repository.GetAllMembers());
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            var member = _repository.GetMember(id);

            if (member == null)
                throw new Exception("Member not found");

            return await Task.FromResult(member);
        }
    }
}