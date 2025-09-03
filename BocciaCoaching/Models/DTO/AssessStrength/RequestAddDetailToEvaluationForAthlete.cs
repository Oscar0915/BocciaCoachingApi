﻿namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class RequestAddDetailToEvaluationForAthlete
    {
        public int BoxNumber { get; set; }
        public int ThrowOrder { get; set; }
        public decimal? TargetDistance { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public bool Status { get; set; } = true;
        public int AthleteId { get; set; }
        public int AssessStrengthId { get; set; }
    }
}
