using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Api.Models
{
	public class Category
	{
		public int Id { get; set; }

        //[Required, MinLength(5), MaxLength(12)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

		public ICollection<Property> Properties { get; set; }	

	}
}