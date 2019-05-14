
use master 
create database storedb
use storedb
---------��������---------
drop database storedb
------------1------------
-----------�� ���������-------------
create table Unit
(
UnitID int not null identity primary key,
UnitName varchar(5) not null
)
----------������-----------------
create table Access
(
AccessID int not null identity primary key,
AccessName varchar(10) not null
)
----------�����-----------
--exec sp_rename 'Firms.CustPhoneNumber', 'FirmPhoneNumber', 'COLUMN'
 
create table Firms
(
FirmID int not null identity primary key,
FirmName varchar(300) not null, 
UNP varchar(200) not null,
FirmLegalAddress varchar(300) not null, 
FirmAccountNumber varchar(200) not null,
FirmBankDetails varchar(300) not null,
FirmDiscountAmount float not null,
FirmPhoneNumber varchar(30) null
)
/*------------�� ���������--------------
----------�������----------- CTRL + K, CTRL + U (����� ��������)
--create table Customer
--(
--CustomerID int not null identity primary key,
--CustLastName varchar(20) not null, 
--CustFirstName varchar(20) not null, 
--CustPatronymic varchar(20) not null, 
--Sex varchar(2) not null ,
--CustDateOfBirth  Date not null, 
--CustAddress varchar(200) not null, 
--CustPhoneNumber varchar(20) not null, 
--CustDiscountAmount tinyint not null
)*/
--------------------------------------
----------������������-----------
create table Users
(
UsersID int not null identity primary key,
AccessID int foreign key references Access(AccessID),
[Login] varchar(50) not null,
[Password] varchar(50) not null,
);
--------------��������--------------
create table Employee 
(EmployeeID int not null identity primary key, 
UsersID int null foreign key references Users(UsersID),
EmpLastName varchar(20) not null, 
EmpFirstName varchar(20) not null, 
EmpPatronymic varchar(20) not null, 
Sex varchar(2) not null ,
EmpDateOfBirth Date not null, 
EmpAddress varchar(200) not null,
EmpPhoneNumber varchar(20) not null,
Position varchar(20) not null,
Experience tinyint not null,
--constrain CheckTeacher check (Sex = '�' OR Sex= '�') 
);

----------��������� ������-------
create table Category
(
CategoryID  int not null identity primary key,
NameCategory varchar(20) not null,
)
----------�����-----------
create table Storage
(
StorageID  int not null identity primary key,
CategoryID int null foreign key references Category(CategoryID),
UnitID int null foreign key references Unit(UnitID),
[Name] varchar(100) not null,
[Count] tinyint not null,
[Description] varchar(350) not null,
Price float not null,
)

----------�������-----------
create table Store
(
StoreID int not null identity primary key,
EmployeeID int null foreign key references Employee(EmployeeID),
FirmID int null foreign key references Firms(FirmID),
StorageID int null foreign key references Storage(StorageID),
[Count] tinyint not null,
TotalPrice float not null,
CurrentDiscountAmount float not null,
PurchaseDay Date not null,
Paid bit not null,
)

-------------------------���������
--SYSDATETIME()
--------------���������� �������
--drop proc InputStore
go
create procedure InputStore 
@EmployeeID int,
@FirmID int,
@StorageID int,
@Count int,
@PurchaseDay Date
AS
begin
declare @Price float
declare @TotalPrice float
declare @CurrentDiscountAmount float 
set @CurrentDiscountAmount = (select FirmDiscountAmount from Firms where FirmID=@FirmID)
set @Price = (select Price from Storage where StorageID=@StorageID)
set @TotalPrice = ((@Price*@Count)-((@Price*@Count)*@CurrentDiscountAmount/100))

INSERT INTO Store(EmployeeID, FirmID, StorageID, [Count], TotalPrice, PurchaseDay, CurrentDiscountAmount,Paid)
                    VALUES (@EmployeeID, @FirmID, @StorageID, @Count, @TotalPrice, @PurchaseDay, @CurrentDiscountAmount,0)					
end
go
--exec InputStore @EmployeeID=3, @FirmID=3, @StorageID=3, @Count=3, @PurchaseDay='13.01.2019'
-------------------------------------------���������� �������
--drop proc InsertFirm
go
create procedure InsertFirm 
@FirmName varchar(200), 
@UNP varchar(200), 
@FirmAccountNumber varchar(200), 
@FirmBankDetails varchar(200) ,
@FirmLegalAddress varchar(200),
@FirmPhoneNumber varchar(50)
AS
begin
INSERT INTO Firms(FirmName,UNP, FirmAccountNumber, FirmBankDetails, FirmLegalAddress, FirmPhoneNumber, FirmDiscountAmount)
        VALUES (@FirmName, @UNP, @FirmAccountNumber, @FirmBankDetails, @FirmLegalAddress, @FirmPhoneNumber, 0)					
