CREATE TABLE [dbo].[CompanyDocumentViews] (
    [DocumentId] INT      NOT NULL,
    [UserId]     INT      NOT NULL,
    [ViewDate]   DATETIME CONSTRAINT [DF_CompanyDocumentViews_ViewDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CompanyDocumentViews] PRIMARY KEY CLUSTERED ([DocumentId] ASC, [UserId] ASC)
);

