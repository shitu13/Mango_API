using Mango_api.Models.Dto;

namespace Mango_api.Data
{
    public static class MangoStore
    {
        public static List<MangoDTO> mangoList= new List<MangoDTO>
            {
                new MangoDTO{Id=1, Name="Fazli", Price=350, Weight=4},
                new MangoDTO{Id=2, Name="Lengra", Price=400, Weight=5}
            };
    }
}
