use [DMA-CSD-V23_10485521];

-- Drop tables in reverse order of creation
DROP TABLE IF EXISTS [Order];
DROP TABLE IF EXISTS ItemCopy;
DROP TABLE IF EXISTS ProfilePicture;
DROP TABLE IF EXISTS ChatEmojis;
DROP TABLE IF EXISTS Item;
DROP TABLE IF EXISTS FriendList;
DROP TABLE IF EXISTS Message;
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
    PasswordHash NVARCHAR (50) NOT NULL,
    InGameName NVARCHAR (50) NOT NULL,
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
INSERT INTO Player (Username, PasswordHash, inGameName, GameLobbyID) VALUES ('Player1', 'hash1', 'InGameName1', 1);
INSERT INTO Player (Username, PasswordHash, inGameName, GameLobbyID) VALUES ('Player2', 'hash2', 'InGameName2', 1);
INSERT INTO Player (Username, PasswordHash, inGameName) VALUES ('Player3', 'hash3', 'InGameName3');
INSERT INTO Player (Username, PasswordHash, inGameName) VALUES ('Player4', 'hash4', 'InGameName4');
INSERT INTO Player (Username, PasswordHash, inGameName, GameLobbyID) VALUES ('Player5', 'hash5', 'InGameName5', 1);
INSERT INTO Player (Username, PasswordHash, inGameName, GameLobbyID) VALUES ('Player6', 'hash6', 'InGameName6', 1);
INSERT INTO Player (Username, PasswordHash, inGameName) VALUES ('Player7', 'hash7', 'InGameName7');
INSERT INTO Player (Username, PasswordHash, inGameName) VALUES ('Player8', 'hash8', 'InGameName8');

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


