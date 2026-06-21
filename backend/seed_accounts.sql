-- ============================================================
-- SEED DATA FOR 3 NEW ACCOUNTS
-- Ayesha Yaqoob  | ayesha28@gmail.com  | Ayesha28!
-- Nimra Yaqoob   | nimra28@gmail.com   | Nimra28!
-- Ahmad Noor     | ahmad28@gmail.com   | Ahmad28!
-- ============================================================

DECLARE @now DATETIME2 = GETUTCDATE();

-- ============================================================
-- 1. CREATE USER ACCOUNTS
-- ============================================================

-- Remove if already exists (re-runnable)
DELETE FROM MealLogs    WHERE UserId IN (SELECT Id FROM Users WHERE Email IN ('ayesha28@gmail.com','nimra28@gmail.com','ahmad28@gmail.com'));
DELETE FROM SymptomLogs WHERE UserId IN (SELECT Id FROM Users WHERE Email IN ('ayesha28@gmail.com','nimra28@gmail.com','ahmad28@gmail.com'));
DELETE FROM SleepLogs   WHERE UserId IN (SELECT Id FROM Users WHERE Email IN ('ayesha28@gmail.com','nimra28@gmail.com','ahmad28@gmail.com'));
DELETE FROM MoodLogs    WHERE UserId IN (SELECT Id FROM Users WHERE Email IN ('ayesha28@gmail.com','nimra28@gmail.com','ahmad28@gmail.com'));
DELETE FROM RefreshTokens WHERE UserId IN (SELECT Id FROM Users WHERE Email IN ('ayesha28@gmail.com','nimra28@gmail.com','ahmad28@gmail.com'));
DELETE FROM Users WHERE Email IN ('ayesha28@gmail.com','nimra28@gmail.com','ahmad28@gmail.com');

-- Ayesha Yaqoob
DECLARE @ayesha UNIQUEIDENTIFIER = NEWID();
INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, IsEmailVerified, CreatedAt, UpdatedAt)
VALUES (@ayesha, 'ayesha28@gmail.com', '$2a$11$DKHR4LAZ5riEwcaO4WS2Ju5DHL1gzaHXYF8jD/VsDk8tkR0YYhT3S', 'Ayesha', 'Yaqoob', 1, @now, @now);

-- Nimra Yaqoob
DECLARE @nimra UNIQUEIDENTIFIER = NEWID();
INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, IsEmailVerified, CreatedAt, UpdatedAt)
VALUES (@nimra, 'nimra28@gmail.com', '$2a$11$cTWAEKkXDLvzekjYjVDPBel0SV21ebFYbZy.iEDB0/uFjsnmXmXCi', 'Nimra', 'Yaqoob', 1, @now, @now);

-- Ahmad Noor
DECLARE @ahmad UNIQUEIDENTIFIER = NEWID();
INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, IsEmailVerified, CreatedAt, UpdatedAt)
VALUES (@ahmad, 'ahmad28@gmail.com', '$2a$11$6qk3QcUdftZmkbgofZ0qwOqeYC0rzzETDeX.HDZAYTllyjAGDX.56', 'Ahmad', 'Noor', 1, @now, @now);


-- ============================================================
-- AYESHA YAQOOB — Active fitness enthusiast, generally healthy
-- Trend: High energy, occasional migraines, strong sleep habits
-- ============================================================

