--��ѯ���������������е���Ϣ
SELECT  b.title AS ��������,d.Title AS ���ݲ���,*
FROM    dbo.eap_VoucherTableColumn AS a
        JOIN dbo.eap_Voucher AS b ON a.VoucherID = b.ID
        JOIN dbo.eap_DTO AS c ON c.Name = b.DtoName
        JOIN dbo.eap_VoucherTable AS d ON d.id=a.TableID
WHERE   c.TableName = 'SA_SaleOrder' AND a.title LIKE '%˰��%'
ORDER BY a.VoucherID,a.TableID,b.Title,d.Title
--��ѯ����UI��ز�����Ϣ
SELECT FieldName,OriginalTitle,a.Title,a.* FROM dbo.eap_VoucherControls AS a
JOIN dbo.eap_Voucher AS b ON b.ID=a.VoucherID
JOIN dbo.eap_DTO AS c ON c.Name=b.DtoName
WHERE c.TableName='SA_SaleOrder' AND a.title LIKE '%ҵ������%'
ORDER BY a.TabIndex
-------------------------------------------------------------
--���²�ѯdto֮��Ĺ�ϵ
SELECT * FROM dbo.eap_DTO WHERE TableName='PU_PurchaseOrder'
SELECT * FROM dbo.eap_DTOProperty WHERE idDTO='DA8CB4FD-CA9A-427F-99AF-4FC636CD087C'
SELECT * FROM dbo.eap_DTORelation
SELECT * FROM dbo.EAP_UserDefineArticleDTO_0001
SELECT * FROM dbo.EAP_UserDefineArticleDTO_0002
SELECT * FROM dbo.EAP_UserDefineArticleDTO_0003
--dto���õ�����dto�ֶ�
SELECT a.id,b.Title,c.Title,d.FieldName,e.FieldName,f.FieldName,a.* 
FROM dbo.eap_DTORelation AS a
JOIN dbo.eap_DTO AS b ON a.idFromDTO=b.ID
JOIN dbo.eap_DTO AS c ON a.idToDTO=c.ID
JOIN dbo.eap_DTOProperty AS d ON a.idProperty=d.ID
JOIN dbo.eap_DTOProperty AS e ON a.idFromProperty=e.ID
JOIN dbo.eap_DTOProperty AS f ON a.idToProperty=f.ID
WHERE idFromDTO='DA8CB4FD-CA9A-427F-99AF-4FC636CD087C'
--������dto�ֶ����õ�dto
SELECT a.id,b.Title,c.Title,d.FieldName,e.FieldName,f.FieldName,a.* 
FROM dbo.eap_DTORelation AS a
JOIN dbo.eap_DTO AS b ON a.idFromDTO=b.ID
JOIN dbo.eap_DTO AS c ON a.idToDTO=c.ID
JOIN dbo.eap_DTOProperty AS d ON a.idProperty=d.ID
JOIN dbo.eap_DTOProperty AS e ON a.idFromProperty=e.ID
JOIN dbo.eap_DTOProperty AS f ON a.idToProperty=f.ID
WHERE a.idToDTO='DA8CB4FD-CA9A-427F-99AF-4FC636CD087C'