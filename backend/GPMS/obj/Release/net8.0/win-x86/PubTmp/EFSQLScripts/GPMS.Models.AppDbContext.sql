IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Users] (
        [UserId] bigint NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Supervisors] (
        [SupervisorId] bigint NOT NULL,
        [Name] VARCHAR(100) NOT NULL,
        [Email] VARCHAR(100) NOT NULL,
        [PasswordHash] varchar(100) NOT NULL,
        [Department] VARCHAR(15) NOT NULL,
        [TeamCount] int NOT NULL,
        [UserId] bigint NOT NULL,
        CONSTRAINT [PK_Supervisors] PRIMARY KEY ([SupervisorId]),
        CONSTRAINT [FK_Supervisors_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Teams] (
        [TeamId] bigint NOT NULL,
        [TeamName] VARCHAR(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [SupervisorId] bigint NULL,
        CONSTRAINT [PK_Teams] PRIMARY KEY ([TeamId]),
        CONSTRAINT [FK_Teams_Supervisors_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisors] ([SupervisorId]) ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Feedbacks] (
        [Id] bigint NOT NULL IDENTITY,
        [Content] text NOT NULL,
        [Date] datetime2 NOT NULL,
        [TeamId] bigint NOT NULL,
        [SupervisorId] bigint NULL,
        CONSTRAINT [PK_Feedbacks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Feedbacks_Supervisors_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisors] ([SupervisorId]) ON DELETE SET NULL,
        CONSTRAINT [FK_Feedbacks_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Projects] (
        [ProjectTitle] varchar(100) NOT NULL,
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(max) NOT NULL,
        [IsCompleted] bit NOT NULL,
        [SupervisorId] bigint NOT NULL,
        [TeamId] bigint NULL,
        CONSTRAINT [PK_Projects] PRIMARY KEY ([ProjectTitle]),
        CONSTRAINT [FK_Projects_Supervisors_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisors] ([SupervisorId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Projects_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Tasks] (
        [Id] bigint NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [Description] text NULL,
        [DueDate] datetime2 NULL,
        [Priority] int NOT NULL,
        [Status] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [TeamId] bigint NOT NULL,
        CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tasks_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Students] (
        [StudentId] bigint NOT NULL,
        [Name] varchar(100) NOT NULL,
        [Status] bit NOT NULL,
        [Department] nvarchar(max) NOT NULL,
        [Email] varchar(100) NOT NULL,
        [PasswordHash] varchar(100) NOT NULL,
        [UserId] bigint NOT NULL,
        [TeamId] bigint NULL,
        [KanbanTaskId] bigint NULL,
        [ProjectTitle] varchar(100) NULL,
        CONSTRAINT [PK_Students] PRIMARY KEY ([StudentId]),
        CONSTRAINT [FK_Students_Projects_ProjectTitle] FOREIGN KEY ([ProjectTitle]) REFERENCES [Projects] ([ProjectTitle]),
        CONSTRAINT [FK_Students_Tasks_KanbanTaskId] FOREIGN KEY ([KanbanTaskId]) REFERENCES [Tasks] ([Id]),
        CONSTRAINT [FK_Students_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Students_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Links] (
        [Id] bigint NOT NULL IDENTITY,
        [Url] varchar(500) NOT NULL,
        [Title] varchar(100) NOT NULL,
        [Date] datetime2 NOT NULL,
        [StudentId] bigint NOT NULL,
        [TeamId] bigint NOT NULL,
        CONSTRAINT [PK_Links] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Links_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Links_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE TABLE [Replys] (
        [Id] bigint NOT NULL IDENTITY,
        [Content] text NOT NULL,
        [Date] datetime2 NOT NULL,
        [FeedbackId] bigint NOT NULL,
        [StudentId] bigint NULL,
        [SupervisorId] bigint NULL,
        CONSTRAINT [PK_Replys] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Replys_Feedbacks_FeedbackId] FOREIGN KEY ([FeedbackId]) REFERENCES [Feedbacks] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Replys_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]),
        CONSTRAINT [FK_Replys_Supervisors_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisors] ([SupervisorId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Feedbacks_SupervisorId] ON [Feedbacks] ([SupervisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Feedbacks_TeamId] ON [Feedbacks] ([TeamId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Links_StudentId] ON [Links] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Links_TeamId] ON [Links] ([TeamId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Projects_Id] ON [Projects] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Projects_SupervisorId] ON [Projects] ([SupervisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Projects_TeamId] ON [Projects] ([TeamId]) WHERE [TeamId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Replys_FeedbackId] ON [Replys] ([FeedbackId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Replys_StudentId] ON [Replys] ([StudentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Replys_SupervisorId] ON [Replys] ([SupervisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Students_Email] ON [Students] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Students_KanbanTaskId] ON [Students] ([KanbanTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Students_ProjectTitle] ON [Students] ([ProjectTitle]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Students_TeamId] ON [Students] ([TeamId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Students_UserId] ON [Students] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Supervisors_Email] ON [Supervisors] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Supervisors_UserId] ON [Supervisors] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Tasks_TeamId] ON [Tasks] ([TeamId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE INDEX [IX_Teams_SupervisorId] ON [Teams] ([SupervisorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Teams_TeamName] ON [Teams] ([TeamName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250920155948_added user entity and change some of relationships and remove studenttask'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250920155948_added user entity and change some of relationships and remove studenttask', N'8.0.0');
END;
GO

COMMIT;
GO

