UPDATE Doctors
SET UserID = (SELECT UserID FROM Users WHERE Username = 'doctor1')
WHERE DoctorName = 'Dr. Ahmed Alzahrani';

UPDATE Doctors
SET UserID = (SELECT UserID FROM Users WHERE Username = 'doctor2')
WHERE DoctorName = 'Dr. Sara Alharbi';

UPDATE Doctors
SET UserID = (SELECT UserID FROM Users WHERE Username = 'doctor3')
WHERE DoctorName = 'Dr. Khalid Ali';