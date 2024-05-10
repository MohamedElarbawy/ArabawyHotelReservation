
--RoomTypes
set IDENTITY_INSERT RoomTypes on
INSERT INTO RoomTypes (RoomTypeId,RoomTypeName)
VALUES (1,'Sea View Room'),
       (2,'Garden View Room'),
       (3,'Standard Room'),
       (4,'Deluxe Room'),
       (5,'Suite Room'),
       (6,'Family Room'),
       (7,'Executive Room'),
       (8,'Penthouse Suite'),
       (9,'Bungalow'),
       (10,'Loft Room');
set IDENTITY_INSERT RoomTypes off


	   
--MealPlans
set IDENTITY_INSERT MealPlans on
INSERT INTO MealPlans (MealPlanId,MealPlanName)
VALUES (1,'Breakfast Only'),
       (2,'Half Board'),
       (3,'Full Board'),
       (4,'All Inclusive'),
       (5,'Room Only'),
       (6,'Bed and Breakfast'),
       (7,'Dinner Only'),
       (8,'No Meals'),
       (9,'Brunch'),
       (10,'Snack Package');

set IDENTITY_INSERT MealPlans off


--mealPlanSeasons
INSERT into MealPlanSeaons ([SeasonStart],
[SeasonEnd],
[RatePerAdult],
[RatePerChild],
[MealPlanId] )

values
('2024-1-1','2024-5-30',5,5,1),
('2024-1-1','2024-5-30',10,10,2),
('2024-1-1','2024-5-30',15,15,3),
('2024-1-1','2024-5-30',20,20,4),
('2024-1-1','2024-5-30',25,25,5),
('2024-6-1','2024-12-31',10,10,1),
('2024-6-1','2024-12-31',20,20,2),
('2024-6-1','2024-12-31',30,30,3),
('2024-6-1','2024-12-31',40,40,4),
('2024-6-1','2024-12-31',50,50,5)



--roomSeasons
insert into RoomSeasons ([SeasonStart],
[SeasonEnd],
[RatePerRoom],
[RoomTypeId])
VALUES
('2024-1-1','2024-1-15',80,1),
('2024-1-16','2024-4-30',50,1),
('2024-5-1','2024-12-31',70,1),
('2024-1-1','2024-1-15',120,2),
('2024-1-16','2024-3-31',90,2),
('2024-4-1','2024-12-31',100,2),
('2024-1-1','2024-2-15',150,3),
('2024-2-16','2024-6-30',130,3),
('2024-7-1','2024-12-31',110,3)


--rooms
DECLARE @Counter INT
SET @Counter = 1

WHILE @Counter < 150
BEGIN
insert into rooms ([RoomNo],
[AdultsCapcity],
[ChildrenCapcity],
[RoomTypeId])
VALUES
(@Counter,2,2,(@Counter+15) / 15 )
    SET @Counter = @Counter + 1
END;


