CREATE TABLE [dbo].[FormField] (
    [id]            INT      IDENTITY (1, 1) NOT NULL,
    [FormControlId] INT      NOT NULL,
    [LabelId]       INT      NULL,
    [TooltipId]     INT      NULL,
    [Created]       DATETIME CONSTRAINT [DF_FormField_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT      NOT NULL,
    [LastUpdated]   DATETIME NULL,
    [LastUpdatedBy] INT      NULL,
    [obsolete]      BIT      CONSTRAINT [DF_FormField_obsolete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FormField] PRIMARY KEY CLUSTERED ([id] ASC)
);

