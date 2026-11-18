-- Cuestionario de bienestar McLean (migración 20261118203405_McLeanWellnessQuestionnaire).
-- Los registros existentes quedan como ScaleVersion = 1 (cuestionario anterior, escala 1-7).
-- Se puede ejecutar más de una vez (MariaDB: ADD COLUMN IF NOT EXISTS).

ALTER TABLE `DailyWellness`
    ADD COLUMN IF NOT EXISTS `Mood` int NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS `ReadinessLevel` varchar(10) CHARACTER SET utf8mb4 NULL,
    ADD COLUMN IF NOT EXISTS `ScaleVersion` int NOT NULL DEFAULT 1,
    ADD COLUMN IF NOT EXISTS `VetoApplied` tinyint(1) NOT NULL DEFAULT 0;

-- OPCIONAL: solo si usas "dotnet ef database update" (existe la tabla __EFMigrationsHistory).
-- INSERT IGNORE INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
-- VALUES ('20261118203405_McLeanWellnessQuestionnaire', '9.0.8');
