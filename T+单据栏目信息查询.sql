--��ѯ���������������е���Ϣ
SELECT  *
FROM    dbo.eap_VoucherTableColumn AS a
        JOIN dbo.eap_Voucher AS b ON a.VoucherID = b.ID
        JOIN dbo.eap_DTO AS c ON c.Name = b.DtoName
WHERE   c.TableName = 'PU_PurchaseOrder' AND a.title LIKE '%%'
--��ѯ����UI��ز�����Ϣ
SELECT FieldName,OriginalTitle,a.Title,a.* FROM dbo.eap_VoucherControls AS a
JOIN dbo.eap_Voucher AS b ON b.ID=a.VoucherID
JOIN dbo.eap_DTO AS c ON c.Name=b.DtoName
WHERE c.TableName='PU_PurchaseOrder' AND a.title LIKE '%%'
ORDER BY a.TabIndex
