/*
drop table SubscriptionMessageLog;
drop table MessageParameter;
drop table MessageContent;
drop table Message;

drop table PartnerSubscriptionParameter;
drop table PartnerSubscription;
drop table MessageProtocol;
drop table Event;
drop table Partner;
*/


create table Message
(
Id	varchar(32) primary key,
CreatedOn DateTime default GETDATE(),
IsDeleted bit,
);
alter table Message add Picked int not null default(0);

create table MessageContent
(
MessageId	varchar(32) primary key references Message(Id),
Content	nvarchar(max),
CreatedOn DateTime default GETDATE()
);

create table MessageParameter
(
Id	varchar(32) primary key,
MessageId varchar(32) references Message(Id),
[Key] varchar(250) not null,
[Value]	varchar(500),
IsDeleted bit
);

create table Partner
(
Id	varchar(32) primary key,
Name	nvarchar(250),
Email	nvarchar(100),
CreatedOn DateTime default GETDATE(),
IsDeleted bit
);

create table Event
(
Id	varchar(32) primary key,
Title	nvarchar(250),
Code	int,
CreatedOn DateTime default GETDATE(),
IsDeleted bit
);

create table MessageProtocol
(
Id	varchar(32) primary key,
Name nvarchar(50),
Type	nvarchar(250),
CreatedOn DateTime default GETDATE(),
IsDeleted bit
);

create table PartnerSubscription
(
Id	varchar(32) primary key,
PartnerId varchar(32) references Partner(Id),
EventId varchar(32) references Event(Id),
MessageProtocolId varchar(32) references MessageProtocol(Id),
);

create table PartnerSubscriptionParameter
(
Id	varchar(32) primary key,
PartnerSubscriptionId varchar(32) references PartnerSubscription(Id),
[Key] varchar(250) not null,
[Value]	varchar(500)
);


create table SubscriptionMessageLog
(
Id	varchar(32) primary key,
PartnerSubscriptionId varchar(32) references PartnerSubscription(Id),
Picked int not null default(0),
MessageId varchar(32) references Message(Id)
);

insert into Event(Id, Title, Code, CreatedOn, IsDeleted) values(replace(NEWID(),'-',''), 'Event Title 1','1',SYSDATETIME(),0);

insert into MessageProtocol(Id,[Name],Type,CreatedOn,IsDeleted) values(replace(NEWID(),'-',''), 'Email','Vontobel.Middleware.IBT.MessageSinks.Email.PartnerEmailSink',SYSDATETIME(),0);
insert into MessageProtocol(Id,[Name],Type,CreatedOn,IsDeleted) values(replace(NEWID(),'-',''), 'XmlFile','Vontobel.Middleware.IBT.MessageSinks.File.PartnerXmlFileSink',SYSDATETIME(),0);

insert into Partner(Id,Name,Email,CreatedOn,IsDeleted) values(replace(NEWID(),'-',''),'Partner A','talha.liaquat@gmail.com',SYSDATETIME(),0);
insert into Partner(Id,Name,Email,CreatedOn,IsDeleted) values(replace(NEWID(),'-',''),'Partner B','talha.liaquat@gmail.com',SYSDATETIME(),0);

insert into PartnerSubscription(Id, PartnerId, EventId, MessageProtocolId) values(
																replace(NEWID(),'-',''),
																(select Id from Partner where Name = 'Partner A'),
																(select Id from Event where Code = '1'),
																(select Id from MessageProtocol where Name = 'Email'));

insert into PartnerSubscription(Id, PartnerId, EventId, MessageProtocolId) values(
																replace(NEWID(),'-',''),
																(select Id from Partner where Name = 'Partner B'),
																(select Id from Event where Code = '1'),
																(select Id from MessageProtocol where Name = 'XmlFile'));

insert into PartnerSubscriptionParameter(Id, PartnerSubscriptionId, [Key], [Value]) values(
													replace(NEWID(),'-',''),
													(select Id from PartnerSubscription where PartnerId = (select Id from Partner where Name = 'Partner B') and MessageProtocolId = (select Id from MessageProtocol where Name = 'XmlFile')),
													'FilePath',
													'C:\Vontobel_Data\PartnerB'
													);