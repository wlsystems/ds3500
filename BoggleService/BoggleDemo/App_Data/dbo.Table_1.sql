CREATE TABLE [dbo].[Games]
(
	[GameID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Player1] VARCHAR(36) NOT NULL, 
    [Player2] VARCHAR(36) NULL, 
    [Board] NCHAR(16) NULL, 
    [TimeLimit] INT NULL, 
    [StartTime] DATETIME NULL, 
    CONSTRAINT [FK_Games_Users] FOREIGN KEY (Player1) REFERENCES Users(UserID), 
    CONSTRAINT [FK_Games_Users2] FOREIGN KEY (Player2) REFERENCES Users(UserID)
)
