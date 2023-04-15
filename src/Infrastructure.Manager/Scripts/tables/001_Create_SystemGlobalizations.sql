IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SystemGlobalization' and xtype='U')
BEGIN
    CREATE TABLE SystemGlobalization (
        [Id] BIGINT PRIMARY KEY IDENTITY(1,1),

        [Key] VARCHAR(255) NOT NULL,
        [Resource] NVARCHAR(MAX) NOT NULL,

        [CreatedOn] DATETIME DEFAULT GETDATE(),
        [CreatedBy] NVARCHAR(255) NOT NULL,
        [ModifiedOn] DATETIME NULL,
        [ModifiedBy] NVARCHAR(255) NULL,

        CONSTRAINT CK_SystemGlobalization_JSON CHECK(ISJSON(Resource) = 1)
    )

    CREATE UNIQUE INDEX IXU_SystemGlobalization_Key ON dbo.SystemGlobalization([Key])
END