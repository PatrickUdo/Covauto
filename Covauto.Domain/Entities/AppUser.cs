using Microsoft.AspNetCore.Identity;

namespace Covauto.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public ICollection<LeenAutoReservering> LeenAutoReserveringen { get; set; }
    }
}
