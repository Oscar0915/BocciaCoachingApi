-- Tabla de refresh tokens (migración 20261117053814_AddRefreshTokens).
-- Se puede ejecutar más de una vez sin problema.

CREATE TABLE IF NOT EXISTS `RefreshToken` (
    `RefreshTokenId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `TokenHash` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
    `FamilyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ExpiresAt` datetime(6) NOT NULL,
    `RevokedAt` datetime(6) NULL,
    `RevokedReason` varchar(32) CHARACTER SET utf8mb4 NULL,
    `ReplacedByTokenId` char(36) COLLATE ascii_general_ci NULL,
    `CreatedByIp` varchar(45) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_RefreshToken` PRIMARY KEY (`RefreshTokenId`),
    UNIQUE INDEX `IX_RefreshToken_TokenHash` (`TokenHash`),
    INDEX `IX_RefreshToken_FamilyId` (`FamilyId`),
    INDEX `IX_RefreshToken_UserId` (`UserId`),
    CONSTRAINT `FK_RefreshToken_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- OPCIONAL: solo si tu base tiene la tabla __EFMigrationsHistory (es decir, si usas "dotnet ef database update").
-- Registra la migración como aplicada para que EF no intente crearla otra vez.
-- INSERT IGNORE INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
-- VALUES ('20261117053814_AddRefreshTokens', '9.0.8');
