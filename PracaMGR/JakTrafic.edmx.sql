
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/16/2018 19:30:54
-- Generated from EDMX file: C:\Users\kamil\OneDrive\Dokumenty\Praca MGR ostateczna wersja\PracaMGR\PracaMGR\JakTrafic.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [jaktrafie];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Odjazdy_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Odjazdy] DROP CONSTRAINT [FK_Odjazdy_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Odpowiedzi_P]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Odpowiedzi] DROP CONSTRAINT [FK_Odpowiedzi_P];
GO
IF OBJECT_ID(N'[dbo].[FK_Posty_K]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posty] DROP CONSTRAINT [FK_Posty_K];
GO
IF OBJECT_ID(N'[dbo].[FK_PrzystankiRaport_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrzystankiRaport] DROP CONSTRAINT [FK_PrzystankiRaport_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LinieRaport] DROP CONSTRAINT [FK_Table_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_TrasyRaport_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TrasyRaport] DROP CONSTRAINT [FK_TrasyRaport_ToTable];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DniSwiateczne]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DniSwiateczne];
GO
IF OBJECT_ID(N'[dbo].[Kategorie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Kategorie];
GO
IF OBJECT_ID(N'[dbo].[Linie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Linie];
GO
IF OBJECT_ID(N'[dbo].[LinieRaport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LinieRaport];
GO
IF OBJECT_ID(N'[dbo].[Oceny]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Oceny];
GO
IF OBJECT_ID(N'[dbo].[Odjazdy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Odjazdy];
GO
IF OBJECT_ID(N'[dbo].[Odpowiedzi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Odpowiedzi];
GO
IF OBJECT_ID(N'[dbo].[Posty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posty];
GO
IF OBJECT_ID(N'[dbo].[Przystanki]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Przystanki];
GO
IF OBJECT_ID(N'[dbo].[PrzystankiRaport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrzystankiRaport];
GO
IF OBJECT_ID(N'[dbo].[Trasy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Trasy];
GO
IF OBJECT_ID(N'[dbo].[TrasyRaport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TrasyRaport];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Kategorie'
CREATE TABLE [dbo].[Kategorie] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nazwa] nvarchar(100)  NULL
);
GO

-- Creating table 'Linie'
CREATE TABLE [dbo].[Linie] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Symbol] varchar(50)  NOT NULL
);
GO

-- Creating table 'LinieRaport'
CREATE TABLE [dbo].[LinieRaport] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_linia] int  NOT NULL,
    [Wystapienia] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Oceny'
CREATE TABLE [dbo].[Oceny] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Punktualnosc] int  NOT NULL,
    [Czystosc] int  NOT NULL,
    [Kultura_os_kierowcy] int  NOT NULL,
    [Komfort_jazdy] int  NOT NULL,
    [Poczucie_bezpieczenstwa] int  NOT NULL,
    [Suma] float  NOT NULL,
    [Symbol_linii] nvarchar(50)  NOT NULL,
    [Uzytkownik] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Odjazdy'
CREATE TABLE [dbo].[Odjazdy] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nr_przystanku] int  NOT NULL,
    [Typ] varchar(50)  NOT NULL,
    [Symbol_linii] varchar(10)  NOT NULL,
    [Nazwa_trasy] varchar(1000)  NOT NULL,
    [Godziny] varchar(1000)  NOT NULL
);
GO

-- Creating table 'Odpowiedzi'
CREATE TABLE [dbo].[Odpowiedzi] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tresc] nvarchar(500)  NOT NULL,
    [Uzytkownik] nvarchar(128)  NOT NULL,
    [Id_Post] int  NOT NULL,
    [Data] datetime  NOT NULL
);
GO

-- Creating table 'Posty'
CREATE TABLE [dbo].[Posty] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tresc] nvarchar(500)  NOT NULL,
    [Id_kategoria] int  NOT NULL,
    [Data] datetime  NOT NULL,
    [Uzytkownik] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'Przystanki'
CREATE TABLE [dbo].[Przystanki] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nazwa] varchar(100)  NOT NULL,
    [X] float  NOT NULL,
    [Y] float  NOT NULL,
    [Numer] int  NOT NULL,
    [Strefa] varchar(10)  NOT NULL,
    [Nazwa_rozklad] varchar(100)  NULL
);
GO

-- Creating table 'PrzystankiRaport'
CREATE TABLE [dbo].[PrzystankiRaport] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Numer_Przystanek] int  NOT NULL,
    [Wystapienia] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Trasy'
CREATE TABLE [dbo].[Trasy] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Linia] varchar(50)  NULL,
    [Przystanki] varchar(max)  NULL,
    [Czas] varchar(max)  NULL,
    [Typ] varchar(50)  NULL,
    [Notka] varchar(50)  NULL,
    [Znacznik] varchar(50)  NULL
);
GO

