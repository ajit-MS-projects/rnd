

go
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'solr_GetProducts')
	BEGIN
		EXEC ('CREATE PROCEDURE [solr_GetProducts] AS SELECT 1')
	END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ajit   
-- Create date:
-- Description:	
-- =============================================
Alter PROCEDURE solr_GetProducts 
@productIds varchar(2000) 
AS
BEGIN

  declare @sql varchar(3000)
  declare @cols varchar(3000)
  set @cols = 'ID,ProductProgramID,ArtikelNumber,Title,Description_short,Description,ProductCategoryID,ProductCategoryText,affiliProductCategoryID,affiliProductCategoryText,Price,Price_old,Currency_Symbol,ImageURL1,ImageURL2,ImageURL3,DeepLink1,DeepLink2,Keywords,Manufacturer,Brand,Distributor,ImgWidth1,ImgHeight1,ImgWidth2,ImgHeight2,ImgWidth3,ImgHeight3,update_date,valid_from,valid_to,EAN,ImageID,HashCode,PricePrefix,PriceSuffix,Shipping,ISBN,ShippingPrefix,ShippingSuffix,SearchText,Properties,PropertyHash,ImageOK,ImageName,BasePrice,BasePriceSuffix,BasePricePrefix,IsAffilimatchProduct';
  set @sql = 'select '+@cols+'  from product(nolock) where id in ('+ @productIds + ')';
  print @sql
  execute (@sql)

	
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
  