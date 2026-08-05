﻿namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class AddAssessStrengthDto
    {
        public required string Description { get; set; }
        public required Guid TeamId  { get; set; }
        public required Guid CoachId { get; set; }
    }
}
