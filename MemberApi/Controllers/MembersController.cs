using MemberApi.Models;
using MemberApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _service;

        public MembersController(IMemberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Member>>> GetAllMembers()
        {
            var result = await _service.GetAllMembersAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            try
            {
                var member = await _service.GetMemberByIdAsync(id);
                return Ok(member);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}