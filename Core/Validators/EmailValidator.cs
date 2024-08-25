using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators
{
	public partial class EmailValidator : AbstractValidator<string>
	{
		public EmailValidator()
		{
			RuleFor(x => x).NotNull().NotEmpty().EmailAddress();
		}
	}
}
