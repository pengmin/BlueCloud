--��ѯ���������������е���Ϣ
SELECT  b.title AS ��������,d.Title AS ���ݲ���,*
FROM    dbo.eap_VoucherTableColumn AS a
        JOIN dbo.eap_Voucher AS b ON a.VoucherID = b.ID
        JOIN dbo.eap_DTO AS c ON c.Name = b.DtoName
        JOIN dbo.eap_VoucherTable AS d ON d.id=a.TableID
WHERE   c.TableName = 'SA_SaleQuotation' AND a.title LIKE '%����%'
ORDER BY a.VoucherID,a.TableID,b.Title,d.Title
--��ѯ����UI��ز�����Ϣ
SELECT FieldName,OriginalTitle,a.Title,a.* FROM dbo.eap_VoucherControls AS a
JOIN dbo.eap_Voucher AS b ON b.ID=a.VoucherID
JOIN dbo.eap_DTO AS c ON c.Name=b.DtoName
WHERE c.TableName='ST_RDRecord' AND a.title LIKE '%%'
ORDER BY a.TabIndex

SELECT * FROM dbo.eap_VoucherTable WHERE id='CF54447F-4A98-4044-83D3-B412E8549F80'