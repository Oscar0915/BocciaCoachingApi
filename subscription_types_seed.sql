-- Script SQL para crear tipos de suscripción por defecto
-- Ejecutar después de correr la migración AddSubscriptionEntities

-- Tipo de suscripción GRATUITA
INSERT INTO SubscriptionType 
(Name, Description, PriceInCents, AnnualPriceInCents, TeamLimit, AthleteLimit, MonthlyEvaluationLimit, 
HasAdvancedStatistics, HasPremiumChat, IsActive, IsDefault, CreatedAt) 
VALUES 
('Free', 'Plan gratuito con funcionalidades básicas para comenzar', 0, 0, 1, 5, 10, 0, 0, 1, 1, NOW());

-- Tipo de suscripción PREMIUM
INSERT INTO SubscriptionType 
(Name, Description, PriceInCents, AnnualPriceInCents, TeamLimit, AthleteLimit, MonthlyEvaluationLimit, 
HasAdvancedStatistics, HasPremiumChat, IsActive, IsDefault, CreatedAt) 
VALUES 
('Premium', 'Plan premium con estadísticas avanzadas y chat premium', 999, 9990, NULL, NULL, NULL, 1, 1, 1, 0, NOW());

-- Tipo de suscripción PRO
INSERT INTO SubscriptionType 
(Name, Description, PriceInCents, AnnualPriceInCents, TeamLimit, AthleteLimit, MonthlyEvaluationLimit, 
HasAdvancedStatistics, HasPremiumChat, IsActive, IsDefault, CreatedAt) 
VALUES 
('Pro', 'Plan profesional para equipos grandes y organizaciones', 1999, 19990, NULL, NULL, NULL, 1, 1, 1, 0, NOW());

-- Verificar que se insertaron correctamente
SELECT * FROM SubscriptionType WHERE IsActive = 1;
