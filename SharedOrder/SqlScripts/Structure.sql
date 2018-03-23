CREATE TABLE IF NOT EXISTS `Settings` 
(
       `DataBaseVersion` varchar(20)
);


CREATE TABLE IF NOT EXISTS `Order` 
(
       `Id` INTEGER PRIMARY KEY AUTOINCREMENT,
       `Name`  varchar(512),
       `IsDeleted` bool  default 0
);

CREATE UNIQUE INDEX IF NOT EXISTS `IDX_Order$Name` ON `Order`(`Name`);
CREATE INDEX IF NOT EXISTS `IDX_Order$IsDeleted` ON `Order`(`IsDeleted`);


