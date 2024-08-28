using CleanBase.Core.Api.Controllers;
using CleanBase.Core.Domain.Generic;
using CleanBase.Core.Services.Core.Base;
using CleanBase.Core.ViewModels.Response;
using CleanBase.Core.ViewModels.Response.Generic;
using Core.Entities;
using Core.Services;
using Core.ViewModels.Requests.User;
using Core.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APis.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = false)]
	public partial class UserController : CRUDBaseController<User, UserRequest, UserResponse, UserGetAllRequest, IUserService>
	{
		public UserController(ICoreProvider coreProvider, IUserService service) : base(coreProvider, service)
		{
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ActionResponse<UserResponse>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FailActionResponse), (int)HttpStatusCode.BadRequest)]
		public virtual async Task<IActionResult> GetById(Guid id)
		{
			return await GetByIdInternal(id);
		}

		[HttpPost("get-basic")]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ActionResponse<ListResult<UserResponse>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FailActionResponse), (int)HttpStatusCode.BadRequest)]
		public virtual async Task<IActionResult> GetAll([FromBody] UserGetAllRequest request)
		{
			return await GetAllInternal(request);
		}


		[HttpPost]
		[AllowAnonymous]

		[ProducesResponseType(typeof(ActionResponse<UserResponse>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FailActionResponse), (int)HttpStatusCode.BadRequest)]
		public virtual async Task<IActionResult> CreateOrUpdate([FromBody] UserRequest entity)
		{
			return await CreateOrUpdateInternal(entity);
		}


		[HttpPost("delete/{id}")]
		[AllowAnonymous]

		[ProducesResponseType(typeof(ActionResponse<bool>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FailActionResponse), (int)HttpStatusCode.BadRequest)]
		public virtual async Task<IActionResult> DeActive(Guid id)
		{
			return await DeActiveInternal(id);
		}

		[HttpGet("get-user-message/{id}")]
		[AllowAnonymous]

		[ProducesResponseType(typeof(ActionResponse<UserResponse>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FailActionResponse), (int)HttpStatusCode.BadRequest)]
		public virtual async Task<IActionResult> GetUserMessage(Guid id)
		{
			var result = await this.Service.GetUserRandomMessage(id);
			return CreateSuccessResult(result);
		}

		[HttpPost("check-valid-user/{id}")]
		[AllowAnonymous]

		[ProducesResponseType(typeof(ActionResponse<bool>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(FailActionResponse), (int)HttpStatusCode.BadRequest)]
		public virtual async Task<IActionResult> CheckValidUser(UserRequest id)
		{
			var result = await this.Service.IsValidHuman(id);
			return CreateSuccessResult(result);
		}
	}
}
