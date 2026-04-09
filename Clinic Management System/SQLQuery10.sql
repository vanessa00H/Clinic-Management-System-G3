IF NOT EXISTS (SELECT 1 FROM Doctors WHERE DoctorName = 'Dr. Ahmed Alzahrani')
INSERT INTO Doctors (DoctorName, Specialization, Department, ConsultationFee, IsAvailable)
VALUES ('Dr. Ahmed Alzahrani', 'General Physician', 'General Medicine', 200, 1);

IF NOT EXISTS (SELECT 1 FROM Doctors WHERE DoctorName = 'Dr. Sara Alharbi')
INSERT INTO Doctors (DoctorName, Specialization, Department, ConsultationFee, IsAvailable)
VALUES ('Dr. Sara Alharbi', 'Dentist', 'Dentistry', 250, 1);

IF NOT EXISTS (SELECT 1 FROM Doctors WHERE DoctorName = 'Dr. Khalid Ali')
INSERT INTO Doctors (DoctorName, Specialization, Department, ConsultationFee, IsAvailable)
VALUES ('Dr. Khalid Ali', 'Dermatologist', 'Dermatology', 300, 1);