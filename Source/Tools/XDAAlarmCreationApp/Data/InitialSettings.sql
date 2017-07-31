-- initial setting categories
INSERT INTO Setting Values('LineCharacteristicsToDisplay', 'Line Characteristics') 
GO 

INSERT INTO Setting Values('ResultsToDisplay', 'Result Characteristics') 
GO 

INSERT INTO Setting Values('ColorGradients', 'Color Gradient Characteristics') 
GO

-- FileTypes categories
INSERT INTO FileType Values('NodeRFile', 'NodeRFile') 
GO 

INSERT INTO FileType Values('NodeZFile', 'NodeZFile') 
GO 

INSERT INTO FileType Values('NodeLDGFile', 'NodeLDGFile') 
GO

INSERT INTO FileType Values('LineFile', 'LineFile') 
GO

-- line characteristics found across the files

INSERT INTO node (ID, Name) VALUES ('13647E0F-C1E0-43D4-8FF2-54EDFBF09530', 'EPRIDriveConfigurationApp') 
GO

INSERT INTO ApplicationRole(Name, Description, NodeID) VALUES('Administrator', 'Admin Role', '13647E0F-C1E0-43D4-8FF2-54EDFBF09530') 
GO

INSERT INTO SecurityGroup(Name, Description) VALUES('S-1-5-32-545', 'All Windows authenticated users') 
GO

INSERT INTO ApplicationRoleSecurityGroup(ApplicationRoleID, SecurityGroupID) VALUES((SELECT ID FROM ApplicationRole), (SELECT ID FROM SecurityGroup)) 
GO

INSERT INTO ApplicationRole(Name, Description, NodeID) VALUES('Editor', 'Editor Role', '13647E0F-C1E0-43D4-8FF2-54EDFBF09530') 
GO

-- Initial Color Gradient
insert into ColorGradients Values ('> 1.75 MW',	'>1.75',	'030BFF',	1)
GO

insert into ColorGradients Values ('1.5-1.75 MW',	'1.5-1.75',	'54B5FF',	2)
GO

insert into ColorGradients Values ('1.25-1.5 MW',	'1.25-1.5',	'7AFFFB',	3)
GO

insert into ColorGradients Values ('1.0-1.25 MW',	'1.0-1.25',	'7FFF7A',	4)
GO

insert into ColorGradients Values ('0.75-1.0 MW',	'0.75-1.0',	'FFF708',	5)
GO

insert into ColorGradients Values ('0.5-0.75 MW',	'0.5-0.75',	'FF8B17',	6)
GO

insert into ColorGradients Values ('0.25-0.5 MW',	'0.25-0.5',	'FF3E17',	7)
GO

insert into ColorGradients Values ('< 0.25 MW',	'<0.25',	'B30511',	8)
GO