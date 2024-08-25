using CleanBase.Core.Services.Core.Generic;
using Core.Entities;
using Core.ViewModels.Requests.User;
using Core.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public interface 
		IUserService : IServiceBase<User, UserRequest, UserResponse, UserGetAllRequest>
	{
		public Task<UserResponse> GetUserRandomMessage(Guid id);

		public Task<bool> IsValidHuman(UserRequest userRequest);
		Task TriggerUpdateUserAge();
	}
}
