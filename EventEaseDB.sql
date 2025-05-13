-- Create the database
CREATE DATABASE EventEaseDB;
GO
USE EventEaseDB;
GO

-- Create the Venue table
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName VARCHAR(255) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(500)
);
GO

-- Create the Event table
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(255) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description TEXT,
    VenueId INT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE SET NULL
);
GO

-- Create the Booking table without VenueId (as discussed)
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EventId) REFERENCES Event(EventId) ON DELETE CASCADE
);
GO

-- Insert sample data into Venue table
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Hall', '123 Main St, Johannesburg', 500, 'https://via.placeholder.com/150'),
('Conference Center', '45 Business Rd, Cape Town', 300, 'https://via.placeholder.com/150');
GO

-- Insert sample data into Event table
INSERT INTO Event (EventName, EventDate, Description, VenueId)
VALUES 
('Tech Expo 2025', '2025-06-15 09:00:00', 'A large tech industry expo.', 1),
('Business Summit', '2025-07-20 10:00:00', 'Annual business networking event.', 2);
GO

-- Insert sample data into Booking table
INSERT INTO Booking (EventId)
VALUES 
(1),
(2);
GO

-- Verify inserted data
SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;
GO

