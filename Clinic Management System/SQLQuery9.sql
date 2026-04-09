IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'doctor1')
INSERT INTO Users (Username, Password, Role)
VALUES ('Ahmed', 'doctor1234', 'Doctor');

IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'doctor2')
INSERT INTO Users (Username, Password, Role)
VALUES ('Sara', 'doctor1234', 'Doctor');

IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'doctor3')
INSERT INTO Users (Username, Password, Role)
VALUES ('Khalid', 'doctor1234', 'Doctor');