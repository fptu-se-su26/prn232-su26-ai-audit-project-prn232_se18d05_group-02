using System.Security.Claims;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using WanderXServer.Dtos.Account;using WanderXServer.Services;
namespace WanderXServer.Controllers;
[ApiController,Route("api/admin/users"),Authorize(Policy="USER_MANAGE")]
public sealed class AdminUsersController:ControllerBase{private readonly AccountManagementService _s;public AdminUsersController(AccountManagementService s)=>_s=s;
 [HttpGet]public async Task<IActionResult> List([FromQuery]AdminUserQuery q)=>Ok(await _s.List(q));[HttpGet("{id:guid}")]public async Task<IActionResult> Detail(Guid id)=>await Run(()=>_s.Detail(id));
 [HttpPut("{id:guid}/roles"),Authorize(Policy="ROLE_ASSIGN")]public async Task<IActionResult> Role(Guid id,ChangeRoleRequest r)=>await Run(()=>_s.ChangeRole(Actor(),id,r));
 [HttpPost("{id:guid}/lock"),Authorize(Policy="USER_LOCK")]public async Task<IActionResult> Lock(Guid id,LockUserRequest r)=>await Run(()=>_s.Lock(Actor(),id,r));
 [HttpPost("{id:guid}/unlock"),Authorize(Policy="USER_LOCK")]public async Task<IActionResult> Unlock(Guid id,UnlockUserRequest r)=>await Run(()=>_s.Unlock(Actor(),id,r));
 private Guid Actor()=>Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);private async Task<IActionResult> Run<T>(Func<Task<T>> a){try{return Ok(await a());}catch(KeyNotFoundException e){return Error(404,e.Message);}catch(ArgumentException e){return Error(400,e.Message);}catch(InvalidOperationException e){return Error(409,e.Message);}}private ObjectResult Error(int status,string code)=>StatusCode(status,new{code,message=code.Replace('_',' ')});}
[ApiController,Route("api/auth/phone"),Authorize]
public sealed class PhoneVerificationController:ControllerBase{private readonly AccountManagementService _s;public PhoneVerificationController(AccountManagementService s)=>_s=s;
 [HttpPost("send-otp")]public async Task<IActionResult> Send(SendPhoneOtpRequest r){try{return Ok(await _s.SendOtp(UserId(),r));}catch(ArgumentException e){return BadRequest(new{code=e.Message});}catch(InvalidOperationException e){return StatusCode(e.Message=="SMS_PROVIDER_ERROR"?503:429,new{code=e.Message});}}
 [HttpPost("verify-otp")]public async Task<IActionResult> Verify(VerifyPhoneOtpRequest r){try{await _s.VerifyOtp(UserId(),r);return Ok(new{message="Phone number verified."});}catch(InvalidOperationException e){return BadRequest(new{code=e.Message});}}
 private Guid UserId()=>Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);}