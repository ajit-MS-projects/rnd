if exists (select * from dbo.sysobjects where id = object_id(N'[GetRoofTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop PROCEDURE [dbo].[GetRoofTypes] 
go

create  PROCEDURE [dbo].[GetRoofTypes] 
	@RoofTypeId int = 0 
AS
begin
	if @RoofTypeId = 0
		SELECT * from RoofTypes;
	else
		SELECT * from RoofTypes where RoofTypeId = @RoofTypeId;
		
end 