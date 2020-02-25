﻿insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('331a23c3-c647-4a2e-a5cd-b9c0cb5f8e17', 'Kate', 'Brook','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('e304ad1d-1fc5-4f4e-a16b-3484727c034f', 'Maze', 'Smith','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('1064272c-c5d5-48d7-b061-5aee13139867', 'Alex', 'Gall','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('7363b82d-a565-4c76-aae9-b36ce90c5fe1', 'Claire', 'Robertson','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('a787942a-a1d6-409d-a4ee-46fe105c3fd5', 'William', 'Barny','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('f9f090dd-1909-478a-b85d-158565e2a51e', 'Suzy', 'Candle','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('1da7e0d2-d5d7-4d68-ace3-826b79846917', 'Bart', 'Town','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('adfccff1-00a2-4388-8ef4-cdb1b3324d15', 'Barry', 'Allen','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('b9999c0e-d8e0-4a99-b6e4-0d5b480bf9fd', 'Stefania', 'Beerzy','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Users(Id,FirstName,LastName,DateOfBirth,IsActive,Role,Sex,CreationDate) values ('d196d791-c631-4701-a6bc-a56816d1c3cd', 'Lucy', 'Hopeson','1996-04-11 12:00','true',2,'false','2019-04-24 13:00')
insert into Events(Id,Title,StartDate,EndDate,CreationDate,IsActive,Type,UserId) values ('498b19ab-a015-4e4c-a3c4-c24e76b94aff', 'Call', '2019-04-17 12:00','2019-05-20 12:00','2019-05-21 12:00','true',1,(select Id from Users where LastName='Some'))
insert into Events(Id,Title,StartDate,EndDate,CreationDate,IsActive,Type,UserId) values ('498b19ab-a015-4e4c-a3c4-c24e76b94aff', 'Call', '2019-04-17 12:00','2019-05-20 12:00','2019-05-21 12:00','true',1,(select Id from Users where LastName='Test user'))

CREATE TABLE [dbo].[PersistedGrants] (
    [Key]           VARCHAR  NOT NULL PRIMARY KEY,
    [Type]    NVARCHAR (MAX)    NOT NULL,
    [SubjectId]     NVARCHAR (MAX)    NOT NULL,
    [ClientId] NVARCHAR (MAX)     NOT NULL,
    [CreationTime]  DATETIME2 (7)    NOT NULL,
    [Expiration]        DATETIME2 (7)   NULL,
    [Data]        NVARCHAR (MAX)    NOT NULL,
);