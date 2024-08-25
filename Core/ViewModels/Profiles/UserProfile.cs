using CleanBase.Core.ViewModels.Profiles;
using Core.Entities;
using Core.ViewModels.Requests.User;
using Core.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Profiles
{
	public partial class UserProfile : ProfileBase
	{
		protected override void DefaultMapping()
		{
			CreateMap<UserRequest, User>();
			CreateMap<User, UserResponse>();
			CreateMap<UserRequest, UserResponse>();	
		}
	}
}
