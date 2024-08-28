using CleanBase.Core.Data.UnitOfWorks;
using CleanBase.Core.Domain.Domain.Services.GenericBase;
using CleanBase.Core.Domain.Exceptions;
using CleanBase.Core.Security;
using CleanBase.Core.Services.Core.Base;
using CleanBase.Core.Validators.Generic;
using Core.Entities;
using Core.Helpers;
using Core.Services;
using Core.ViewModels.Requests.User;
using Core.ViewModels.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
	public class UserService : ServiceBase<User, UserRequest, UserResponse, UserGetAllRequest>, IUserService
	{
		private readonly IValidator<UserRequest> _validator;
		public UserService(ICoreProvider coreProvider, IUnitOfWork unitOfWork) : base(coreProvider, unitOfWork)
		{
		}

		public async Task<UserResponse> GetUserRandomMessage(Guid id)
		{
			//Log.Information("haha");

			//var user = await this.Repository.FirstOrDefaultAsync(u => u.Id == id);

			////Guard.ThrowIfFalse(user == null, "value not found", "value_not_found", httpCode: 400);
			////if(true)
			////{
			////	throw new DomainException("haha","400",null);
			////}
			Logger.Information("start");
			var list = this.Repository.GetAll().ToList();
			list.ForEach(x =>
			{
				x.Id = Guid.NewGuid();
			});
			Repository.BatchAdd(list);
			return null;
		}

		public async Task<bool> IsValidHuman(UserRequest userRequest)
		{
			return _validator.Validate(userRequest).IsValid;
		}

		public async Task TriggerUpdateUserAge()
		{
			//var users = await this.Repository.GetAllAsync();

			//foreach (var user in users)
			//{
			//	user.Age++;
			//}

			//var userslist = await users.ToListAsync();
			//Repository.BatchUpdate(userslist,true);
				
		}
	}
}
