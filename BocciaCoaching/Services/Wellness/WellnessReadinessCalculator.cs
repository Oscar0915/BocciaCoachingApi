using BocciaCoaching.Models.DTO.Wellness;

namespace BocciaCoaching.Services.Wellness
{
    /// <summary>Semáforo de disponibilidad para entrenar.</summary>
    public enum ReadinessLevel
    {
        Green,
        Yellow,
        Red
    }

    /// <summary>ES: Resultado del cálculo de un cuestionario McLean.</summary>
    public sealed record WellnessEvaluation(
        int TotalScore,
        double AverageScore,
        ReadinessLevel Level,
        bool VetoApplied,
        WellnessFeedbackDto Feedback);

    /// <summary>
    /// ES: Lógica del cuestionario de bienestar de McLean et al. (2010), derivado del Índice de Hooper.
    /// - 5 ítems de 1 (pésimo) a 5 (excelente). Total de 5 a 25.
    /// - Semáforo por total: Verde 20-25 (Óptimo), Amarillo 15-19 (Precaución), Rojo 5-14 (Peligro).
    /// - Veto individual: si el total es Verde pero algún ítem es ≤ 2, se degrada a Amarillo.
    /// - Alertas específicas para cada ítem ≤ 2, en cualquier nivel.
    /// Sin dependencias: se puede probar de forma aislada.
    /// </summary>
    public static class WellnessReadinessCalculator
    {
        public const int MinItemScore = 1;
        public const int MaxItemScore = 5;
        public const int ItemCount = 5;
        public const int GreenMinTotal = 20;
        public const int YellowMinTotal = 15;
        public const int VetoItemThreshold = 2;

        public static bool AreValid(int fatigue, int sleep, int musclePain, int stress, int mood)
            => new[] { fatigue, sleep, musclePain, stress, mood }.All(v => v is >= MinItemScore and <= MaxItemScore);

        public static WellnessEvaluation Evaluate(int fatigue, int sleep, int musclePain, int stress, int mood)
        {
            if (!AreValid(fatigue, sleep, musclePain, stress, mood))
                throw new ArgumentOutOfRangeException(nameof(fatigue), "Todos los ítems deben estar entre 1 y 5.");

            var total = fatigue + sleep + musclePain + stress + mood;
            var average = Math.Round(total / (double)ItemCount, 2);

            var level = total >= GreenMinTotal ? ReadinessLevel.Green
                : total >= YellowMinTotal ? ReadinessLevel.Yellow
                : ReadinessLevel.Red;

            var alerts = BuildItemAlerts(fatigue, sleep, musclePain, stress, mood);

            // Regla de veto: Verde por total, pero con algún ítem crítico → Amarillo.
            var vetoApplied = level == ReadinessLevel.Green && alerts.Count > 0;
            if (vetoApplied)
                level = ReadinessLevel.Yellow;

            var feedback = BuildFeedback(level, vetoApplied);
            feedback.ItemAlerts = alerts;

            return new WellnessEvaluation(total, average, level, vetoApplied, feedback);
        }

        /// <summary>
        /// ES: Reconstruye la retroalimentación de un registro guardado (los mensajes no se guardan en la BD:
        /// así se pueden mejorar los textos sin migrar datos).
        /// </summary>
        public static WellnessFeedbackDto? FeedbackFor(ResponseDailyWellnessDto record)
        {
            if (record.ScaleVersion != Models.Entities.DailyWellness.McLeanScaleVersion ||
                !AreValid(record.Fatigue, record.Sleep, record.MusclePain, record.Stress, record.Mood))
                return null;

            return Evaluate(record.Fatigue, record.Sleep, record.MusclePain, record.Stress, record.Mood).Feedback;
        }

        private static List<WellnessItemAlertDto> BuildItemAlerts(int fatigue, int sleep, int musclePain, int stress, int mood)
        {
            var alerts = new List<WellnessItemAlertDto>();

            void AddIfCritical(string item, int score, string message)
            {
                if (score <= VetoItemThreshold)
                    alerts.Add(new WellnessItemAlertDto { Item = item, Score = score, Message = message });
            }

            AddIfCritical("MusclePain", musclePain,
                "Atención: Reportaste dolor muscular agudo. Evita trabajar esa zona específica hoy para prevenir un desgarro o mayor lesión.");
            AddIfCritical("Sleep", sleep,
                "Tu coordinación y tiempo de reacción están reducidos por falta de sueño. Evita lanzamientos técnicos complejos.");
            AddIfCritical("Stress", stress,
                "El estrés mental alto aumenta la percepción de esfuerzo. Sé amable contigo mismo/a si el entrenamiento se siente más pesado de lo normal.");
            AddIfCritical("Fatigue", fatigue,
                "Reportaste fatiga alta. Prioriza la calidad sobre la cantidad de lanzamientos y toma pausas más largas entre series.");
            AddIfCritical("Mood", mood,
                "Tu estado de ánimo está bajo hoy. Coméntaselo a tu entrenador si lo necesitas; ajustar la sesión también es parte del entrenamiento.");

            return alerts;
        }

