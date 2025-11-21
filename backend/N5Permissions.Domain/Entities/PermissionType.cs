using System;
using System.Configuration;

namespace N5Permissions.Domain.Entities
{
	public class PermissionType
	{
		public int Id { get; private set; }
		public string Description { get; private set; } = string.Empty;

        public PermissionType(string description)
		{
			SetDescription(description);
		}

		public void SetDescription(string description)
		{
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Descrição inválida!");

			Description = description;
		}

    }
}
