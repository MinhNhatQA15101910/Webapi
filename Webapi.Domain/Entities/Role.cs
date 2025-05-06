using System;
using Microsoft.AspNetCore.Identity;

namespace Webapi.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
