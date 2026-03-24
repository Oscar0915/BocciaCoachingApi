-- =====================================================
-- Script SQL para crear las tablas de evaluación 
-- de control de dirección (AssessDirection)
-- Base de datos: MySQL
-- Fecha: 2026-03-23
-- =====================================================

-- 1. Tabla cabecera: AssessDirection
CREATE TABLE IF NOT EXISTS `AssessDirection` (
    `AssessDirectionId` int NOT NULL AUTO_INCREMENT,
    `EvaluationDate` datetime(6) NOT NULL,
    `Description` longtext NULL,
    `State` longtext NULL,
    `TeamId` int NOT NULL,
    `CoachId` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`AssessDirectionId`),
    CONSTRAINT `FK_AssessDirection_Teams_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `Teams` (`TeamId`) ON DELETE CASCADE,
    CONSTRAINT `FK_AssessDirection_Users_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. Tabla detalle de lanzamientos: EvaluationDetailDirection
-- Contiene los 24 lanzamientos por atleta
-- 8 a 3 metros (4 box derecho + 4 box izquierdo)
-- 8 a 6 metros (4 box derecho + 4 box izquierdo)
-- 8 a 9 metros (4 box derecho + 4 box izquierdo)
-- Incluye campos DeviatedRight y DeviatedLeft para indicar desviación
CREATE TABLE IF NOT EXISTS `EvaluationDetailDirection` (
    `EvaluationDetailDirectionId` int NOT NULL AUTO_INCREMENT,
    `BoxNumber` int NOT NULL COMMENT '1=Box Derecho, 2=Box Izquierdo',
    `ThrowOrder` int NOT NULL COMMENT 'Orden del lanzamiento (1-24)',
    `TargetDistance` decimal(65,30) NULL COMMENT 'Distancia objetivo: 3, 6 o 9 metros',
    `ScoreObtained` decimal(65,30) NULL COMMENT 'Puntaje obtenido en el lanzamiento',
    `Observations` longtext NULL,
    `Status` tinyint(1) NOT NULL DEFAULT 1,
    `AthleteId` int NOT NULL,
    `AssessDirectionId` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `CoordinateX` double NOT NULL DEFAULT 0,
    `CoordinateY` double NOT NULL DEFAULT 0,
    `DeviatedRight` tinyint(1) NOT NULL DEFAULT 0 COMMENT 'Indica si el lanzamiento se desvió a la derecha',
    `DeviatedLeft` tinyint(1) NOT NULL DEFAULT 0 COMMENT 'Indica si el lanzamiento se desvió a la izquierda',
    PRIMARY KEY (`EvaluationDetailDirectionId`),
    CONSTRAINT `FK_EvaluationDetailDirection_Users_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE,
    CONSTRAINT `FK_EvaluationDetailDirection_AssessDirection_AssessDirectionId` FOREIGN KEY (`AssessDirectionId`) REFERENCES `AssessDirection` (`AssessDirectionId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. Tabla puente atletas-evaluación: AthletesToEvaluatedDirection
CREATE TABLE IF NOT EXISTS `AthletesToEvaluatedDirection` (
    `AthletesToEvaluatedDirectionId` int NOT NULL AUTO_INCREMENT,
    `CoachId` int NOT NULL,
    `AthleteId` int NOT NULL,
    `AssessDirectionId` int NOT NULL,
    PRIMARY KEY (`AthletesToEvaluatedDirectionId`),
    CONSTRAINT `FK_AthletesToEvaluatedDirection_Users_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE,
    CONSTRAINT `FK_AthletesToEvaluatedDirection_Users_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE,
    CONSTRAINT `FK_AthletesToEvaluatedDirection_AssessDirection_AssessDirectionId` FOREIGN KEY (`AssessDirectionId`) REFERENCES `AssessDirection` (`AssessDirectionId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. Tabla estadísticas: DirectionStatistics
-- Se calculan a partir de 24 lanzamientos (no 36 como en fuerza)
-- Incluye estadísticas de desviación izquierda/derecha
CREATE TABLE IF NOT EXISTS `DirectionStatistics` (
    `DirectionStatisticsId` int NOT NULL AUTO_INCREMENT,
    
    -- Estadísticas generales
    `EffectivenessPercentage` double NOT NULL DEFAULT 0 COMMENT 'Porcentaje de precisión general',
    `AccuracyPercentage` double NOT NULL DEFAULT 0 COMMENT 'Porcentaje de efectividad general',
    `EffectiveThrow` int NOT NULL DEFAULT 0 COMMENT 'Lanzamientos efectivos (score >= 3)',
    `FailedThrow` int NOT NULL DEFAULT 0 COMMENT 'Lanzamientos fallidos (score < 3)',
    
    -- Lanzamientos efectivos por distancia
    `ShortThrow` int NOT NULL DEFAULT 0 COMMENT 'Lanzamientos efectivos a 3 metros',
    `MediumThrow` int NOT NULL DEFAULT 0 COMMENT 'Lanzamientos efectivos a 6 metros',
    `LongThrow` double NOT NULL DEFAULT 0 COMMENT 'Lanzamientos efectivos a 9 metros',
    
    -- Porcentajes de efectividad por distancia
    `ShortEffectivenessPercentage` double NOT NULL DEFAULT 0 COMMENT 'Efectividad a 3 metros (de 8 lanzamientos)',
    `MediumEffectivenessPercentage` double NOT NULL DEFAULT 0 COMMENT 'Efectividad a 6 metros (de 8 lanzamientos)',
    `LongEffectivenessPercentage` double NOT NULL DEFAULT 0 COMMENT 'Efectividad a 9 metros (de 8 lanzamientos)',
    
    -- Precisión por distancia (puntos obtenidos)
    `ShortThrowAccuracy` int NOT NULL DEFAULT 0 COMMENT 'Puntos obtenidos a 3 metros',
    `MediumThrowAccuracy` int NOT NULL DEFAULT 0 COMMENT 'Puntos obtenidos a 6 metros',
    `LongThrowAccuracy` int NOT NULL DEFAULT 0 COMMENT 'Puntos obtenidos a 9 metros',
    
    -- Porcentajes de precisión por distancia
    `ShortAccuracyPercentage` double NOT NULL DEFAULT 0 COMMENT 'Precisión a 3 metros (max 40 pts)',
    `MediumAccuracyPercentage` double NOT NULL DEFAULT 0 COMMENT 'Precisión a 6 metros (max 40 pts)',
    `LongAccuracyPercentage` double NOT NULL DEFAULT 0 COMMENT 'Precisión a 9 metros (max 40 pts)',
    
    -- Estadísticas de desviación
    `TotalDeviatedRight` int NOT NULL DEFAULT 0 COMMENT 'Total de lanzamientos desviados a la derecha',
    `TotalDeviatedLeft` int NOT NULL DEFAULT 0 COMMENT 'Total de lanzamientos desviados a la izquierda',
    `DeviatedRightPercentage` double NOT NULL DEFAULT 0 COMMENT 'Porcentaje de desviación a la derecha',
    `DeviatedLeftPercentage` double NOT NULL DEFAULT 0 COMMENT 'Porcentaje de desviación a la izquierda',
    
    -- Desviaciones por distancia
    `ShortDeviatedRight` int NOT NULL DEFAULT 0 COMMENT 'Desviaciones derecha a 3 metros',
    `ShortDeviatedLeft` int NOT NULL DEFAULT 0 COMMENT 'Desviaciones izquierda a 3 metros',
    `MediumDeviatedRight` int NOT NULL DEFAULT 0 COMMENT 'Desviaciones derecha a 6 metros',
    `MediumDeviatedLeft` int NOT NULL DEFAULT 0 COMMENT 'Desviaciones izquierda a 6 metros',
    `LongDeviatedRight` int NOT NULL DEFAULT 0 COMMENT 'Desviaciones derecha a 9 metros',
    `LongDeviatedLeft` int NOT NULL DEFAULT 0 COMMENT 'Desviaciones izquierda a 9 metros',
    
    -- Foreign Keys
    `AssessDirectionId` int NOT NULL,
    `AthleteId` int NOT NULL,
    
    PRIMARY KEY (`DirectionStatisticsId`),
    CONSTRAINT `FK_DirectionStatistics_AssessDirection_AssessDirectionId` FOREIGN KEY (`AssessDirectionId`) REFERENCES `AssessDirection` (`AssessDirectionId`) ON DELETE CASCADE,
    CONSTRAINT `FK_DirectionStatistics_Users_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================
-- Índices para mejorar rendimiento de consultas
-- =====================================================
CREATE INDEX `IX_AssessDirection_TeamId` ON `AssessDirection` (`TeamId`);
CREATE INDEX `IX_AssessDirection_CoachId` ON `AssessDirection` (`CoachId`);
CREATE INDEX `IX_AssessDirection_State` ON `AssessDirection` (`State`(10));

CREATE INDEX `IX_EvaluationDetailDirection_AthleteId` ON `EvaluationDetailDirection` (`AthleteId`);
CREATE INDEX `IX_EvaluationDetailDirection_AssessDirectionId` ON `EvaluationDetailDirection` (`AssessDirectionId`);

CREATE INDEX `IX_AthletesToEvaluatedDirection_CoachId` ON `AthletesToEvaluatedDirection` (`CoachId`);
CREATE INDEX `IX_AthletesToEvaluatedDirection_AthleteId` ON `AthletesToEvaluatedDirection` (`AthleteId`);
CREATE INDEX `IX_AthletesToEvaluatedDirection_AssessDirectionId` ON `AthletesToEvaluatedDirection` (`AssessDirectionId`);

CREATE INDEX `IX_DirectionStatistics_AssessDirectionId` ON `DirectionStatistics` (`AssessDirectionId`);
CREATE INDEX `IX_DirectionStatistics_AthleteId` ON `DirectionStatistics` (`AthleteId`);