INSERT INTO MoodLogs (Id, UserId, MoodScore, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ayesha, 8,  'Morning run gave me so much energy today!',             DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Good day, little tired in the afternoon',               DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Crushed my workout, feeling unstoppable',                DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 6,  'Headache hit by evening, annoying',                     DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Recovered well, yoga session in the morning',           DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Average mood, busy with tasks all day',                 DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Weekend hike! Mood through the roof',                   DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Felt rested and productive',                            DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 6,  'Migraine started in the afternoon',                     DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 5,  'Migraine still lingering, low energy',                  DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Recovered! Light walk helped a lot',                    DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Back to full energy, great gym session',                DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Best mood this week, everything clicked',               DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Good overall, a bit stressed about work',               DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Family dinner, positive evening',                       DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 6,  'Slight dip, didn''t sleep great last night',           DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Made up for it with a long restful sleep',              DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Super productive day, all tasks done early',            DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Calm and steady, no major highs or lows',              DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Evening walk was so refreshing',                        DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Slightly fatigued but positive',                        DATEADD(day,-9, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Celebrated a friend''s birthday, great vibes',         DATEADD(day,-8, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Focused and calm, journaled in the morning',            DATEADD(day,-7, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Mild eye strain from screens, rested eyes',             DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Long weekend run, endorphin rush all day',              DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Healthy cooking day, tried new recipe',                 DATEADD(day,-4, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7,  'Good mood, slight afternoon slump',                     DATEADD(day,-3, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Meditated 30 min, incredibly clear-headed',             DATEADD(day,-2, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8,  'Finished reading a book I loved',                       DATEADD(day,-1, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9,  'Feeling at my best, healthy and energized!',            CAST(GETDATE() AS DATE), @now, @now);

INSERT INTO SleepLogs (Id, UserId, HoursSlept, QualityScore, BedTime, WakeTime, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ayesha, 7.5, 8,  '22:30', '06:00', DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 8,  '22:00', '06:00', DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.5, 9,  '21:45', '06:15', DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 6.0, 5,  '00:30', '06:30', DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 9,  '22:00', '06:00', DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.0, 7,  '23:00', '06:00', DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9.0, 10, '21:30', '06:30', DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 8,  '22:00', '06:00', DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 5.5, 4,  '01:00', '06:30', DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 6.0, 5,  '00:45', '06:45', DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.5, 8,  '22:30', '06:00', DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 9,  '22:00', '06:00', DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.5, 9,  '21:45', '06:15', DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.0, 7,  '23:00', '06:00', DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 8,  '22:00', '06:00', DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 6.5, 6,  '23:30', '06:00', DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9.0, 10, '21:30', '06:30', DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 9,  '22:00', '06:00', DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.5, 8,  '22:30', '06:00', DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 8,  '22:00', '06:00', DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.0, 7,  '23:00', '06:00', DATEADD(day,-9, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 9,  '22:00', '06:00', DATEADD(day,-8, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.5, 9,  '21:45', '06:15', DATEADD(day,-7, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.0, 7,  '23:00', '06:00', DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 9.0, 10, '21:30', '06:30', DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 8,  '22:00', '06:00', DATEADD(day,-4, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 7.5, 8,  '22:30', '06:00', DATEADD(day,-3, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.5, 9,  '21:45', '06:15', DATEADD(day,-2, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.0, 9,  '22:00', '06:00', DATEADD(day,-1, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 8.5, 10, '21:30', '06:00', CAST(GETDATE() AS DATE), @now, @now);

INSERT INTO SymptomLogs (Id, UserId, SymptomName, Severity, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ayesha, 'Migraine',    7, 'Throbbing pain, light sensitivity, had to rest in dark room', DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Migraine',    5, 'Migraine residual, dull ache persisting all morning',          DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Headache',    4, 'Mild tension headache after staring at screen',                DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Eye Strain',  3, 'Long hours on laptop, eyes feel dry and tired',               DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Fatigue',     4, 'Post-workout tiredness, muscles a bit sore',                  DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Muscle Ache', 3, 'Legs sore after weekend hike, nothing serious',               DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now);

INSERT INTO MealLogs (Id, UserId, MealType, Description, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ayesha, 'Breakfast', 'Smoothie bowl with acai, banana, granola',              DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Grilled chicken with roasted sweet potato and broccoli',DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Dinner',    'Stir-fried tofu with brown rice and vegetables',        DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Snack',     'Apple slices with almond butter',                       DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Overnight oats with chia seeds and blueberries',        DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Salmon poke bowl with edamame and avocado',             DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Protein shake, boiled eggs, whole grain toast',         DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Lentil and spinach soup with crusty bread',             DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Dinner',    'Grilled salmon fillet with quinoa salad',               DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Greek yogurt parfait with mixed berries and honey',     DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Turkey and avocado wrap with side salad',               DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Dinner',    'Zucchini noodles with homemade pesto and cherry tomatoes', DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Scrambled eggs with spinach and feta on rye bread',    DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Chickpea buddha bowl with tahini dressing',             DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Fresh fruit salad with cottage cheese',                 DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Grilled veggie panini with hummus',                     DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Dinner',    'Baked cod with asparagus and lemon butter',             DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Avocado toast with poached egg and chilli flakes',      DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Rainbow salad with grilled shrimp',                     DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ayesha, 'Breakfast', 'Mango smoothie with protein powder and flax seeds',     CAST(GETDATE() AS DATE), @now, @now),
(NEWID(), @ayesha, 'Lunch',     'Whole wheat pasta with tomato basil sauce',             CAST(GETDATE() AS DATE), @now, @now);


-- ============================================================
-- NIMRA YAQOOB — Student, irregular schedule, stress-prone
-- Trend: Variable mood, poor sleep on late nights, anxiety
-- ============================================================

INSERT INTO MoodLogs (Id, UserId, MoodScore, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @nimra, 6,  'Okay day, lot of studying to do',                        DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4,  'Anxious about upcoming exam, didn''t sleep well',        DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5,  'Exhausted but got through lectures',                      DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Took a break, watched a movie, felt better',             DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4,  'Headache and stress, exam tomorrow',                     DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'Exam done! Relief, but tired',                           DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8,  'Weekend, slept in, feeling refreshed',                   DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Caught up with friends, good day',                       DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5,  'Back to study grind, a bit low',                         DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4,  'Very tired, skipped breakfast, bad idea',                DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'Got some rest, better than yesterday',                   DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Productive study session, felt accomplished',             DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5,  'Stress crept back, worried about assignments',           DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'Submitted assignment, relief!',                           DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8,  'Went for a walk, mood improved a lot',                   DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Nice day, had a proper cooked meal',                     DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5,  'Monday blues, hard to get started',                      DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'Okay, had tea and study session went well',              DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4,  'Stomach ache, couldn''t focus at all',                  DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'Recovered, lighter day today',                           DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Good study vibe, nice weather helped',                   DATEADD(day,-9, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8,  'Birthday dinner with family, wonderful!',                DATEADD(day,-8, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'A bit sluggish after big dinner yesterday',              DATEADD(day,-7, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5,  'Deadline stress, working late',                          DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Deadline met! Feeling proud of myself',                  DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8,  'Rest day, binge-watched shows guiltlessly',              DATEADD(day,-4, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6,  'Back to routine, manageable',                            DATEADD(day,-3, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Had a good conversation with a mentor, motivated',       DATEADD(day,-2, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8,  'Feeling hopeful and positive about the week ahead',      DATEADD(day,-1, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7,  'Good start, healthy breakfast and journaling',           CAST(GETDATE() AS DATE), @now, @now);

INSERT INTO SleepLogs (Id, UserId, HoursSlept, QualityScore, BedTime, WakeTime, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @nimra, 6.5, 6,  '00:00', '06:30', DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5.0, 3,  '02:00', '07:00', DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5.5, 4,  '01:30', '07:00', DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.0, 7,  '23:00', '06:00', DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4.5, 3,  '02:30', '07:00', DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6.0, 5,  '00:30', '06:30', DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 9.0, 9,  '22:00', '07:00', DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8.0, 8,  '22:30', '06:30', DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5.5, 4,  '01:00', '06:30', DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4.0, 2,  '03:00', '07:00', DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.0, 6,  '23:00', '06:00', DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.5, 7,  '23:00', '06:30', DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5.0, 4,  '01:30', '06:30', DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6.5, 6,  '00:00', '06:30', DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8.0, 8,  '22:00', '06:00', DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.5, 7,  '22:30', '06:00', DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5.5, 4,  '01:00', '06:30', DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 6.5, 6,  '23:30', '06:00', DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 5.0, 3,  '01:30', '06:30', DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.0, 7,  '23:00', '06:00', DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.5, 8,  '22:30', '06:00', DATEADD(day,-9, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8.5, 9,  '22:00', '06:30', DATEADD(day,-8, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.0, 7,  '23:00', '06:00', DATEADD(day,-7, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 4.5, 3,  '02:30', '07:00', DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.5, 8,  '22:30', '06:00', DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 9.0, 9,  '22:00', '07:00', DATEADD(day,-4, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.0, 7,  '23:00', '06:00', DATEADD(day,-3, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.5, 8,  '22:30', '06:00', DATEADD(day,-2, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 8.0, 8,  '22:00', '06:00', DATEADD(day,-1, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 7.5, 8,  '22:30', '06:00', CAST(GETDATE() AS DATE), @now, @now);

INSERT INTO SymptomLogs (Id, UserId, SymptomName, Severity, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @nimra, 'Anxiety',       7, 'Pre-exam panic, heart racing, hard to breathe normally',    DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Anxiety',       5, 'Residual nervousness after exam, calmed down by evening',   DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Headache',      5, 'Tension headache from too much screen and stress',          DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Fatigue',       7, 'Completely drained, skipping meals made it worse',          DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Stomach Ache',  5, 'Nausea and cramps, probably ate junk food late at night',   DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Eye Strain',    4, 'Burning eyes from 8+ hours of studying on laptop',          DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Insomnia',      6, 'Couldn''t fall asleep despite being tired, mind racing',    DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Neck Stiffness',3, 'Poor posture while studying on bed all day',               DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now);

INSERT INTO MealLogs (Id, UserId, MealType, Description, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @nimra, 'Breakfast', 'Cereal with milk, quick and easy',                              DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Instant noodles with boiled egg - late lunch',                  DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Dinner',    'Rice with daal and salad, home cooked',                         DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Snack',     'Chips and a can of soda while studying',                        DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Skipped - overslept and had to rush to class',                  DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Shawarma from campus canteen',                                  DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Toast with peanut butter and banana',                           DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Vegetable biryani, mom cooked',                                 DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Dinner',    'Grilled cheese sandwich and tomato soup',                       DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Snack',     'Dark chocolate and green tea',                                  DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Paratha with chai - comfort food',                              DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Leftover biryani, still delicious',                             DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Eggs and toast with orange juice',                              DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Dinner',    'Chicken karahi with naan, family dinner',                       DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Yogurt with honey and almonds',                                 DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Falafel wrap with tahini sauce',                                DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Dinner',    'Pasta aglio e olio, tried cooking myself',                      DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Smoothie: banana, spinach, almond milk',                        DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Chicken sandwich with fries from cafe',                         DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @nimra, 'Breakfast', 'Oatmeal with raisins and cinnamon',                             CAST(GETDATE() AS DATE), @now, @now),
(NEWID(), @nimra, 'Lunch',     'Greek salad with grilled chicken',                              CAST(GETDATE() AS DATE), @now, @now);


-- ============================================================
-- AHMAD NOOR — Office worker, gym-goer, manages chronic back pain
-- Trend: Consistent routine, back pain flare-ups, decent sleep
-- ============================================================

INSERT INTO MoodLogs (Id, UserId, MoodScore, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ahmad, 7,  'Good Monday, started work with fresh energy',             DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Back pain acting up after long sitting',                  DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Physiotherapy session helped a lot',                      DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Gym session felt strong today',                           DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Steady mood, work project going well',                    DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Tired after long meeting, back sore again',               DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9,  'Weekend football with friends, amazing!',                 DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Rest day, watched cricket, relaxed',                      DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Back to work, Monday grind',                             DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Lunch break walk helped my mood and back',               DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Great gym PR today, bench press personal best',          DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Back pain flare-up, skipped gym',                        DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 5,  'Pain worse, took pain relief, rested',                   DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Slight improvement, went for a gentle walk',             DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Back to normal, physiotherapy again',                    DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Feeling strong, light workout done',                     DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Good focus at work, productive day',                     DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Evening cricket match, fun and relaxing',                DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Steady, consistent routine paying off',                  DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Mild fatigue, long day at office',                       DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Had a great workout, mood lifted',                       DATEADD(day,-9, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9,  'Weekend trip to hills, refreshing',                      DATEADD(day,-8, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Relaxed Sunday, cooked a nice meal',                     DATEADD(day,-7, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Work week starting, feeling prepared',                   DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6,  'Back twinge after heavy deadlift, being careful',        DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7,  'Pain subsided with stretching, better day',              DATEADD(day,-4, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Gym and a good meal, solid Thursday',                    DATEADD(day,-3, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9,  'Weekend ahead, excited for rest and leisure',             DATEADD(day,-2, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'Good family time, relaxed and happy',                    DATEADD(day,-1, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8,  'New week, positive mindset and full energy',             CAST(GETDATE() AS DATE), @now, @now);

INSERT INTO SleepLogs (Id, UserId, HoursSlept, QualityScore, BedTime, WakeTime, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ahmad, 7.0, 7,  '23:00', '06:00', DATEADD(day,-29,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6.5, 5,  '23:30', '06:00', DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 7,  '22:30', '06:00', DATEADD(day,-27,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-26,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.0, 7,  '23:00', '06:00', DATEADD(day,-25,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6.5, 5,  '23:30', '06:00', DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9.0, 9,  '22:00', '07:00', DATEADD(day,-23,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9.5, 10, '21:30', '07:00', DATEADD(day,-22,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.0, 6,  '23:00', '06:00', DATEADD(day,-21,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 7,  '22:30', '06:00', DATEADD(day,-20,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 5.5, 4,  '01:00', '06:30', DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.0, 6,  '23:00', '06:00', DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 7,  '22:30', '06:00', DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-15,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 8,  '22:30', '06:00', DATEADD(day,-14,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.0, 7,  '23:00', '06:00', DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-12,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 7,  '22:30', '06:00', DATEADD(day,-11,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6.5, 6,  '23:30', '06:00', DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-9, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9.0, 9,  '21:30', '06:30', DATEADD(day,-8, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 9.5, 10, '21:00', '06:30', DATEADD(day,-7, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.0, 7,  '23:00', '06:00', DATEADD(day,-6, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 6.5, 5,  '23:30', '06:00', DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 7,  '22:30', '06:00', DATEADD(day,-4, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-3, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.5, 9,  '21:45', '06:15', DATEADD(day,-2, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 8.0, 8,  '22:00', '06:00', DATEADD(day,-1, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 7.5, 8,  '22:30', '06:00', CAST(GETDATE() AS DATE), @now, @now);

INSERT INTO SymptomLogs (Id, UserId, SymptomName, Severity, Notes, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ahmad, 'Lower Back Pain', 7, 'Severe flare-up, couldn''t sit comfortably for more than 20 min',  DATEADD(day,-17,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lower Back Pain', 6, 'Still bad, took anti-inflammatory medication',                     DATEADD(day,-18,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lower Back Pain', 5, 'Improving slowly after physiotherapy session',                     DATEADD(day,-16,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lower Back Pain', 4, 'Mild twinge after heavy deadlift, stopped session early',          DATEADD(day,-5, CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lower Back Pain', 3, 'Dull ache after long office hours, standing desk helped',          DATEADD(day,-28,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Fatigue',         5, 'Post-gym tiredness combined with long work day',                   DATEADD(day,-24,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Muscle Soreness', 4, 'DOMS after leg day at the gym, difficult to walk down stairs',     DATEADD(day,-19,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Headache',        3, 'Dehydration headache, forgot to drink water during meetings',      DATEADD(day,-13,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Wrist Pain',      3, 'Typing all day caused wrist strain, used wrist brace',            DATEADD(day,-10,CAST(GETDATE() AS DATE)), @now, @now);

INSERT INTO MealLogs (Id, UserId, MealType, Description, LogDate, CreatedAt, UpdatedAt) VALUES
(NEWID(), @ahmad, 'Breakfast', 'Eggs scrambled with vegetables, black coffee',                  DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Grilled chicken breast with brown rice and salad',              DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Dinner',    'Beef stir-fry with bell peppers over noodles',                  DATEADD(day,-7,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Snack',     'Protein bar and banana post-workout',                           DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'Protein oatmeal with peanut butter',                           DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Tuna salad sandwich on whole wheat with soup',                  DATEADD(day,-6,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'Paratha with omelette and green tea',                           DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Chicken karahi with chapati at dhaba',                          DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Dinner',    'Grilled fish with steamed broccoli and sweet potato',           DATEADD(day,-5,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'Greek yogurt with nuts and a boiled egg',                       DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Beef biryani, office lunch order',                              DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Dinner',    'Homemade lentil soup with whole grain bread',                   DATEADD(day,-4,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'Whole grain toast with nut butter and black coffee',            DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Grilled chicken salad with olive oil dressing',                 DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Dinner',    'Mutton stew with naan, family dinner',                          DATEADD(day,-3,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'Smoothie: whey protein, spinach, banana, milk',                 DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Leftover stew with roti',                                       DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Dinner',    'Baked chicken thighs with roasted vegetables',                  DATEADD(day,-2,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'Boiled eggs, whole grain toast, orange juice',                  DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Quinoa bowl with chickpeas and roasted capsicum',               DATEADD(day,-1,CAST(GETDATE() AS DATE)), @now, @now),
(NEWID(), @ahmad, 'Breakfast', 'High-protein pancakes with fresh berries',                      CAST(GETDATE() AS DATE), @now, @now),
(NEWID(), @ahmad, 'Lunch',     'Chicken wrap with tzatziki and salad',                          CAST(GETDATE() AS DATE), @now, @now);

PRINT 'All 3 new accounts seeded successfully!';
