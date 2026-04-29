using MemberApi.Interfaces;
using MemberApi.Models;

namespace MemberApi.Repositories
{
    public class MembersRepository : IMembers
    {
        private readonly List<Member> _members = new List<Member>
        {
            new Member{MemberId=1, FirstName="Kirtesh", LastName="Shah", Address="Vadodara" },
            new Member{MemberId=2, FirstName="Nitya", LastName="Shah", Address="Vadodara" },
            new Member{MemberId=3, FirstName="Dilip", LastName="Shah", Address="Vadodara" },
            new Member{MemberId=4, FirstName="Atul", LastName="Shah", Address="Vadodara" },
            new Member{MemberId=5, FirstName="Swati", LastName="Shah", Address="Vadodara" },
            new Member{MemberId=6, FirstName="Rashmi", LastName="Shah", Address="Vadodara" },
        };

        public List<Member> GetAllMembers()
        {
            return _members;
        }

        public Member GetMember(int id)
        {
            return _members.FirstOrDefault(x => x.MemberId == id);
        }
    }
}