-- Migration Script: AddSubscriptionEntities
-- Run this script after creating the migration with Entity Framework

-- Create SubscriptionType table
CREATE TABLE SubscriptionType (
    SubscriptionTypeId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(500),
    PriceInCents INT NOT NULL DEFAULT 0,
    AnnualPriceInCents INT NULL,
    StripeProductId VARCHAR(255) NULL,
    StripeMonthlyPriceId VARCHAR(255) NULL,
    StripeAnnualPriceId VARCHAR(255) NULL,
    Features TEXT NULL,
    TeamLimit INT NULL,
    AthleteLimit INT NULL,
    MonthlyEvaluationLimit INT NULL,
    HasAdvancedStatistics TINYINT(1) NOT NULL DEFAULT 0,
    HasPremiumChat TINYINT(1) NOT NULL DEFAULT 0,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    IsDefault TINYINT(1) NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    INDEX IX_SubscriptionType_IsActive (IsActive),
    INDEX IX_SubscriptionType_IsDefault (IsDefault)
);

-- Create Subscription table
CREATE TABLE Subscription (
    SubscriptionId INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    SubscriptionTypeId INT NOT NULL,
    StripeSubscriptionId VARCHAR(255) NULL,
    StripeCustomerId VARCHAR(255) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'active',
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NULL,
    NextRenewalDate DATETIME NULL,
    CanceledAt DATETIME NULL,
    IsTrial TINYINT(1) NOT NULL DEFAULT 0,
    TrialEndDate DATETIME NULL,
    IsAnnual TINYINT(1) NOT NULL DEFAULT 0,
    PricePaidInCents INT NULL,
    Currency VARCHAR(3) NOT NULL DEFAULT 'USD',
    Notes TEXT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (SubscriptionTypeId) REFERENCES SubscriptionType(SubscriptionTypeId) ON DELETE RESTRICT,
    INDEX IX_Subscription_UserId (UserId),
    INDEX IX_Subscription_SubscriptionTypeId (SubscriptionTypeId),
    INDEX IX_Subscription_Status (Status),
    INDEX IX_Subscription_StripeSubscriptionId (StripeSubscriptionId)
);

-- Create Payment table
CREATE TABLE Payment (
    PaymentId INT AUTO_INCREMENT PRIMARY KEY,
    SubscriptionId INT NOT NULL,
    UserId INT NOT NULL,
    StripePaymentIntentId VARCHAR(255) NULL,
    StripeInvoiceId VARCHAR(255) NULL,
    AmountInCents INT NOT NULL,
    Currency VARCHAR(3) NOT NULL DEFAULT 'USD',
    Status VARCHAR(20) NOT NULL DEFAULT 'pending',
    PaymentMethod VARCHAR(20) NULL DEFAULT 'card',
    Description VARCHAR(500) NULL,
    PaymentDate DATETIME NOT NULL,
    ProcessedAt DATETIME NULL,
    RefundedAt DATETIME NULL,
    RefundedAmountInCents INT NULL,
    FailureReason TEXT NULL,
    FailureCode VARCHAR(255) NULL,
    Metadata TEXT NULL,
    ReceiptNumber VARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (SubscriptionId) REFERENCES Subscription(SubscriptionId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    INDEX IX_Payment_SubscriptionId (SubscriptionId),
    INDEX IX_Payment_UserId (UserId),
    INDEX IX_Payment_Status (Status),
    INDEX IX_Payment_StripePaymentIntentId (StripePaymentIntentId)
);

-- Insert default subscription types
INSERT INTO SubscriptionType 
(Name, Description, PriceInCents, AnnualPriceInCents, TeamLimit, AthleteLimit, MonthlyEvaluationLimit, 
HasAdvancedStatistics, HasPremiumChat, IsActive, IsDefault, CreatedAt) 
VALUES 
('Free', 'Plan gratuito con funcionalidades básicas para comenzar', 0, 0, 1, 5, 10, 0, 0, 1, 1, NOW()),
('Premium', 'Plan premium con estadísticas avanzadas y chat premium', 999, 9990, NULL, NULL, NULL, 1, 1, 1, 0, NOW()),
('Pro', 'Plan profesional para equipos grandes y organizaciones', 1999, 19990, NULL, NULL, NULL, 1, 1, 1, 0, NOW());
