using System.ComponentModel.DataAnnotations;

namespace Livraria.Models;

class Livro
{
    [Key]
    public int id { get; set; }

    [Required]
    [StringLength(150)]
    public string? titulo { get; set; }

    [Required]
    [StringLength(100)]
    public string? genero { get; set; }

    [Required]
    [StringLength(100)]
    public string? autor { get; set; }
}