end
go
--exec InsertFirm @StorageID=3, @Count=3
-------------------------------------------���������� ������������ ������
--drop proc InsertStoragesName
go
create procedure InsertStoragesName
@CategoryID int, 
@UnitID int, 
@Name varchar(100), 
@Description varchar(350),
@Price float
AS
begin
INSERT INTO Storage(CategoryID, UnitID, [Name], [Count], [Description],Price)
        VALUES (@CategoryID,@UnitID,@Name,0,@Description,@Price)					
end
go
--exec InsertStoragesName @CategoryID, @UnitID, @Name, @Description, @Price
-------------------------------------------�������� ������ (��������)
--drop proc Shipment
go
create procedure Shipment 
@ID int,
@Date Date
AS
begin
update Store 
set Paid=1
where FirmId=@ID and Store.PurchaseDay=@Date
end
go
--exec Shipment @ID=2
--update Store 
--set Paid=0
----------------- ------------------------
-------------------------------------------�������� ������ (��������)
--drop proc AddGoods
go
create procedure AddGoods 
@ID int,
@Count int
AS
begin
update Storage 
set [Count]=[Count]+@Count
where StorageID=@ID
end
go
----------------- ------------------------
insert Access(AccessName) 
values
('High'),
('Middle');
----------------------------------------
insert Users([Login], [Password],AccessID) 
values
('Admin',   '1',1),
('T',       '1',2),
('T3',      '1',2),
('T4',      '1',2),
('T5',      '1',2),
('T6',      '1',2),
('T7',      '1',2),
('T8',      '1',2),
('T9',      '1',2),
('T10',     '1',2),
('T11',     '1',2),
('T12',     '1',2),
('T13',     '1',2),
('T14',     '1',2),
('T15',     '1',2),
('T16',     '1',2),
('T1000',   '1',2);
--------------------------------------------------
insert Employee(UsersID,EmpLastName, EmpFirstName, EmpPatronymic, Sex, EmpDateOfBirth, EmpAddress, EmpPhoneNumber, Position, Experience) 
values 
('1','��������','Admin','����������',   '�', '1982-08-27','�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('2','�������','���',  '�����������',  '�', '1982-10-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('3','�������','������', '�����������',  '�', '1982-02-27','�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('4','�������','����',   '�����������', '�', '1982-01-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('5','�������','�����',   '�����������', '�', '1982-03-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('6','�������','��������', '�����������',   '�', '1982-04-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('7','�������','��������',  '�����������',  '�', '1982-05-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('8','�������','����', '�����������',   '�', '1982-06-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('9','�������','����',  '�����������',  '�', '1982-07-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('10','�������','����',  '�����������',  '�', '1982-01-18' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('11','�������','��������',  '�����������',  '�', '1982-01-19' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('12','�������','����',   '�����������', '�', '1982-01-20' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('13','�������','�������','�����������',    '�', '1982-01-21' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('14','�������','�����','�����������','�', '1982-01-22' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('15','�������','����','�����������','�', '1982-01-23' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('16','�������','�������','�����������',    '�', '1982-01-24' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('17','�������','����', '�����������',   '�', '1982-01-25' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3);
insert Firms(FirmName,UNP, FirmAccountNumber, FirmBankDetails, FirmLegalAddress, FirmDiscountAmount) 
values 
('��������������� ���������� ����������� "��������� ������������"', '190583856', 'BY40BPSB30121642290179330000', '��� � 704 ��� "���-��������", BIC: BPSBBY2X ��� � 704 ��� "���-��������"', '�. �����, 1-�� ���. ����������, �. 50, �. 4',0)	,
('������������ ��������� �������� � ������ ���������� ����� ���������� ��������', '190583857', 'BY40BPSB30121642290179331111', '��� � 704 ��� "���-��������", BIC: BPSBBY2X ��� � 705 ��� "���-��������"', '�. �����, 1-�� ���. ����������, �. 50, �. 5',0)	; --,
--('', '', '', '', '',0)	;
------------------------
insert Unit (UnitName)
values
('�'),
('��'),
('��'),
('�'),
('��'), --5
('�'),
('��'),
('��');
-------------------
insert Category (NameCategory)
values 
('���������� ���������'),
('����������������'),
('���������� ���������'),
('�������� ���������'),
('���������'),
('������ � ������'),
('������ ���������'),
('��� � ������');
-----------------------------------
insert Storage (CategoryID, UnitID, [Name], [Count], [Description], Price)
values
(1, 5,'����������� � �������������',100,'����������� �������� (12.5 ��) ����������� Knauf 60 x 150 ��',4.20),
(1, 5,'����������� � �������������',100,'���������� ����������� �������� (12.5 ��) ����������� 60 x 120 ��',3.09),
(1, 5,'����������� � �������������',100,'����������� �������� (12.5 ��) ����������� ������� 120 x 250 ��',9.49),
(1, 5,'����������� � �������������',100,'����������� �������� (12.5 ��) ������������ Knauf 60 x 150 ��',4.90),
(1, 5,'����������� � �������������',100,'����������� �������� (12.5 ��) ����������� ������� 120 x 300 ��',11.14),
(1, 5,'����� ������������',100,'������ (��������������) �500 �20, 25 ��',5.49),
(1, 5,'����� ������������',100,'������ (��������������) �500 �0, 25 ��',6.50),
(1, 5,'����� ������������',100,'��������� �� ���������� ��������� Weber Vetonit LR+, 20 ��',15.20),
(1, 5,'����� ������������',100,'��������� �������� Sniezka Acryl Putz ST10 (����� + �����), 20 ��',15.98),
(1, 5,'����� ������������',100,'��������� �������� Knauf Fugen, 10 ��',7.60),
(1, 5,'������ � �����',100,'������ ��������� Sniezka Ultra Biel �����, 1 �',4.12),
(1, 5,'������ � �����',100,'������ ��������� Sniezka Ultra Biel �����, 3 �',11.33),
(1, 5,'������ � �����',100,'������ ��������� Sniezka Ultra Biel �����, 5 �',17.30),
(1, 5,'������ � �����',100,'������ ��������� Sniezka Ultra Biel �����, 10 �',29.32),
(1, 5,'������ � �����',100,'������ ��������� ������ ������ ���� (��) �����, 5 �',16.85),
(1, 5,'������ � ���������',100,'��������� ������ ���������� Ceresit CT17 Profigrunt, 2 �',7.39),
(1, 5,'������ � ���������',100,'��������� ������ ���������� Ceresit CT17 Profigrunt, 5 �',17.68),
(1, 5,'������ � ���������',100,'��������� ������ ���������� Ceresit CT17 Profigrunt, 10 �',28.80),
(1, 5,'������ � ���������',100,'��������� ���������� ���������� Ceresit CT17 Supergrunt, 2 �',7.75),
(1, 5,'������ � ���������',100,'��������� ���������� ���������� Ceresit CT17 Supergrunt, 5 �',18.38),
(1, 5,'�����, �����������',100,'����� ������� ��-1 (3.0 ��, 100 x 100 ��) 1 x 2 �',3.95),
(1, 5,'�����, �����������',100,'����������� �������� "��������" Welton-������, 50 �2',25.79),
(1, 5,'�����, �����������',100,'����������� �������� "��������" Nortex Deco, 50 �2',24.80),
(1, 5,'�����, �����������',100,'����������� �������� "��������" Nortex Ultra, 50 �2',27.96),
(1, 5,'�����, �����������',100,'����� �������������� ����������� ����-160, 10 �2',12.87),
(1, 5,'������ � �����',100,'������ ��������������� �����������, 2.5 �',0.98),
(1, 5,'������ � �����',100,'������ ��������������� �����������, 3 �',1.19),
(1, 5,'������ � �����',100,'������ ��������������� �������� ���������, 2.5 �',1.12),
(1, 5,'������ � �����',100,'������ ��������������� �������� ���������, 3 �',1.52),
(1, 5,'������ � �����',100,'������ ����������� �� �������������� ������, 2.5 �',1.87),
(1, 5,'�������� ��� ������',100,'�������� ��� ������ (����������) Goldbastik BB 20, 10 � (�� 40 �.��.), ��',18.88),
(1, 5,'�������� ��� ������',100,'�������� ��� ������ (����������) Goldbastik BB 20, 5 � (�� 20 �.��.), ��',10.94),
(1, 5,'�������� ��� ������',100,'���������� "����������" Goldbastik BB26, ��������� ������ ���������, 5 �',9.91),
(1, 5,'�������� ��� ������',100,'���������� "����������" Goldbastik BB26, ��������� ������ ���������, 10 �',18.03),
(1, 5,'�������� ��� ������',100,'�������� ��� ������ (����������) Vidaron ���������� 1:9 ����������, 5 �� (80 �.��.), ��',69.00),
(1, 5,'����, ���������, ����',100,'���������� ���� �������, 500 ��',5.38),
(1, 5,'����, ���������, ����',100,'���������� ��������� ��������� ���� PENOSIL Cured Foam Remover 340 ��',9.65),
(1, 5,'����, ���������, ����',100,'���������� ��� ��������� ���� Penosil Foam Cleaner, 460 ��',9.65),
(1, 5,'����, ���������, ����',100,'���� ��� ����������� ������� Bostik 70, 5 �',24.67),
(1, 5,'����, ���������, ����',100,'���� ��� ����������� ������� Bostik 70, 15 �',53.31),
(2, 5,'����� � �������',100,'���� �������������� (D500) 625 x 250 x 100 �� ����',1.69),
(2, 5,'����� � �������',100,'���� �������������� (D500) 625 x 250 x 120 �� ����',2.09),
(2, 5,'����� � �������',100,'���� �������������� (D500) 625 x 250 x 125 �� ��������',2.38),
(2, 5,'����� � �������',100,'���� �������������� (D500) 625 x 250 x 200 �� ��������',3.60),
(2, 5,'����� � �������',100,'������ (��������������) �500 �20, 25 ��',5.49),
(3, 5,'�����',100,'����� ����������������� 8-�������� ����� (5.2 ��) 113 x 175 ��, ��',7.20),
(3, 5,'�����',100,'����� ����������������� 8-�������� ����� (5.8 ��) 113 x 175 ��, ��',8.20),
(3, 5,'�����',100,'����� ����������������� 8-�������� ����� (5.2 ��) 113 x 175 ��, ��',7.80),
(3, 5,'�����',100,'����� ����������������� 8-�������� ����� (5.8 ��) 113 x 175 ��, ��',8.40),
(3, 5,'�����',100,'����� ����������������� ������� ����� (6 ��) 175 x 110 ��, ��',9.95),
(4, 5,'������� ������',100,'���������� ''����'' ������ ������ �22� ��� �������, 25 ��',12.90),
(4, 5,'������� ������',100,'���������� ''�����������'' Ceresit CT36 ��� �������, 25 ��',12.89),
(4, 5,'������� ������',100,'������ �������� ��������� Condor Fassandenfarde-Object, 15 ��',59.95),
(4, 5,'������� ������',100,'���������� ''������'' (2.0 ��) ������ ������ �23.2 ��� �������, 25 ��',10.73),
(4, 5,'������� ������',100,'���������� ''������'' (3.0 ��) ������ ������ �23.3 ��� �������, 25 ��',10.39),
(5, 5,'������ � �������',100,'������ ��� �-�� 2-� ������� ������, ������� 1,5�� (0,66 ��)',1.06),
(5, 5,'������ � �������',100,'������ ��� �-�� 2-� ������� ������, ������� 2,5�� (0,66 ��)',1.36),
(5, 5,'������ � �������',100,'������ ��� �-�� (�) 3-� ������� ������, ������� 2,5 �� (0,66 ��)',1.88),
(5, 5,'������ � �������',100,'������ ��� 3�0,75�� ������ (0,66 ��) ����������, �����',0.65),
(5, 5,'������ � �������',100,'������ ���-� 2�0,75 �����, ����������-�����',0.59),
(6, 8,'�������� � ������',100,'������� 3.5*25 �� ��� ������� ��� � �������, ������ (100 �� � ���-����) STARFIX (SMZ2-96507-100)',3.15),
(6, 8,'�������� � ������',100,'������� 3.5*35 �� ��� ������� ��� � �������, ������ (50 �� � ���-����) STARFIX (SMZ2-96517-50)',2.38),
(6, 8,'�������� � ������',100,'������� 3.5*35 �� ��� ������� ��� � �������, ������ (1000 �� � ����. ��.) STARFIX (SMC3-96517-1000)',16.07),
(6, 8,'�������� � ������',100,'������� 3.5*45 �� ��� ������� ��� � �������, ������ (500 �� � ����. ��.) STARFIX (SMC3-96527-500)',9.72),
(6, 8,'�������� � ������',100,'������� 3.5*55 �� ��� ������� ��� � �������, ������ (500 �� � ����. ��.) STARFIX (SMC3-96537-500)',11.78),
(7, 5,'������ ��� ����',100,'������ ������������ ������� ���������� ��������� ���-200 �������',0.55),
(7, 5,'������ ��� ����',100,'������ ������� ���������� ��������� ��� �200 (�������, ��� �1)',0.60),
(7, 5,'������ ��� ����',100,'������ ������� ���������� ��������� ��� �200 � ������������ ����� (�������, ��� �1)',0.95),
(7, 5,'������� ��� ����',100,'��������-����������� ����������� ����� ������ ������ ������, 25 ��',17.52),
(7, 5,'������� ��� ����',100,'������� �������� ������������, 15��',7.50),
(8, 5,'������� ���������',100,'����� ������� ECO WB6203-1 (65�, 150��, 1 ������������ 3.25-8) (WB6203-1)',53.84),
(8, 5,'������� ���������',100,'����� ������� DGM GT-1081 (80�, 150��, 1 ������������ 3.25-8) (GT-1081)',53.47),
(8, 5,'������� ���������',100,'����� �����������-������� ����-1� (100�, 120 ��, 1 ������������ 400*90��, ��� 15 ��) (���) (����-1�)',85.25),
(8, 5,'������� ���������',100,'�������� �17 ����� (2.1x10�) (4810751573830)',5.41),
(8, 5,'������� ���������',100,'�������� �30 ����� (����� 1,6*300�, 960�.��.) (1108568252004)',272.89);
insert Store (EmployeeID,FirmID,StorageID,[Count],TotalPrice,PurchaseDay, CurrentDiscountAmount,Paid)
values
(2,1,3,1,9.49,'12-1-2019',0,0),
(3,2,3,1,9.49,'12-1-2019',0,1),
(4,1,3,1,9.49,'12-1-2019',0,0),
(2,2,3,1,9.49,'12-1-2019',0,1),
(3,1,3,1,9.49,'12-1-2019',0,0);

---------------------------�������
---------������� �� ������� TotalPrice (Store)
---------������� �� �������� ��� �������, ��� �� �� ������ ���� ���������� ����������


----------������ �� ��������� ������ ���������� ����� ���������� �������
go
create trigger insertStore
on Store
after insert
as
begin
---���������� ������ �� ������--------------------------------
declare @StorageID tinyint
declare @Count tinyint
declare @AvRating float
set @StorageID = (select inserted.StorageID from  inserted )
set @Count = (select inserted.[Count] from  inserted )
update Storage
set 
[Count]=[Count]-@Count
where StorageID=@StorageID
---------------------------------------------------------------
declare @TotalTotalPrice float
declare @TotalPrice float
declare @FirmID int
set @FirmID = (select inserted.FirmID from  inserted )
set @TotalTotalPrice = 0
set @TotalPrice = 0
declare cursorStore cursor  
--set @cursorStore = CURSOR scroll
for select TotalPrice from store where FirmID = @FirmID

open cursorStore
fetch next from cursorStore into @TotalPrice
while @@FETCH_STATUS=0
begin 
set @TotalTotalPrice = @TotalTotalPrice+@TotalPrice
fetch next from cursorStore into @TotalPrice
end
close cursorStore

if (@TotalTotalPrice <1000) 
begin 
update Firms
set FirmDiscountAmount = 1
where FirmID = @FirmID 
end 
if (@TotalTotalPrice <2000) 
begin 
update Firms
set FirmDiscountAmount = 2
where FirmID = @FirmID 
end 
if (@TotalTotalPrice <3000) 
begin 
update Firms
set FirmDiscountAmount = 3
where FirmID = @FirmID 
end 
--set @AvRating = (select Avg(Rating) from Logs where (Logs.StudentID = @ID))
--update Student set AverRating =  @AvRating where Student.StudentID=@ID
end
go

drop trigger insertStore


--select Storage.[Description], UnitName, Store.[Count], Store.TotalPrice, Storage.Price from Store
--join Storage on (Store.StorageID= Storage.StorageID)
--join Unit on (Storage.UnitID = Unit.UnitID)
--where Store.PurchaseDay='' and
--Store.FirmID=''
select FirmName, UNP, FirmLegalAddress, FirmAccountNumber,FirmBankDetails, FirmPhoneNumber from Firms where FirmID=2

select EmpFirstName, EmpLastName--, sum(Store.TotalPrice)  
                from Employee 
              --join Firms on(Store.FirmID = Firms.FirmID) 
                join Store on (Store.EmployeeID=Employee.EmployeeID)
              --join Storage on(Store.StorageID= Storage.StorageID) 
				group by EmpFirstName, 2

select sum(TotalPrice) from Store where EmployeeID = (select EmployeeID from Employee where EmpFirstName like '%4')

select Employee.EmpFirstName, Employee.EmpLastName, sum(TotalPrice) as "������� �� �����"
from Store 
join Employee on (Store.EmployeeID=Employee.EmployeeID)
group by Employee.EmpFirstName, Employee.EmpLastName

		
		