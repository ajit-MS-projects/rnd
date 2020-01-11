if exists (select * from dbo.sysobjects where id = object_id(N'[GetManufacturers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop PROCEDURE [dbo].[GetManufacturers] 
go

create  PROCEDURE [dbo].[GetManufacturers] 
AS
begin
		SELECT * from Manufacturers;
end 