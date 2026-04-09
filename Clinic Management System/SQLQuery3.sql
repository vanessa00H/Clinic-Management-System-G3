UPDATE Users 
SET Role='Patient'
WHERE Role IS NULL;

UPDATE Users
SET Role='Admin'
WHERE Username='admin';