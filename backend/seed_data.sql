-- Seed data for Zimal Hameed (220918BC-5879-41AD-9A9B-71271FB08EDF)
DECLARE @uid UNIQUEIDENTIFIER = '220918BC-5879-41AD-9A9B-71271FB08EDF';
DECLARE @now DATETIME2 = GETUTCDATE();

-- ============================================================
-- MOOD LOGS - 30 days
-- ============================================================
DELETE FROM MoodLogs WHERE UserId = @uid;
INSERT INTO MoodLogs (Id, UserId, MoodScore, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @uid, 7, 'Feeling pretty good today, got some fresh air', DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6, 'A bit tired but manageable', DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Great morning workout, full of energy!', DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 5, 'Stressful day at work, headache by evening', DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Feeling balanced, good sleep helped', DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 9, 'Best mood in weeks! Finished a big project', DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Weekend vibes, relaxed and happy', DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 4, 'Feeling low, didn''t sleep well', DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6, 'Recovery day, slow but steady', DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Meditated for 20 min, helped a lot', DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Walked 8000 steps, feeling accomplished', DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6, 'Mild anxiety in the morning, better by noon', DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Good day overall, healthy lunch helped', DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 5, 'Rain and gloomy weather, mood affected', DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 9, 'Caught up with friends, wonderful evening', DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Productive work session, mood boosted', DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6, 'Average day, nothing special', DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Yoga class in the evening, feel refreshed', DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Good energy levels throughout the day', DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 5, 'Slight headache, took it easy', DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Felt motivated, completed goals for the day', DATEADD(day,-9,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 9, 'Amazing day, everything went smoothly', DATEADD(day,-8,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Consistent mood, healthy routine paying off', DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6, 'Bit anxious about upcoming deadline', DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Deadline done! Huge relief and happy mood', DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Weekend rest, feeling recharged', DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Good family time, positive vibes', DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7, 'Back to routine, focused and calm', DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8, 'Great start to the day, feeling energized', DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 9, 'Top of the world today! Healthy and happy', CAST(GETDATE() AS DATE), @now, @now);

-- ============================================================
-- SLEEP LOGS - 30 days
-- ============================================================
DELETE FROM SleepLogs WHERE UserId = @uid;
INSERT INTO SleepLogs (Id, UserId, HoursSlept, QualityScore, BedTime, WakeTime, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @uid, 7.0, 7, '23:00', '06:00', DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6.5, 6, '23:30', '06:00', DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.0, 9, '22:30', '06:30', DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 5.5, 4, '01:00', '06:30', DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.5, 8, '22:45', '06:15', DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.5, 9, '22:00', '06:30', DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 9.0, 10, '22:00', '07:00', DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 5.0, 3, '02:00', '07:00', DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6.5, 6, '23:00', '05:30', DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.0, 7, '23:00', '06:00', DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.5, 8, '22:30', '06:00', DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6.0, 5, '00:00', '06:00', DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.0, 7, '23:00', '06:00', DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6.5, 6, '23:30', '06:00', DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.0, 9, '22:00', '06:00', DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.5, 8, '22:30', '06:00', DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.0, 7, '23:00', '06:00', DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.0, 9, '22:00', '06:00', DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.5, 8, '22:30', '06:00', DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6.0, 5, '00:30', '06:30', DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.5, 8, '22:30', '06:00', DATEADD(day,-9,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.5, 9, '22:00', '06:30', DATEADD(day,-8,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.0, 7, '23:00', '06:00', DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 6.5, 6, '23:30', '06:00', DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.0, 9, '22:00', '06:00', DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 9.0, 10, '21:30', '06:30', DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.0, 9, '22:00', '06:00', DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.5, 8, '22:30', '06:00', DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 7.0, 7, '23:00', '06:00', DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 8.0, 9, '22:00', '06:00', CAST(GETDATE() AS DATE), @now, @now);

-- ============================================================
-- SYMPTOM LOGS
-- ============================================================
DELETE FROM SymptomLogs WHERE UserId = @uid;
INSERT INTO SymptomLogs (Id, UserId, SymptomName, Severity, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @uid, 'Headache', 6, 'Tension headache after long screen time', DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Fatigue', 7, 'Very tired after poor sleep', DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Anxiety', 5, 'Morning anxiety, calmed down by afternoon', DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Back Pain', 4, 'Mild lower back pain from sitting too long', DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Headache', 4, 'Mild headache in the evening', DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Fatigue', 5, 'Afternoon slump, needed coffee', DATEADD(day,-8,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Neck Stiffness', 3, 'Slight stiffness from desk work', DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Anxiety', 4, 'Pre-deadline nerves, breathing exercises helped', DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Eye Strain', 3, 'Too much screen time today', DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Headache', 2, 'Very mild, went away after water and rest', DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now);

-- ============================================================
-- MEAL LOGS
-- ============================================================
DELETE FROM MealLogs WHERE UserId = @uid;
INSERT INTO MealLogs (Id, UserId, MealType, Description, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @uid, 'Breakfast', 'Oatmeal with berries, green tea', DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Grilled chicken salad with quinoa', DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Dinner', 'Salmon with steamed vegetables', DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Greek yogurt with honey and walnuts', DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Lentil soup with whole wheat bread', DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Smoothie bowl: banana, spinach, protein powder', DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Brown rice with chickpea curry', DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Dinner', 'Grilled fish tacos with avocado', DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Eggs on toast with orange juice', DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Caesar salad with grilled shrimp', DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Dinner', 'Homemade pasta with tomato sauce and vegetables', DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Chia pudding with mango', DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Turkey wrap with spinach and hummus', DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Avocado toast with poached eggs', DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Mixed grain bowl with roasted veggies', DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Dinner', 'Baked chicken with sweet potato', DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Protein pancakes with fresh fruit', DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Lunch', 'Sushi platter with miso soup', DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @uid, 'Breakfast', 'Overnight oats with almond butter', CAST(GETDATE() AS DATE), @now, @now),
(NEWID(), @uid, 'Lunch', 'Quinoa salad with feta and cucumber', CAST(GETDATE() AS DATE), @now, @now);

PRINT 'Seed data inserted successfully!';
