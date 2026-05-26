namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    /// <summary>Respuesta combinada con los tipos de microciclo configurados y los que están construidos en la aplicación</summary>
    public class MicrocycleTypesOverviewDto
    {
        /// <summary>Tipos de microciclo configurados en el catálogo del sistema</summary>
        public List<MicrocycleTypeWithCountDto> ConfiguredTypes { get; set; } = new();

        /// <summary>Resumen de los tipos de microciclos que están efectivamente construidos en macrociclos</summary>
        public List<BuiltMicrocycleTypeSummaryDto> BuiltTypes { get; set; } = new();
    }

    /// <summary>Tipo de microciclo del catálogo incluyendo cuántos microciclos han sido construidos con ese tipo</summary>
    public class MicrocycleTypeWithCountDto
    {
        public string MicrocycleTypeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public List<MicrocycleTypeDayDto> Days { get; set; } = new();

        /// <summary>Cantidad de microciclos construidos en macrociclos que utilizan este tipo</summary>
        public int TotalBuilt { get; set; }
    }

    /// <summary>Resumen de un tipo de microciclo construido en la aplicación</summary>
    public class BuiltMicrocycleTypeSummaryDto
    {
        /// <summary>Nombre del tipo tal como está almacenado en los microciclos (ej: ordinario, choque, activacion)</summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>Cantidad de microciclos construidos con este tipo</summary>
        public int Count { get; set; }
    }
}

