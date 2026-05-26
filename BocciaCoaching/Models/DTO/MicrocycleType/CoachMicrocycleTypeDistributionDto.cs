namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    /// <summary>
    /// DTO para consultar la distribución de componentes de entrenamiento personalizada
    /// de un coach para un tipo de microciclo específico.
    /// </summary>
    public class CoachMicrocycleTypeDistributionDto
    {
        public string CoachMicrocycleTypeDistributionId { get; set; } = string.Empty;
        public int CoachId { get; set; }
        public string MicrocycleTypeId { get; set; } = string.Empty;
        public string MicrocycleTypeName { get; set; } = string.Empty;
        public string? MicrocycleTypeShortCode { get; set; }

        /// <summary>Porcentaje de Física General (0.0 a 1.0)</summary>
        public double FisicaGeneral { get; set; }

        /// <summary>Porcentaje de Física Especial (0.0 a 1.0)</summary>
        public double FisicaEspecial { get; set; }

        /// <summary>Porcentaje de Técnica (0.0 a 1.0)</summary>
        public double Tecnica { get; set; }

        /// <summary>Porcentaje de Táctica (0.0 a 1.0)</summary>
        public double Tactica { get; set; }

        /// <summary>Porcentaje de Teórica (0.0 a 1.0)</summary>
        public double Teorica { get; set; }

        /// <summary>Porcentaje de Psicológica (0.0 a 1.0)</summary>
        public double Psicologica { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO para crear o actualizar la distribución personalizada de un coach.
    /// La suma de FisicaGeneral + FisicaEspecial + Tecnica + Tactica + Teorica + Psicologica debe ser 1.0.
    /// </summary>
    public class UpsertCoachMicrocycleTypeDistributionDto
    {
        public int CoachId { get; set; }
        public string MicrocycleTypeId { get; set; } = string.Empty;

        /// <summary>Porcentaje de Física General (0.0 a 1.0)</summary>
        public double FisicaGeneral { get; set; }

        /// <summary>Porcentaje de Física Especial (0.0 a 1.0)</summary>
        public double FisicaEspecial { get; set; }

        /// <summary>Porcentaje de Técnica (0.0 a 1.0)</summary>
        public double Tecnica { get; set; }

        /// <summary>Porcentaje de Táctica (0.0 a 1.0)</summary>
        public double Tactica { get; set; }

        /// <summary>Porcentaje de Teórica (0.0 a 1.0)</summary>
        public double Teorica { get; set; }

        /// <summary>Porcentaje de Psicológica (0.0 a 1.0)</summary>
        public double Psicologica { get; set; }
    }
}