-- Creating table 'TrasyRaport'
CREATE TABLE [dbo].[TrasyRaport] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_Trasy] int  NOT NULL,
    [Wystapienia] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'DniSwiateczne'
CREATE TABLE [dbo].[DniSwiateczne] (
    [Id] int  NOT NULL,
    [data] datetime  NOT NULL,
    [opis] varchar(500)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Kategorie'
ALTER TABLE [dbo].[Kategorie]
ADD CONSTRAINT [PK_Kategorie]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Linie'
ALTER TABLE [dbo].[Linie]
ADD CONSTRAINT [PK_Linie]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LinieRaport'
ALTER TABLE [dbo].[LinieRaport]
ADD CONSTRAINT [PK_LinieRaport]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Oceny'
ALTER TABLE [dbo].[Oceny]
ADD CONSTRAINT [PK_Oceny]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Odjazdy'
ALTER TABLE [dbo].[Odjazdy]
ADD CONSTRAINT [PK_Odjazdy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Odpowiedzi'
ALTER TABLE [dbo].[Odpowiedzi]
ADD CONSTRAINT [PK_Odpowiedzi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Posty'
ALTER TABLE [dbo].[Posty]
ADD CONSTRAINT [PK_Posty]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Numer] in table 'Przystanki'
ALTER TABLE [dbo].[Przystanki]
ADD CONSTRAINT [PK_Przystanki]
    PRIMARY KEY CLUSTERED ([Numer] ASC);
GO

-- Creating primary key on [Id] in table 'PrzystankiRaport'
ALTER TABLE [dbo].[PrzystankiRaport]
ADD CONSTRAINT [PK_PrzystankiRaport]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Trasy'
ALTER TABLE [dbo].[Trasy]
ADD CONSTRAINT [PK_Trasy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TrasyRaport'
ALTER TABLE [dbo].[TrasyRaport]
ADD CONSTRAINT [PK_TrasyRaport]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DniSwiateczne'
ALTER TABLE [dbo].[DniSwiateczne]
ADD CONSTRAINT [PK_DniSwiateczne]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id_kategoria] in table 'Posty'
ALTER TABLE [dbo].[Posty]
ADD CONSTRAINT [FK_Posty_K]
    FOREIGN KEY ([Id_kategoria])
    REFERENCES [dbo].[Kategorie]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Posty_K'
CREATE INDEX [IX_FK_Posty_K]
ON [dbo].[Posty]
    ([Id_kategoria]);
GO

-- Creating foreign key on [Id_linia] in table 'LinieRaport'
ALTER TABLE [dbo].[LinieRaport]
ADD CONSTRAINT [FK_Table_ToTable]
    FOREIGN KEY ([Id_linia])
    REFERENCES [dbo].[Linie]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_ToTable'
CREATE INDEX [IX_FK_Table_ToTable]
ON [dbo].[LinieRaport]
    ([Id_linia]);
GO

-- Creating foreign key on [Nr_przystanku] in table 'Odjazdy'
ALTER TABLE [dbo].[Odjazdy]
ADD CONSTRAINT [FK_Odjazdy_ToTable]
    FOREIGN KEY ([Nr_przystanku])
    REFERENCES [dbo].[Przystanki]
        ([Numer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Odjazdy_ToTable'
CREATE INDEX [IX_FK_Odjazdy_ToTable]
ON [dbo].[Odjazdy]
    ([Nr_przystanku]);
GO

-- Creating foreign key on [Id_Post] in table 'Odpowiedzi'
ALTER TABLE [dbo].[Odpowiedzi]
ADD CONSTRAINT [FK_Odpowiedzi_P]
    FOREIGN KEY ([Id_Post])
    REFERENCES [dbo].[Posty]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Odpowiedzi_P'
CREATE INDEX [IX_FK_Odpowiedzi_P]
ON [dbo].[Odpowiedzi]
    ([Id_Post]);
GO

-- Creating foreign key on [Numer_Przystanek] in table 'PrzystankiRaport'
ALTER TABLE [dbo].[PrzystankiRaport]
ADD CONSTRAINT [FK_PrzystankiRaport_ToTable]
    FOREIGN KEY ([Numer_Przystanek])
    REFERENCES [dbo].[Przystanki]
        ([Numer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrzystankiRaport_ToTable'
CREATE INDEX [IX_FK_PrzystankiRaport_ToTable]
ON [dbo].[PrzystankiRaport]
    ([Numer_Przystanek]);
GO

-- Creating foreign key on [Id_Trasy] in table 'TrasyRaport'
ALTER TABLE [dbo].[TrasyRaport]
ADD CONSTRAINT [FK_TrasyRaport_ToTable]
    FOREIGN KEY ([Id_Trasy])
    REFERENCES [dbo].[Trasy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TrasyRaport_ToTable'
CREATE INDEX [IX_FK_TrasyRaport_ToTable]
ON [dbo].[TrasyRaport]
    ([Id_Trasy]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------