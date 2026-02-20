﻿namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class AddAssessStrengthDto
    {
        public required string Description { get; set; }
        public required int TeamId  { get; set; }
        public required int CoachId { get; set; }
    }
}
