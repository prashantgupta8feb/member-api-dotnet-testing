using MemberApi.Models;

namespace MemberApi.Interfaces
{
    public interface IMembers
    {
        List<Member> GetAllMembers();
        Member GetMember(int id);
    }
}