CREATE TABLE [dbo].[CompanyDocumentDownloads] (
    [id]           INT      IDENTITY (1, 1) NOT NULL,
    [DocumentId]   INT      NOT NULL,
    [UserId]       INT      NOT NULL,
    [DownloadDate] DATETIME CONSTRAINT [DF_CompanyDocumentDownloads_DownloadDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CompanyDocumentDownloads] PRIMARY KEY CLUSTERED ([id] ASC)
);

