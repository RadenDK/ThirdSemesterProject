use [DMA-CSD-V23_10478735];

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
        CreatedDate DATETIME DEFAULT GETDATE(),
        ChatType NVARCHAR (50) NOT NULL
    );


    CREATE TABLE GameLobby ( 
        GameLobbyID INT IDENTITY(1,1) PRIMARY KEY,
        LobbyName NVARCHAR (50) NOT NULL,
        AmountOfPlayers INT DEFAULT 10,
        PasswordHash NVARCHAR (50) DEFAULT NULL,
        InviteLink VARCHAR (50) NOT NULL,
        LobbyChatId INT NOT NULL,
        FOREIGN KEY (LobbyChatId) REFERENCES Chat(ChatID)
    );


    CREATE TABLE Player(
        PlayerID INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR (50) NOT NULL,
        PasswordHash NVARCHAR (200) NOT NULL,
        InGameName NVARCHAR (50) NOT NULL,
        Email VARCHAR (50) DEFAULT NULL,
        Birthday DATETIME NOT NULL,
        Elo INT DEFAULT 0,
        Banned bit DEFAULT 0,
        CurrencyAmount INT DEFAULT 0,
        GameLobbyID INT DEFAULT NULL,
        OnlineStatus bit DEFAULT 0,
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


 CREATE TABLE Admin (
        AdminID INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR (50) NOT NULL,
        Email VARCHAR (50) NOT NULL,
        CprNumber VARCHAR (10) NOT NULL UNIQUE,
        PhoneNumber VARCHAR (10) NOT NULL,
        AddressId INT NOT NULL,
        FOREIGN KEY (AddressId) REFERENCES Address(AddressId)
    );

    CREATE TABLE [Address] (
        AddressId INT IDENTITY(1,1) PRIMARY KEY,
        StreetName NVARCHAR (50) NOT NULL,
        StreetNumber INT NOT NULL,
        ZipCode INT NOT NULL,
        FOREIGN KEY (ZipCode) REFERENCES City(ZipCode)
    );

    CREATE TABLE City (
        ZipCode INT PRIMARY KEY,
        CityName NVARCHAR (50) NOT NULL
    );