        private static WellnessFeedbackDto BuildFeedback(ReadinessLevel level, bool vetoApplied) => level switch
        {
            ReadinessLevel.Green => new WellnessFeedbackDto
            {
                Level = nameof(ReadinessLevel.Green),
                Status = "Óptimo",
                Title = "Listo para entrenar",
                Diagnosis = "El sistema nervioso y muscular está recuperado. Alta capacidad de adaptación al estrés físico.",
                TrainingActions = { "Puedes entrenar con la intensidad planificada." },
                LifestyleActions = { "Mantén tus rutinas de sueño e hidratación." }
            },
            ReadinessLevel.Yellow => new WellnessFeedbackDto
            {
                Level = nameof(ReadinessLevel.Yellow),
                Status = "Precaución",
                Title = "Modificación Inteligente",
                Diagnosis = vetoApplied
                    ? "Tu puntaje general es bueno, pero reportaste un indicador crítico. Ajusta tu sesión de hoy."
                    : "Tu cuerpo muestra señales de fatiga moderada.",
                TrainingActions =
                {
                    "Reduce la intensidad de tu sesión de hoy un 20%.",
                    "Elimina los ejercicios de fortalecimiento o potencia hoy.",
                    "Mantén la técnica, tu efectividad puede estar más baja."
                },
                LifestyleActions =
                {
                    "Intenta dormir 30 minutos extra esta noche.",
                    "Asegura tu hidratación post-entreno."
                }
            },
            _ => new WellnessFeedbackDto
            {
                Level = nameof(ReadinessLevel.Red),
                Status = "Peligro",
                Title = "Recuperación Obligatoria",
                Diagnosis = "Tu capacidad de recuperación está comprometida. Entrenar fuerte hoy es contraproducente.",
                TrainingActions =
                {
                    "RECOMENDADO: Cambia tu sesión por Descanso Activo: pocos lanzamientos, entrenamiento teórico táctico o estratégico.",
                    "SI DECIDES ENTRENAR: Limita la sesión a 30 minutos y mantén la intensidad del entrenamiento baja."
                },
                LifestyleActions =
                {
                    "Prioridad absoluta al sueño hoy (siestas recomendadas).",
                    "Realiza actividades de descompresión mental (lectura, meditación)."
                }
            }
        };

        /// <summary>ES: Definición del cuestionario (preguntas y etiquetas) para que la app lo pinte.</summary>
        public static WellnessQuestionnaireDto Questionnaire { get; } = new()
        {
            ScaleVersion = Models.Entities.DailyWellness.McLeanScaleVersion,
            Name = "Cuestionario de Bienestar de McLean (2010)",
            Source = "McLean, B. D., et al. (2010). Journal of Science and Medicine in Sport.",
            MinScore = MinItemScore,
            MaxScore = MaxItemScore,
            Items =
            {
                new() { Field = "fatigue", Dimension = "Fatiga", Question = "¿Qué tan fatigado te sientes hoy?",
                    Options = { "Siempre cansado", "Muy cansado", "Normal", "Fresco", "Muy fresco" } },
                new() { Field = "sleep", Dimension = "Calidad de Sueño", Question = "¿Cómo dormiste anoche?",
                    Options = { "Insomnio / Pésimo", "Sueño agitado", "Dificultad media", "Buen sueño", "Muy reparador" } },
                new() { Field = "musclePain", Dimension = "Dolor Muscular", Question = "¿Sientes dolor muscular general?",
                    Options = { "Muy adolorido", "Adolorido", "Normal", "Poco dolor", "Sin dolor / Genial" } },
                new() { Field = "stress", Dimension = "Niveles de Estrés", Question = "¿Qué tan estresado te sientes?",
                    Options = { "Muy estresado", "Estresado", "Normal", "Relajado", "Muy relajado" } },
                new() { Field = "mood", Dimension = "Estado de Ánimo", Question = "¿Cuál es tu estado de ánimo?",
                    Options = { "Muy molesto/Irritable", "Molesto", "Normal", "Buen humor", "Muy positivo" } }
            }
        };
    }
}
