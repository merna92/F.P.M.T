-- ========================================
-- Yad El-Awn Database Cleanup Script
-- ========================================

-- Disable FK constraints
ALTER TABLE [Audit_Log] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Matches] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Status_History] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Message] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Notification] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Food] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Medicine] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Clothes] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Medical_Supplies] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Donation] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Charity] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Beneficiary] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Donor] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Admin] NOCHECK CONSTRAINT ALL;
ALTER TABLE [Evaluate] NOCHECK CONSTRAINT ALL;

-- Delete all data
DELETE FROM [Audit_Log];
DELETE FROM [Matches];
DELETE FROM [Status_History];
DELETE FROM [Message];
DELETE FROM [Notification];
DELETE FROM [Food];
DELETE FROM [Medicine];
DELETE FROM [Clothes];
DELETE FROM [Medical_Supplies];
DELETE FROM [Donation];
DELETE FROM [Charity];
DELETE FROM [Beneficiary];
DELETE FROM [Donor];
DELETE FROM [Admin];
DELETE FROM [Evaluate];
DELETE FROM [Location];
DELETE FROM [User];

-- Reset identity seeds
DBCC CHECKIDENT ('[User]', RESEED, 0);
DBCC CHECKIDENT ('[Location]', RESEED, 0);
DBCC CHECKIDENT ('[Donation]', RESEED, 0);
DBCC CHECKIDENT ('[Matches]', RESEED, 0);
DBCC CHECKIDENT ('[Status_History]', RESEED, 0);
DBCC CHECKIDENT ('[Message]', RESEED, 0);
DBCC CHECKIDENT ('[Notification]', RESEED, 0);
DBCC CHECKIDENT ('[Audit_Log]', RESEED, 0);

-- Re-enable FK constraints
ALTER TABLE [Audit_Log] CHECK CONSTRAINT ALL;
ALTER TABLE [Matches] CHECK CONSTRAINT ALL;
ALTER TABLE [Status_History] CHECK CONSTRAINT ALL;
ALTER TABLE [Message] CHECK CONSTRAINT ALL;
ALTER TABLE [Notification] CHECK CONSTRAINT ALL;
ALTER TABLE [Food] CHECK CONSTRAINT ALL;
ALTER TABLE [Medicine] CHECK CONSTRAINT ALL;
ALTER TABLE [Clothes] CHECK CONSTRAINT ALL;
ALTER TABLE [Medical_Supplies] CHECK CONSTRAINT ALL;
ALTER TABLE [Donation] CHECK CONSTRAINT ALL;
ALTER TABLE [Charity] CHECK CONSTRAINT ALL;
ALTER TABLE [Beneficiary] CHECK CONSTRAINT ALL;
ALTER TABLE [Donor] CHECK CONSTRAINT ALL;
ALTER TABLE [Admin] CHECK CONSTRAINT ALL;
ALTER TABLE [Evaluate] CHECK CONSTRAINT ALL;

-- ========================================
-- Insert Sample Data
-- ========================================

-- Add Locations
INSERT INTO [Location] ([City_Area], [GPS_Coordinates]) VALUES ('Cairo - Heliopolis', '30.0444,31.3619');
INSERT INTO [Location] ([City_Area], [GPS_Coordinates]) VALUES ('Cairo - Zamalek', '30.0615,31.2535');
INSERT INTO [Location] ([City_Area], [GPS_Coordinates]) VALUES ('Giza - Dokki', '30.0196,31.2021');

-- Add Users (Admin)
INSERT INTO [User] ([FName], [LName], [Email], [Password], [Phone], [Address], [IsVerified], [UserType])
VALUES ('Admin', 'System', 'admin@example.com', 'hashed_password_123', '01000000000', 'Cairo', 1, 'Admin');

-- Add Admin
INSERT INTO [Admin] ([AdminID]) VALUES (1);

-- Add Users (Donors)
INSERT INTO [User] ([FName], [LName], [Email], [Password], [Phone], [Address], [IsVerified], [UserType])
VALUES ('Ali', 'Mohamed', 'ali@example.com', 'hashed_password_456', '01111111111', 'Cairo', 1, 'Donor');

INSERT INTO [User] ([FName], [LName], [Email], [Password], [Phone], [Address], [IsVerified], [UserType])
VALUES ('Fatma', 'Ahmed', 'fatma@example.com', 'hashed_password_789', '01222222222', 'Giza', 1, 'Donor');

-- Add Donors
INSERT INTO [Donor] ([DonorID], [Donation_Count]) VALUES (2, 0);
INSERT INTO [Donor] ([DonorID], [Donation_Count]) VALUES (3, 0);

-- Add Users (Beneficiaries)
INSERT INTO [User] ([FName], [LName], [Email], [Password], [Phone], [Address], [IsVerified], [UserType])
VALUES ('Sara', 'Hassan', 'sara@example.com', 'hashed_password_101', '01333333333', 'Cairo', 1, 'Beneficiary');

INSERT INTO [User] ([FName], [LName], [Email], [Password], [Phone], [Address], [IsVerified], [UserType])
VALUES ('Omar', 'Khalid', 'omar@example.com', 'hashed_password_102', '01444444444', 'Alexandria', 1, 'Beneficiary');

-- Add Beneficiaries
INSERT INTO [Beneficiary] ([BeneficiaryID], [LocationID]) VALUES (4, 1);
INSERT INTO [Beneficiary] ([BeneficiaryID], [LocationID]) VALUES (5, 2);

-- Add Users (Charities)
INSERT INTO [User] ([FName], [LName], [Email], [Password], [Phone], [Address], [IsVerified], [UserType])
VALUES ('Charity', 'One', 'charity1@example.com', 'hashed_password_103', '01555555555', 'Cairo', 1, 'Charity');

-- Add Charities
INSERT INTO [Charity] ([CharityID], [License_Number], [CoverageArea], [LocationID])
VALUES (6, 'LIC123456', 'Cairo & Giza', 1);

PRINT 'Database cleaned and sample data inserted successfully!';
