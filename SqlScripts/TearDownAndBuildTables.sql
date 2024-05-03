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
    DROP TABLE IF EXISTS [Admin];
    DROP TABLE IF EXISTS [Address];
    DROP TABLE IF EXISTS [City];


    -- Creating the tables


    CREATE TABLE Chat(
        ChatID INT IDENTITY(1,1) PRIMARY KEY,
        CreatedDate DATETIME DEFAULT GETDATE(),
        ChatType NVARCHAR (50) NOT NULL
    );


    CREATE TABLE GameLobby ( 
        GameLobbyID INT IDENTITY(1,1) PRIMARY KEY,
        LobbyName NVARCHAR (50) NOT NULL,
        AmountOfPlayers INT DEFAULT 10,
        PasswordHash NVARCHAR (MAX) DEFAULT NULL,
        InviteLink VARCHAR (50) NOT NULL,
        LobbyChatId INT NOT NULL,
        FOREIGN KEY (LobbyChatId) REFERENCES Chat(ChatID)
    );


    CREATE TABLE Player(
        PlayerID INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR (50) NOT NULL,
        PasswordHash NVARCHAR (MAX) NOT NULL,
        InGameName NVARCHAR (50) NOT NULL,
        Email VARCHAR (50) DEFAULT NULL,
        Birthday DATETIME NOT NULL,
        Elo INT DEFAULT 0,
        Banned bit DEFAULT 0,
        CurrencyAmount INT DEFAULT 0,
        GameLobbyID INT DEFAULT NULL,
        OnlineStatus bit DEFAULT 0,
        IsOwner bit DEFAULT 0,
        FOREIGN KEY (GameLobbyID) REFERENCES GameLobby(GameLobbyID)
    );


    CREATE TABLE Message (
        MessageID INT IDENTITY(1,1) PRIMARY KEY,
        TextMessage NVARCHAR (MAX) NOT NULL,
        [TimeStamp] DATETIME DEFAULT GETDATE(),
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
        FOREIGN KEY (Player1ID) REFERENCES Player(PlayerID),
        FOREIGN KEY (Player2ID) REFERENCES Player(PlayerID),
        FOREIGN KEY (ChatId) REFERENCES Chat(ChatID),
        CONSTRAINT PK_FriendList PRIMARY KEY (Player1ID, Player2ID)
    );


    CREATE TABLE Item (
        ItemID INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR (50) NOT NULL,
        ReleaseDate DATETIME DEFAULT GETDATE(),
        Price INT NOT NULL,
        CopyQuantity INT NOT NULL,
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

     CREATE TABLE City (
        ZipCode INT PRIMARY KEY,
        CityName NVARCHAR (50) NOT NULL
    );
    
    CREATE TABLE [Address] (
            AddressId INT IDENTITY(1,1) PRIMARY KEY,
            StreetName NVARCHAR (50) NOT NULL,
            StreetNumber INT NOT NULL,
            ZipCode INT NOT NULL,
            FOREIGN KEY (ZipCode) REFERENCES City(ZipCode)
        );

    CREATE TABLE Admin (
            AdminID INT IDENTITY(1,1) PRIMARY KEY,
            [Name] NVARCHAR (50) NOT NULL,
            Email VARCHAR (50) NOT NULL,
            CprNumber VARCHAR (10) NOT NULL UNIQUE,
            PhoneNumber VARCHAR (10) NOT NULL,
            AddressId INT NOT NULL,
            PasswordHash NVARCHAR (MAX) NOT NULL,
            FOREIGN KEY (AddressId) REFERENCES Address(AddressId)
        );

   




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
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player1', 'hash1', 'InGameName1', GETDATE(), 'player1@example.com', 1);
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player2', 'hash2', 'InGameName2', GETDATE(), 'player2@example.com', 1);
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player3', 'hash3', 'InGameName3', GETDATE(), 'player3@example.com');
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player4', 'hash4', 'InGameName4', GETDATE(), 'player4@example.com');
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player5', 'hash5', 'InGameName5', GETDATE(), 'player5@example.com', 1);
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email, GameLobbyID) VALUES ('Player6', 'hash6', 'InGameName6', GETDATE(), 'player6@example.com', 1);
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player7', 'hash7', 'InGameName7', GETDATE(), 'player7@example.com');
INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) VALUES ('Player8', 'hash8', 'InGameName8', GETDATE(), 'player8@example.com');

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


    -- Insert data into City
INSERT INTO City (ZipCode, CityName) VALUES (1000, 'Copenhagen');
INSERT INTO City (ZipCode, CityName) VALUES (2000, 'Frederiksberg');


-- Insert data into Address
INSERT INTO Address (StreetName, StreetNumber, ZipCode) VALUES ('Main Street', 1, 1000);
INSERT INTO Address (StreetName, StreetNumber, ZipCode) VALUES ('Second Street', 2, 2000);


-- Insert an admin named admin1 with password admin1
INSERT INTO Admin ([Name], Email, CprNumber, PhoneNumber, AddressId, PasswordHash) 
VALUES ('admin1', 'admin1@example.com', '1234567890', '1234567890', 1, 'admin1');


