namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Cuestionario de bienestar registrado, con su semáforo y la retroalimentación para mostrar
    /// al atleta. En registros antiguos (<see cref="ScaleVersion"/> = 1) no hay semáforo ni feedback.
    /// </summary>
    public class ResponseDailyWellnessDto
    {
        public Guid DailyWellnessId { get; set; }
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public Guid? TeamId { get; set; }
        public DateTime AssessmentDate { get; set; }

        /// <summary>1 = cuestionario anterior (4 ítems, 1-7); 2 = McLean (5 ítems, 1-5).</summary>
        public int ScaleVersion { get; set; }

        public int Fatigue { get; set; }
        public int Sleep { get; set; }
        public int MusclePain { get; set; }
        public int Stress { get; set; }
        public int Mood { get; set; }

        /// <summary>Suma de los ítems (5-25 en McLean).</summary>
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }

        /// <summary>"Green", "Yellow" o "Red". Null en registros antiguos.</summary>
        public string? ReadinessLevel { get; set; }

        /// <summary>true si el total era Verde pero un ítem ≤ 2 lo degradó a Amarillo.</summary>
        public bool VetoApplied { get; set; }

        public string? Observations { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Mensajes para el atleta. Null en registros antiguos.</summary>
        public WellnessFeedbackDto? Feedback { get; set; }
    }

    /// <summary>ES: Retroalimentación que la app muestra justo después de enviar el cuestionario.</summary>
    public class WellnessFeedbackDto
    {
        /// <summary>"Green", "Yellow" o "Red".</summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>Óptimo, Precaución o Peligro.</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>Título de la alerta (p. ej. "Modificación Inteligente").</summary>
        public string Title { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;
        public List<string> TrainingActions { get; set; } = new();
        public List<string> LifestyleActions { get; set; } = new();

        /// <summary>Alertas específicas por cada ítem con puntaje ≤ 2.</summary>
        public List<WellnessItemAlertDto> ItemAlerts { get; set; } = new();
    }

    public class WellnessItemAlertDto
    {
        /// <summary>Fatigue, Sleep, MusclePain, Stress o Mood.</summary>
        public string Item { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// ES: Respuesta de <c>GET /api/v1/wellness/me/today</c>: lo que la app consulta al ingresar.
    /// </summary>
    public class WellnessTodayStatusDto
    {
        /// <summary>Día local del atleta que se evaluó.</summary>
        public DateTime Date { get; set; }

        /// <summary>Zona horaria usada para determinar "hoy".</summary>
        public string TimeZone { get; set; } = string.Empty;

        /// <summary>true si el usuario es atleta y por tanto debe responder el cuestionario cada día.</summary>
        public bool Required { get; set; }

        /// <summary>true si ya lo respondió hoy.</summary>
        public bool Completed { get; set; }

        /// <summary>true si la app debe mostrar el cuestionario ahora (Required y no Completed).</summary>
        public bool MustComplete => Required && !Completed;

        /// <summary>El cuestionario de hoy (con semáforo y feedback) si ya lo respondió.</summary>
        public ResponseDailyWellnessDto? Assessment { get; set; }
    }

    /// <summary>ES: Resumen diario de un equipo para el entrenador.</summary>
    public class TeamWellnessSummaryDto
    {
        public Guid TeamId { get; set; }
        public DateTime Date { get; set; }
        public int TotalAthletes { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Green { get; set; }
        public int Yellow { get; set; }
        public int Red { get; set; }

        /// <summary>Cuestionarios del día, de peor a mejor estado (rojos primero).</summary>
        public List<ResponseDailyWellnessDto> Assessments { get; set; } = new();

        /// <summary>Atletas del equipo que aún no lo responden.</summary>
        public List<WellnessPendingAthleteDto> PendingAthletes { get; set; } = new();
    }

    public class WellnessPendingAthleteDto
    {
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
    }

    /// <summary>ES: Definición del cuestionario para que la app lo pinte desde el backend.</summary>
    public class WellnessQuestionnaireDto
    {
        public int ScaleVersion { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public List<WellnessQuestionDto> Items { get; set; } = new();
    }

    public class WellnessQuestionDto
    {
        /// <summary>Nombre del campo a enviar: fatigue, sleep, musclePain, stress o mood.</summary>
        public string Field { get; set; } = string.Empty;
        public string Dimension { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;

        /// <summary>Etiquetas de 1 a 5 (índice 0 = valor 1).</summary>
        public List<string> Options { get; set; } = new();
    }
}
