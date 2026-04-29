using MemberApi.Models;

namespace MemberApi.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();
        Task<Member> GetMemberByIdAsync(int id);
    }
}