use master
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'ST10448895Ease')
DROP DATABASE ST10448895Ease
CREATE DATABASE ST10448895Ease

USE ST10448895Ease

-- Step 1: Create the Venue table
CREATE TABLE Venue(
    Venue_ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    [Venue_Name] VARCHAR(250) NOT NULL,
    Locations VARCHAR(250) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(500) NULL
    -- Optional: Change to VARBINARY(MAX) if storing actual images
);

-- Step 2: Create the EventType table
CREATE TABLE EventType (
    EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

-- Step 3: Create the EventS table
CREATE TABLE EventS(
    Event_ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    [Event_Name] VARCHAR(250) NOT NULL,
    Event_Date DATE NOT NULL,
    Descriptions VARCHAR(250) NOT NULL,
    Venue_ID INT,
    EventTypeID INT NULL
);

-- Step 4: Add foreign keys to EventS
ALTER TABLE EventS
ADD CONSTRAINT FK_EventS_Venue
FOREIGN KEY (Venue_ID) REFERENCES Venue(Venue_ID) ON DELETE CASCADE;

ALTER TABLE EventS
ADD CONSTRAINT FK_EventS_EventType
FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID) ON DELETE SET NULL;

-- Step 5: Create the Bookings table
CREATE TABLE Bookings(
    Booking_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Venue_ID INT,
    Event_ID INT,
    Booking_Date DATE
);

-- Step 6: Add foreign keys to Bookings
ALTER TABLE Bookings
ADD CONSTRAINT FK_Bookings_Venue
FOREIGN KEY (Venue_ID) REFERENCES Venue(Venue_ID) ON DELETE CASCADE;

ALTER TABLE Bookings
ADD CONSTRAINT FK_Bookings_Event
FOREIGN KEY (Event_ID) REFERENCES EventS(Event_ID) ON DELETE NO ACTION;

CREATE VIEW vwBookingDetails AS
SELECT 
    b.Booking_ID,
    b.Booking_Date,
    v.Venue_ID,
    v.Venue_Name,
    v.Locations,
    e.Event_ID,
    e.Event_Name,
    e.Descriptions
FROM Bookings b
INNER JOIN EventS e ON b.Event_ID = e.Event_ID
INNER JOIN Venue v ON b.Venue_ID = v.Venue_ID;


-- Step 7: Insert sample data into EventType
INSERT INTO EventType (Name)
VALUES 
('Music'), 
('Sports'), 
('Theater'), 
('Conference');

-- Step 8: Insert sample data into Venue
INSERT INTO Venue (Venue_Name, Locations, Capacity, ImageUrl)
VALUES 
('Grand St', '123 Main', 5000, NULL);

-- Step 9: Update the Venue with a sample image URL (converted as string to VARBINARY)
UPDATE Venue
SET ImageUrl = CONVERT(VARBINARY(MAX), 'https://www.google.com/url?sa=i&url=https%3A%2F%2Fjunebugweddings.com%2Fwedding-blog%2Fthe-ultimate-guide-to-finding-your-wedding-venue%2F&psig=AOvVaw1xzDzLxQ8pLromXcML0MBU&ust=1741947487771000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCJCWwKDqhowDFQAAAAAdAAAAABAE')
WHERE Venue_ID = 1;

-- Step 10: Insert sample data into EventS
INSERT INTO EventS (Event_Name, Event_Date, Descriptions, Venue_ID, EventTypeID)
VALUES 
('Concert', '2025-04-12', 'Live Show', 1, 1); -- 1 = Music

-- Step 11: Insert sample data into Bookings
INSERT INTO Bookings (Venue_ID, Event_ID, Booking_Date)
VALUES 
(1, 1, '2025-03-12');

-- Step 12: View all data
SELECT * FROM Venue;
SELECT * FROM EventType;
SELECT * FROM EventS;
SELECT * FROM Bookings;