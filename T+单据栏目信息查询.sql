--查询单据有所表所有列的信息
SELECT  b.title AS 单据名称,d.Title AS 单据部分,*
FROM    dbo.eap_VoucherTableColumn AS a
        JOIN dbo.eap_Voucher AS b ON a.VoucherID = b.ID
        JOIN dbo.eap_DTO AS c ON c.Name = b.DtoName
        JOIN dbo.eap_VoucherTable AS d ON d.id=a.TableID
WHERE   c.TableName = 'ST_RDRecord' AND a.title LIKE '%税率%'
ORDER BY a.VoucherID,a.TableID,b.Title,d.Title
--查询单据UI相关部件信息
SELECT FieldName,OriginalTitle,a.Title,a.* FROM dbo.eap_VoucherControls AS a
JOIN dbo.eap_Voucher AS b ON b.ID=a.VoucherID
JOIN dbo.eap_DTO AS c ON c.Name=b.DtoName
WHERE c.TableName='SA_SaleOrder' AND a.title LIKE '%业务类型%'
ORDER BY a.TabIndex
-------------------------------------------------------------
--以下查询dto之间的关系
SELECT * FROM dbo.eap_DTO WHERE TableName='ST_RDRecord'
SELECT * FROM dbo.eap_DTOProperty WHERE idDTO='90D88CAA-D328-4817-B5FF-612B6E8EFFFF' and title LIKE '%业%'
SELECT * FROM dbo.eap_DTORelation
SELECT * FROM dbo.EAP_UserDefineArticleDTO_0001
SELECT * FROM dbo.EAP_UserDefineArticleDTO_0002
SELECT * FROM dbo.EAP_UserDefineArticleDTO_0003
--dto引用的其他dto字段
SELECT a.id,b.Title,c.Title,e.FieldName,c.title,c.TableName,f.FieldName,a.* 
FROM dbo.eap_DTORelation AS a
JOIN dbo.eap_DTO AS b ON a.idFromDTO=b.ID
JOIN dbo.eap_DTO AS c ON a.idToDTO=c.ID
JOIN dbo.eap_DTOProperty AS d ON a.idProperty=d.ID
JOIN dbo.eap_DTOProperty AS e ON a.idFromProperty=e.ID
JOIN dbo.eap_DTOProperty AS f ON a.idToProperty=f.ID
WHERE idFromDTO='DA8CB4FD-CA9A-427F-99AF-4FC636CD087C'
--被其他dto字段引用的dto
SELECT a.id,b.Title,c.Title,d.FieldName,e.FieldName,f.FieldName,a.* 
FROM dbo.eap_DTORelation AS a
JOIN dbo.eap_DTO AS b ON a.idFromDTO=b.ID
JOIN dbo.eap_DTO AS c ON a.idToDTO=c.ID
JOIN dbo.eap_DTOProperty AS d ON a.idProperty=d.ID
JOIN dbo.eap_DTOProperty AS e ON a.idFromProperty=e.ID
JOIN dbo.eap_DTOProperty AS f ON a.idToProperty=f.ID
WHERE a.idToDTO='DA8CB4FD-CA9A-427F-99AF-4FC636CD087C'
--获取单据表信息的sql
SELECT * FROM dbo.eap_Voucher WHERE DtoName='PurchaseOrderDTO'
--以下是单据设计页面的左边菜单树信息
SELECT * FROM dbo.eap_VoucherModule
SELECT * FROM dbo.eap_VoucherDesignTree
--以下获取单据非表头、表尾部分的信息
SELECT * FROM dbo.eap_VoucherTable WHERE VoucherID='F47B4F5D-3916-48EF-AB08-879773C6426F'
SELECT * FROM dbo.eap_VoucherTableColumn WHERE TableID='936B6DCA-F85E-4550-924E-CD41AB299029' ORDER BY ColIndex
SELECT * FROM dbo.eap_VoucherTab WHERE VoucherID='D96BC991-75C8-4ABB-BB93-6BEA1C4C058C'
--以下获取单据表头表尾部分信息
SELECT * FROM dbo.eap_VoucherControls WHERE VoucherID='F47B4F5D-3916-48EF-AB08-879773C6426F' AND TabPageName='TabHead1' ORDER BY TabIndex