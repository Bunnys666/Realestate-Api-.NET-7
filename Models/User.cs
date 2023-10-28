using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Api.Models
{
	public class User
	{
		public int Id { get; set; }

		//[Required, MinLength(5), MaxLength(12)]
		public string Name { get; set; }

		//[Required]
		//[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		//[Required, MinLength(8) , MaxLength(20)]
		public string Password { get; set; }

		//[Required, MinLength(8), MaxLength(15)]
		public string Phone { get; set; }

		// 1 to M between the user and property
		public ICollection<Property> Properties { get; set; }
	}
}

