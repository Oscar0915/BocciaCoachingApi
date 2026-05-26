namespace BocciaCoaching.Models.DTO.Macrocycle
{
    /// <summary>DTO para actualizar los días y porcentajes de un microciclo concreto</summary>
    public class UpdateMicycleDaysDto
    {
        public int MicrocycleId { get; set; }

        /// <summary>
        /// Días con sus porcentajes de lanzamiento. Reemplaza completamente los días actuales del microciclo.
        /// Si se envía vacío, se borran todos los días.
        /// </summary>
        public List<MicrocycleDayUpdateDto> Days { get; set; } = new();
    }
}

