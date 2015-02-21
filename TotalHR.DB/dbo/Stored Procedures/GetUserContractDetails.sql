
CREATE Proc [dbo].[GetUserContractDetails]
	@userid int,
	@contractid int = null
as
Begin	
	
	select top 1 uc.id as ContractId, uc.TemplateId as CTTemplateId, uc.Created as CTCreated, uc.CreatedBy as CTCreatedBy, 
		uc.LastUpdatedBy as CTlastUpdatedBy, uc.LastUpdated as CTLastUpdated, uc.StatusId as CTStatusId,
		uc.Userid as CTUserId, (select firstname + ' ' + surname from [user] where id =uc.Userid) as CTUser,
		f.Id as FormId, f.FormSchema, f.Name as FormName, f.FormTypeId, uca.Data as CTData, uca.LastUpdated as DataLastUpdated, 
		(select firstname + ' ' + surname from [user] where id = uca.LastUpdatedBy) as DataLastUpdatedBy
	from Form f
	inner join ContractTemplate ct on ct.FormId = f.Id
	inner join UserContract uc on uc.TemplateId = ct.id
	left join usercontractdata uca on uca.contractid = uc.id and uca.userid = @userid
	where uc.Userid = @userid and (@contractid is null or @contractid = uc.id)
	
End