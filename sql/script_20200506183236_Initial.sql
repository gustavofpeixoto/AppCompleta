IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Providers] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(200) NOT NULL,
    [Document] varchar(14) NOT NULL,
    [ProviderType] int NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Providers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Addresses] (
    [Id] uniqueidentifier NOT NULL,
    [ProviderId] uniqueidentifier NOT NULL,
    [PublicPlace] varchar(200) NOT NULL,
    [Number] varchar(50) NOT NULL,
    [AddressComplement] varchar(250) NOT NULL,
    [ZipCode] varchar(8) NOT NULL,
    [Neighborhood] varchar(100) NOT NULL,
    [City] varchar(100) NOT NULL,
    [State] varchar(50) NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Products] (
    [Id] uniqueidentifier NOT NULL,
    [ProviderId] uniqueidentifier NOT NULL,
    [Name] varchar(200) NOT NULL,
    [Description] varchar(1000) NOT NULL,
    [Picture] varchar(100) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [RegistrationDate] datetime2 NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE UNIQUE INDEX [IX_Addresses_ProviderId] ON [Addresses] ([ProviderId]);

GO

CREATE INDEX [IX_Products_ProviderId] ON [Products] ([ProviderId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200506183236_Initial', N'3.1.3');

GO

