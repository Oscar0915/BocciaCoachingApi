CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `AssessStrength` (
        `AssessStrengthId` int NOT NULL AUTO_INCREMENT,
        `EvaluationDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_AssessStrength` PRIMARY KEY (`AssessStrengthId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `Rol` (
        `RolId` int NOT NULL AUTO_INCREMENT,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Rol` PRIMARY KEY (`RolId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `User` (
        `UserId` int NOT NULL AUTO_INCREMENT,
        `Dni` longtext CHARACTER SET utf8mb4 NULL,
        `FirstName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `LastName` longtext CHARACTER SET utf8mb4 NULL,
        `Email` longtext CHARACTER SET utf8mb4 NULL,
        `Password` longtext CHARACTER SET utf8mb4 NULL,
        `Address` longtext CHARACTER SET utf8mb4 NULL,
        `Country` longtext CHARACTER SET utf8mb4 NULL,
        `Seniority` datetime(6) NULL,
        `Status` tinyint(1) NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_User` PRIMARY KEY (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `AthletesToEvaluated` (
        `AthletesToEvaluatedId` int NOT NULL AUTO_INCREMENT,
        `CoachId` int NOT NULL,
        `AthleteId` int NOT NULL,
        `AssessStrengthId` int NOT NULL,
        CONSTRAINT `PK_AthletesToEvaluated` PRIMARY KEY (`AthletesToEvaluatedId`),
        CONSTRAINT `FK_AthletesToEvaluated_AssessStrength_AssessStrengthId` FOREIGN KEY (`AssessStrengthId`) REFERENCES `AssessStrength` (`AssessStrengthId`) ON DELETE CASCADE,
        CONSTRAINT `FK_AthletesToEvaluated_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE,
        CONSTRAINT `FK_AthletesToEvaluated_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `EvaluationDetailStrength` (
        `EvaluationDetailStrengthId` int NOT NULL AUTO_INCREMENT,
        `BoxNumber` int NOT NULL,
        `ThrowOrder` int NOT NULL,
        `TargetDistance` decimal(65,30) NULL,
        `ScoreObtained` decimal(65,30) NULL,
        `Observations` longtext CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NOT NULL,
        `AthleteId` int NOT NULL,
        `AssessStrengthId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_EvaluationDetailStrength` PRIMARY KEY (`EvaluationDetailStrengthId`),
        CONSTRAINT `FK_EvaluationDetailStrength_AssessStrength_AssessStrengthId` FOREIGN KEY (`AssessStrengthId`) REFERENCES `AssessStrength` (`AssessStrengthId`) ON DELETE CASCADE,
        CONSTRAINT `FK_EvaluationDetailStrength_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `Session` (
        `SessionId` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        CONSTRAINT `PK_Session` PRIMARY KEY (`SessionId`),
        CONSTRAINT `FK_Session_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE TABLE `UserRol` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `RolId` int NOT NULL,
        `DateCreation` datetime(6) NOT NULL,
        CONSTRAINT `PK_UserRol` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_UserRol_Rol_RolId` FOREIGN KEY (`RolId`) REFERENCES `Rol` (`RolId`) ON DELETE CASCADE,
        CONSTRAINT `FK_UserRol_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_AthletesToEvaluated_AssessStrengthId` ON `AthletesToEvaluated` (`AssessStrengthId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_AthletesToEvaluated_AthleteId` ON `AthletesToEvaluated` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_AthletesToEvaluated_CoachId` ON `AthletesToEvaluated` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_EvaluationDetailStrength_AssessStrengthId` ON `EvaluationDetailStrength` (`AssessStrengthId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_EvaluationDetailStrength_AthleteId` ON `EvaluationDetailStrength` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE UNIQUE INDEX `IX_Session_UserId` ON `Session` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_UserRol_RolId` ON `UserRol` (`RolId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    CREATE INDEX `IX_UserRol_UserId` ON `UserRol` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916004905_InitialMigration') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20250916004905_InitialMigration', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    ALTER TABLE `User` MODIFY COLUMN `FirstName` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    CREATE TABLE `Team` (
        `TeamId` int NOT NULL AUTO_INCREMENT,
        `NameTeam` longtext CHARACTER SET utf8mb4 NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `CoachId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_Team` PRIMARY KEY (`TeamId`),
        CONSTRAINT `FK_Team_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    CREATE TABLE `TeamUser` (
        `IdTeamUser` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `TeamId` int NOT NULL,
        `DateCreation` datetime(6) NOT NULL,
        CONSTRAINT `PK_TeamUser` PRIMARY KEY (`IdTeamUser`),
        CONSTRAINT `FK_TeamUser_Team_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE,
        CONSTRAINT `FK_TeamUser_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    CREATE INDEX `IX_Team_CoachId` ON `Team` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    CREATE INDEX `IX_TeamUser_TeamId` ON `TeamUser` (`TeamId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    CREATE INDEX `IX_TeamUser_UserId` ON `TeamUser` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250916143919_AddTeamAndUser') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20250916143919_AddTeamAndUser', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251012181901_AddAudit') THEN

    CREATE TABLE `ModuleError` (
        `ModuleErrorId` int NOT NULL AUTO_INCREMENT,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_ModuleError` PRIMARY KEY (`ModuleErrorId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251012181901_AddAudit') THEN

    CREATE TABLE `LogError` (
        `LogErrorId` int NOT NULL AUTO_INCREMENT,
        `ModuleErrorId` int NOT NULL,
        `Location` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ErrorMessage` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_LogError` PRIMARY KEY (`LogErrorId`),
        CONSTRAINT `FK_LogError_ModuleError_ModuleErrorId` FOREIGN KEY (`ModuleErrorId`) REFERENCES `ModuleError` (`ModuleErrorId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251012181901_AddAudit') THEN

    CREATE INDEX `IX_LogError_ModuleErrorId` ON `LogError` (`ModuleErrorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251012181901_AddAudit') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251012181901_AddAudit', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251015032015_AddDescriptionForEvaluationForce') THEN

    ALTER TABLE `AssessStrength` ADD `Description` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251015032015_AddDescriptionForEvaluationForce') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251015032015_AddDescriptionForEvaluationForce', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251015034716_AddStateEvaluationForce') THEN

    ALTER TABLE `AssessStrength` ADD `State` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251015034716_AddStateEvaluationForce') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251015034716_AddStateEvaluationForce', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016031938_AddStateTeam') THEN

    ALTER TABLE `Team` ADD `Status` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016031938_AddStateTeam') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251016031938_AddStateTeam', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016032135_AddRelationUserTeam') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251016032135_AddRelationUserTeam', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174416_AddNotificationTables') THEN

    CREATE TABLE `NotificationMessage` (
        `NotificationMessageId` int NOT NULL AUTO_INCREMENT,
        `Message` longtext CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NULL,
        `Image` longtext CHARACTER SET utf8mb4 NULL,
        `CoachId` int NOT NULL,
        `AthleteId` int NOT NULL,
        CONSTRAINT `PK_NotificationMessage` PRIMARY KEY (`NotificationMessageId`),
        CONSTRAINT `FK_NotificationMessage_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE,
        CONSTRAINT `FK_NotificationMessage_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174416_AddNotificationTables') THEN

    CREATE TABLE `NotificationType` (
        `NotificationTypeId` int NOT NULL AUTO_INCREMENT,
        `Name` longtext CHARACTER SET utf8mb4 NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_NotificationType` PRIMARY KEY (`NotificationTypeId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174416_AddNotificationTables') THEN

    CREATE INDEX `IX_NotificationMessage_AthleteId` ON `NotificationMessage` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174416_AddNotificationTables') THEN

    CREATE INDEX `IX_NotificationMessage_CoachId` ON `NotificationMessage` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174416_AddNotificationTables') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251016174416_AddNotificationTables', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174720_AddRelationNotificationType') THEN

    ALTER TABLE `NotificationMessage` ADD `NotificationTypeId` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174720_AddRelationNotificationType') THEN

    CREATE INDEX `IX_NotificationMessage_NotificationTypeId` ON `NotificationMessage` (`NotificationTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174720_AddRelationNotificationType') THEN

    ALTER TABLE `NotificationMessage` ADD CONSTRAINT `FK_NotificationMessage_NotificationType_NotificationTypeId` FOREIGN KEY (`NotificationTypeId`) REFERENCES `NotificationType` (`NotificationTypeId`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251016174720_AddRelationNotificationType') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251016174720_AddRelationNotificationType', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `User` ADD `Category` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `User` ADD `Image` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `Team` ADD `Bc1` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `Team` ADD `Bc2` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `Team` ADD `Bc3` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `Team` ADD `Bc4` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `Team` ADD `Country` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    ALTER TABLE `Team` ADD `Region` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017000353_AddNewFieldForTeam') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251017000353_AddNewFieldForTeam', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    ALTER TABLE `Team` ADD `Pairs` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    ALTER TABLE `Team` ADD `Teams` tinyint(1) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    CREATE TABLE `LevelEvent` (
        `LevelEventId` int NOT NULL AUTO_INCREMENT,
        `NameLevel` longtext CHARACTER SET utf8mb4 NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_LevelEvent` PRIMARY KEY (`LevelEventId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    CREATE TABLE `Event` (
        `EventId` int NOT NULL AUTO_INCREMENT,
        `NameEvent` longtext CHARACTER SET utf8mb4 NULL,
        `DescriptionEvent` longtext CHARACTER SET utf8mb4 NULL,
        `Location` longtext CHARACTER SET utf8mb4 NULL,
        `Country` longtext CHARACTER SET utf8mb4 NULL,
        `EndDate` datetime(6) NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `Status` tinyint(1) NULL,
        `UserId` int NOT NULL,
        `LevelEventId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Event` PRIMARY KEY (`EventId`),
        CONSTRAINT `FK_Event_LevelEvent_LevelEventId` FOREIGN KEY (`LevelEventId`) REFERENCES `LevelEvent` (`LevelEventId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Event_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    CREATE TABLE `Achievement` (
        `AchievementId` int NOT NULL AUTO_INCREMENT,
        `Status` tinyint(1) NULL,
        `Ranked` int NOT NULL,
        `EventId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Achievement` PRIMARY KEY (`AchievementId`),
        CONSTRAINT `FK_Achievement_Event_EventId` FOREIGN KEY (`EventId`) REFERENCES `Event` (`EventId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    CREATE INDEX `IX_Achievement_EventId` ON `Achievement` (`EventId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    CREATE INDEX `IX_Event_LevelEventId` ON `Event` (`LevelEventId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    CREATE INDEX `IX_Event_UserId` ON `Event` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251017185755_AddTablesAchievement') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251017185755_AddTablesAchievement', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251028034807_AddRelationAssetStrengthAndTeam') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251028034807_AddRelationAssetStrengthAndTeam', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251119042025_addStatisticTable') THEN

    CREATE TABLE `StrengthStatistics` (
        `StrengthStatisticsId` int NOT NULL AUTO_INCREMENT,
        `EffectivenessPercentage` double NOT NULL,
        `AccuracyPercentage` double NOT NULL,
        `EffectiveThrow` int NOT NULL,
        `FailedThrow` int NOT NULL,
        `ShortThrow` int NOT NULL,
        `MediumThrow` int NOT NULL,
        `LongThrow` double NOT NULL,
        `ShortEffectivenessPercentage` double NOT NULL,
        `MediumEffectivenessPercentage` double NOT NULL,
        `LongEffectivenessPercentage` double NOT NULL,
        `ShortThrowAccuracy` int NOT NULL,
        `MediumThrowAccuracy` int NOT NULL,
        `LongThrowAccuracy` int NOT NULL,
        `ShortAccuracyPercentage` double NOT NULL,
        `MediumAccuracyPercentage` double NOT NULL,
        `LongAccuracyPercentage` double NOT NULL,
        `AssessStrengthId` int NOT NULL,
        `AthleteId` int NOT NULL,
        CONSTRAINT `PK_StrengthStatistics` PRIMARY KEY (`StrengthStatisticsId`),
        CONSTRAINT `FK_StrengthStatistics_AssessStrength_AssessStrengthId` FOREIGN KEY (`AssessStrengthId`) REFERENCES `AssessStrength` (`AssessStrengthId`) ON DELETE CASCADE,
        CONSTRAINT `FK_StrengthStatistics_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251119042025_addStatisticTable') THEN

    CREATE INDEX `IX_StrengthStatistics_AssessStrengthId` ON `StrengthStatistics` (`AssessStrengthId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251119042025_addStatisticTable') THEN

    CREATE INDEX `IX_StrengthStatistics_AthleteId` ON `StrengthStatistics` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251119042025_addStatisticTable') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251119042025_addStatisticTable', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` DROP FOREIGN KEY `FK_NotificationMessage_User_AthleteId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` DROP FOREIGN KEY `FK_NotificationMessage_User_CoachId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` RENAME COLUMN `CoachId` TO `SenderId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` RENAME COLUMN `AthleteId` TO `ReceiverId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` RENAME INDEX `IX_NotificationMessage_CoachId` TO `IX_NotificationMessage_SenderId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` RENAME INDEX `IX_NotificationMessage_AthleteId` TO `IX_NotificationMessage_ReceiverId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `AssessStrength` MODIFY COLUMN `State` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `AssessStrength` MODIFY COLUMN `Description` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `AssessStrength` ADD `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `AssessStrength` ADD `UpdatedAt` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` ADD CONSTRAINT `FK_NotificationMessage_User_ReceiverId` FOREIGN KEY (`ReceiverId`) REFERENCES `User` (`UserId`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    ALTER TABLE `NotificationMessage` ADD CONSTRAINT `FK_NotificationMessage_User_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `User` (`UserId`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212204537_RenameNotificationMessageFields') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251212204537_RenameNotificationMessageFields', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212211410_AddReferenceIdToNotificationMessage') THEN

    ALTER TABLE `NotificationMessage` ADD `ReferenceId` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251212211410_AddReferenceIdToNotificationMessage') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251212211410_AddReferenceIdToNotificationMessage', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251213050000_AddChatTables') THEN

    CREATE TABLE `Conversations` (
        `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Participants` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ParticipantsData` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        `LastMessageId` varchar(255) CHARACTER SET utf8mb4 NULL,
        `UnreadCount` int NOT NULL DEFAULT 0,
        CONSTRAINT `PK_Conversations` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251213050000_AddChatTables') THEN

    CREATE TABLE `Messages` (
        `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `ConversationId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `SenderId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `SenderName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `SenderPhoto` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `Text` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
        `Timestamp` datetime(6) NOT NULL,
        `IsRead` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Messages` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Messages_Conversations_ConversationId` FOREIGN KEY (`ConversationId`) REFERENCES `Conversations` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251213050000_AddChatTables') THEN

    CREATE UNIQUE INDEX `IX_Conversations_LastMessageId` ON `Conversations` (`LastMessageId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251213050000_AddChatTables') THEN

    CREATE INDEX `IX_Messages_ConversationId` ON `Messages` (`ConversationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251213050000_AddChatTables') THEN

    ALTER TABLE `Conversations` ADD CONSTRAINT `FK_Conversations_Messages_LastMessageId` FOREIGN KEY (`LastMessageId`) REFERENCES `Messages` (`Id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251213050000_AddChatTables') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251213050000_AddChatTables', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251215184229_AddStatusToTeamUser') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251215184229_AddStatusToTeamUser', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE TABLE `SubscriptionType` (
        `SubscriptionTypeId` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Description` varchar(500) CHARACTER SET utf8mb4 NULL,
        `PriceInCents` int NOT NULL,
        `AnnualPriceInCents` int NULL,
        `StripeProductId` longtext CHARACTER SET utf8mb4 NULL,
        `StripeMonthlyPriceId` longtext CHARACTER SET utf8mb4 NULL,
        `StripeAnnualPriceId` longtext CHARACTER SET utf8mb4 NULL,
        `Features` longtext CHARACTER SET utf8mb4 NULL,
        `TeamLimit` int NULL,
        `AthleteLimit` int NULL,
        `MonthlyEvaluationLimit` int NULL,
        `HasAdvancedStatistics` tinyint(1) NOT NULL,
        `HasPremiumChat` tinyint(1) NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        `IsDefault` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_SubscriptionType` PRIMARY KEY (`SubscriptionTypeId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE TABLE `Subscription` (
        `SubscriptionId` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `SubscriptionTypeId` int NOT NULL,
        `StripeSubscriptionId` longtext CHARACTER SET utf8mb4 NULL,
        `StripeCustomerId` longtext CHARACTER SET utf8mb4 NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NULL,
        `NextRenewalDate` datetime(6) NULL,
        `CanceledAt` datetime(6) NULL,
        `IsTrial` tinyint(1) NOT NULL,
        `TrialEndDate` datetime(6) NULL,
        `IsAnnual` tinyint(1) NOT NULL,
        `PricePaidInCents` int NULL,
        `Currency` varchar(3) CHARACTER SET utf8mb4 NOT NULL,
        `Notes` longtext CHARACTER SET utf8mb4 NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_Subscription` PRIMARY KEY (`SubscriptionId`),
        CONSTRAINT `FK_Subscription_SubscriptionType_SubscriptionTypeId` FOREIGN KEY (`SubscriptionTypeId`) REFERENCES `SubscriptionType` (`SubscriptionTypeId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Subscription_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE TABLE `Payment` (
        `PaymentId` int NOT NULL AUTO_INCREMENT,
        `SubscriptionId` int NOT NULL,
        `UserId` int NOT NULL,
        `StripePaymentIntentId` longtext CHARACTER SET utf8mb4 NULL,
        `StripeInvoiceId` longtext CHARACTER SET utf8mb4 NULL,
        `AmountInCents` int NOT NULL,
        `Currency` varchar(3) CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `PaymentMethod` varchar(20) CHARACTER SET utf8mb4 NULL,
        `Description` varchar(500) CHARACTER SET utf8mb4 NULL,
        `PaymentDate` datetime(6) NOT NULL,
        `ProcessedAt` datetime(6) NULL,
        `RefundedAt` datetime(6) NULL,
        `RefundedAmountInCents` int NULL,
        `FailureReason` longtext CHARACTER SET utf8mb4 NULL,
        `FailureCode` longtext CHARACTER SET utf8mb4 NULL,
        `Metadata` longtext CHARACTER SET utf8mb4 NULL,
        `ReceiptNumber` longtext CHARACTER SET utf8mb4 NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_Payment` PRIMARY KEY (`PaymentId`),
        CONSTRAINT `FK_Payment_Subscription_SubscriptionId` FOREIGN KEY (`SubscriptionId`) REFERENCES `Subscription` (`SubscriptionId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Payment_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE INDEX `IX_Payment_SubscriptionId` ON `Payment` (`SubscriptionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE INDEX `IX_Payment_UserId` ON `Payment` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE INDEX `IX_Subscription_SubscriptionTypeId` ON `Subscription` (`SubscriptionTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    CREATE INDEX `IX_Subscription_UserId` ON `Subscription` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260102174629_AddSubscriptionEntities') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260102174629_AddSubscriptionEntities', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103170744_AddCoachRelationToAssessStrength') THEN

    ALTER TABLE `AssessStrength` ADD `CoachId` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103170744_AddCoachRelationToAssessStrength') THEN

    CREATE INDEX `IX_AssessStrength_CoachId` ON `AssessStrength` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103170744_AddCoachRelationToAssessStrength') THEN

    ALTER TABLE `AssessStrength` ADD CONSTRAINT `FK_AssessStrength_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103170744_AddCoachRelationToAssessStrength') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260103170744_AddCoachRelationToAssessStrength', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103172033_DeleteChatFunction') THEN

    ALTER TABLE `Conversations` DROP FOREIGN KEY `FK_Conversations_Messages_LastMessageId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103172033_DeleteChatFunction') THEN

    DROP TABLE `Messages`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103172033_DeleteChatFunction') THEN

    DROP TABLE `Conversations`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260103172033_DeleteChatFunction') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260103172033_DeleteChatFunction', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224033820_addCordinates') THEN

    ALTER TABLE `EvaluationDetailStrength` ADD `CoordinateX` double NOT NULL DEFAULT 0.0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224033820_addCordinates') THEN

    ALTER TABLE `EvaluationDetailStrength` ADD `CoordinateY` double NOT NULL DEFAULT 0.0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224033820_addCordinates') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260224033820_addCordinates', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224135648_addcomentarydetailforce') THEN

    ALTER TABLE `EvaluationDetailStrength` ADD `IsCadence` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224135648_addcomentarydetailforce') THEN

    ALTER TABLE `EvaluationDetailStrength` ADD `IsDirection` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224135648_addcomentarydetailforce') THEN

    ALTER TABLE `EvaluationDetailStrength` ADD `IsStrength` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224135648_addcomentarydetailforce') THEN

    ALTER TABLE `EvaluationDetailStrength` ADD `IsTrajectory` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260224135648_addcomentarydetailforce') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260224135648_addcomentarydetailforce', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE TABLE `AssessDirection` (
        `AssessDirectionId` int NOT NULL AUTO_INCREMENT,
        `EvaluationDate` datetime(6) NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `State` longtext CHARACTER SET utf8mb4 NULL,
        `TeamId` int NOT NULL,
        `CoachId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_AssessDirection` PRIMARY KEY (`AssessDirectionId`),
        CONSTRAINT `FK_AssessDirection_Team_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE,
        CONSTRAINT `FK_AssessDirection_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE TABLE `AthletesToEvaluatedDirection` (
        `AthletesToEvaluatedDirectionId` int NOT NULL AUTO_INCREMENT,
        `CoachId` int NOT NULL,
        `AthleteId` int NOT NULL,
        `AssessDirectionId` int NOT NULL,
        CONSTRAINT `PK_AthletesToEvaluatedDirection` PRIMARY KEY (`AthletesToEvaluatedDirectionId`),
        CONSTRAINT `FK_AthletesToEvaluatedDirection_AssessDirection_AssessDirection~` FOREIGN KEY (`AssessDirectionId`) REFERENCES `AssessDirection` (`AssessDirectionId`) ON DELETE CASCADE,
        CONSTRAINT `FK_AthletesToEvaluatedDirection_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE,
        CONSTRAINT `FK_AthletesToEvaluatedDirection_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE TABLE `DirectionStatistics` (
        `DirectionStatisticsId` int NOT NULL AUTO_INCREMENT,
        `EffectivenessPercentage` double NOT NULL,
        `AccuracyPercentage` double NOT NULL,
        `EffectiveThrow` int NOT NULL,
        `FailedThrow` int NOT NULL,
        `ShortThrow` int NOT NULL,
        `MediumThrow` int NOT NULL,
        `LongThrow` double NOT NULL,
        `ShortEffectivenessPercentage` double NOT NULL,
        `MediumEffectivenessPercentage` double NOT NULL,
        `LongEffectivenessPercentage` double NOT NULL,
        `ShortThrowAccuracy` int NOT NULL,
        `MediumThrowAccuracy` int NOT NULL,
        `LongThrowAccuracy` int NOT NULL,
        `ShortAccuracyPercentage` double NOT NULL,
        `MediumAccuracyPercentage` double NOT NULL,
        `LongAccuracyPercentage` double NOT NULL,
        `TotalDeviatedRight` int NOT NULL,
        `TotalDeviatedLeft` int NOT NULL,
        `DeviatedRightPercentage` double NOT NULL,
        `DeviatedLeftPercentage` double NOT NULL,
        `ShortDeviatedRight` int NOT NULL,
        `ShortDeviatedLeft` int NOT NULL,
        `MediumDeviatedRight` int NOT NULL,
        `MediumDeviatedLeft` int NOT NULL,
        `LongDeviatedRight` int NOT NULL,
        `LongDeviatedLeft` int NOT NULL,
        `AssessDirectionId` int NOT NULL,
        `AthleteId` int NOT NULL,
        CONSTRAINT `PK_DirectionStatistics` PRIMARY KEY (`DirectionStatisticsId`),
        CONSTRAINT `FK_DirectionStatistics_AssessDirection_AssessDirectionId` FOREIGN KEY (`AssessDirectionId`) REFERENCES `AssessDirection` (`AssessDirectionId`) ON DELETE CASCADE,
        CONSTRAINT `FK_DirectionStatistics_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE TABLE `EvaluationDetailDirection` (
        `EvaluationDetailDirectionId` int NOT NULL AUTO_INCREMENT,
        `BoxNumber` int NOT NULL,
        `ThrowOrder` int NOT NULL,
        `TargetDistance` decimal(65,30) NULL,
        `ScoreObtained` decimal(65,30) NULL,
        `Observations` longtext CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NOT NULL,
        `AthleteId` int NOT NULL,
        `AssessDirectionId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        `CoordinateX` double NOT NULL,
        `CoordinateY` double NOT NULL,
        `DeviatedRight` tinyint(1) NOT NULL,
        `DeviatedLeft` tinyint(1) NOT NULL,
        CONSTRAINT `PK_EvaluationDetailDirection` PRIMARY KEY (`EvaluationDetailDirectionId`),
        CONSTRAINT `FK_EvaluationDetailDirection_AssessDirection_AssessDirectionId` FOREIGN KEY (`AssessDirectionId`) REFERENCES `AssessDirection` (`AssessDirectionId`) ON DELETE CASCADE,
        CONSTRAINT `FK_EvaluationDetailDirection_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_AssessDirection_CoachId` ON `AssessDirection` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_AssessDirection_TeamId` ON `AssessDirection` (`TeamId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_AthletesToEvaluatedDirection_AssessDirectionId` ON `AthletesToEvaluatedDirection` (`AssessDirectionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_AthletesToEvaluatedDirection_AthleteId` ON `AthletesToEvaluatedDirection` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_AthletesToEvaluatedDirection_CoachId` ON `AthletesToEvaluatedDirection` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_DirectionStatistics_AssessDirectionId` ON `DirectionStatistics` (`AssessDirectionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_DirectionStatistics_AthleteId` ON `DirectionStatistics` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_EvaluationDetailDirection_AssessDirectionId` ON `EvaluationDetailDirection` (`AssessDirectionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    CREATE INDEX `IX_EvaluationDetailDirection_AthleteId` ON `EvaluationDetailDirection` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260324155803_AddDirectionAssessmentTables') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260324155803_AddDirectionAssessmentTables', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `Macrocycle` (
        `MacrocycleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `AthleteId` int NOT NULL,
        `AthleteName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `Notes` longtext CHARACTER SET utf8mb4 NULL,
        `CoachId` int NOT NULL,
        `TeamId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_Macrocycle` PRIMARY KEY (`MacrocycleId`),
        CONSTRAINT `FK_Macrocycle_Team_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Macrocycle_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Macrocycle_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `SaremasEvaluation` (
        `SaremasEvaluationId` int NOT NULL AUTO_INCREMENT,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `TeamId` int NOT NULL,
        `CoachId` int NOT NULL,
        `EvaluationDate` datetime(6) NOT NULL,
        `State` longtext CHARACTER SET utf8mb4 NULL,
        `TotalScore` int NULL,
        `AverageScore` double NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_SaremasEvaluation` PRIMARY KEY (`SaremasEvaluationId`),
        CONSTRAINT `FK_SaremasEvaluation_Team_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE,
        CONSTRAINT `FK_SaremasEvaluation_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `MacrocycleEvent` (
        `MacrocycleEventId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `MacrocycleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `Location` longtext CHARACTER SET utf8mb4 NULL,
        `Notes` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_MacrocycleEvent` PRIMARY KEY (`MacrocycleEventId`),
        CONSTRAINT `FK_MacrocycleEvent_Macrocycle_MacrocycleId` FOREIGN KEY (`MacrocycleId`) REFERENCES `Macrocycle` (`MacrocycleId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `MacrocyclePeriod` (
        `MacrocyclePeriodId` int NOT NULL AUTO_INCREMENT,
        `MacrocycleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `Weeks` int NOT NULL,
        CONSTRAINT `PK_MacrocyclePeriod` PRIMARY KEY (`MacrocyclePeriodId`),
        CONSTRAINT `FK_MacrocyclePeriod_Macrocycle_MacrocycleId` FOREIGN KEY (`MacrocycleId`) REFERENCES `Macrocycle` (`MacrocycleId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `Mesocycle` (
        `MesocycleId` int NOT NULL AUTO_INCREMENT,
        `MacrocycleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Number` int NOT NULL,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `Weeks` int NOT NULL,
        `Objective` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Mesocycle` PRIMARY KEY (`MesocycleId`),
        CONSTRAINT `FK_Mesocycle_Macrocycle_MacrocycleId` FOREIGN KEY (`MacrocycleId`) REFERENCES `Macrocycle` (`MacrocycleId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `Microcycle` (
        `MicrocycleId` int NOT NULL AUTO_INCREMENT,
        `MacrocycleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Number` int NOT NULL,
        `WeekNumber` int NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
        `PeriodName` longtext CHARACTER SET utf8mb4 NULL,
        `MesocycleName` longtext CHARACTER SET utf8mb4 NULL,
        `HasPeakPerformance` tinyint(1) NOT NULL,
        `TrainingDistribution` text CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Microcycle` PRIMARY KEY (`MicrocycleId`),
        CONSTRAINT `FK_Microcycle_Macrocycle_MacrocycleId` FOREIGN KEY (`MacrocycleId`) REFERENCES `Macrocycle` (`MacrocycleId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `SaremasAthleteEvaluation` (
        `SaremasAthleteEvaluationId` int NOT NULL AUTO_INCREMENT,
        `SaremasEvalId` int NOT NULL,
        `AthleteId` int NOT NULL,
        `AthleteName` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_SaremasAthleteEvaluation` PRIMARY KEY (`SaremasAthleteEvaluationId`),
        CONSTRAINT `FK_SaremasAthleteEvaluation_SaremasEvaluation_SaremasEvalId` FOREIGN KEY (`SaremasEvalId`) REFERENCES `SaremasEvaluation` (`SaremasEvaluationId`) ON DELETE CASCADE,
        CONSTRAINT `FK_SaremasAthleteEvaluation_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE TABLE `SaremasThrow` (
        `SaremasThrowId` int NOT NULL AUTO_INCREMENT,
        `SaremasEvalId` int NOT NULL,
        `AthleteId` int NOT NULL,
        `ThrowNumber` int NOT NULL,
        `Diagonal` longtext CHARACTER SET utf8mb4 NOT NULL,
        `TechnicalComponent` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ScoreObtained` int NOT NULL,
        `Observations` longtext CHARACTER SET utf8mb4 NULL,
        `FailureTags` longtext CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NOT NULL,
        `WhiteBallX` double NULL,
        `WhiteBallY` double NULL,
        `ColorBallX` double NULL,
        `ColorBallY` double NULL,
        `EstimatedDistance` double NULL,
        `LaunchPointX` double NULL,
        `LaunchPointY` double NULL,
        `DistanceToLaunchPoint` double NULL,
        `Timestamp` datetime(6) NOT NULL,
        CONSTRAINT `PK_SaremasThrow` PRIMARY KEY (`SaremasThrowId`),
        CONSTRAINT `FK_SaremasThrow_SaremasEvaluation_SaremasEvalId` FOREIGN KEY (`SaremasEvalId`) REFERENCES `SaremasEvaluation` (`SaremasEvaluationId`) ON DELETE CASCADE,
        CONSTRAINT `FK_SaremasThrow_User_AthleteId` FOREIGN KEY (`AthleteId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_Macrocycle_AthleteId` ON `Macrocycle` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_Macrocycle_CoachId` ON `Macrocycle` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_Macrocycle_TeamId` ON `Macrocycle` (`TeamId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_MacrocycleEvent_MacrocycleId` ON `MacrocycleEvent` (`MacrocycleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_MacrocyclePeriod_MacrocycleId` ON `MacrocyclePeriod` (`MacrocycleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_Mesocycle_MacrocycleId` ON `Mesocycle` (`MacrocycleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_Microcycle_MacrocycleId` ON `Microcycle` (`MacrocycleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_SaremasAthleteEvaluation_AthleteId` ON `SaremasAthleteEvaluation` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_SaremasAthleteEvaluation_SaremasEvalId` ON `SaremasAthleteEvaluation` (`SaremasEvalId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_SaremasEvaluation_CoachId` ON `SaremasEvaluation` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_SaremasEvaluation_TeamId` ON `SaremasEvaluation` (`TeamId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_SaremasThrow_AthleteId` ON `SaremasThrow` (`AthleteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    CREATE INDEX `IX_SaremasThrow_SaremasEvalId` ON `SaremasThrow` (`SaremasEvalId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260329222053_AddSaremasAndMacrocycle') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260329222053_AddSaremasAndMacrocycle', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    CREATE TABLE `TrainingSession` (
        `TrainingSessionId` int NOT NULL AUTO_INCREMENT,
        `MicrocycleId` int NOT NULL,
        `DayOfWeek` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Duration` int NOT NULL,
        `Status` longtext CHARACTER SET utf8mb4 NOT NULL,
        `StartTime` datetime(6) NULL,
        `EndTime` datetime(6) NULL,
        `PhotoEvidence1` longtext CHARACTER SET utf8mb4 NULL,
        `PhotoEvidence2` longtext CHARACTER SET utf8mb4 NULL,
        `PhotoEvidence3` longtext CHARACTER SET utf8mb4 NULL,
        `PhotoEvidence4` longtext CHARACTER SET utf8mb4 NULL,
        `ThrowPercentage` double NOT NULL,
        `TotalThrowsBase` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_TrainingSession` PRIMARY KEY (`TrainingSessionId`),
        CONSTRAINT `FK_TrainingSession_Microcycle_MicrocycleId` FOREIGN KEY (`MicrocycleId`) REFERENCES `Microcycle` (`MicrocycleId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    CREATE TABLE `SessionPart` (
        `SessionPartId` int NOT NULL AUTO_INCREMENT,
        `TrainingSessionId` int NOT NULL,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Order` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_SessionPart` PRIMARY KEY (`SessionPartId`),
        CONSTRAINT `FK_SessionPart_TrainingSession_TrainingSessionId` FOREIGN KEY (`TrainingSessionId`) REFERENCES `TrainingSession` (`TrainingSessionId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    CREATE TABLE `SessionSection` (
        `SessionSectionId` int NOT NULL AUTO_INCREMENT,
        `SessionPartId` int NOT NULL,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `NumberOfThrows` int NOT NULL,
        `Status` longtext CHARACTER SET utf8mb4 NOT NULL,
        `IsOwnDiagonal` tinyint(1) NOT NULL,
        `StartTime` datetime(6) NULL,
        `EndTime` datetime(6) NULL,
        `Observation` text CHARACTER SET utf8mb4 NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_SessionSection` PRIMARY KEY (`SessionSectionId`),
        CONSTRAINT `FK_SessionSection_SessionPart_SessionPartId` FOREIGN KEY (`SessionPartId`) REFERENCES `SessionPart` (`SessionPartId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    CREATE INDEX `IX_SessionPart_TrainingSessionId` ON `SessionPart` (`TrainingSessionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    CREATE INDEX `IX_SessionSection_SessionPartId` ON `SessionSection` (`SessionPartId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    CREATE INDEX `IX_TrainingSession_MicrocycleId` ON `TrainingSession` (`MicrocycleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260330040557_AddTrainingSessionEntities') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260330040557_AddTrainingSessionEntities', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    CREATE TABLE `MicrocycleType` (
        `MicrocycleTypeId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Description` varchar(500) CHARACTER SET utf8mb4 NULL,
        `Status` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_MicrocycleType` PRIMARY KEY (`MicrocycleTypeId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    CREATE TABLE `CoachMicrocycleTypeDay` (
        `CoachMicrocycleTypeDayId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `CoachId` int NOT NULL,
        `MicrocycleTypeId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `DayOfWeek` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `ThrowPercentage` double NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_CoachMicrocycleTypeDay` PRIMARY KEY (`CoachMicrocycleTypeDayId`),
        CONSTRAINT `FK_CoachMicrocycleTypeDay_MicrocycleType_MicrocycleTypeId` FOREIGN KEY (`MicrocycleTypeId`) REFERENCES `MicrocycleType` (`MicrocycleTypeId`) ON DELETE CASCADE,
        CONSTRAINT `FK_CoachMicrocycleTypeDay_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    CREATE TABLE `MicrocycleTypeDayDefault` (
        `MicrocycleTypeDayDefaultId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `MicrocycleTypeId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `DayOfWeek` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `ThrowPercentage` double NOT NULL,
        CONSTRAINT `PK_MicrocycleTypeDayDefault` PRIMARY KEY (`MicrocycleTypeDayDefaultId`),
        CONSTRAINT `FK_MicrocycleTypeDayDefault_MicrocycleType_MicrocycleTypeId` FOREIGN KEY (`MicrocycleTypeId`) REFERENCES `MicrocycleType` (`MicrocycleTypeId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    CREATE INDEX `IX_CoachMicrocycleTypeDay_CoachId` ON `CoachMicrocycleTypeDay` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    CREATE INDEX `IX_CoachMicrocycleTypeDay_MicrocycleTypeId` ON `CoachMicrocycleTypeDay` (`MicrocycleTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    CREATE INDEX `IX_MicrocycleTypeDayDefault_MicrocycleTypeId` ON `MicrocycleTypeDayDefault` (`MicrocycleTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260402055340_AddMicrocycleTypeTables') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260402055340_AddMicrocycleTypeTables', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526165957_AddMicrocycleDayAndTypeRelation') THEN

    ALTER TABLE `Microcycle` ADD `MicrocycleTypeId` varchar(255) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526165957_AddMicrocycleDayAndTypeRelation') THEN

    CREATE TABLE `MicrocycleDay` (
        `MicrocycleDayId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `MicrocycleId` int NOT NULL,
        `DayOfWeek` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `ThrowPercentage` double NOT NULL,
        `IsCustom` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_MicrocycleDay` PRIMARY KEY (`MicrocycleDayId`),
        CONSTRAINT `FK_MicrocycleDay_Microcycle_MicrocycleId` FOREIGN KEY (`MicrocycleId`) REFERENCES `Microcycle` (`MicrocycleId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526165957_AddMicrocycleDayAndTypeRelation') THEN

    CREATE INDEX `IX_Microcycle_MicrocycleTypeId` ON `Microcycle` (`MicrocycleTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526165957_AddMicrocycleDayAndTypeRelation') THEN

    CREATE INDEX `IX_MicrocycleDay_MicrocycleId` ON `MicrocycleDay` (`MicrocycleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526165957_AddMicrocycleDayAndTypeRelation') THEN

    ALTER TABLE `Microcycle` ADD CONSTRAINT `FK_Microcycle_MicrocycleType_MicrocycleTypeId` FOREIGN KEY (`MicrocycleTypeId`) REFERENCES `MicrocycleType` (`MicrocycleTypeId`) ON DELETE SET NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526165957_AddMicrocycleDayAndTypeRelation') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260526165957_AddMicrocycleDayAndTypeRelation', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    ALTER TABLE `MicrocycleType` ADD `ShortCode` varchar(10) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    ALTER TABLE `Microcycle` ADD `LoadPercentage` double NOT NULL DEFAULT 0.0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    ALTER TABLE `MacrocyclePeriod` ADD `StageCode` varchar(10) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    ALTER TABLE `MacrocycleEvent` ADD `Level` varchar(50) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    CREATE TABLE `CoachMicrocycleTypeDistribution` (
        `CoachMicrocycleTypeDistributionId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `CoachId` int NOT NULL,
        `MicrocycleTypeId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `FisicaGeneral` double NOT NULL,
        `FisicaEspecial` double NOT NULL,
        `Tecnica` double NOT NULL,
        `Tactica` double NOT NULL,
        `Teorica` double NOT NULL,
        `Psicologica` double NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_CoachMicrocycleTypeDistribution` PRIMARY KEY (`CoachMicrocycleTypeDistributionId`),
        CONSTRAINT `FK_CoachMicrocycleTypeDistribution_MicrocycleType_MicrocycleTyp~` FOREIGN KEY (`MicrocycleTypeId`) REFERENCES `MicrocycleType` (`MicrocycleTypeId`) ON DELETE CASCADE,
        CONSTRAINT `FK_CoachMicrocycleTypeDistribution_User_CoachId` FOREIGN KEY (`CoachId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    CREATE INDEX `IX_CoachMicrocycleTypeDistribution_CoachId` ON `CoachMicrocycleTypeDistribution` (`CoachId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    CREATE INDEX `IX_CoachMicrocycleTypeDistribution_MicrocycleTypeId` ON `CoachMicrocycleTypeDistribution` (`MicrocycleTypeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260526172040_AddPeriodizationPlanFields') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260526172040_AddPeriodizationPlanFields', '9.0.8');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

