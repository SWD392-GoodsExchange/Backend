Drop
Database GoodsExchange;
GO

CREATE DATABASE GoodsExchange;
GO
USE GoodsExchange;
GO
-- Create tables
CREATE TABLE [Role] (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL,
    CONSTRAINT CHK_RoleName CHECK (RoleName IN ('Admin', 'Member','Moderator'))
    );
GO
CREATE TABLE [Member] (
    FeID NVARCHAR(8) PRIMARY KEY,
    RoleID INT NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    PasswordSalt VARBINARY(128)NOT NULL,
    PasswordHash VARBINARY(128) NOT NULL,
    [Address] NVARCHAR(255) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15) NOT NULL,
    CreatedTime DATETIME NOT NULL,
    UpdatedTime DATETIME NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    Dob DATE,
    FOREIGN KEY (RoleID) REFERENCES [Role](RoleID),
    CONSTRAINT CHK_StatusMember CHECK ([Status] IN ('Banned', 'Available'))

    );
GO
CREATE TABLE [Notification](
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    SenderID NVARCHAR(8) not null,
    SenderUsername NVARCHAR(100) NOT NULL,
    RecipientID NVARCHAR(8) not null,
    RecipientUsername NVARCHAR(100) NOT NULL,
    OnwerProductId NVarchar(255),
    ExchangerProductIds NVarchar(255),
    Content NVARCHAR(100) NOT NULL,
    DateRead Datetime,
    CreatedDate DATETIME NOT NULL,
    [Type] NVARCHAR(50) NOT NULL,
    FOREIGN KEY (SenderID) REFERENCES [Member](FeID),
    FOREIGN KEY (RecipientID) REFERENCES [Member](FeID),
    CONSTRAINT CHK_TypeNoti CHECK ([Type] IN ('ExchangeRequest', 'Notification'))
    )
    GO
CREATE TABLE Category (
    CateID INT IDENTITY(1,1) PRIMARY KEY,
    CateName NVARCHAR(100) NOT NULL
);
GO
CREATE TABLE Product (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    FeID NVARCHAR(8) NOT NULL,
    CateID INT NOT NULL,
    [Description] NVARCHAR(255) NOT NULL,
    Origin NVARCHAR(100) NOT NULL,
    [Type] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    CreatedTime DATETIME NOT NULL,
    UpdatedTime DATETIME NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    FOREIGN KEY (FeID) REFERENCES [Member](FeID),
    FOREIGN KEY (CateID) REFERENCES Category(CateID),
    CONSTRAINT CHK_TypeProduct CHECK ([Type] IN ('Exchange', 'Trade')),
    CONSTRAINT CHK_StatusProduct CHECK ([Status] IN ('Selling', 'Processing','Sold'))
    );
GO
CREATE TABLE Image (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ImageUrl NVARCHAR(255) NOT NULL,
    ProductID INT NOT NULL,
    PublicID NVARCHAR(50) NOT NULL,
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
GO
CREATE TABLE [Order] (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    BuyerID NVARCHAR(8) NOT NULL,
    CreatedTime DATETIME NOT NULL,
    UpdatedTime DATETIME NOT NULL,
    TotalAmount DECIMAL(18, 2),
    TotalOrderDetails INT,
    [Type] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    FOREIGN KEY (BuyerID) REFERENCES [Member](FeID),
    CONSTRAINT CHK_StatusOrder CHECK ([Status] IN ('Processing', 'Completed','Cancelled')),
    CONSTRAINT CHK_TypeOrder CHECK ([Type] IN ('Exchange', 'Trade'))
    );
GO
CREATE TABLE OrderDetail (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    SellerID NVARCHAR(8) NOT NULL,
    Amount DECIMAL(18, 2),
    FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
);
GO
CREATE TABLE Bookmark (
    ProductID INT NOT NULL,
    FeID NVARCHAR(8) NOT NULL,
    CreateTime DATETIME NOT NULL,
    PRIMARY KEY (ProductID, FeID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    FOREIGN KEY (FeID) REFERENCES [Member](FeID),
);
GO
CREATE TABLE Report (
    ReportID INT IDENTITY(1,1) PRIMARY KEY,
    FeID NVARCHAR(8) NOT NULL,
    ProductID INT NOT NULL,
    [Message] NVARCHAR(255) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    CreatedTime DATETIME NOT NULL,
	UpdatedTime DATETIME NOT NULL,
    FOREIGN KEY (FeID) REFERENCES [Member](FeID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT CHK_StatusReport CHECK ([Status] IN ('Processing', 'Approved','Rejected'))
    );
GO

CREATE TABLE RefreshToken(
    RefreshTokenID INT IDENTITY(1,1) PRIMARY KEY,
    FeID NVARCHAR(8) NOT NULL,
    Token NVARCHAR(255) NOT NULL,
    ExpiryDate DATETIME NOT NULL,
    FOREIGN KEY (FeID) REFERENCES [Member](FeID)
);
GO