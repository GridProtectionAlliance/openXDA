---------------- PQDigest TableSpace -------------
CREATE TABLE [PQDigest.Setting](
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.Url', 'http://localhost:8989', '')
GO
INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.APIKey', '', '')
GO
INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.APIToken', '', '')
GO