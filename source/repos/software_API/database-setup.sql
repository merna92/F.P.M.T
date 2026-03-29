-- PostgreSQL Database Setup Script for Yad El-Awn

-- ????? User Table
CREATE TABLE "User" (
    "UserID" SERIAL PRIMARY KEY,
    "FName" VARCHAR(50) NOT NULL,
    "LName" VARCHAR(50) NOT NULL,
    "Email" VARCHAR(100) UNIQUE NOT NULL,
    "Password" VARCHAR(255) NOT NULL,
    "Phone" VARCHAR(20),
    "Address" VARCHAR(255),
    "IsVerified" BOOLEAN DEFAULT FALSE,
    "UserType" VARCHAR(50)
);

-- ????? Location Table
CREATE TABLE "Location" (
    "LocationID" SERIAL PRIMARY KEY,
    "City_Area" VARCHAR(100) NOT NULL,
    "GPS_Coordinates" VARCHAR(100)
);

-- ????? Donor Table
CREATE TABLE "Donor" (
    "DonorID" INT PRIMARY KEY REFERENCES "User"("UserID"),
    "Donation_Count" INT DEFAULT 0
);

-- ????? Beneficiary Table
CREATE TABLE "Beneficiary" (
    "BeneficiaryID" INT PRIMARY KEY REFERENCES "User"("UserID"),
    "LocationID" INT REFERENCES "Location"("LocationID")
);

-- ????? Charity Table
CREATE TABLE "Charity" (
    "CharityID" INT PRIMARY KEY REFERENCES "User"("UserID"),
    "License_Number" VARCHAR(50) UNIQUE,
    "CoverageArea" VARCHAR(100),
    "LocationID" INT REFERENCES "Location"("LocationID")
);

-- ????? Admin Table
CREATE TABLE "Admin" (
    "AdminID" INT PRIMARY KEY REFERENCES "User"("UserID")
);

-- ????? Donation Table
CREATE TABLE "Donation" (
    "DonationID" SERIAL PRIMARY KEY,
    "Status" VARCHAR(50),
    "Image" VARCHAR(255),
    "DonorID" INT NOT NULL REFERENCES "Donor"("DonorID"),
    "LocationID" INT REFERENCES "Location"("LocationID")
);

-- ????? Food Table
CREATE TABLE "Food" (
    "DonationID" INT PRIMARY KEY REFERENCES "Donation"("DonationID"),
    "Product_Name" VARCHAR(100),
    "Food_Type" VARCHAR(50),
    "Expiry_Date" DATE,
    "Quantity" VARCHAR(50),
    "Category" VARCHAR(50),
    "Storage_Condition" VARCHAR(100),
    "Shelf_Life" VARCHAR(50)
);

-- ????? Medicine Table
CREATE TABLE "Medicine" (
    "DonationID" INT PRIMARY KEY REFERENCES "Donation"("DonationID"),
    "Medicine_Name" VARCHAR(100),
    "Medicine_Type" VARCHAR(100),
    "Expiry_Date" DATE,
    "Quantity" VARCHAR(50),
    "Safety_Conditions" TEXT
);

-- ????? Clothes Table
CREATE TABLE "Clothes" (
    "DonationID" INT PRIMARY KEY REFERENCES "Donation"("DonationID"),
    "Gender" VARCHAR(50),
    "Size" VARCHAR(20),
    "Season" VARCHAR(50),
    "Condition" VARCHAR(50),
    "Cleaning_Status" VARCHAR(100)
);

-- ????? Medical_Supplies Table
CREATE TABLE "Medical_Supplies" (
    "DonationID" INT PRIMARY KEY REFERENCES "Donation"("DonationID"),
    "Supply_Name" VARCHAR(100),
    "Quantity" VARCHAR(50),
    "Condition" VARCHAR(100)
);

-- ????? Match Table
CREATE TABLE "Matches" (
    "MatchID" SERIAL PRIMARY KEY,
    "DonationID" INT NOT NULL REFERENCES "Donation"("DonationID"),
    "BeneficiaryID" INT NOT NULL REFERENCES "Beneficiary"("BeneficiaryID"),
    "CharityID" INT NOT NULL REFERENCES "Charity"("CharityID"),
    "Urgency_Level" VARCHAR(50),
    "Distance" DECIMAL(10, 2),
    "Match_Date" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ????? Status_History Table
CREATE TABLE "Status_History" (
    "HistoryID" SERIAL PRIMARY KEY,
    "DonationID" INT NOT NULL REFERENCES "Donation"("DonationID"),
    "Old_Status" VARCHAR(50),
    "New_Status" VARCHAR(50),
    "Change_Date" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ????? Message Table
CREATE TABLE "Message" (
    "MessageID" SERIAL PRIMARY KEY,
    "SenderID" INT NOT NULL REFERENCES "User"("UserID"),
    "ReceiverID" INT NOT NULL REFERENCES "User"("UserID"),
    "Content" TEXT,
    "Is_Read" BOOLEAN DEFAULT FALSE,
    "Sent_Date_Time" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ????? Notification Table
CREATE TABLE "Notification" (
    "NotifID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL REFERENCES "User"("UserID"),
    "Content" TEXT,
    "Is_Read" BOOLEAN DEFAULT FALSE,
    "Timestamp" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ????? Audit_Log Table
CREATE TABLE "Audit_Log" (
    "LogID" SERIAL PRIMARY KEY,
    "AdminID" INT NOT NULL REFERENCES "Admin"("AdminID"),
    "Action_Taken" TEXT,
    "Action_Date" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ????? Evaluate Table
CREATE TABLE "Evaluate" (
    "CharityID" INT NOT NULL REFERENCES "Charity"("CharityID"),
    "DonorID" INT NOT NULL REFERENCES "Donor"("DonorID"),
    "Rating" INT,
    PRIMARY KEY ("CharityID", "DonorID")
);

-- ????? View ???????? ???????? ???????
CREATE OR REPLACE VIEW "vw_AvailableFoodDonations" AS
SELECT 
    d."DonationID",
    u."FName" || ' ' || u."LName" AS "DonorName",
    f."Product_Name",
    f."Expiry_Date",
    f."Quantity",
    l."City_Area",
    d."Status"
FROM "Donation" d
JOIN "Donor" dr ON d."DonorID" = dr."DonorID"
JOIN "User" u ON dr."DonorID" = u."UserID"
JOIN "Food" f ON d."DonationID" = f."DonationID"
JOIN "Location" l ON d."LocationID" = l."LocationID"
WHERE d."Status" = 'Available';

-- Indexes for better performance
CREATE INDEX idx_user_email ON "User"("Email");
CREATE INDEX idx_donation_status ON "Donation"("Status");
CREATE INDEX idx_donation_donor ON "Donation"("DonorID");
CREATE INDEX idx_match_donation ON "Matches"("DonationID");
CREATE INDEX idx_match_charity ON "Matches"("CharityID");
CREATE INDEX idx_message_receiver ON "Message"("ReceiverID");
