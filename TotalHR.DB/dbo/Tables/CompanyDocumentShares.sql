CREATE TABLE [dbo].[CompanyDocumentShares] (
    [DocumentId]  INT      NOT NULL,
    [UserId]      INT      NOT NULL,
    [DateShared]  DATETIME NOT NULL,
    [TypeofShare] INT      NOT NULL,
    CONSTRAINT [PK_CompanyDocumentShares] PRIMARY KEY CLUSTERED ([DocumentId] ASC, [UserId] ASC)
);

