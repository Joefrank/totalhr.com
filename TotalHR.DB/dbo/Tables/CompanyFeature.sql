CREATE TABLE [dbo].[CompanyFeature] (
    [CompanyId] INT      NOT NULL,
    [FeatureId] INT      NOT NULL,
    [Created]   DATETIME CONSTRAINT [DF_CompanyFeature_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] INT      NOT NULL,
    CONSTRAINT [PK_CompanyFeature] PRIMARY KEY CLUSTERED ([CompanyId] ASC, [FeatureId] ASC)
);

