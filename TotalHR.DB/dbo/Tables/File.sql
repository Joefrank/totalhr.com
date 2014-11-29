CREATE TABLE [dbo].[File] (
    [id]            INT            IDENTITY (1, 1) NOT NULL,
    [shortname]     NVARCHAR (255) NOT NULL,
    [typeid]        INT            NOT NULL,
    [size]          BIGINT         NOT NULL,
    [created]       DATETIME       NOT NULL,
    [createdby]     INT            NOT NULL,
    [extension]     NVARCHAR (10)  NULL,
    [lastupdated]   DATETIME       NULL,
    [lastupdatedby] INT            NULL,
    CONSTRAINT [PK_files] PRIMARY KEY CLUSTERED ([id] ASC)
);

