using System.ComponentModel.DataAnnotations;

namespace Mango_api.Models.Dto
{
    public class MangoDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Price {  get; set; }
    }
}
