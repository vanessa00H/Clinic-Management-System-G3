IF OBJECT_ID('Appointments', 'U') IS NULL
BEGIN
    CREATE TABLE Appointments (
        AppointmentID INT PRIMARY KEY IDENTITY(1,1),
        PatientName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        AppointmentDate DATE NOT NULL,
        AppointmentTime TIME NOT NULL,
        DoctorName NVARCHAR(100) NOT NULL,
        Department NVARCHAR(100) NOT NULL,
        AppointmentType NVARCHAR(50) NOT NULL,
        AdditionalService NVARCHAR(100) NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Pending'
    );
END