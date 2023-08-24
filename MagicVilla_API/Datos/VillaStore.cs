using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1, Nombre="Vista a la Piscina", Ocupantes= 5, MetrosCuadrados=10},
            new VillaDto{Id=2, Nombre="Vista a la Playa", Ocupantes=8, MetrosCuadrados= 15},
            new VillaDto{Id=3, Nombre="Vista a la Avenida", Ocupantes=4, MetrosCuadrados= 8}
        };
    }
}
