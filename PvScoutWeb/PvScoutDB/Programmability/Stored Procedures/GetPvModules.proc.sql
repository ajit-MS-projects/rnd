if exists (select * from dbo.sysobjects where id = object_id(N'[GetPvModules]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop PROCEDURE [dbo].[GetPvModules] 
go

create  PROCEDURE [dbo].[GetPvModules] 
	@ManufId varchar(250)=null,
	@PVscoutArticleNumber varchar(250)=null
	as
begin
	if @PvscoutArticleNumber is not null
		SELECT * from PvModules where PVscoutArticleNumber=@PVscoutArticleNumber;
	else
		SELECT * from PvModules where ManufId = @ManufId;
		
end 