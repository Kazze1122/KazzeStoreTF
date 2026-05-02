using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KazzeStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

    }
}