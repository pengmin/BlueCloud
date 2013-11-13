DELETE dbo.EAP_BusOperation WHERE id='79569722-D723-4270-84E5-F6266F0D667E'
INSERT INTO dbo.EAP_BusOperation
        ( id ,
          OperationName ,
          FunctionCode ,
          Description ,
          Code ,
          ViewType ,
          TabIndex ,
          IsSplit ,
          SourceKey ,
          IsAuthControl ,
          Title ,
          Visibility ,
          ParentID ,
          ShortCut ,
          ToolTip ,
          idfunctionauth ,
          issystem ,
          IsHasAction ,
          IsEnd
        )
VALUES  ( '79569722-D723-4270-84E5-F6266F0D667E' , -- id - uniqueidentifier
          'TestAction' , -- OperationName - nvarchar(50)
          '' , -- FunctionCode - varchar(20)
          '' , -- Description - nvarchar(100)
          'SA03' , -- Code - nvarchar(50)
          2 , -- ViewType - smallint
          9998 , -- TabIndex - int
          0 , -- IsSplit - bit
          NULL , -- SourceKey - nvarchar(30)
          0 , -- IsAuthControl - bit
          '测试按钮' , -- Title - nvarchar(50)
          1 , -- Visibility - bit
          NULL , -- ParentID - uniqueidentifier
          NULL , -- ShortCut - char(1)
          '测试按钮' , -- ToolTip - nvarchar(50)
          NULL , -- idfunctionauth - uniqueidentifier
          0 , -- issystem - bit
          NULL , -- IsHasAction - bit
          1 
        )