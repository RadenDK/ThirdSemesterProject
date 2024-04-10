    use [DMA-CSD-V23_10485521];

    -- Drop tables in reverse order of creation
    DROP TABLE IF EXISTS [Order];
    DROP TABLE IF EXISTS ItemCopy;
    DROP TABLE IF EXISTS ProfilePicture;
    DROP TABLE IF EXISTS ChatEmojis;
    DROP TABLE IF EXISTS Item;
    DROP TABLE IF EXISTS FriendList;
    DROP TABLE IF EXISTS [Message];
    DROP TABLE IF EXISTS Player;
    DROP TABLE IF EXISTS GameLobby;
    DROP TABLE IF EXISTS Chat;

    -- Creating the tables

    CREATE TABLE Chat(
        ChatID INT IDENTITY(1,1) PRIMARY KEY,
        CreatedDate DATETIME DEFAULT GETDATE() CHECK (CreatedDate <= GETDATE()),
        ChatType NVARCHAR (50) NOT NULL
    );


    CREATE TABLE GameLobby ( 
        GameLobbyID INT IDENTITY(1,1) PRIMARY KEY,
        LobbyName NVARCHAR (50) NOT NULL,
        AmountOfPlayers INT DEFAULT 10 CHECK (AmountOfPlayers BETWEEN 1 AND 10),
        PasswordHash NVARCHAR (50) DEFAULT NULL,
        InviteLink VARCHAR (50) NOT NULL,
        LobbyChatId INT NOT NULL,
        FOREIGN KEY (LobbyChatId) REFERENCES Chat(ChatID) ON DELETE SET NULL
    );


    CREATE TABLE Player(
        PlayerID INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR (50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR (200) NOT NULL,
        InGameName NVARCHAR (50) NOT NULL UNIQUE,
        Email VARCHAR (50) NOT NULL,
        Birthday DATETIME NOT NULL CHECK (Birthday <= GETDATE()),
        Elo INT DEFAULT 0 CHECK (Elo >= 0),
        Banned bit DEFAULT 0,
        CurrencyAmount INT DEFAULT 0 CHECK (CurrencyAmount >= 0),
        GameLobbyID INT DEFAULT NULL,
        OnlineStatus bit DEFAULT 0,
        FOREIGN KEY (GameLobbyID) REFERENCES GameLobby(GameLobbyID) ON DELETE SET NULL
    );


    CREATE TABLE Message (
        MessageID INT IDENTITY(1,1) PRIMARY KEY,
        TextMessage NVARCHAR (MAX) NOT NULL,
        [TimeStamp] DATETIME DEFAULT GETDATE() CHECK ([TimeStamp] <= GETDATE()),
        SenderID INT NOT NULL,
        ChatID INT NOT NULL,
        [Read] bit DEFAULT 0,
        FOREIGN KEY (SenderID) REFERENCES Player(PlayerID), 
        FOREIGN KEY (ChatID) REFERENCES Chat(ChatID)
    );


    CREATE TABLE FriendList(
        Player1ID INT NOT NULL,
        Player2ID INT NOT NULL,
        ChatId INT DEFAULT NULL,
        FOREIGN KEY (Player1ID) REFERENCES Player(PlayerID), ON DELETE CASCADE,
        FOREIGN KEY (Player2ID) REFERENCES Player(PlayerID), ON DELETE CASCADE,
        FOREIGN KEY (ChatId) REFERENCES Chat(ChatID) ON DELETE SET NULL,
        CONSTRAINT PK_FriendList PRIMARY KEY (Player1ID, Player2ID)
    );


    CREATE TABLE Item (
        ItemID INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR (50) NOT NULL UNIQUE,
        ReleaseDate DATETIME DEFAULT GETDATE(),
        Price INT NOT NULL CHECK (Price >= 0),
        CopyQuantity INT NOT NULL CHECK (CopyQuantity >= 0),
        ItemType NVARCHAR (50) NOT NULL
    );


    CREATE TABLE ChatEmojis (
        ChatEmojiID INT PRIMARY KEY,
        [Image] VARBINARY(MAX) NOT NULL,
        FOREIGN KEY (ChatEmojiID) REFERENCES Item(ItemID)
    );


    CREATE TABLE ProfilePicture (
        ProfilePictureID INT IDENTITY(1,1) PRIMARY KEY,
        [Image] VARBINARY(MAX) NOT NULL,
        FOREIGN KEY (ProfilePictureID) REFERENCES Item(ItemID)
    );


    CREATE TABLE ItemCopy (
        ItemCopyID INT IDENTITY(1,1) PRIMARY KEY,
        ItemID INT NOT NULL,
        FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
    );


    CREATE TABLE [Order] (
        OrderID INT IDENTITY(1,1) PRIMARY KEY,
        OrderDate DATETIME DEFAULT GETDATE(),
        PlayerID INT NOT NULL,
        ItemCopyID INT NOT NULL,
        FOREIGN KEY (ItemCopyID) REFERENCES ItemCopy(ItemCopyID),
        FOREIGN KEY (PlayerID) REFERENCES Player(PlayerID)
    );

    CREATE TABLE Admin (
        AdminID INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR (50) NOT NULL,
        Email VARCHAR (50) NOT NULL,
        CprNumber VARCHAR (10) NOT NULL UNIQUE,
        PhoneNumber VARCHAR (10) NOT NULL,
        AddressId INT NOT NULL,
        FOREIGN KEY (AddressId) REFERENCES Address(AddressId)
    )

    CREATE TABLE [Address] (
        AddressId INT IDENTITY(1,1) PRIMARY KEY,
        StreetName NVARCHAR (50) NOT NULL,
        StreetNumber INT NOT NULL,
        ZipCode INT NOT NULL,
        FOREIGN KEY (ZipCode) REFERENCES City(ZipCode)
    )

    CREATE TABLE City (
        ZipCode INT PRIMARY KEY,
        CityName NVARCHAR (50) NOT NULL
    )


    -- Inserting mock data

    -- Insert data into Chat
    INSERT INTO Chat (chatType) VALUES ('GameLobby');
    INSERT INTO Chat (chatType) VALUES ('GameLobby');
    INSERT INTO Chat (chatType) VALUES ('Friend');
    INSERT INTO Chat (chatType) VALUES ('Friend');
    INSERT INTO Chat (chatType) VALUES ('Friend');


    -- Insert data into GameLobby
    INSERT INTO GameLobby (LobbyName, PasswordHash, inviteLink, lobbyChatId) VALUES ('Lobby1', NULL, 'link1', 1);


    -- Insert data into Player
    -- Insert data into Player
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player1', '$2a$11$GsmfIz3OPipR6f5avJUDTuFMItDbPZtiCmYScex0uZxo1z4Q6iP/i', 'InGameName1', GETDATE(), 'player1@example.com', 1); -- Password is a hashed version of "Player1"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player2', '$2a$11$j3D2bz5NUgeivWNZOc0dyOmSlfsHkMgmmcXK5cJTxUg6xZAZTZbKe', 'InGameName2', GETDATE(), 'player2@example.com', 1); -- Password is a hashed version of "Player2"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player3', '$2a$11$I9Z4yRD6qBWux3xeoKZmDuKB4VQRBg.yDTCdRE4toQPVHA6b54Oze', 'InGameName3', GETDATE(), 'player3@example.com'); -- Password is a hashed version of "Player3"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player4', '$2a$11$V8M5WAQWb9cl68sm0LFHCe0mIaxXexs4yj0QnAy352LDtRsBF5TsK', 'InGameName4', GETDATE(), 'player4@example.com'); -- Password is a hashed version of "Player4"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player5', '$2a$11$6ule5Buo7y1LSAKCA5VLreoKA5gFLIa.RnVH1Cg66HHucedfuSu3C', 'InGameName5', GETDATE(), 'player5@example.com', 1); -- Password is a hashed version of "Player5"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player6', '$2a$11$8nKGqpk2d4SRpXo2Nn13Z.E1nvCzoPBph79pD/pYgc.62mB3vP/a.', 'InGameName6', GETDATE(), 'player6@example.com', 1); -- Password is a hashed version of "Player6"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player7', '$2a$11$o0ZNk3a0JBhXjqSzUP0ZQOlMFBVjwqsLkaxFhJhihWOhgyQHErwyK', 'InGameName7', GETDATE(), 'player7@example.com'); -- Password is a hashed version of "Player7"
    INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player8', '$2a$11$3Y.Pqf9.U4ydTMEYvr4QFuAiWx6P9pQHAI1M/31pcXTsHa3pbHIHC', 'InGameName8', GETDATE(), 'player8@example.com'); -- Password is a hashed version of "Player8"

    -- Insert data into Message

    -- Messages in gamelobby 1
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('Hello', 1, 1);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('How are everyone doing?', 1, 1);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('I am fine', 2, 1);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('I am also fine', 5, 1);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('How are you doing player 1?', 2, 1);


    -- Messages between different players
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('Wanna game?', 3, 3);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('HELLOO?', 3, 3);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('Sorry I did not see you message', 4, 3);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('I am ready later if you are still down', 4, 3);


    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('Tip top tissemand', 1, 4);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('Hvad fanden snakker du om?', 5, 4);
    INSERT INTO Message (textMessage, senderID, ChatID) VALUES ('Flink flot fissekone', 1, 4);


    -- Insert data into FriendList
    INSERT INTO FriendList (Player1ID, Player2ID, chatId) VALUES (3, 4, 3);
    INSERT INTO FriendList (Player1ID, Player2ID) VALUES (2, 1);
    INSERT INTO FriendList (Player1ID, Player2ID) VALUES (2, 7);
    INSERT INTO FriendList (Player1ID, Player2ID) VALUES (7, 8);
    INSERT INTO FriendList (Player1ID, Player2ID) VALUES (1, 3);
    INSERT INTO FriendList (Player1ID, Player2ID, chatId) VALUES (1, 5, 4);