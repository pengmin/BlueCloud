--查询单据有所表所有列的信息
SELECT  *
FROM    dbo.eap_VoucherTableColumn AS a
        JOIN dbo.eap_Voucher AS b ON a.VoucherID = b.ID
        JOIN dbo.eap_DTO AS c ON c.Name = b.DtoName
WHERE   c.TableName = 'PU_PurchaseOrder' AND a.title LIKE '%%'
--查询单据UI相关部件信息
SELECT FieldName,OriginalTitle,a.Title,a.* FROM dbo.eap_VoucherControls AS a
JOIN dbo.eap_Voucher AS b ON b.ID=a.VoucherID
JOIN dbo.eap_DTO AS c ON c.Name=b.DtoName
WHERE c.TableName='PU_PurchaseOrder' AND a.title LIKE '%%'
ORDER BY a.TabIndex
