--��ѯ���������������е���Ϣ
SELECT  b.title AS ��������,d.Title AS ���ݲ���,*
FROM    dbo.eap_VoucherTableColumn AS a
        JOIN dbo.eap_Voucher AS b ON a.VoucherID = b.ID
        JOIN dbo.eap_DTO AS c ON c.Name = b.DtoName
        JOIN dbo.eap_VoucherTable AS d ON d.id=a.TableID
WHERE   c.TableName = 'ST_RDRecord' AND a.title LIKE '%ҵ������%'
ORDER BY a.VoucherID,a.TableID,b.Title,d.Title
--��ѯ����UI��ز�����Ϣ
SELECT FieldName,OriginalTitle,a.Title,a.* FROM dbo.eap_VoucherControls AS a
JOIN dbo.eap_Voucher AS b ON b.ID=a.VoucherID
JOIN dbo.eap_DTO AS c ON c.Name=b.DtoName
WHERE c.TableName='ST_RDRecord' AND a.title LIKE '%ҵ������%'
ORDER BY a.TabIndex

SELECT * FROM dbo.eap_Voucher WHERE DtoName='RDRecordDTO'
SELECT * FROM dbo.eap_DTO WHERE TableName='ST_RDRecord'
SELECT * FROM dbo.eap_DTOProperty WHERE idDTO='90D88CAA-D328-4817-B5FF-612B6E8EFFFF' AND FieldName LIKE '%idbu%'
SELECT * FROM dbo.eap_DTORelation WHERE idProperty='8619CBEE-38C1-42EB-9F66-2CEE68B3E22E'

SELECT * FROM dbo.eap_VoucherControls WHERE title LIKE '%ҵ������%' AND VoucherID='F47B4F5D-3916-48EF-AB08-879773C6426F'

SELECT * FROM dbo.eap_Enum WHERE id='972BFAC1-8DB8-4F54-A0F6-CA5878FAB250'
SELECT * FROM dbo.eap_EnumItem WHERE Name LIKE '%�ɹ�%'
SELECT * FROM enum