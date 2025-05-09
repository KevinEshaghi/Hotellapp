SELECT [r].[RoomNumber], [r].[RoomType], [r].[MaxOccupants]
FROM [Rooms] AS [r]
WHERE LOWER([r].[RoomType]) = 'dubbelrum'
ORDER BY [r].[RoomNumber] ASC
